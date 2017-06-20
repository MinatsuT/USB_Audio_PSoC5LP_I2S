/*******************************************************************************
* Copyright 2008-2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
********************************************************************************/

using System;
using System.Collections.Generic;
using CyDesigner.Extensions.Common;
using CyDesigner.Extensions.Gde;
using System.Collections;
using CharLCD_I2C_v1_5;

namespace CharLCD_I2C_v1_5
{
    partial class CyCustomizer : ICyAPICustomize_v1
    {
        #region ICyAPICustomize_v1 Members
        private const string LCD_CFILE_NAME = "CharLCD_I2C.c";
        private const string CUST_CHARS_CFILE_NAME = "I2C_CustChars.c";
        private const string BARGRAPH_CFILE_NAME = "I2C_BarGraph.c";
        private const string CUST_CHAR_MACRONAME = "CUST_CHAR";

        private const string INSTANCE_NAME_PARAM = "INSTANCE_NAME";
        private const string CUSTOM_CHARACTER_SET_PARAM = "CustomCharacterSet";
        private const string CONVERSION_ROUTINE_PARAM = "ConversionRoutines";
        private const string UD_PARAM = "CUSTOM";

        private CyAPICustomizer m_custChar_CFile;
        private CyAPICustomizer m_barGraph_CFile;

        private CyCustomCharacterSetTypes m_customCharacterSet;
        private bool m_conversionRoutines = false;

        public IEnumerable<CyAPICustomizer> CustomizeAPIs(
            ICyInstQuery_v1 query,
            ICyTerminalQuery_v1 termQuery,
            IEnumerable<CyAPICustomizer> apis)
        {
            List<CyAPICustomizer> customizers = new List<CyAPICustomizer>(apis);
            Dictionary<string, string> paramDict = new Dictionary<string, string>();
            string instanceName = "";

            // Get the parameters from the characterLCD.c file customizer
            for (int i = 0; i < customizers.Count; i++)
            {
                CyAPICustomizer api = customizers[i];

                // Get dict from main file. 
                if (api.OriginalName.EndsWith(LCD_CFILE_NAME))
                {
                    paramDict = api.MacroDictionary;
                    paramDict.TryGetValue(INSTANCE_NAME_PARAM, out instanceName);
                }
                else if (api.OriginalName.EndsWith(CUST_CHARS_CFILE_NAME))
                {
                    m_custChar_CFile = api;
                }
                else if (api.OriginalName.EndsWith(BARGRAPH_CFILE_NAME))
                {
                    m_barGraph_CFile = api;
                }
            }

            // Determine Custom Character Set
            string value;
            paramDict.TryGetValue(CUSTOM_CHARACTER_SET_PARAM, out value);
            m_customCharacterSet = (CyCustomCharacterSetTypes)(int.Parse(value));

            // Determine existence of ASCII Routines
            paramDict.TryGetValue(CONVERSION_ROUTINE_PARAM, out value);
            m_conversionRoutines = (value == "1");

            #region New Code Substitution Values
            const string CUSTOM_CHAR_DEFINES_MACRO = "CustomCharDefines_API_GEN";
            const string CONVERSION_ROUTINE_MACRO = "ConversionRoutines_DEF";

            string customCharDeleted = "0";
            string customCharDefined = ((byte)m_customCharacterSet).ToString();


 

            #region Literal String Code for Conversion Routines
            string conversionRoutineDeleted = "0";

            string conversionRoutineDefined = "1";
            #endregion
            #endregion

            // If a character set is selected, build c file with data in it.
            switch (m_customCharacterSet)
            {
                case CyCustomCharacterSetTypes.USERDEFINED:
                    ConvertCharacters(LoadUserDefinedCharacters(paramDict), paramDict);
                    paramDict.Add(CUSTOM_CHAR_DEFINES_MACRO, customCharDefined);
                    customizers.Remove(m_barGraph_CFile);
                    break;
                case CyCustomCharacterSetTypes.VERTICAL:
                    GenerateVerticalBargraph(paramDict);
                    paramDict.Add(CUSTOM_CHAR_DEFINES_MACRO, customCharDefined);
                    customizers.Remove(m_custChar_CFile);
                    break;
                case CyCustomCharacterSetTypes.HORIZONTAL:
                    GenerateHorizontalBargraph(paramDict);
                    paramDict.Add(CUSTOM_CHAR_DEFINES_MACRO, customCharDefined);
                    customizers.Remove(m_custChar_CFile);
                    break;
                default:
                    paramDict.Add(CUSTOM_CHAR_DEFINES_MACRO, customCharDeleted);
                    customizers.Remove(m_barGraph_CFile);
                    customizers.Remove(m_custChar_CFile);
                    break;
            }

            // If conversion routines are selected, import them
            if (m_conversionRoutines)
            {
                paramDict.Add(CONVERSION_ROUTINE_MACRO, conversionRoutineDefined);
            }
            else
            {
                paramDict.Add(CONVERSION_ROUTINE_MACRO, conversionRoutineDeleted);
            }
            // Replace macro dictionaries with paramDict
            for (int i = 0; i < customizers.Count; i++)
            {
                CyAPICustomizer api = customizers[i];
                api.MacroDictionary = paramDict;
            }

            return customizers;
        }
        #endregion

