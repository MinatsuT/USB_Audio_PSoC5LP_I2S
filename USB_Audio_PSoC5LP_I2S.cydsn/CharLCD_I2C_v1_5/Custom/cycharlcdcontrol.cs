/*******************************************************************************
* Copyright 2008-2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
********************************************************************************/

using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using CharLCD_I2C_v1_5;
using CyDesigner.Extensions.Gde;

namespace CharLCD_I2C_v1_5
{
    public partial class CyCharLCDControl : UserControl
    {

        // Object passed in by CyDesigner containing parameter values
        ICyInstEdit_v1 m_component;

        // CyDesigner Component Names
        private const string CR_PARAM = "ConversionRoutines";
        private const string CUSTOM_CHARACTER_SET_PARAM = "CustomCharacterSet";
        private const string CHARACTER_PARAM_NAME = "CUSTOM";

        private const int VERTICAL_CHARACTER_OFFSET = 2;
        private ArrayList m_cyCharacterStrings = new ArrayList();
        private bool m_internalChanges;

        public CyCharLCDControl()
        {
            InitializeComponent();
        }
        
        public CyCharLCDControl(ICyInstEdit_v1 component)
        {
            this.m_component = component;
            InitializeComponent();

            // Add Arrow Key Event Handling For customCharacterGroupBox
            customCharacterGroupBox.arrowPressedEvent +=
                new ArrowKeyPressEvent(customCharacterGroupBox_arrowPressedEvent);

            // Collections of each character set.
            m_userDefinedCharacters = new System.Collections.ArrayList();
            m_hbgCharacters = new System.Collections.ArrayList();
            m_vbgCharacters = new System.Collections.ArrayList();
            m_noneCharacters = new System.Collections.ArrayList();

            #region Populate Characters

            m_highlighted = this.character0;
            characterEditor.Match(m_highlighted);

            #region User Defined Characters

            m_userDefinedCharacters.Add(character0);
            m_userDefinedCharacters.Add(character1);
            m_userDefinedCharacters.Add(character2);
            m_userDefinedCharacters.Add(character3);
            m_userDefinedCharacters.Add(character4);
            m_userDefinedCharacters.Add(character5);
            m_userDefinedCharacters.Add(character6);
            m_userDefinedCharacters.Add(character7);
            #endregion

            LoadBarGraphs();
            CreateBarGraphs();

            #region Empty Characters for "None" character set
            m_noneCharacters.Add(customCharacter1);
            m_noneCharacters.Add(customCharacter2);
            m_noneCharacters.Add(customCharacter3);
            m_noneCharacters.Add(customCharacter4);
            m_noneCharacters.Add(customCharacter5);
            m_noneCharacters.Add(customCharacter6);
            m_noneCharacters.Add(customCharacter7);
            m_noneCharacters.Add(customCharacter8);
            #endregion
            #endregion
        }

        #region Generic Events (Button Clicks)
        // Event Handler when LCD Control is opened up.
        protected override void OnLoad(EventArgs e)
        {
            InitializeParameters();
            UnpackageCyStrings();
        }
                       
        // Event Handler for check box to include conversion routine.
        private void ConversionRoutines_Checked(object sender, EventArgs e)
        {
            if (m_internalChanges) return;

            CheckBox cb = (CheckBox)sender;
            if (cb.Checked)
            {
                m_component.SetParamExpr(CR_PARAM, "true");
                m_component.CommitParamExprs();
            }
            else
            {
                m_component.SetParamExpr(CR_PARAM, "false");
                m_component.CommitParamExprs();
            }
        }

        // Event handler for a change in character set selection.  <Horizontal, vertical Bargraphs and custom chars>
        private void CharacterSetSelection_Changed(object sender, EventArgs e)
        {
            RadioButton selectedButton = (RadioButton)sender;

            switch (selectedButton.Name)
            {
                case "noneRadioButton":
                    ShowCharacters(m_noneCharacters);
                    CustomCharactersOff();
                    HideCharacterEditor();
                    m_characterSet = CyCustomCharacterSetTypes.NONE;
                    currentEditableCharacter.Visible = false;
                    CharacterSetPostProcess();
                    break;
                case "vbgRadioButton":
                    ShowCharacters(m_vbgCharacters);
                    CustomCharactersOn();
                    HideCharacterEditor();
                    m_characterSet = CyCustomCharacterSetTypes.VERTICAL;
                    currentEditableCharacter.Visible = false;
                    CharacterSetPostProcess();
                    break;
                case "hbgRadioButton":
                    ShowCharacters(m_hbgCharacters);
                    CustomCharactersOn();
                    HideCharacterEditor();
                    m_characterSet = CyCustomCharacterSetTypes.HORIZONTAL;
                    currentEditableCharacter.Visible = false;
                    CharacterSetPostProcess();
                    break;
                case "udRadioButton":
                    CustomCharactersOn();
                    ShowCharacters(m_userDefinedCharacters);
                    m_characterSet = CyCustomCharacterSetTypes.USERDEFINED;
                    ShowCharacterEditor();
                    currentEditableCharacter.Text = m_highlighted.DisplayName;
                    currentEditableCharacter.Visible = true;
                    CharacterSetPostProcess();
                    break;
                default:
                    break;
            }
        }

