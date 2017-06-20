/*******************************************************************************
* File Name: `$INSTANCE_NAME`_I2C.h
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  This header file contains registers and constants associated with the
* CharLCD_I2C component.
*
* Note:
*
********************************************************************************
* Copyright 2008-2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
*******************************************************************************/

#if !defined(CY_CHARLCD_`$INSTANCE_NAME`_H)
#define CY_CHARLCD_`$INSTANCE_NAME`_H

#include "cytypes.h"
#include "cyfitter.h"


/***************************************
*   Conditional Compilation Parameters
***************************************/

#define `$INSTANCE_NAME`_CONVERSION_ROUTINES     (`$ConversionRoutines_DEF`u)
#define `$INSTANCE_NAME`_CUSTOM_CHAR_SET         (`$CustomCharDefines_API_GEN`u)

/* Custom character set types */
#define `$INSTANCE_NAME`_NONE                     (0u)    /* No Custom Fonts      */
#define `$INSTANCE_NAME`_HORIZONTAL_BG            (1u)    /* Horizontal Bar Graph */
#define `$INSTANCE_NAME`_VERTICAL_BG              (2u)    /* Vertical Bar Graph   */
#define `$INSTANCE_NAME`_USER_DEFINED             (3u)    /* User Defined Fonts   */


/***************************************
*     Data Struct Definitions
***************************************/

/* Sleep Mode API Support */
typedef struct
{
    uint8 enableState;
} 
`$INSTANCE_NAME`_BACKUP_STRUCT;


/***************************************
*        Function Prototypes
***************************************/

void `$INSTANCE_NAME`_Init(void) `=ReentrantKeil($INSTANCE_NAME . "_Init")`;
void `$INSTANCE_NAME`_Enable(void) `=ReentrantKeil($INSTANCE_NAME . "_Enable")`;
void `$INSTANCE_NAME`_Start(void) `=ReentrantKeil($INSTANCE_NAME . "_Start")`;
void `$INSTANCE_NAME`_Stop(void) `=ReentrantKeil($INSTANCE_NAME . "_Stop")`;
void `$INSTANCE_NAME`_WrtCntrlUpNib(uint8 cByte) `=ReentrantKeil($INSTANCE_NAME . "_WrtCntrlUpNib")`;
void `$INSTANCE_NAME`_WriteControl(uint8 cByte) `=ReentrantKeil($INSTANCE_NAME . "_WriteControl")`;
void `$INSTANCE_NAME`_WriteData(uint8 dByte) `=ReentrantKeil($INSTANCE_NAME . "_WriteData")`;
void `$INSTANCE_NAME`_PrintString(char8 const string[]) `=ReentrantKeil($INSTANCE_NAME . "_PrintString")`;
void `$INSTANCE_NAME`_Position(uint8 row, uint8 column) `=ReentrantKeil($INSTANCE_NAME . "_Position")`;
void `$INSTANCE_NAME`_PutChar(char8 character) `=ReentrantKeil($INSTANCE_NAME . "_PutChar")`;
void `$INSTANCE_NAME`_IsReady(void) `=ReentrantKeil($INSTANCE_NAME . "_IsReady")`;
void `$INSTANCE_NAME`_SaveConfig(void) `=ReentrantKeil($INSTANCE_NAME . "_SaveConfig")`;
void `$INSTANCE_NAME`_RestoreConfig(void) `=ReentrantKeil($INSTANCE_NAME . "_RestoreConfig")`;
void `$INSTANCE_NAME`_Sleep(void) `=ReentrantKeil($INSTANCE_NAME . "_Sleep")`;
void `$INSTANCE_NAME`_Wakeup(void) `=ReentrantKeil($INSTANCE_NAME . "_Wakeup")`;

#define `$INSTANCE_NAME`_PosPrintString(row, col, string) `$INSTANCE_NAME`_Position(row, col); `$INSTANCE_NAME`_PrintString(string); 
#define `$INSTANCE_NAME`_PosPutChar(row, col, character) `$INSTANCE_NAME`_Position(row, col); `$INSTANCE_NAME`_PutChar(character); 

