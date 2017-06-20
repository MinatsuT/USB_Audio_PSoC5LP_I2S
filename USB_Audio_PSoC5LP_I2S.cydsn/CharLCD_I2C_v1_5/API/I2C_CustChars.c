/*******************************************************************************
* File Name: `$INSTANCE_NAME`_L2C_CustChars.c
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  This file provides the source code for the CharLCD custom character
*  API.
*
* Note:
*
********************************************************************************
* Copyright 2008-2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
*******************************************************************************/

#include "`$INSTANCE_NAME`_I2C.h"


uint8 const CYCODE `$INSTANCE_NAME`_customFonts[] = \
{
    /* Character `$INSTANCE_NAME`_CUSTOM_0   */
    `$CUST_CHAR0`, \
    /* Character `$INSTANCE_NAME`_CUSTOM_1   */
    `$CUST_CHAR1`, \
    /* Character `$INSTANCE_NAME`_CUSTOM_2   */
    `$CUST_CHAR2`, \
    /* Character `$INSTANCE_NAME`_CUSTOM_3   */
    `$CUST_CHAR3`, \
    /* Character `$INSTANCE_NAME`_CUSTOM_4   */
    `$CUST_CHAR4`, \
    /* Character `$INSTANCE_NAME`_CUSTOM_5   */
    `$CUST_CHAR5`, \
    /* Character `$INSTANCE_NAME`_CUSTOM_6   */
    `$CUST_CHAR6`, \
    /* Character `$INSTANCE_NAME`_CUSTOM_7   */
    `$CUST_CHAR7` \
 };


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_LoadCustomFonts
********************************************************************************
*
* Summary:
*  Loads 8 custom font characters into the LCD Module for use.  Cannot use
*  characters from two different font sets at once, but font sets can be
*  switched out during runtime.
*
* Parameters:
*  customData:  pointer to a constant array of 64 bytes representing 8 custom
*               font characters.
* Return:
*  None.
*
* Theory:
*  Prior to using this function user need to import the pointer to custom
*  font array to your project by writting the following in the source code file
*  where custom font will be used:
*       extern uint8 const CYCODE LCD_Char_customFonts[];
*  This function is not automatically called by the Start() routine and must be
*  called manually by the user.
*
*******************************************************************************/
void `$INSTANCE_NAME`_LoadCustomFonts(uint8 const customData[]) `=ReentrantKeil($INSTANCE_NAME . "_LoadCustomFonts")`
{
    uint8 indexU8;

    `$INSTANCE_NAME`_IsReady();

    /* Set starting address in the LCD Module.
    * Optionally: Read the current address to restore at a later time.
    */
    `$INSTANCE_NAME`_WriteControl(`$INSTANCE_NAME`_CGRAM_0);

    /* Load in the 64 bytes of CustomChar Data */
    for(indexU8 = 0u; indexU8 < `$INSTANCE_NAME`_CUSTOM_CHAR_SET_LEN; indexU8++)
    {
        `$INSTANCE_NAME`_WriteData(customData[indexU8]);
    }

    `$INSTANCE_NAME`_IsReady();
    `$INSTANCE_NAME`_WriteControl(`$INSTANCE_NAME`_DDRAM_0);
}


/* [] END OF FILE */