        // Toggles for mouse down events.
        bool m_activate = false;

        #region Mouse Events on Custom Characters
        private void custArray_MouseDown(object sender, MouseEventArgs e)
        {
            CharLCD_I2C_v1_5.CyCustomCharacter current = (CharLCD_I2C_v1_5.CyCustomCharacter)sender;
            CharLCD_I2C_v1_5.CyBox clickedBox = current.GetBoxByLocation(e.X, e.Y);
            if (clickedBox != null)
            {
                clickedBox.IsActive = !clickedBox.IsActive;
                m_activate = clickedBox.IsActive;
                current.Invalidate();
            }
            this.m_highlighted.Invalidate();
        }

        private void custArray_MouseMove(object sender, MouseEventArgs e)
        {
            if ((e.Button == MouseButtons.Left) || (e.Button == MouseButtons.Right))
            {
                CharLCD_I2C_v1_5.CyCustomCharacter current = (CharLCD_I2C_v1_5.CyCustomCharacter)sender;
                CharLCD_I2C_v1_5.CyBox currentBox = current.GetBoxByLocation(e.X, e.Y);
                if (currentBox != null)
                {
                    currentBox.IsActive = m_activate;
                    current.Invalidate();
                }
            }
            this.m_highlighted.Invalidate();
        }

        private void custArray_MouseUp(object sender, MouseEventArgs e)
        {
            m_highlighted.Invalidate();
        }

        #endregion

        #region Events on Custom Characters to change editable character
        private void custArrayHighlight_Click(object sender, MouseEventArgs e)
        {
            CharLCD_I2C_v1_5.CyCustomCharacter selectedCharacter = (CharLCD_I2C_v1_5.CyCustomCharacter)sender;
            UpdateHighlighted(selectedCharacter);
        }

        private void customCharacterGroupBox_arrowPressedEvent(CyArrowArgs e)
        {
            switch (e.ArrowDirection)
            {
                case CyArrowArgs.CyDirection.LEFT:
                    HighlightPrevious();
                    break;
                case CyArrowArgs.CyDirection.RIGHT:
                    HighlightNext();
                    break;
                case CyArrowArgs.CyDirection.UP:
                    HighlightAbove();
                    break;
                case CyArrowArgs.CyDirection.DOWN:
                    HighlightBelow();
                    break;
                default:
                    break;
            }
        }
        #endregion
        
        #endregion
      
        #region Data Processing for the CyDesigner to Customizer Interface (Initialize, Post-Process, CheckErrors)

        public void InitializeParameters()
        {
            m_internalChanges = true;
            CyCompDevParam param;
            // Update Customizer controls to match parameters
            param = m_component.GetCommittedParam(CR_PARAM);
            if (param.ErrorCount == 0)
                crCheckBox.Checked = Convert.ToBoolean(param.Value);

            param = m_component.GetCommittedParam(CUSTOM_CHARACTER_SET_PARAM);
            if (param.ErrorCount == 0)
            {
                switch ((CyCustomCharacterSetTypes) byte.Parse(param.Value))
                {
                    case CyCustomCharacterSetTypes.HORIZONTAL:
                        hbgRadioButton.Checked = true;
                        break;
                    case CyCustomCharacterSetTypes.VERTICAL:
                        vbgRadioButton.Checked = true;
                        break;
                    case CyCustomCharacterSetTypes.USERDEFINED:
                        udRadioButton.Checked = true;
                        break;
                    default:
                        noneRadioButton.Checked = true;
                        break;
                }
            }

            m_internalChanges = false;
        }

        #region Custom Character Processing for CyDesigner Parameters
        // These Methods handle the requirements of cydesigner strings.  CyStrings are not arrays of bytes,
        // but are arrays of a comma seperated hex values. 

        // Set appropriate parameters based on character set selection.
        private void CharacterSetPostProcess()
        {
            if (m_internalChanges) return;

            m_component.SetParamExpr(CUSTOM_CHARACTER_SET_PARAM, GetEnumString(m_characterSet));
            m_component.CommitParamExprs();
        }