#if((`$INSTANCE_NAME`_CUSTOM_CHAR_SET == `$INSTANCE_NAME`_VERTICAL_BG) || \
                (`$INSTANCE_NAME`_CUSTOM_CHAR_SET == `$INSTANCE_NAME`_HORIZONTAL_BG))

    void  `$INSTANCE_NAME`_LoadCustomFonts(uint8 const customData[])
                        `=ReentrantKeil($INSTANCE_NAME . "_LoadCustomFonts")`;

    void  `$INSTANCE_NAME`_DrawHorizontalBG(uint8 row, uint8 column, uint8 maxCharacters, uint8 value)
                         `=ReentrantKeil($INSTANCE_NAME . "_DrawHorizontalBG")`;

    void `$INSTANCE_NAME`_DrawVerticalBG(uint8 row, uint8 column, uint8 maxCharacters, uint8 value)
                        `=ReentrantKeil($INSTANCE_NAME . "_DrawVerticalBG")`;

#endif /* ((`$INSTANCE_NAME`_CUSTOM_CHAR_SET == `$INSTANCE_NAME`_VERTICAL_BG) */

#if(`$INSTANCE_NAME`_CUSTOM_CHAR_SET == `$INSTANCE_NAME`_USER_DEFINED)

    void `$INSTANCE_NAME`_LoadCustomFonts(uint8 const customData[])
                            `=ReentrantKeil($INSTANCE_NAME . "_LoadCustomFonts")`;

#endif /* ((`$INSTANCE_NAME`_CUSTOM_CHAR_SET == `$INSTANCE_NAME`_USER_DEFINED) */

#if(`$INSTANCE_NAME`_CONVERSION_ROUTINES == 1u)

    /* ASCII Conversion Routines */
    void `$INSTANCE_NAME`_PrintInt8(uint8 value) `=ReentrantKeil($INSTANCE_NAME . "_PrintInt8")`;
    void `$INSTANCE_NAME`_PrintInt16(uint16 value) `=ReentrantKeil($INSTANCE_NAME . "_PrintInt16")`;
    void `$INSTANCE_NAME`_PrintNumber(uint16 value) `=ReentrantKeil($INSTANCE_NAME . "_PrintNumber")`; 
	
	#define `$INSTANCE_NAME`_PosPrintInt8(row, col, value) `$INSTANCE_NAME`_Position(row, col); `$INSTANCE_NAME`_PrintInt8(value); 
	#define `$INSTANCE_NAME`_PosPrintInt16(row, col, value) `$INSTANCE_NAME`_Position(row, col); `$INSTANCE_NAME`_PrintInt16(value);
	#define `$INSTANCE_NAME`_PosPrintNumber(row, col, value) `$INSTANCE_NAME`_Position(row, col); `$INSTANCE_NAME`_PrintNumber(value);

#endif /* `$INSTANCE_NAME`_CONVERSION_ROUTINES == 1u */

/* Clear Macro */
#define `$INSTANCE_NAME`_ClearDisplay() `$INSTANCE_NAME`_WriteControl(`$INSTANCE_NAME`_CLEAR_DISPLAY)

/* Off Macro */
#define `$INSTANCE_NAME`_DisplayOff() `$INSTANCE_NAME`_WriteControl(`$INSTANCE_NAME`_DISPLAY_CURSOR_OFF)

/* On Macro */
#define `$INSTANCE_NAME`_DisplayOn() `$INSTANCE_NAME`_WriteControl(`$INSTANCE_NAME`_DISPLAY_ON_CURSOR_OFF)


/***************************************
*           Global Variables
***************************************/

extern uint8 `$INSTANCE_NAME`_initVar;
extern uint8 `$INSTANCE_NAME`_enableState;
extern uint8 const CYCODE `$INSTANCE_NAME`_customFonts[64u];


