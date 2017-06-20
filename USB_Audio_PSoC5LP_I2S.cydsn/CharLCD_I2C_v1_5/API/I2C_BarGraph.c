/*******************************************************************************
* File Name: `$INSTANCE_NAME`_I2C_BarGraph.c
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  This file provides the custom character data and API for creating and using
*  bar graphs with the CharLDC_I2C component.
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
*******************************************************************************/
void `$INSTANCE_NAME`_LoadCustomFonts(uint8 const customData[]) `=ReentrantKeil($INSTANCE_NAME . "_LoadCustomFonts")`
{
    uint8 indexU8;

    `$INSTANCE_NAME`_IsReady();
    /* Set starting address in the LCD Module */
    /* Optionally: Read the current address to restore at a later time */
    `$INSTANCE_NAME`_WriteControl(`$INSTANCE_NAME`_CGRAM_0);

    /* Load in the 64 bytes of CustomChar Data */
    for(indexU8 = 0u; indexU8 < `$INSTANCE_NAME`_CUSTOM_CHAR_SET_LEN; indexU8++)
    {
        /* Delay between each write */
        `$INSTANCE_NAME`_WriteData(customData[indexU8]);
    }

    `$INSTANCE_NAME`_IsReady();
    `$INSTANCE_NAME`_WriteControl(`$INSTANCE_NAME`_DDRAM_0);
}


#if (`$INSTANCE_NAME`_CUSTOM_CHAR_SET == `$INSTANCE_NAME`_HORIZONTAL_BG)

    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_DrawHorizontalBG
    ********************************************************************************
    *
    * Summary:
    *  Draws the horizontal bargraph.
    *
    * Parameters:
    *  row:            The row in which the bar graph starts.
    *  column:         The column in which the bar graph starts.
    *  maxCharacters:  The max length of the graph in whole characters.
    *  value:          The current length or height of the graph in pixels.
    *
    * Return:
    *  void.
    *
    *******************************************************************************/
    void `$INSTANCE_NAME`_DrawHorizontalBG(uint8 row, uint8 column, uint8 maxCharacters, uint8 value) \
                                                            `=ReentrantKeil($INSTANCE_NAME . "_DrawHorizontalBG")`
    {
        /* 8-bit Counter */
        uint8 count8;
        uint8 fullChars;
        uint8 remainingPixels;

        /* Number of full characters to draw */
        fullChars = value / `$INSTANCE_NAME`_CHARACTER_WIDTH;

        /* Number of remaining pixels to draw */
        remainingPixels = value % `$INSTANCE_NAME`_CHARACTER_WIDTH;

        /* Ensure that the maximum character limit is followed. */
        if(fullChars >= maxCharacters)
        {
            fullChars = maxCharacters;
        }

        /* Put Cursor at start position */
        `$INSTANCE_NAME`_Position(row, column);

        /* Write full characters */
        for(count8 = 0u; count8 < fullChars; count8++)
        {
            `$INSTANCE_NAME`_WriteData(`$INSTANCE_NAME`_CUSTOM_5);
        }

        if(fullChars < maxCharacters)
        {
            /* Write remaining pixels */
            `$INSTANCE_NAME`_WriteData(remainingPixels);

            if(fullChars < (maxCharacters - 1u))
            {
                /* Fill with whitespace to end of bar graph */
                for(count8 = 0u; count8 < (maxCharacters - fullChars - 1u); count8++)
                {
                    `$INSTANCE_NAME`_WriteData(`$INSTANCE_NAME`_CUSTOM_0);
                }
            }
        }
    }

#endif /* `$INSTANCE_NAME`_CUSTOM_CHAR_SET == `$INSTANCE_NAME`_HORIZONTAL_BG */


#if (`$INSTANCE_NAME`_CUSTOM_CHAR_SET == `$INSTANCE_NAME`_VERTICAL_BG)

    /*******************************************************************************
    *  Function Name: `$INSTANCE_NAME`_DrawVerticalBG
    ********************************************************************************
    *
    * Summary:
    *  Draws the vertical bargraph.
    *
    * Parameters:
    *  row:            The row in which the bar graph starts.
    *  column:         The column in which the bar graph starts.
    *  maxCharacters:  The max height of the graph in whole characters.
    *  value:          The current length or height of the graph in pixels.
    *
    * Return:
    *  void.
    *
    *******************************************************************************/
    void `$INSTANCE_NAME`_DrawVerticalBG(uint8 row, uint8 column, uint8 maxCharacters, uint8 value) \
                                                            `=ReentrantKeil($INSTANCE_NAME . "_DrawVerticalBG")`
    {
        /* 8-bit Counter */
        uint8 count8 = 0u;
        /* Current Row Tracker */
        int8 currentRow;
        uint8 fullChars;
        uint8 remainingPixels;

        /* Number of full characters to draw */
        fullChars = value / `$INSTANCE_NAME`_CHARACTER_HEIGHT;

        /* Number of remaining pixels to draw */
        remainingPixels = value % `$INSTANCE_NAME`_CHARACTER_HEIGHT;

        /* Put Cursor at start position */
        `$INSTANCE_NAME`_Position(row, column);

        /* Make sure the bar graph fits inside the space allotted */
        if(fullChars >= maxCharacters)
        {
            fullChars = maxCharacters;
        }

        /*  Write full characters */
        while(count8 < fullChars)
        {
            `$INSTANCE_NAME`_WriteData(`$INSTANCE_NAME`_CUSTOM_7);

            count8++;

            /* Each pass through, move one row higher */
            if((((int8) row) - ((int8) count8)) >= 0)
            {
                `$INSTANCE_NAME`_Position(row - count8, column);
            }
            else
            {
                break;
            }
        }

        if(((((int8) row) - ((int8) count8)) >= 0) && (fullChars < maxCharacters))
        {
            /* Write remaining pixels */
            if(remainingPixels == 0u)
            {
                `$INSTANCE_NAME`_WriteData((uint8) ' ');
            }
            else
            {
                `$INSTANCE_NAME`_WriteData(remainingPixels - 1u);
            }

            currentRow = ((int8) row) - ((int8) count8) - 1;

            if(currentRow >= 0)
            {
                /* Move up one row and fill with whitespace till top of bar graph */
                for(count8 = 0u; count8 < (maxCharacters - fullChars - 1u); count8++)
                {
                    `$INSTANCE_NAME`_Position((uint8)currentRow, column);
                    `$INSTANCE_NAME`_WriteData((uint8) ' ');
                    currentRow --;
                }
            }
        }
    }

#endif /* `$INSTANCE_NAME`_CUSTOM_CHAR_SET == `$INSTANCE_NAME`_VERTICAL_BG */


/* [] END OF FILE */
