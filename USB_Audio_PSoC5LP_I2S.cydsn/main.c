/*******************************************************************************
* CY8CKIT-059(PSoC 5LP) USB Audio with Internal DAC and I2S Dual Out
*
* Author: Minatsu Tukisima
*
*******************************************************************************/
#include <project.h>
#include <stdio.h>
#include <math.h>

/* UBSFS device constants. */
#define USBFS_AUDIO_DEVICE  (0u)
#define AUDIO_INTERFACE     (1u)
#define OUT_EP_NUM          (2u)
#define AUDIO_CH            (2u)
#define BYTES_PER_CH        (2u)
#define USB_BUF_SIZE        (384u)

/* Audio buffer constants. */
#define TRANSFER_SIZE       (384u/AUDIO_CH/BYTES_PER_CH)
#define NUM_OF_BUFFERS      (10u)
#define BUFFER_SIZE         (TRANSFER_SIZE * NUM_OF_BUFFERS)
#define sHALF_BUFFER_SIZE   ((int16)(BUFFER_SIZE/2u))

#define I2S_CLOCK_FACTOR    (I2S_DATA_BITS*AUDIO_CH*2)
#define I2S_DATA_SIZE       (I2S_DATA_BITS/8*AUDIO_CH)
#define I2S_TRANSFER_SIZE   (TRANSFER_SIZE*I2S_DATA_SIZE)
#define I2S_BUFFER_SIZE     (I2S_TRANSFER_SIZE * NUM_OF_BUFFERS)

/* Circular buffer for audio stream. */
uint8 tmpEpBuf[USB_BUF_SIZE];
uint8 soundBuffer_L[BUFFER_SIZE];
uint8 soundBuffer_R[BUFFER_SIZE];
uint8 soundBuffer_I2S[I2S_BUFFER_SIZE];
volatile uint16 outIndex = 0u;
volatile uint16 inIndex = 0u;

/* Variables for VDACoutDMA. */
uint8 VdacOutDmaCh_L;
uint8 VdacOutDmaCh_R;
uint8 VdacOutDmaTd_L[NUM_OF_BUFFERS];
uint8 VdacOutDmaTd_R[NUM_OF_BUFFERS];

/* DMA Configuration for VDACoutDMA (Memory to VDAC) */
#define VDAC_DMA_BYTES_PER_BURST    (1u)
#define VDAC_DMA_REQUEST_PER_BURST  (1u)
#define VDAC_DMA_TD_TERMOUT_EN      (VdacDma_L__TD_TERMOUT_EN)
#define VDAC_DMA_DST_BASE           (CYDEV_PERIPH_BASE)
#define VDAC_DMA_SRC_BASE           (CY_PSOC5LP) ? ((uint32) soundBuffer_L) : (CYDEV_SRAM_BASE)
#define VDAC_DMA_ENABLE_PRESERVE_TD (1u)

/* Variables for I2S_DMA. */
uint8 I2SDmaCh;
uint8 I2SDmaTd[NUM_OF_BUFFERS];

/* DMA Configuration for I2S_DMA (Memory to I2S) */
#define I2S_DMA_BYTES_PER_BURST    (1u)
#define I2S_DMA_REQUEST_PER_BURST  (1u)
#define I2S_DMA_TD_TERMOUT_EN      (I2S_DMA__TD_TERMOUT_EN)
#define I2S_DMA_DST_BASE           (CYDEV_PERIPH_BASE)
#define I2S_DMA_SRC_BASE           (CY_PSOC5LP) ? ((uint32) soundBuffer_L) : (CYDEV_SRAM_BASE)
#define I2S_DMA_ENABLE_PRESERVE_TD (1u)

/* Configuration for I2S BitClk generator adjustment. */
#define adjustInterval              (40u)
#define adjustTic                   (fabs(distAverage-sHALF_BUFFER_SIZE)*30.0)
#define UpperAdjustRange            (+(int16)TRANSFER_SIZE*3/2)
#define LowerAdjustRange            (-(int16)TRANSFER_SIZE*3/2)
#define MovingAverageWeight         (fs/100000.0*0.01)
#define SGN(x)                      (((x) < 0) ? -1 : (((x) > 0) ? 1 : 0))

#define div_MAX                     0x7fffffffu
#define div_MIN                     1

#define DIVIDER_SOURCE_FREQ         (32000000)

/* Variables for BitClk frequency counter. */
volatile float bitClkFrequency = 0;
volatile uint32 bitClkCountWait = 0;
float tmpBitClkFreq;