/***************************************
*           API Constants
***************************************/

/* Full Byte Commands Sent as Two Nibbles */

/* 2 Lines 5x7 Characters 8 bit mode*/
#define `$INSTANCE_NAME`_DISPLAY_8_BIT_INIT       (0x38u)
/* 2 lines 5x7 Charcaters 4 bit mode*/
#define `$INSTANCE_NAME`_DISPLAY_4_BIT_INIT       (0x28u)

#define `$INSTANCE_NAME`_DISPLAY_CURSOR_OFF       (0x08u)
#define `$INSTANCE_NAME`_CLEAR_DISPLAY            (0x01u)
#define `$INSTANCE_NAME`_CURSOR_AUTO_INCR_ON      (0x06u)
#define `$INSTANCE_NAME`_DISPLAY_CURSOR_ON        (0x0Eu)
#define `$INSTANCE_NAME`_DISPLAY_ON_CURSOR_OFF    (0x0Cu)
#define `$INSTANCE_NAME`_DISPLAY_2_LINES_5x10     (0x2Cu)

#define `$INSTANCE_NAME`_RESET_CURSOR_POSITION    (0x03u)
#define `$INSTANCE_NAME`_CURSOR_WINK              (0x0Du)
#define `$INSTANCE_NAME`_CURSOR_BLINK             (0x0Fu)
#define `$INSTANCE_NAME`_CURSOR_SH_LEFT           (0x10u)
#define `$INSTANCE_NAME`_CURSOR_SH_RIGHT          (0x14u)
#define `$INSTANCE_NAME`_CURSOR_HOME              (0x02u)
#define `$INSTANCE_NAME`_CURSOR_LEFT              (0x04u)
#define `$INSTANCE_NAME`_CURSOR_RIGHT             (0x06u)

/* Point to Character Generator Ram 0 */
#define `$INSTANCE_NAME`_CGRAM_0                  (0x40u)

/* Point to Display Data Ram 0 */
#define `$INSTANCE_NAME`_DDRAM_0                  (0x80u)

/* LCD Characteristics */
#define `$INSTANCE_NAME`_CHARACTER_WIDTH          (0x05u)
#define `$INSTANCE_NAME`_CHARACTER_HEIGHT         (0x08u)

#if(`$INSTANCE_NAME`_CONVERSION_ROUTINES == 1u)
    #define `$INSTANCE_NAME`_NUMBER_OF_REMAINDERS (0x05u)
    #define `$INSTANCE_NAME`_TEN                  (0x0Au)
#endif /* `$INSTANCE_NAME`_CONVERSION_ROUTINES == 1u */

/* Nibble Offset and Mask */
#define `$INSTANCE_NAME`_NIBBLE_SHIFT             (0x04u)
#define `$INSTANCE_NAME`_NIBBLE_MASK              (0x0Fu)

/* LCD Module Address Constants */
#define `$INSTANCE_NAME`_ROW_0_START              (0x80u)
#define `$INSTANCE_NAME`_ROW_1_START              (0xC0u)
#define `$INSTANCE_NAME`_ROW_2_START              (0x94u)
#define `$INSTANCE_NAME`_ROW_3_START              (0xD4u)

/* Custom Character References */
#define `$INSTANCE_NAME`_CUSTOM_0                 (0x00u)
#define `$INSTANCE_NAME`_CUSTOM_1                 (0x01u)
#define `$INSTANCE_NAME`_CUSTOM_2                 (0x02u)
#define `$INSTANCE_NAME`_CUSTOM_3                 (0x03u)
#define `$INSTANCE_NAME`_CUSTOM_4                 (0x04u)
#define `$INSTANCE_NAME`_CUSTOM_5                 (0x05u)
#define `$INSTANCE_NAME`_CUSTOM_6                 (0x06u)
#define `$INSTANCE_NAME`_CUSTOM_7                 (0x07u)

