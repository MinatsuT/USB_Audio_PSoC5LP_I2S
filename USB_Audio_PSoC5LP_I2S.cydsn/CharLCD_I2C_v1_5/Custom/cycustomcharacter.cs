/*******************************************************************************
* Copyright 2008-2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
********************************************************************************/

using System;
using System.Drawing;
using System.Windows.Forms;

namespace CharLCD_I2C_v1_5
{
    public partial class CyCustomCharacter : UserControl
    {
        public CyBox[,] m_box;
        private int m_boxWidth;
        private int m_boxHeight;
        private const int HIGHLIGHT_BORDER_WIDTH = 1;

        #region Properties: Brushes, Columns, Rows, Name
        private bool m_selected = false;

        SolidBrush m_borderBrush = new SolidBrush(Color.LightGray);
        SolidBrush m_activeBrush = new SolidBrush(Color.Black);
        SolidBrush m_inactiveBrush = new SolidBrush(Color.White);
        
        private int m_borderWidth = 1;
        private int m_columns = 5;
        private int m_rows = 8;

        private string m_displayName = "Custom Character";

        public SolidBrush BorderBrush
        {
            get { return m_borderBrush; }
            set { m_borderBrush = value; }
        }
        public SolidBrush ActiveBrush
        {
            get { return m_activeBrush; }
            set { m_activeBrush = value; }
        }
        public SolidBrush InactiveBrush
        {
            get { return m_inactiveBrush; }
            set { m_inactiveBrush = value; }
        }
        public bool Selected
        {
            get { return m_selected; }
            set { m_selected = value; }
        }
        public int BorderWidth
        {
            get { return m_borderWidth; }
            set 
            { 
                if (value < this.Size.Height & value < this.Size.Width)
                    m_borderWidth = value;
            }
        }
        public int Columns
        {
            get { return m_columns; }
            set 
            { 
                m_columns = value;
                CheckBoxArray();
            }
        }
        public int Rows
        {
            get { return m_rows; }
            set 
            {
                m_rows = value;
                CheckBoxArray();
            }
        }
        public string DisplayName
        {
            get { return m_displayName;}
            set { m_displayName = value;}
        }
        #endregion

        public CyCustomCharacter() 
        {
            InitializeComponent();
            CheckBoxArray();
        }
        public override string ToString()
        {
            return this.Name;
        }

        #region Appearance.  Box size. Colors. OnPaint method.
        // Before runtime, if the number of columns or rows is changed, update.   
        // Causes loss of box state data.
        private void CheckBoxArray()
        {
            CheckBoxSize();
            m_box = new CyBox[m_rows, m_columns];
            for (int row = 0; row < m_rows; row++)
            {
                for (int column = 0; column < m_columns; column++)
                {
                    m_box[row, column] = new CyBox(row, column);
                }
            }
        }

        // Recalculate box width and box height.    
        public void CheckBoxSize()
        {
            m_boxWidth = (Size.Width - m_borderWidth) / m_columns;
            m_boxHeight = (Size.Height - m_borderWidth) / m_rows;
        }