        #region CustomCharacter Helper Methods
        // Load the User Defined Characters into CharLCDCustomizer ArrayList userDefinedCharacters.
        ArrayList LoadUserDefinedCharacters(Dictionary<string, string> parameters)
        {
            int index = 0;
            string temp = "";
            ArrayList characters = new ArrayList();
            while (parameters.TryGetValue(UD_PARAM + index.ToString(), out temp))
            {
                CyCustomCharacter character = new CyCustomCharacter();
                temp = temp.Trim('"').TrimEnd(',');
                CyParameterToCustomCharacter(character, temp);
                characters.Add(character);
                index++;
            }
            return characters;
        }

        // Given an ArrayList of Characters, convert array list into a C Data File.
        private void ConvertCharacters(ArrayList customCharacters, Dictionary<string, string> dict)
        {
            int stringIndex = 0;
            string key = "";
            string value = "";

            for (int i = 0; i < customCharacters.Count; i++)
            {
                CyCustomCharacter character = (CyCustomCharacter)customCharacters[i];

                // Define Value
                value = CharacterToCCode(character);

                // Define Key Name
                key = CUST_CHAR_MACRONAME + stringIndex.ToString();

                dict.Add(key, value);

                stringIndex++;
            }
        }

        // Given a CustomCharacter, convert it into the lines of code to build an LCD custom char Array.
        string CharacterToCCode(CyCustomCharacter customCharacter)
        {
            // "characterString" represents the character after conversion to C Code
            String characterString = "";

            CharLCD_I2C_v1_5.CyBox[,] pixelArray = customCharacter.GetBoxArray();

            // Indivudual row value to be calculated 
            byte rowValue;

            for (int row = 0; row < customCharacter.Rows; row++)
            {
                rowValue = 0;
                for (int column = 0; column < customCharacter.Columns; column++)
                {
                    if (pixelArray[row, column].IsActive)
                    {
                        // If active find out which pixel it is.  The 0th is the furthest right
                        // and has a value of 1.  'customCharacter.Columns - 1' is the furthest left
                        // and has a value of 2^(customCharacter.Columns -1).  Values in between are
                        // Exponential powers of 2 based on position. 
                        rowValue += (byte)getExponent(customCharacter.Columns - 1 - column);
                    }
                }
                // Convert to 2 digit hex. Build Code for this row of the character. 
                string temp = rowValue.ToString("X");
                if (temp.Length != 2)
                    characterString += String.Format("    0x0{0}u", rowValue.ToString("X"));
                else
                    characterString += String.Format("    0x{0}u", rowValue.ToString("X"));
                // No Comma after the last hex value
                if (row != customCharacter.Rows - 1)
                    characterString += ",";
            }
            return characterString;
        }

        /// <summary>
        /// This method takes a string with each char (byte) representing the locations of active 
        /// on a CyCustomCharacterArray. 
        /// </summary>
        /// <param name="customCharacter"> The instance of a character to be updated from CyDesigner data.</param>
        /// <param name="cyParameterString"> The string representing byte array from CyDesigner.</param>
        public void CyParameterToCustomCharacter(CyCustomCharacter customCharacter, string cyParameterString)
        {
            char[] chars = new char[customCharacter.Rows];
            int index = 0;

            // Remove last comma and seperate into indivudual strings. 
            string[] hexCharacterArray = cyParameterString.TrimEnd(',').Split(',');
            for (int i = 0; i < hexCharacterArray.Length; i++)
            {
                chars[index++] = (char)byte.Parse(hexCharacterArray[i], System.Globalization.NumberStyles.HexNumber);
            }

            CharLCD_I2C_v1_5.CyBox[,] boxes = customCharacter.GetBoxArray();

            for (int row = 0; row < customCharacter.Rows; row++)
            {
                for (int column = 0; column < customCharacter.Columns; column++)
                {
                    if ((((byte)chars[row]) & getExponent(customCharacter.Columns - 1 - column)) != 0)
                        boxes[row, column].IsActive = true;
                }
            }
        }