/* Debug print buffer. */
char dbuf[256];
#define dp(x)                       DP_PutString(x);

/* EZI2C buffer watched by external device (PC). */
struct _EZI2C_buf {
    float bitClkFreqency;
    uint32 div;
    uint16 dist;
    uint16 distAvrerage;
    int16 clockAdjust;
    uint8 L;
    uint8 R;
    uint8 flag;
} EZI2C_buf;

/*
 * Operation Flag.
 *  bit 0 => (unused)
 *  bit 1 => DMA for VDAC is stopped due to buffer under-run.
 *  bit 2 => USB packet is dropped due to buffer over-run.
 */
volatile uint8 flag = 0u;
#define DMA_STOP_FLAG            (1u<<1)
#define USB_DROP_FLAG            (1u<<2)

/* Function prototype deffinitions. */
void initComponents(void);
void initDMAs(void);
CY_ISR_PROTO(VdacDmaDone);
CY_ISR_PROTO(FreqCapt);

/*******************************************************************************
* main
*******************************************************************************/
int main() {
    uint16 i;
    uint16 readSize;

    /* Current sampling rate specified by USB host. */
    float fs;

    /* Variables for BitClk generator */
    uint32 initialDiv;
    float divAdj = 0;
    uint32 div;
    uint32 tmpDiv = 0;

    /* Variables for BitClk frequency control. */
    uint16 dist;
    float distAverage;
    uint16 adjustIntervalCount = 0u;
    int16 clockAdjust = 0;
    float bitClkFreq;

    /* Variables used to manage DMA. */
    uint8 syncDma = 0u;

    /* Initialize components. */
    initComponents();

    dp("\n\n");
    dp("========================================\n");
    dp(" PSoC USB Audio start.\n");
    dp("========================================\n");
    sprintf(dbuf, "BUFFER_SIZE=%d\n", BUFFER_SIZE); dp(dbuf);
    sprintf(dbuf, "Sizeof(EZI2C_buf)=%d\n", sizeof(EZI2C_buf)); dp(dbuf);

    /* Start USBFS Operation with 5V operation. */
    USBFS_Start(USBFS_AUDIO_DEVICE, USBFS_5V_OPERATION);

    /* Wait for device enumeration. */
    while (0u == USBFS_GetConfiguration()) {
    }

    /*******************************************************************************
    * Main loop.
    *******************************************************************************/
    for (;;) {
        /* Check if configuration or interface settings are changed. */
        if (0u != USBFS_IsConfigurationChanged()) {
            /* Check active alternate setting. */
            if ( (0u != USBFS_GetConfiguration()) && (0u != USBFS_GetInterfaceSetting(AUDIO_INTERFACE)) ) {
                /* Alternate settings 1: Audio is streaming. */

                /* Reset VDAC output level. */
                VDAC8_L_Data = 128u;
                VDAC8_R_Data = 128u;

                /* Reset variables. */
                syncDma = 0u;
                adjustIntervalCount = 0u;
                bitClkFrequency = 0;
                bitClkCountWait = 2;

                /* Enable OUT endpoint to receive audio stream. */
                USBFS_EnableOutEP(OUT_EP_NUM);

                CharLCD_Position(0u, 0u);
                CharLCD_PrintString("Audio ON ");
                dp("Audio=[ON]\n");
            } else {
                /* Alternate settings 0: Audio is not streaming (mute). */

                /* Stop BitClk generator to stop DMA transfer. */
                FracDiv_Stop();

                /* Reset VDAC output level. */
                VDAC8_L_Data = 128u;
                VDAC8_R_Data = 128u;

                CharLCD_Position(0u, 0u);
                CharLCD_PrintString("Audio OFF");
                dp("\nAudio=[OFF]\n");
            }

            if (USBFS_GetConfiguration() != 0u) {
                /* Enable OUT endpoint to receive data from host. */
                USBFS_EnableOutEP(OUT_EP_NUM);
            }
        }

        /*******************************************************************************
        * Check if sampling frequency is changed by USB host.
        *******************************************************************************/
        if ((USBFS_frequencyChanged != 0u) && (USBFS_transferState == USBFS_TRANS_STATE_IDLE)) {
            /* Get current sampling frequency. */
            float tmpFs = USBFS_currentSampleFrequency[OUT_EP_NUM][0] +
                          (USBFS_currentSampleFrequency[OUT_EP_NUM][1]<<8) +
                          (USBFS_currentSampleFrequency[OUT_EP_NUM][2]<<16);

            if (tmpFs != fs) {
                fs = tmpFs;

                initialDiv = fs*I2S_CLOCK_FACTOR*(float)0x7fffffffu/(float)DIVIDER_SOURCE_FREQ;
                divAdj = 0;
                div = initialDiv+divAdj;
                FracDiv_Write(div, 0x7fffffffu);

                sprintf(dbuf, "InitialDiv=[%ld]\n", div); dp(dbuf);
                sprintf(dbuf, "NominalFreq=[%4.3fMHz]\n", ((float)DIVIDER_SOURCE_FREQ)*div/(float)0x7fffffffu/1000000.0); dp(dbuf);

                sprintf(dbuf, "%4.1fkHz", fs/1000.0);
                CharLCD_Position(1u, 0u);
                CharLCD_PrintString(dbuf);
                sprintf(dbuf, "Freq=[%4.1fkHz]\n", fs/1000.0); dp(dbuf);

                USBFS_frequencyChanged = 0u;
            }
        }
        
        /*******************************************************************************
        * Receive data from USB and extract into audio buffers.
        *******************************************************************************/
        if (USBFS_OUT_BUFFER_FULL == USBFS_GetEPState(OUT_EP_NUM)) {
            bitClkFreq = bitClkFrequency;

            readSize = USBFS_GetEPCount(OUT_EP_NUM);
            /* Trigger DMA to copy data from OUT endpoint buffer. */
            USBFS_ReadOutEP(OUT_EP_NUM, tmpEpBuf, readSize);

            /* Wait until DMA completes copying data from OUT endpoint buffer. */
            while (USBFS_OUT_BUFFER_FULL == USBFS_GetEPState(OUT_EP_NUM)) ;

            /* Enable OUT endpoint to receive data from host. */
            USBFS_EnableOutEP(OUT_EP_NUM);

            /* Separate 2-channel data and append into each buffer. */
            for (i = 0u; i < readSize/(AUDIO_CH*BYTES_PER_CH); i++) {
                soundBuffer_L[inIndex] = tmpEpBuf[BYTES_PER_CH*AUDIO_CH*i + 1]+128u;
                soundBuffer_R[inIndex] = tmpEpBuf[BYTES_PER_CH*AUDIO_CH*i + 3]+128u;
                soundBuffer_I2S[inIndex*I2S_DATA_SIZE+0] = tmpEpBuf[BYTES_PER_CH*AUDIO_CH*i +1];
                soundBuffer_I2S[inIndex*I2S_DATA_SIZE+1] = tmpEpBuf[BYTES_PER_CH*AUDIO_CH*i +0];
                soundBuffer_I2S[inIndex*I2S_DATA_SIZE+2] = tmpEpBuf[BYTES_PER_CH*AUDIO_CH*i +3];
                soundBuffer_I2S[inIndex*I2S_DATA_SIZE+3] = tmpEpBuf[BYTES_PER_CH*AUDIO_CH*i +2];
                inIndex = (inIndex+1) % BUFFER_SIZE;
            }
            dist = (inIndex - outIndex*TRANSFER_SIZE + BUFFER_SIZE)%BUFFER_SIZE;
            distAverage = distAverage*(1-MovingAverageWeight) + dist*MovingAverageWeight;

            /* Start DMA transfers when half of the sound buffer is fulfilled. */
            if (!syncDma && (dist >= sHALF_BUFFER_SIZE)) {
                /* Disable underflow delayed start. */
                syncDma = 1u;

                /* Reset dist average. */
                distAverage = dist;

                /* Start BitClk Generator to start DMA transfer. */
                FracDiv_Start();

                sprintf(dbuf, "\nDMA Clock START dist=%d\n", dist); dp(dbuf);
            }

            /*******************************************************************************
            * BitClk adjustment.
            *******************************************************************************/
            if (syncDma) {
                if (++adjustIntervalCount >= adjustInterval) {
                    adjustIntervalCount = 0u;
                    if (fs > 1 && bitClkFreq > 1) {
                        float d = bitClkFreq -fs;
                        if ( fabs(d/fs) > (1/100.0)) {
                            /* Rapid (coarse) frequency adjustment. */
                            divAdj += (initialDiv+divAdj)*(fs/bitClkFreq - 1.0)*0.8;
                            dp("0");
                        } else if ( fabs(d/fs) > (1/150.0)) {
                            /* Slower (fine) frequency adjustment. */
                            divAdj += (initialDiv+divAdj)*(fs/bitClkFreq - 1.0)*0.4;
                            dp("1");
                        } else {
                            /* Precise frequency adjustment. */
                            divAdj += (initialDiv+divAdj)*(fs/bitClkFreq - 1.0)*0.1;

                            clockAdjust = 0;
                            /* Buffered size based precise adjustment. */
                            /* If buffered size is over half and still increasing, then faster the clock (decrease the divider). */
                            if ( distAverage > (sHALF_BUFFER_SIZE+UpperAdjustRange)) {
                                divAdj += adjustTic;
                                clockAdjust = UpperAdjustRange;
                            }
                            /* If buffered size is under half and still decreasing, then slower the clock (increase the divider). */
                            if ( distAverage < (sHALF_BUFFER_SIZE+LowerAdjustRange)) {
                                divAdj -= adjustTic;
                                clockAdjust = LowerAdjustRange;
                            }
                        }
                    }

                    tmpDiv = initialDiv+divAdj;
                    if (div != tmpDiv) {
                        div = tmpDiv;
                        div = (div < div_MIN) ? div_MIN : div;
                        div = (div > div_MAX) ? div_MAX : div;
                        FracDiv_Write(div, 0x7fffffffu);
                    }
                }
            }

            /*******************************************************************************
            * Update EZI2C monitoring values.
            *******************************************************************************/
            if ( (EZI2C_GetActivity() & EZI2C_STATUS_BUSY) == 0u ) {
                EZI2C_buf.bitClkFreqency = bitClkFrequency;
                EZI2C_buf.div = div;
                EZI2C_buf.dist = dist;
                EZI2C_buf.distAvrerage = distAverage;
                EZI2C_buf.clockAdjust = clockAdjust;
                EZI2C_buf.L = VDAC8_L_Data;
                EZI2C_buf.R = VDAC8_R_Data;
                EZI2C_buf.flag = flag;
            }
        }
    }
}