        // Calls CyStringToCharacter Foreach Character
        private void UnpackageCyStrings()
        {
            int stringIndex = 0;
            string value;

            for (int index = 0; index < 8; index++)
            {
                value = m_component.GetCommittedParam(CHARACTER_PARAM_NAME + index).Value.Trim('"');
                m_cyCharacterStrings.Add(new KeyValuePair<string, string>(CHARACTER_PARAM_NAME + index, value));
            }

            //if (m_userDefinedCharacters.Count != m_cyCharacterStrings.Count)
            //    throw new Exception(
            //       "Number of custom characters in CyDesigner does not match number supported by the the LCD Wizard");

            for (int i = 0; i < m_userDefinedCharacters.Count; i++)
            {
                CyCustomCharacter character = (CyCustomCharacter)m_userDefinedCharacters[i];
                KeyValuePair<string, string> kvp = (KeyValuePair<string, string>)m_cyCharacterStrings[stringIndex++];
                string cyCharacterString = kvp.Value;

                if (cyCharacterString != null)
                    CYStringToCharacter(character, cyCharacterString);
            }
        }

        // Convert userDefined Characters to CyStrings
        private void ConvertUserDefinedCharactersToCY()
        {
            int stringIndex = 0;

            for (int i = 0; i < m_userDefinedCharacters.Count; i++)
            {
                CyCustomCharacter character = (CyCustomCharacter)m_userDefinedCharacters[i];
                KeyValuePair<string, string> kvp = (KeyValuePair<string, string>)m_cyCharacterStrings[stringIndex++];
                string cyStringName = kvp.Key;
                string cyStringValue = "\"" + CharacterToCYString(character) + "\"";
                m_component.SetParamExpr(cyStringName, cyStringValue);
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

        #region CY string parameter methods
        /// <summary>
        /// This method takes a string with each char (byte) representing the locations of active 
        /// on a CyCustomCharacterArray. 
        /// </summary>
        /// <param name="customCharacter"> The instance of a character to be updated from CyDesigner data.</param>
        /// <param name="cyCharacterString"> The string representing byte array from CyDesigner.</param>
        public void CYStringToCharacter(CyCustomCharacter customCharacter, string cyCharacterString)
        {
            char[] chars = new char[customCharacter.Rows];
            int index = 0;

            // Remove last comma and seperate into indivudual strings. 
            string[] hexCharacterArray = cyCharacterString.TrimEnd(',').Split(',');
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

        /// <summary>
        /// This method takes a user defined custom character and converts the locations of active 
        /// pixels to an array of bytes, cast into a string to be stored on the LCD component object in CyDesigner.
        /// </summary>
        /// <param name="customCharacter"> The string representing byte array from CyDesigner.</param>
        public string CharacterToCYString(CyCustomCharacter customCharacter)
        {
            // Comma Seperated String of Hex Values
            string cyCharacterString = "";
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
                cyCharacterString += rowValue.ToString("X") + ",";
            }
            // Convert Char arry to string and return it.
            return cyCharacterString;
        }
        #endregion

        private void custArray_Leave(object sender, EventArgs e)
        {
            ConvertUserDefinedCharactersToCY();
            m_component.CommitParamExprs();
        }

        #endregion

        #region
        /// <summary>
        ///  Populate the appropriate arraylists with the bargraph characters.
        /// </summary>
        private void LoadBarGraphs()
        {
            #region Horizontal Bar Graph Characters
            m_hbgCharacters.Add(hbg0);
            m_hbgCharacters.Add(hbg1);
            m_hbgCharacters.Add(hbg2);
            m_hbgCharacters.Add(hbg3);
            m_hbgCharacters.Add(hbg4);
            m_hbgCharacters.Add(hbg5);
            m_hbgCharacters.Add(hbg6);
            m_hbgCharacters.Add(hbg7);
            #endregion

            #region Vertical Bargraph Characters
            m_vbgCharacters.Add(vbg0);
            m_vbgCharacters.Add(vbg1);
            m_vbgCharacters.Add(vbg2);
            m_vbgCharacters.Add(vbg3);
            m_vbgCharacters.Add(vbg4);
            m_vbgCharacters.Add(vbg5);
            m_vbgCharacters.Add(vbg6);
            m_vbgCharacters.Add(vbg7);
            #endregion
        }

        #region Bar Graph Generation and Custom Character arraylist definitions/declarations

        // Character Sets
        private System.Collections.ArrayList m_userDefinedCharacters;
        private System.Collections.ArrayList m_hbgCharacters;
        private System.Collections.ArrayList m_vbgCharacters;
        private System.Collections.ArrayList m_noneCharacters;

        private CyCustomCharacterSetTypes m_characterSet;
        // Pointer to the selected character.
        private CyCustomCharacter m_highlighted;

        
         // Create bar graphs activates the appropriate pixels on two bargraph 
         // Sets, horizontal and vertical: May need adjustments in future.
        
        private void CreateBarGraphs()
        {
            #region Horizontal Bargraph
            int largestColumn = 0;

            // Loop returns items in the order they were added to the ArrayList. 
            for (int i = 0; i < m_hbgCharacters.Count; i++)
            {
                CyCustomCharacter character = (CyCustomCharacter)m_hbgCharacters[i];
                for (int index = 0; index < largestColumn; index++)
                {
                    character.SetColumn(index);
                }

                largestColumn++;

                if (largestColumn > character.Columns)
                    break;
            }
            #endregion

            #region Vertical Bargraph
            // Does not leave an empty character in the Custom Fonts
            int lastRow = 7;

            for (int i = 0; i < m_vbgCharacters.Count; i++)
            {
                CyCustomCharacter character = (CyCustomCharacter)m_vbgCharacters[i];
                for (int index = 7; index >= lastRow; index--)
                {
                    character.SetRow(index);
                }
                lastRow--;
            }
            #endregion
        }
        #endregion

        #region Custom Character Highlighting Helper Methods

        private void HighlightNext()
        {
            // Find next.
            int next = m_userDefinedCharacters.IndexOf(m_highlighted) + 1;
            next = next % m_userDefinedCharacters.Count;
            // Cast from Object to CustomCharacter
            UpdateHighlighted((CharLCD_I2C_v1_5.CyCustomCharacter)m_userDefinedCharacters[next]);
        }

        private void HighlightPrevious()
        {
            int previous = m_userDefinedCharacters.IndexOf(m_highlighted) - 1;
            // Protects against '-1'.  if current is 7 previous should be 6 : 
            //7-1 = 6.  6+8 =14. 14%8 = 6.
            previous = (previous + m_userDefinedCharacters.Count) % m_userDefinedCharacters.Count;
            // Cast from Object to CustomCharacter
            UpdateHighlighted((CharLCD_I2C_v1_5.CyCustomCharacter)m_userDefinedCharacters[previous]);
        }

        private void HighlightAbove()
        {
            int above = m_userDefinedCharacters.IndexOf(m_highlighted) - VERTICAL_CHARACTER_OFFSET;
            // Protects against '-1' or '-2'.  if current is 7 above should be 5 : 
            // 7-2 = 5.  5+8 =13. 13%8 = 5. 
            // If current is '1' above should be 7.  1 -2 = -1.  -1 + 8 % 8 = 7.    
            above = (above + m_userDefinedCharacters.Count) % m_userDefinedCharacters.Count;
            // Cast from Object to CustomCharacter
            UpdateHighlighted((CharLCD_I2C_v1_5.CyCustomCharacter)m_userDefinedCharacters[above]);
        }
        private void HighlightBelow()
        {
            int below = m_userDefinedCharacters.IndexOf(m_highlighted) + VERTICAL_CHARACTER_OFFSET;
            // Valid range is from zero to number of characters.
            below = (below + m_userDefinedCharacters.Count) % m_userDefinedCharacters.Count;
            // Cast from Object to CustomCharacter
            UpdateHighlighted((CharLCD_I2C_v1_5.CyCustomCharacter)m_userDefinedCharacters[below]);
        }

        private void UpdateHighlighted(CyCustomCharacter selectedCharacter)
        {
            m_highlighted.Match(characterEditor);
            m_highlighted.Selected = false;
            m_highlighted.Invalidate();
            m_highlighted = selectedCharacter;
            m_highlighted.Selected = true;
            m_highlighted.Invalidate();
            currentEditableCharacter.Text = m_highlighted.DisplayName;
            this.characterEditor.Match(selectedCharacter);
            this.characterEditor.Invalidate();
        }
        #endregion

        #region Show and Hide methods for Character Sets
        private void ShowCharacters(System.Collections.ArrayList characters)
        {
            for (int i = 0; i < this.panel1.Controls.Count; i++)
            {
                this.panel1.Controls[i].Visible = false;
            }

            for (int i = 0; i < characters.Count; i++)
            {
                ((Control)characters[i]).Visible = true;
            }

        }

        // Greys out the character editor and prevents user interaction with it
        // by making actual editor invisible, and replacing with an empty one.
        private void HideCharacterEditor()
        {
            characterEditor.Visible = false;
            emptyCharacterEditor.Visible = true;
            emptyCharacterEditor.ActiveBrush = new SolidBrush(Color.LightGray);
            emptyCharacterEditor.InactiveBrush = new SolidBrush(Color.WhiteSmoke);
            emptyCharacterEditor.BorderBrush = new SolidBrush(Color.LightGray);
            emptyCharacterEditor.Invalidate();
        }

        // Activates the character editor for user defined characters and hides 
        // the empty place holder editor, "emptyCharacterEditor"
        private void ShowCharacterEditor()
        {
            emptyCharacterEditor.Visible = false;
            characterEditor.Visible = true;
        }

        private void CustomCharactersOff()
        {
            m_highlighted.Selected = false;
            for (int i = 0; i < this.panel1.Controls.Count; i++)
            {
                CharLCD_I2C_v1_5.CyCustomCharacter character = (CharLCD_I2C_v1_5.CyCustomCharacter)this.panel1.Controls[i];
                if (character.Visible == true)
                {
                    character.ActiveBrush = new SolidBrush(Color.LightGray);
                    character.InactiveBrush = new SolidBrush(Color.WhiteSmoke);
                    character.BorderBrush = new SolidBrush(Color.LightGray);
                    character.Invalidate();
                }
            }
            customCharacterGroupBox.Enabled = false;
        }

        private void CustomCharactersOn()
        {
            customCharacterGroupBox.Enabled = true;

            m_highlighted.Selected = true;
            for (int i = 0; i < this.panel1.Controls.Count; i++)
            {
                CharLCD_I2C_v1_5.CyCustomCharacter character = (CharLCD_I2C_v1_5.CyCustomCharacter)this.panel1.Controls[i];
                character.BorderBrush = new SolidBrush(Color.LightGray);
                character.ActiveBrush = new SolidBrush(Color.Black);
                character.InactiveBrush = new SolidBrush(Color.White);
                character.Invalidate();
            }
        }
        #endregion

        #endregion    

        public static string GetEnumString(object value)
        {
            String res = String.Empty;
            Type enType = value.GetType();
            FieldInfo field = enType.GetField(Enum.GetName(enType, value));
            DescriptionAttribute descAttr = (DescriptionAttribute) Attribute.GetCustomAttribute(
                                                                       field, typeof (DescriptionAttribute));
            if (descAttr != null)
                res = descAttr.Description;
            else
                res = value.ToString();
            return res;
        }
    }

    enum CyCustomCharacterSetTypes : byte
    {
        [Description("None")] NONE, 
        [Description("Horizontal")] HORIZONTAL,
        [Description("Vertical")] VERTICAL,
        [Description("UserDefined")] USERDEFINED
    }
  
    #region Arrow Key Press Group Box Code
    public delegate void ArrowKeyPressEvent(CyArrowArgs e);

    public class CyCustomCharacterGroupBox : System.Windows.Forms.GroupBox
    {
        public event ArrowKeyPressEvent arrowPressedEvent;

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            const int WM_KEYDOWN = 0x100;
            const int WM_SYSKEYDOWN = 0x104;

            if ((msg.Msg == WM_KEYDOWN) || (msg.Msg == WM_SYSKEYDOWN))
            {
                switch (keyData)
                {
                    case Keys.Left:
                        RaiseArrowEvent(CyArrowArgs.CyDirection.LEFT);
                        break;

                    case Keys.Right:
                        RaiseArrowEvent(CyArrowArgs.CyDirection.RIGHT);
                        break;
                    case Keys.Up:
                        RaiseArrowEvent(CyArrowArgs.CyDirection.UP);
                        break;
                    case Keys.Down:
                        RaiseArrowEvent(CyArrowArgs.CyDirection.DOWN);
                        break;
                    default:
                        break;
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void RaiseArrowEvent(CyArrowArgs.CyDirection keyDirection)
        {
            if (arrowPressedEvent != null)
                arrowPressedEvent(new CyArrowArgs(keyDirection));
        }
    }

    public class CyArrowArgs : EventArgs
    {
        public enum CyDirection { LEFT, RIGHT, UP, DOWN };
        private CyDirection m_arrowDirection;

        public CyDirection ArrowDirection
        {
            get { return m_arrowDirection; }
            set { m_arrowDirection = value; }
        }

        public CyArrowArgs(CyDirection keyDirection)
        {
            this.m_arrowDirection = keyDirection;
        }
    }
    #endregion

}