/* Other constants */
#define `$INSTANCE_NAME`_BYTE_UPPER_NIBBLE_SHIFT  (0x04u)
#define `$INSTANCE_NAME`_BYTE_LOWER_NIBBLE_MASK   (0x0Fu)
#define `$INSTANCE_NAME`_U16_UPPER_BYTE_SHIFT     (0x08u)
#define `$INSTANCE_NAME`_U16_LOWER_BYTE_MASK      (0xFFu)
#define `$INSTANCE_NAME`_CUSTOM_CHAR_SET_LEN      (0x40u)

#define `$INSTANCE_NAME`_CMD_DELAY_US			 (2000u)/* 2.0 msec Delay */
#define `$INSTANCE_NAME`_DATA_DELAY_US			 (500u)/*  0.50 msec Delay */
#define `$INSTANCE_NAME`_NIB_DELAY_US			 (100u)/*  0.10 msec Delay */
#define `$INSTANCE_NAME`_INIT_DELAY			  	 (20u) /*  20.0 msec Delay */
#define `$INSTANCE_NAME`_INIT_UP_NIB_DELAY	  	 (5u) /*   5.0 msec Delay */
#define `$INSTANCE_NAME`_INIT_CMD_DELAY		  	 (5u) /*   5.0 msec Delay */

/**************************************
*             Registers               *
***************************************/
/* I2C 7 bit slave address for the PCF8574A A0=1,A1=1,A2=1*/

//#define `$INSTANCE_NAME`_I2C_SLAVE_ADDR  		(0x3Fu)
#define `$INSTANCE_NAME`_I2C_SLAVE_ADDR  		(0x27u)

/* Buffer and packet size */
#define `$INSTANCE_NAME`_UPPER_NIB_BUFFER_SIZE	(2u)
#define `$INSTANCE_NAME`_UPPER_NIB_PACKET_SIZE	(`$INSTANCE_NAME`_UPPER_NIB_BUFFER_SIZE)
#define `$INSTANCE_NAME`_BUFFER_SIZE     		(4u)
#define `$INSTANCE_NAME`_PACKET_SIZE     		(`$INSTANCE_NAME`_BUFFER_SIZE)

/* Packet positions */
#define `$INSTANCE_NAME`_PACKET_0_POS  (0u)
#define `$INSTANCE_NAME`_PACKET_1_POS  (1u)
#define `$INSTANCE_NAME`_PACKET_2_POS  (2u)
#define `$INSTANCE_NAME`_PACKET_3_POS  (3u)

/* Nibble Offset and Mask */

#define `$INSTANCE_NAME`_LOWER_NIB_SHIFT        (0x04u)
#define `$INSTANCE_NAME`_LOWER_NIB_MASK         (0x0Fu)
#define `$INSTANCE_NAME`_UPPER_NIB_MASK         (0xF0u)

/* I2C LOGIC CONSTANTS */

#define `$INSTANCE_NAME`_BLH				    (0x08u)/* Back Light On */
#define `$INSTANCE_NAME`_EH					(0x04u)/* E Input High */
#define `$INSTANCE_NAME`_RWH				    (0x02u)/* RW Input High */
#define `$INSTANCE_NAME`_RSH				    (0x01u)/* RS Input High */


/***************************************
*       Obsolete function names
***************************************/
#if(`$INSTANCE_NAME`_CONVERSION_ROUTINES == 1u)
    /* This function names are obsolete an they will be removed in future 
    * revisions of component.
    */
    #define `$INSTANCE_NAME`_PrintDecUint16(x)   `$INSTANCE_NAME`_PrintNumber(x)  
    #define `$INSTANCE_NAME`_PrintHexUint8(x)    `$INSTANCE_NAME`_PrintInt8(x)
    #define `$INSTANCE_NAME`_PrintHexUint16(x)   `$INSTANCE_NAME`_PrintInt16(x)        

#endif /* `$INSTANCE_NAME`_CONVERSION_ROUTINES == 1u */

#endif /* CY_CHARLCD_`$INSTANCE_NAME`_H */


/* [] END OF FILE */
