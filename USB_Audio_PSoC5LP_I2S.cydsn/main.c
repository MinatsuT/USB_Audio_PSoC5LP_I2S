/*******************************************************************************
* File Name: main.c
*
* Version: 2.0
*
* Description:
*  This USB example project implements a USB Audio Device that connects to the
*  PC via the USB interface. The USB Audio Device does not require a special
*  USB driver, because the USB Audio support is already built into Windows.
*  The device appears in the system as a mono speaker with 8-bit resolution and
*  a sample rate of 32 kHz.
*
********************************************************************************
* Copyright 2012-2015, Cypress Semiconductor Corporation. All rights reserved.
* This software is owned by Cypress Semiconductor Corporation and is protected
* by and subject to worldwide patent and copyright laws and treaties.
* Therefore, you may use this software only as provided in the license agreement
* accompanying the software package from which you obtained this software.
* CYPRESS AND ITS SUPPLIERS MAKE NO WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
* WITH REGARD TO THIS SOFTWARE, INCLUDING, BUT NOT LIMITED TO, NONINFRINGEMENT,
* IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE.
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

/* Variables used to manage DMA. */
volatile uint8 syncDma = 0u;

/* Variable declarations for VDACoutDMA. */
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

/* Variable declarations for I2S_DMA. */
uint8 I2SDmaCh;
uint8 I2SDmaTd[NUM_OF_BUFFERS];

/* DMA Configuration for I2S_DMA (Memory to I2S) */
#define I2S_DMA_BYTES_PER_BURST    (1u)
#define I2S_DMA_REQUEST_PER_BURST  (1u)
#define I2S_DMA_TD_TERMOUT_EN      (I2S_DMA__TD_TERMOUT_EN)
#define I2S_DMA_DST_BASE           (CYDEV_PERIPH_BASE)
#define I2S_DMA_SRC_BASE           (CY_PSOC5LP) ? ((uint32) soundBuffer_L) : (CYDEV_SRAM_BASE)
#define I2S_DMA_ENABLE_PRESERVE_TD (1u)

CY_ISR_PROTO(VdacDmaDone);

/* Configuration for I2S clock generator (VCO) adjustment. */
#define adjustInterval              (10u)
#define adjustTic                   (vctrl/10000.0*0.3 * fabs(distAverage-sHALF_BUFFER_SIZE)*0.01)
#define UpperAdjustRange            (+(int16)TRANSFER_SIZE*3/2)
#define LowerAdjustRange            (-(int16)TRANSFER_SIZE*3/2)
#define MovingAverageWeight         (fs/100000.0*0.01)
#define SGN(x)                      (((x) < 0) ? -1 : (((x) > 0) ? 1 : 0))

#define Vctrl_HIGH                  65535
#define Vctrl_LOW                   10000
#define Vctrl_HIGHEST               65535
#define Vctrl_LOWEST                10000

/* PWM macros. */
#define Vctrl_Read()                PWM_ReadCompare()
#define Vctrl_Write(x)              PWM_WriteCompare(x)
#define Vctrl_Start()               PWM_Start()
#define Vctrl_Stop()                PWM_Stop()

/* Debug print buffer. */
char dbuf[256];
#define dp(x)                       DP_PutString(x);