        // Return an 2 to the power of "power"  Helper Function.
        private int getExponent(int power)
        {
            if (power > 0)
                return 2 << power - 1;
            else
                return 1;
        }

        #endregion

        #region Hard Coded Bargraph Character Values
        void GenerateVerticalBargraph(Dictionary<string, string> dict)
        {
            // Character LCD_1_CUSTOM_0   
            dict.Add(CUST_CHAR_MACRONAME + "0", "0x00u,    0x00u,    0x00u,    0x00u,    0x00u,    0x00u,    0x00u,    0x1Fu");
            // Character LCD_1_CUSTOM_1   
            dict.Add(CUST_CHAR_MACRONAME + "1", "0x00u,    0x00u,    0x00u,    0x00u,    0x00u,    0x00u,    0x1Fu,    0x1Fu");
            // Character LCD_1_CUSTOM_2    
            dict.Add(CUST_CHAR_MACRONAME + "2", "0x00u,    0x00u,    0x00u,    0x00u,    0x00u,    0x1Fu,    0x1Fu,    0x1Fu");
            // Character LCD_1_CUSTOM_3   
            dict.Add(CUST_CHAR_MACRONAME + "3", "0x00u,    0x00u,    0x00u,    0x00u,    0x1Fu,    0x1Fu,    0x1Fu,    0x1Fu");
            // Character LCD_1_CUSTOM_4    
            dict.Add(CUST_CHAR_MACRONAME + "4", "0x00u,    0x00u,    0x00u,    0x1Fu,    0x1Fu,    0x1Fu,    0x1Fu,    0x1Fu");
            // Character LCD_1_CUSTOM_5    
            dict.Add(CUST_CHAR_MACRONAME + "5", "0x00u,    0x00u,    0x1Fu,    0x1Fu,    0x1Fu,    0x1Fu,    0x1Fu,    0x1Fu");
            // Character LCD_1_CUSTOM_6    
            dict.Add(CUST_CHAR_MACRONAME + "6", "0x00u,    0x1Fu,    0x1Fu,    0x1Fu,    0x1Fu,    0x1Fu,    0x1Fu,    0x1Fu");
            // Character LCD_1_CUSTOM_7    
            dict.Add(CUST_CHAR_MACRONAME + "7", "0x1Fu,    0x1Fu,    0x1Fu,    0x1Fu,    0x1Fu,    0x1Fu,    0x1Fu,    0x1Fu");
        }

        void GenerateHorizontalBargraph(Dictionary<string, string> dict)
        {
            dict.Add(CUST_CHAR_MACRONAME + "0", "0x00u,    0x00u,    0x00u,    0x00u,    0x00u,    0x00u,    0x00u,    0x00u");
            // Character LCD_1_CUSTOM_1     
            dict.Add(CUST_CHAR_MACRONAME + "1", "0x10u,    0x10u,    0x10u,    0x10u,    0x10u,    0x10u,    0x10u,    0x10u");
            // Character LCD_1_CUSTOM_2     
            dict.Add(CUST_CHAR_MACRONAME + "2", "0x18u,    0x18u,    0x18u,    0x18u,    0x18u,    0x18u,    0x18u,    0x18u");
            // Character LCD_1_CUSTOM_3     
            dict.Add(CUST_CHAR_MACRONAME + "3", "0x1Cu,    0x1Cu,    0x1Cu,    0x1Cu,    0x1Cu,    0x1Cu,    0x1Cu,    0x1Cu");
            // Character LCD_1_CUSTOM_4     
            dict.Add(CUST_CHAR_MACRONAME + "4", "0x1Eu,    0x1Eu,    0x1Eu,    0x1Eu,    0x1Eu,    0x1Eu,    0x1Eu,    0x1Eu");
            // Character LCD_1_CUSTOM_5     
            dict.Add(CUST_CHAR_MACRONAME + "5", "0x1Fu,    0x1Fu,    0x1Fu,    0x1Fu,    0x1Fu,    0x1Fu,    0x1Fu,    0x1Fu");
            // Character LCD_1_CUSTOM_6     
            dict.Add(CUST_CHAR_MACRONAME + "6", "0x00u,    0x00u,    0x00u,    0x00u,    0x00u,    0x00u,    0x00u,    0x00u");
            // Character LCD_1_CUSTOM_7     
            dict.Add(CUST_CHAR_MACRONAME + "7", "0x00u,    0x00u,    0x00u,    0x00u,    0x00u,    0x00u,    0x00u,    0x00u");
        }
        #endregion
    }
}