/*******************************************************************************
* Initialize components.
*******************************************************************************/
void initComponents() {
    /* Enable global interrupts. */
    CyGlobalIntEnable;
    
    /* Start UART for debug print. */
    DP_Start();

    /* Start EZI2C for debug monitoring. */
    EZI2C_SetBuffer1(sizeof(EZI2C_buf), 0, (void *)&EZI2C_buf);
    EZI2C_Start();

    /* "Stop" BitClk Generator. */
    FracDiv_Stop();
    
    /* Start BitClk_Counter. */
    BitClk_Counter_Start();

    /* Start BitClkFrequency capture event ISR. */
    FreqCapt_StartEx(&FreqCapt);

    /* Start VDAC8 to generate output wave. */
    VDAC8_L_Start();
    VDAC8_R_Start();

    /* Start LCD display for indication. */
    I2C_CharLCD_Start();
    CharLCD_Start();

    /* Start PGAs */
    PGA_L_Start();
    PGA_R_Start();

    /* Start PGA for 3v3 generation. */
    I2S_3v3_Start();

    /* Start I2S */
    I2S_Start();
    I2S_EnableTx();

    /* Initialize DMAs. */
    initDMAs();

    /* Start DMA completion ISR. */
    VdacDmaDone_StartEx(&VdacDmaDone);
}