/* EZI2C buffer watched by external device (PC). */
struct _EZI2C_buf {
    uint16 dist;
    uint16 distAvrerage;
    int16 distAvDiff;
    int16 clockAdjust;
    uint16 vctrl;
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

uint32 currentFs();
void initDMAs();

/*******************************************************************************
* Function Name: main
********************************************************************************
*
* Summary:
*
*  The main function performs the following actions:
*   1. Initializes LCD and VDAC components.
*   2. Configures the DMA channel and transfers the descriptors.
*      The source is souundBuffer (SRAM) and the destination is the VDAC data register.
*   3. Starts the USBFS component and waits until the device is got enumerated
*      by the host.
*   4. When the PC starts streaming, the DMA starts copying data into the VDAC with
*      the 32-kHz frequency. None of the synchronization methods is implemented:
*      If the PC (source) transfers data faster than the device (sink), transfers it  ????
*      to VDAC the extra samples are dropped.
*      If the PC (source) transfers data slower than the device (sink), transfers it  ????
*      to VDAC the transfer is stopped and starts after the buffer is half-full.
*
* Parameters:
*  None.
*
* Return:
*  None.
*
*******************************************************************************/
float tmpVcoFreq;

int main() {
    uint8 i;
    uint16 j;
    uint8 td;
    uint16 readSize;
    uint8 skipNextOut = 0u;
    float fs;
    int16 clockAdjust = 0;
    uint16 adjustIntervalCount = 0u;
    uint16 dist;
    float distAverage;
    float lastDistAverage = 0;
    float distAvDiff = 0;

    uint16 initialVctrl;
    uint16 vctrl;
    float vctrlAdj = 0;

    uint16 lastVcoCount = 0;
    uint16 vcoCount = 0;
    float vcoFreq = 0;
    uint8 vcoCountWait = 0;

    uint8 freqCtrlCount = 0;

    /* Start UART for debug print. */
    DP_Start();
    dp("\n\n");
    dp("========================================\n");
    dp(" PSoC USB Audio start\n");
    dp("========================================\n");
    sprintf(dbuf, "BUFFER_SIZE=%d (%d)\n", BUFFER_SIZE, sHALF_BUFFER_SIZE); dp(dbuf);
    sprintf(dbuf, "Sizeof(EZI2C_buf)=%d\n", sizeof(EZI2C_buf)); dp(dbuf);

    /* Start EZI2C for debug monitoring. */
    EZI2C_SetBuffer1(sizeof(EZI2C_buf), 0, (void *)&EZI2C_buf);
    EZI2C_Start();

    /* Start VCO_Counter. */
    VCO_Counter_Start();

    /* Start VDAC for clock generator. */
    Vctrl_Start();
    //Vctrl_Write((Vctrl_HIGH+Vctrl_LOW)/2);
    //Vctrl_Write(Vctrl_HIGH);
    Vctrl_Write(20000);
    vctrl = initialVctrl = Vctrl_Read();

    /* Start Comparator for clock generator. */
    CMP_Start();

    /* Start VDAC8 to generate output wave. */
    VDAC8_L_Start();
    VDAC8_R_Start();

    /* Initialize DMAs. */
    initDMAs();

    /* Start DMA completion interrupt. */
    VdacDmaDone_StartEx(&VdacDmaDone);

    /* Enable global interrupts. */
    CyGlobalIntEnable;

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

    /* Start USBFS Operation with 5V operation. */
    USBFS_Start(USBFS_AUDIO_DEVICE, USBFS_5V_OPERATION);

    /* Wait for device enumeration. */
    while (0u == USBFS_GetConfiguration()) {
    }

    for (;;) {
        /* Check if configuration or interface settings are changed. */
        if (0u != USBFS_IsConfigurationChanged()) {
            dp("[USB Configuration Changed]");

            /* Check active alternate setting. */
            if ( (0u != USBFS_GetConfiguration()) && (0u != USBFS_GetInterfaceSetting(AUDIO_INTERFACE)) ) {
                /* Alternate settings 1: Audio is streaming. */

                /* Reset VDAC output level. */
                VDAC8_L_Data = 128u;
                VDAC8_R_Data = 128u;

                /* Reset variables. */
                syncDma = 0u;
                skipNextOut = 0u;
                adjustIntervalCount = 0u;
                lastVcoCount = 0;
                vcoCountWait = 4;
                vcoFreq = 0;

                /* Get current sampling frequency. */
                fs = (fs == 0) ? currentFs() : fs;

                /* Enable OUT endpoint to receive audio stream. */
                USBFS_EnableOutEP(OUT_EP_NUM);

                CharLCD_Position(0u, 0u);
                CharLCD_PrintString("Audio ON ");
                dp(" Audio=[ON]");
            } else {
                /* Alternate settings 0: Audio is not streaming (mute). */

                /* Stop VCO to stop DMA transfer. */
                IDAC_Stop();

                /* Reset VDAC output level. */
                VDAC8_L_Data = 128u;
                VDAC8_R_Data = 128u;

                CharLCD_Position(0u, 0u);
                CharLCD_PrintString("Audio OFF");
                dp(" Audio=[OFF]");
            }

            /* Check if sample frequency is changed by host. */
            if ((USBFS_frequencyChanged != 0u) && (USBFS_transferState == USBFS_TRANS_STATE_IDLE)) {
                USBFS_frequencyChanged = 0u;

                /* Get current sampling frequency. */
                fs = currentFs();

                sprintf(dbuf, "%4.1fkHz", fs/1000.0);
                CharLCD_Position(1u, 0u);
                CharLCD_PrintString(dbuf);
                sprintf(dbuf, " Freq=[%4.1fkHz]", fs/1000.0); dp(dbuf);
            }
            
            dp("\n");
        }

        /* Check if EP buffer is full. */
        if (USBFS_OUT_BUFFER_FULL == USBFS_GetEPState(OUT_EP_NUM)) {
            if (0u == skipNextOut) {
                readSize = USBFS_GetEPCount(OUT_EP_NUM);
                /* Trigger DMA to copy data from OUT endpoint buffer. */
                USBFS_ReadOutEP(OUT_EP_NUM, tmpEpBuf, readSize);

                /* Wait until DMA completes copying data from OUT endpoint buffer. */
                while (USBFS_OUT_BUFFER_FULL == USBFS_GetEPState(OUT_EP_NUM)) ;

                /* Enable OUT endpoint to receive data from host. */
                USBFS_EnableOutEP(OUT_EP_NUM);

                /* Separate 2-channel data and append into each buffer. */
                for (j = 0u; j < readSize/(AUDIO_CH*BYTES_PER_CH); j++) {
                    soundBuffer_L[inIndex] = tmpEpBuf[BYTES_PER_CH*AUDIO_CH*j + 1]+128u;
                    soundBuffer_R[inIndex] = tmpEpBuf[BYTES_PER_CH*AUDIO_CH*j + 3]+128u;
                    soundBuffer_I2S[inIndex*I2S_DATA_SIZE+0] = tmpEpBuf[BYTES_PER_CH*AUDIO_CH*j +1];
                    soundBuffer_I2S[inIndex*I2S_DATA_SIZE+1] = tmpEpBuf[BYTES_PER_CH*AUDIO_CH*j +0];
                    soundBuffer_I2S[inIndex*I2S_DATA_SIZE+2] = tmpEpBuf[BYTES_PER_CH*AUDIO_CH*j +3];
                    soundBuffer_I2S[inIndex*I2S_DATA_SIZE+3] = tmpEpBuf[BYTES_PER_CH*AUDIO_CH*j +2];
                    inIndex = (inIndex+1) % BUFFER_SIZE;
                }
                dist = (inIndex - outIndex*TRANSFER_SIZE + BUFFER_SIZE)%BUFFER_SIZE;

                /* Enable DMA transfers when sound buffer is half-full. */
                if ((0u == syncDma) && (dist > sHALF_BUFFER_SIZE)) {
                    /* Disable underflow delayed start. */
                    syncDma = 1u;
                    lastDistAverage = distAverage = dist;

                    /* Start VCO to start DMA transfer. */
                    IDAC_Start();

                    CyDmaChStatus(VdacOutDmaCh_L, &td, NULL);
                    sprintf(dbuf, "\nDMA Clock START td=%d dist=%d\n", td, dist); dp(dbuf);
                }
            } else {
                /* Ignore received data from host and arm OUT endpoint
                 * without reading if overflow is detected.
                 */
                USBFS_EnableOutEP(OUT_EP_NUM);
                skipNextOut = 0u;
            }

            distAverage = distAverage*(1-MovingAverageWeight) + dist*MovingAverageWeight;

            /* Clock adjustment. */
            if (syncDma == 1u) {
                if (++adjustIntervalCount >= adjustInterval) {
                    adjustIntervalCount = 0u;
                    if (lastDistAverage == 0) lastDistAverage = distAverage;
                    distAvDiff = distAverage - lastDistAverage;
                    lastDistAverage = distAverage;

                    if (++freqCtrlCount > 5) {
                        freqCtrlCount = 0;
                        if (fs > 1 && vcoFreq > 1) {
                            float d = vcoFreq-fs;
                            if ( fabs(d/fs) > (1/100.0)) {
                                /* Rapid (coarse) frequency adjustment. */
                                vctrlAdj += (1/fs-1/vcoFreq) * (initialVctrl+vctrlAdj)/(1/vcoFreq)*0.3;
                                dp("0");
                                #if 0
                                sprintf(dbuf, "fs=%f ", fs); dp(dbuf);
                                sprintf(dbuf, "initialVctrl=%d vctrlAdj=%f ", initialVctrl, vctrlAdj); dp(dbuf);
                                sprintf(dbuf, "vcoFreq=%f vctrl=%f\n", vcoFreq, initialVctrl+vctrlAdj); dp(dbuf);
                                #endif
                            } else if ( fabs(d/fs) > (1/200.0)) {
                                /* Slower (fine) frequency adjustment. */
                                vctrlAdj += (1/fs-1/vcoFreq) * (initialVctrl+vctrlAdj)/(1/vcoFreq)*0.03;
                                dp("1");
                            } else {
                                /* Precise frequency adjustment. */
                                vctrlAdj += (1/fs-1/vcoFreq) * (initialVctrl+vctrlAdj)/(1/vcoFreq)*0.02;

                                clockAdjust = 0;
                                #if 1
                                /* Buffered size based precise adjustment. */
                                /* If buffered size is over half and still increasing, then faster the clock (decrease the divider). */
                                if ( distAverage > (sHALF_BUFFER_SIZE+UpperAdjustRange)) {
                                    vctrlAdj -= adjustTic;
                                    clockAdjust = UpperAdjustRange;
                                }
                                /* If buffered size is under half and still decreasing, then slower the clock (increase the divider). */
                                if ( distAverage < (sHALF_BUFFER_SIZE+LowerAdjustRange)) {
                                    vctrlAdj += adjustTic;
                                    clockAdjust = LowerAdjustRange;
                                }
                                #endif
                            }
                        }
                    }

                    uint16 tmpVctrl = initialVctrl+vctrlAdj;
                    if (vctrl != tmpVctrl) {
                        vctrl = tmpVctrl;

                        vctrl = (vctrl < Vctrl_LOWEST) ? Vctrl_LOWEST : vctrl;
                        vctrl = (vctrl > Vctrl_HIGHEST) ? Vctrl_HIGHEST : vctrl;
                        Vctrl_Write(vctrl);
                    }
                }
            }

            /* Measure VCO Frequency. */
            vcoCount = VCO_Counter_COUNTER_LSB;
            tmpVcoFreq = (((uint32)vcoCount - (uint32)lastVcoCount + 65536ul)%65536ul)*1000/64.0;
            lastVcoCount = vcoCount;
            if (tmpVcoFreq > 0) {
                if (vcoCountWait > 0u) {
                    vcoCountWait--;
                } else {
                    vcoFreq = (vcoFreq == 0) ? tmpVcoFreq : vcoFreq*0.9 + tmpVcoFreq*0.1;
                }
            }

            /* If EZI2C is idle, then update monitoring values. */
            if ( (EZI2C_GetActivity() & EZI2C_STATUS_BUSY) == 0u ) {
                EZI2C_buf.dist = dist;
                EZI2C_buf.distAvrerage = distAverage;
                EZI2C_buf.distAvDiff = distAvDiff;
                EZI2C_buf.clockAdjust = clockAdjust;
                EZI2C_buf.vctrl = vctrl;
                EZI2C_buf.L = VDAC8_L_Data;
                EZI2C_buf.R = VDAC8_R_Data;
                EZI2C_buf.flag = flag;
            }

            /* When internal sampling clock is slower than PC traffic and buffer is likely over-run,
             * then skip next transfer from PC.
             */
            if ( dist > TRANSFER_SIZE*8u ) {
                //skipNextOut = 1u;
                flag |= USB_DROP_FLAG;
            } else {
                flag &= ~USB_DROP_FLAG;
            }
        }
    }
}



/*******************************************************************************
* Function Name: VdacDmaDone
********************************************************************************
*
* Summary:
*  The Interrupt Service Routine for a DMA transfer completion event. The DMA is
*  stopped when there is no data to send.
*
* Parameters:
*  None.
*
* Return:
*  None.
*
*******************************************************************************/
uint16 vdacDist;

CY_ISR(VdacDmaDone) {
    /* Move to next buffer location and adjust to be within buffer size. */
    outIndex = (outIndex + 1) % NUM_OF_BUFFERS;

    vdacDist = ((inIndex - outIndex*TRANSFER_SIZE+BUFFER_SIZE)%BUFFER_SIZE);

    /* When there is no data to transfer to VDAC stop DMA and wait until
     * buffer is half-full to continue operation.
     */
    if ( vdacDist < TRANSFER_SIZE*2u ) {
        /* Stop SampleClk */
        //SampleClk_Stop();

        flag |= DMA_STOP_FLAG;
    } else {
        flag &= ~DMA_STOP_FLAG;
    }

}

uint32 currentFs() {
    uint32 fs = USBFS_currentSampleFrequency[OUT_EP_NUM][0] +
                (USBFS_currentSampleFrequency[OUT_EP_NUM][1]<<8) +
                (USBFS_currentSampleFrequency[OUT_EP_NUM][2]<<16);
    return fs;
}

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

    inIndex = 0u;
    outIndex = 0u;

    /* Start DMA operation. */
    CyDmaChEnable(VdacOutDmaCh_L, VDAC_DMA_ENABLE_PRESERVE_TD);
    CyDmaChEnable(VdacOutDmaCh_R, VDAC_DMA_ENABLE_PRESERVE_TD);
    CyDmaChEnable(I2SDmaCh, I2S_DMA_ENABLE_PRESERVE_TD);
}

/* [] END OF FILE */