        // Paint the boxes based on state
        protected override void OnPaint(PaintEventArgs e)
        {

            // Update Borders: Border Widths : Selection
            Graphics graphics = e.Graphics;
            // Draw Right and Bottom Border
            
                     
            for (int row = 0; row < m_rows; row++)
            {
                for (int column = 0; column < m_columns; column++)
                {
                    if (m_box[row,column].IsActive)
                    {
                        graphics.FillRectangle(m_borderBrush, column * m_boxWidth, row * m_boxHeight, m_boxWidth,
                                               m_boxHeight);
                        graphics.FillRectangle(m_activeBrush, column * m_boxWidth + m_borderWidth,
                                               row*m_boxHeight + m_borderWidth, m_boxWidth - m_borderWidth,
                                               m_boxHeight - m_borderWidth);
                    }
                    else
                    {
                        // Draw box which forms top and left border
                        graphics.FillRectangle(m_borderBrush, column * m_boxWidth, row * m_boxHeight, m_boxWidth,
                                               m_boxHeight);
                        // Draw standard box over border box so they overlap
                        graphics.FillRectangle(m_inactiveBrush, column * m_boxWidth + m_borderWidth,
                                               row*m_boxHeight + m_borderWidth, m_boxWidth - m_borderWidth,
                                               m_boxHeight - m_borderWidth);
                    }

                    using (Pen borderPen = new Pen(m_activeBrush.Color, m_borderWidth))
                    {
                        // Alignment == Inset instead of Center
                        borderPen.Alignment = System.Drawing.Drawing2D.PenAlignment.Inset;
                        graphics.DrawRectangle(borderPen, 0, 0, m_columns*m_boxWidth + m_borderWidth - 1,
                                               m_rows*m_boxHeight + m_borderWidth - 1);
                    }
                    if (m_selected)
                    {
                        using (Pen pen = new Pen(Color.Blue, HIGHLIGHT_BORDER_WIDTH))
                        {
                            // Alignment == Inset instead of Center
                            pen.Alignment = System.Drawing.Drawing2D.PenAlignment.Inset;
                            graphics.DrawRectangle(pen, 0, 0, m_columns*m_boxWidth + m_borderWidth - 1,
                                                   m_rows*m_boxHeight + m_borderWidth - 1);
                        }
                        using (Pen pen2 = new Pen(Color.DodgerBlue, HIGHLIGHT_BORDER_WIDTH))
                        {
                            pen2.Alignment = System.Drawing.Drawing2D.PenAlignment.Inset;
                            graphics.DrawRectangle(pen2, HIGHLIGHT_BORDER_WIDTH, HIGHLIGHT_BORDER_WIDTH,
                                                   m_columns*m_boxWidth + m_borderWidth - 2*HIGHLIGHT_BORDER_WIDTH - 1,
                                                   m_rows*m_boxHeight + m_borderWidth - 2*HIGHLIGHT_BORDER_WIDTH - 1);
                        }
                    }
                }
            }
        }

        // Event handler for a change in control size
        private void ArraySizeChanged(object sender, EventArgs e)
        {
            CheckBoxSize();
        }
        #endregion

        #region Box Manipulation
        /// <summary>
        /// Given a mouse location on the control GetBoxByLocation calculates which 
        /// row and column the box is and returns a reference to that box.
        /// 
        /// Returns null for invalid values.
        /// </summary>
        /// <param name="x"> X coordinate of mouse click</param>
        /// <param name="y"> Y coordinate of mouse click</param>
        /// <returns> A Box object if a valid location (inside the control) is passed in.  
        /// Otherwise it returns null.</returns>
        public CyBox GetBoxByLocation(int x, int y)
        {
                
            int pixPerRow = (Size.Height - m_borderWidth) / m_rows;
            int row = y / pixPerRow;
            int pixPerCol = (Size.Width - m_borderWidth) / m_columns;
            int column = x / pixPerCol;
            if (row >= 0 && row < m_rows && column >=0 && column < m_columns)
                return m_box[row, column];
            else
                return null;
        }

        /// <summary>
        /// GetBoxArray returns the 2-Dimensional array of boxes ("cells", "pixels", etc) to 
        /// allow the user to process the meaning of the states.
        /// </summary>
        /// <returns> A 2-D array of type Box </returns>
        public CyBox[,] GetBoxArray()
        {
            return this.m_box;
        }

        /// <summary>
        /// Match accepts a CustomCharacter as an input and matches the current CustomCharacter 
        /// to the the pixel set of the input CustomCharacter
        /// </summary>
        /// <param name="character"> a CustomCharacter object for the current CustomCharacter to copy.</param>
        public void Match(CharLCD_I2C_v1_5.CyCustomCharacter character)
        {
            this.m_box = character.GetBoxArray();
        }

        /// <summary>
        /// SetRow takes an input argument between zero and the number of rows and sets every cell 
        /// on that row to be active.
        /// </summary>
        /// <param name="row"></param>
        public void SetRow(int row)
        {
            for (int i = 0; i < m_columns; i++)
            {
                m_box[row, i].IsActive = true;
            }
        }

        /// <summary>
        /// SetColumn takes an input argument between zero and the number of columns and sets every cell 
        /// on that column to be active.
        /// </summary>
        /// <param name="column"></param>
        public void SetColumn(int column)
        {
            for (int i = 0; i < m_rows; i++)
            {
                m_box[i, column].IsActive = true;
            }
        }
        #endregion
    }

    public class CyBox
    {
        #region Variables and Properties
        // Properties
        private readonly int m_row;
        private readonly int m_column;
        private bool m_isActive = false;
        public bool IsActive
        {
            get { return m_isActive; }
            set { m_isActive = value; }
        }
        #endregion

        public CyBox(int row, int column)
        {
            this.m_row = row;
            this.m_column = column;
        }
    }
}