/*******************************************************************************
* Initialize (1)VdacDma_L, (2)VdacDma_R and (3)I2S_DMA.
*******************************************************************************/
void initDMAs() {
    uint8 i;

    /* Initialize DMA channel. */
    VdacOutDmaCh_L = VdacDma_L_DmaInitialize(VDAC_DMA_BYTES_PER_BURST, VDAC_DMA_REQUEST_PER_BURST,
                                             HI16(VDAC_DMA_SRC_BASE), HI16(VDAC_DMA_DST_BASE));
    VdacOutDmaCh_R = VdacDma_R_DmaInitialize(VDAC_DMA_BYTES_PER_BURST, VDAC_DMA_REQUEST_PER_BURST,
                                             HI16(VDAC_DMA_SRC_BASE), HI16(VDAC_DMA_DST_BASE));
    I2SDmaCh = I2S_DMA_DmaInitialize(I2S_DMA_BYTES_PER_BURST, I2S_DMA_REQUEST_PER_BURST,
                                     HI16(I2S_DMA_SRC_BASE), HI16(I2S_DMA_DST_BASE));

    /* Allocate transfer descriptors for each buffer chunk. */
    for (i = 0u; i < NUM_OF_BUFFERS; ++i) {
        VdacOutDmaTd_L[i] = CyDmaTdAllocate();
        VdacOutDmaTd_R[i] = CyDmaTdAllocate();
        I2SDmaTd[i] = CyDmaTdAllocate();
    }

    /* Configure DMA transfer descriptors. */
    for (i = 0u; i < (NUM_OF_BUFFERS - 1u); ++i) {
        /* Chain current and next DMA transfer descriptors to be in row. */
        CyDmaTdSetConfiguration(VdacOutDmaTd_L[i], TRANSFER_SIZE, VdacOutDmaTd_L[i + 1u],
                                (TD_INC_SRC_ADR | VDAC_DMA_TD_TERMOUT_EN));
        CyDmaTdSetConfiguration(VdacOutDmaTd_R[i], TRANSFER_SIZE, VdacOutDmaTd_R[i + 1u],
                                (TD_INC_SRC_ADR));
        CyDmaTdSetConfiguration(I2SDmaTd[i], I2S_TRANSFER_SIZE, I2SDmaTd[i + 1u],
                                (TD_INC_SRC_ADR));
    }
    /* Chain last and 1st DMA transfer descriptors to make cyclic buffer. */
    CyDmaTdSetConfiguration(VdacOutDmaTd_L[NUM_OF_BUFFERS - 1u], TRANSFER_SIZE, VdacOutDmaTd_L[0u],
                            (TD_INC_SRC_ADR | VDAC_DMA_TD_TERMOUT_EN));
    CyDmaTdSetConfiguration(VdacOutDmaTd_R[NUM_OF_BUFFERS - 1u], TRANSFER_SIZE, VdacOutDmaTd_R[0u],
                            (TD_INC_SRC_ADR));
    CyDmaTdSetConfiguration(I2SDmaTd[NUM_OF_BUFFERS - 1u], I2S_TRANSFER_SIZE, I2SDmaTd[0u],
                            (TD_INC_SRC_ADR));


    for (i = 0u; i < NUM_OF_BUFFERS; i++) {
        /* Set source and destination addresses. */
        CyDmaTdSetAddress(VdacOutDmaTd_L[i], LO16((uint32) &soundBuffer_L[i * TRANSFER_SIZE]),
                          LO16((uint32) VDAC8_L_Data_PTR));
        CyDmaTdSetAddress(VdacOutDmaTd_R[i], LO16((uint32) &soundBuffer_R[i * TRANSFER_SIZE]),
                          LO16((uint32) VDAC8_R_Data_PTR));
        CyDmaTdSetAddress(I2SDmaTd[i], LO16((uint32) &soundBuffer_I2S[i * I2S_TRANSFER_SIZE]),
                          LO16((uint32) I2S_TX_CH0_F0_PTR));
    }

    /* Set 1st transfer descriptor to execute. */
    CyDmaChSetInitialTd(VdacOutDmaCh_L, VdacOutDmaTd_L[0u]);
    CyDmaChSetInitialTd(VdacOutDmaCh_R, VdacOutDmaTd_R[0u]);
    CyDmaChSetInitialTd(I2SDmaCh, I2SDmaTd[0u]);

    /* Start DMA operation. */
    CyDmaChEnable(VdacOutDmaCh_L, VDAC_DMA_ENABLE_PRESERVE_TD);
    CyDmaChEnable(VdacOutDmaCh_R, VDAC_DMA_ENABLE_PRESERVE_TD);
    CyDmaChEnable(I2SDmaCh, I2S_DMA_ENABLE_PRESERVE_TD);
}

/*******************************************************************************
*  The Interrupt Service Routine for a DMA transfer completion event. The DMA is
*  stopped when there is no data to send.
*******************************************************************************/
CY_ISR(VdacDmaDone) {
    /* Move to next buffer location and adjust to be within buffer size. */
    outIndex = (outIndex + 1) % NUM_OF_BUFFERS;
}

/*******************************************************************************
*  The Interrupt Service Routine for BitClk_Counter capture event.
*******************************************************************************/
CY_ISR(FreqCapt) {
    /* Measure BitClk Frequency. */
    tmpBitClkFreq = BitClk_Counter_ReadCounter()*1000/(float)I2S_CLOCK_FACTOR;
    BitClk_Counter_ClearFIFO();

    if (tmpBitClkFreq > 0) {
        if (bitClkCountWait > 0u) {
            bitClkCountWait--;
        } else {
            bitClkFrequency = (bitClkFrequency == 0) ? tmpBitClkFreq : bitClkFrequency*0.8 + tmpBitClkFreq*0.2;
            //bitClkFrequency = tmpBitClkFreq;
        }
    }
}

/* [] END OF FILE */
