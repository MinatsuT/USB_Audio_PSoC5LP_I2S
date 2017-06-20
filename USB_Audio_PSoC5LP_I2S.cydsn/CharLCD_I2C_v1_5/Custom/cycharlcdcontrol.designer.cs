/*******************************************************************************
* Copyright 2008-2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
********************************************************************************/

namespace CharLCD_I2C_v1_5
{
    partial class CyCharLCDControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.crCheckBox = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.vbgRadioButton = new System.Windows.Forms.RadioButton();
            this.hbgRadioButton = new System.Windows.Forms.RadioButton();
            this.udRadioButton = new System.Windows.Forms.RadioButton();
            this.noneRadioButton = new System.Windows.Forms.RadioButton();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.customCharacterGroupBox = new CharLCD_I2C_v1_5.CyCustomCharacterGroupBox();
            this.currentEditableCharacter = new System.Windows.Forms.Label();
            this.characterEditor = new CharLCD_I2C_v1_5.CyCustomCharacter();
            this.panel1 = new System.Windows.Forms.Panel();
            this.character1 = new CharLCD_I2C_v1_5.CyCustomCharacter();
            this.character6 = new CharLCD_I2C_v1_5.CyCustomCharacter();
            this.character0 = new CharLCD_I2C_v1_5.CyCustomCharacter();
            this.customCharacter2 = new CharLCD_I2C_v1_5.CyCustomCharacter();
            this.hbg1 = new CharLCD_I2C_v1_5.CyCustomCharacter();
            this.character4 = new CharLCD_I2C_v1_5.CyCustomCharacter();
            this.customCharacter7 = new CharLCD_I2C_v1_5.CyCustomCharacter();
            this.vbg1 = new CharLCD_I2C_v1_5.CyCustomCharacter();
            this.hbg0 = new CharLCD_I2C_v1_5.CyCustomCharacter();
            this.hbg6 = new CharLCD_I2C_v1_5.CyCustomCharacter();
            this.vbg6 = new CharLCD_I2C_v1_5.CyCustomCharacter();
            this.customCharacter4 = new CharLCD_I2C_v1_5.CyCustomCharacter();
            this.hbg2 = new CharLCD_I2C_v1_5.CyCustomCharacter();
            this.vbg0 = new CharLCD_I2C_v1_5.CyCustomCharacter();
            this.customCharacter3 = new CharLCD_I2C_v1_5.CyCustomCharacter();
            this.hbg4 = new CharLCD_I2C_v1_5.CyCustomCharacter();
            this.customCharacter8 = new CharLCD_I2C_v1_5.CyCustomCharacter();
            this.customCharacter6 = new CharLCD_I2C_v1_5.CyCustomCharacter();
            this.vbg2 = new CharLCD_I2C_v1_5.CyCustomCharacter();
            this.character2 = new CharLCD_I2C_v1_5.CyCustomCharacter();
            this.vbg4 = new CharLCD_I2C_v1_5.CyCustomCharacter();
            this.customCharacter5 = new CharLCD_I2C_v1_5.CyCustomCharacter();
            this.hbg3 = new CharLCD_I2C_v1_5.CyCustomCharacter();
            this.vbg3 = new CharLCD_I2C_v1_5.CyCustomCharacter();
            this.character3 = new CharLCD_I2C_v1_5.CyCustomCharacter();
            this.hbg5 = new CharLCD_I2C_v1_5.CyCustomCharacter();
            this.vbg5 = new CharLCD_I2C_v1_5.CyCustomCharacter();
            this.character5 = new CharLCD_I2C_v1_5.CyCustomCharacter();
            this.customCharacter1 = new CharLCD_I2C_v1_5.CyCustomCharacter();
            this.vbg7 = new CharLCD_I2C_v1_5.CyCustomCharacter();
            this.hbg7 = new CharLCD_I2C_v1_5.CyCustomCharacter();
            this.character7 = new CharLCD_I2C_v1_5.CyCustomCharacter();
            this.emptyCharacterEditor = new CharLCD_I2C_v1_5.CyCustomCharacter();
            this.groupBox4.SuspendLayout();
            this.customCharacterGroupBox.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // crCheckBox
            // 
            this.crCheckBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F);
            this.crCheckBox.Location = new System.Drawing.Point(12, 174);
            this.crCheckBox.Name = "crCheckBox";
            this.crCheckBox.Size = new System.Drawing.Size(179, 39);
            this.crCheckBox.TabIndex = 3;
            this.crCheckBox.Text = "Include ASCII to Number Conversion Routines";
            this.crCheckBox.UseVisualStyleBackColor = true;
            this.crCheckBox.CheckedChanged += new System.EventHandler(this.ConversionRoutines_Checked);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F);
            this.label7.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label7.Location = new System.Drawing.Point(6, 29);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(177, 17);
            this.label7.TabIndex = 99;
            this.label7.Text = "LCD Custom Character Set";
            // 
            // vbgRadioButton
            // 
            this.vbgRadioButton.AutoSize = true;
            this.vbgRadioButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.vbgRadioButton.Location = new System.Drawing.Point(40, 76);
            this.vbgRadioButton.Name = "vbgRadioButton";
            this.vbgRadioButton.Size = new System.Drawing.Size(136, 21);
            this.vbgRadioButton.TabIndex = 1;
            this.vbgRadioButton.TabStop = true;
            this.vbgRadioButton.Text = "Vertical Bargraph";
            this.vbgRadioButton.UseVisualStyleBackColor = true;
            this.vbgRadioButton.CheckedChanged += new System.EventHandler(this.CharacterSetSelection_Changed);
            // 
            // hbgRadioButton
            // 
            this.hbgRadioButton.AutoSize = true;
            this.hbgRadioButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hbgRadioButton.Location = new System.Drawing.Point(40, 103);
            this.hbgRadioButton.Name = "hbgRadioButton";
            this.hbgRadioButton.Size = new System.Drawing.Size(153, 21);
            this.hbgRadioButton.TabIndex = 2;
            this.hbgRadioButton.TabStop = true;
            this.hbgRadioButton.Text = "Horizontal Bargraph";
            this.hbgRadioButton.UseVisualStyleBackColor = true;
            this.hbgRadioButton.CheckedChanged += new System.EventHandler(this.CharacterSetSelection_Changed);
            // 
            // udRadioButton
            // 
            this.udRadioButton.AutoSize = true;
            this.udRadioButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.udRadioButton.Location = new System.Drawing.Point(40, 130);
            this.udRadioButton.Name = "udRadioButton";
            this.udRadioButton.Size = new System.Drawing.Size(109, 21);
            this.udRadioButton.TabIndex = 3;
            this.udRadioButton.TabStop = true;
            this.udRadioButton.Text = "User Defined";
            this.udRadioButton.UseVisualStyleBackColor = true;
            this.udRadioButton.CheckedChanged += new System.EventHandler(this.CharacterSetSelection_Changed);
            // 
            // noneRadioButton
            // 
            this.noneRadioButton.AutoSize = true;
            this.noneRadioButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.noneRadioButton.Location = new System.Drawing.Point(40, 49);
            this.noneRadioButton.Name = "noneRadioButton";
            this.noneRadioButton.Size = new System.Drawing.Size(60, 21);
            this.noneRadioButton.TabIndex = 0;
            this.noneRadioButton.TabStop = true;
            this.noneRadioButton.Text = "None";
            this.noneRadioButton.UseVisualStyleBackColor = true;
            this.noneRadioButton.CheckedChanged += new System.EventHandler(this.CharacterSetSelection_Changed);
            // 
            // groupBox4
            // 
            this.groupBox4.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBox4.Controls.Add(this.noneRadioButton);
            this.groupBox4.Controls.Add(this.udRadioButton);
            this.groupBox4.Controls.Add(this.hbgRadioButton);
            this.groupBox4.Controls.Add(this.vbgRadioButton);
            this.groupBox4.Controls.Add(this.label7);
            this.groupBox4.Controls.Add(this.crCheckBox);
            this.groupBox4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox4.ForeColor = System.Drawing.SystemColors.ControlText;
            this.groupBox4.Location = new System.Drawing.Point(7, 4);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(228, 233);
            this.groupBox4.TabIndex = 99;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Parameters";
            // 
            // customCharacterGroupBox
            // 
            this.customCharacterGroupBox.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.customCharacterGroupBox.Controls.Add(this.currentEditableCharacter);
            this.customCharacterGroupBox.Controls.Add(this.characterEditor);
            this.customCharacterGroupBox.Controls.Add(this.panel1);
            this.customCharacterGroupBox.Controls.Add(this.emptyCharacterEditor);
            this.customCharacterGroupBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.customCharacterGroupBox.ForeColor = System.Drawing.SystemColors.ControlText;
            this.customCharacterGroupBox.Location = new System.Drawing.Point(241, 4);
            this.customCharacterGroupBox.Name = "customCharacterGroupBox";
            this.customCharacterGroupBox.Size = new System.Drawing.Size(220, 233);
            this.customCharacterGroupBox.TabIndex = 99;
            this.customCharacterGroupBox.TabStop = false;
            this.customCharacterGroupBox.Text = "Custom Character Editor";
            this.customCharacterGroupBox.Leave += new System.EventHandler(this.custArray_Leave);
            // 
            // currentEditableCharacter
            // 
            this.currentEditableCharacter.AutoSize = true;
            this.currentEditableCharacter.Location = new System.Drawing.Point(47, 199);
            this.currentEditableCharacter.Name = "currentEditableCharacter";
            this.currentEditableCharacter.Size = new System.Drawing.Size(51, 13);
            this.currentEditableCharacter.TabIndex = 99;
            this.currentEditableCharacter.Text = "Custom 0";
            this.currentEditableCharacter.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.currentEditableCharacter.Visible = false;
            // 
            // characterEditor
            // 
            this.characterEditor.BorderWidth = 1;
            this.characterEditor.Columns = 5;
            this.characterEditor.DisplayName = "Custom Character";
            this.characterEditor.Location = new System.Drawing.Point(20, 35);
            this.characterEditor.Margin = new System.Windows.Forms.Padding(4);
            this.characterEditor.MinimumSize = new System.Drawing.Size(5, 5);
            this.characterEditor.Name = "characterEditor";
            this.characterEditor.Rows = 8;
            this.characterEditor.Selected = false;
            this.characterEditor.Size = new System.Drawing.Size(101, 161);
            this.characterEditor.TabIndex = 99;
            this.characterEditor.TabStop = false;
            this.characterEditor.MouseMove += new System.Windows.Forms.MouseEventHandler(this.custArray_MouseMove);
            this.characterEditor.MouseDown += new System.Windows.Forms.MouseEventHandler(this.custArray_MouseDown);
            this.characterEditor.MouseUp += new System.Windows.Forms.MouseEventHandler(this.custArray_MouseUp);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.character1);
            this.panel1.Controls.Add(this.character6);
            this.panel1.Controls.Add(this.character0);
            this.panel1.Controls.Add(this.customCharacter2);
            this.panel1.Controls.Add(this.hbg1);
            this.panel1.Controls.Add(this.character4);
            this.panel1.Controls.Add(this.customCharacter7);
            this.panel1.Controls.Add(this.vbg1);
            this.panel1.Controls.Add(this.hbg0);
            this.panel1.Controls.Add(this.hbg6);
            this.panel1.Controls.Add(this.vbg6);
            this.panel1.Controls.Add(this.customCharacter4);
            this.panel1.Controls.Add(this.hbg2);
            this.panel1.Controls.Add(this.vbg0);
            this.panel1.Controls.Add(this.customCharacter3);
            this.panel1.Controls.Add(this.hbg4);
            this.panel1.Controls.Add(this.customCharacter8);
            this.panel1.Controls.Add(this.customCharacter6);
            this.panel1.Controls.Add(this.vbg2);
            this.panel1.Controls.Add(this.character2);
            this.panel1.Controls.Add(this.vbg4);
            this.panel1.Controls.Add(this.customCharacter5);
            this.panel1.Controls.Add(this.hbg3);
            this.panel1.Controls.Add(this.vbg3);
            this.panel1.Controls.Add(this.character3);
            this.panel1.Controls.Add(this.hbg5);
            this.panel1.Controls.Add(this.vbg5);
            this.panel1.Controls.Add(this.character5);
            this.panel1.Controls.Add(this.customCharacter1);
            this.panel1.Controls.Add(this.vbg7);
            this.panel1.Controls.Add(this.hbg7);
            this.panel1.Controls.Add(this.character7);
            this.panel1.Location = new System.Drawing.Point(124, 22);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(92, 189);
            this.panel1.TabIndex = 99;
            // 
            // character1
            // 
            this.character1.BorderWidth = 0;
            this.character1.Columns = 5;
            this.character1.DisplayName = "Custom 1";
            this.character1.Location = new System.Drawing.Point(53, 15);
            this.character1.Margin = new System.Windows.Forms.Padding(18);
            this.character1.MinimumSize = new System.Drawing.Size(5, 5);
            this.character1.Name = "character1";
            this.character1.Rows = 8;
            this.character1.Selected = false;
            this.character1.Size = new System.Drawing.Size(20, 32);
            this.character1.TabIndex = 99;
            this.character1.TabStop = false;
            this.character1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.custArrayHighlight_Click);
            // 
            // character6
            // 
            this.character6.BorderWidth = 0;
            this.character6.Columns = 5;
            this.character6.DisplayName = "Custom 6";
            this.character6.Location = new System.Drawing.Point(15, 137);
            this.character6.Margin = new System.Windows.Forms.Padding(18);
            this.character6.MinimumSize = new System.Drawing.Size(5, 5);
            this.character6.Name = "character6";
            this.character6.Rows = 8;
            this.character6.Selected = false;
            this.character6.Size = new System.Drawing.Size(20, 32);
            this.character6.TabIndex = 99;
            this.character6.TabStop = false;
            this.character6.MouseClick += new System.Windows.Forms.MouseEventHandler(this.custArrayHighlight_Click);
            // 
            // character0
            // 
            this.character0.BorderWidth = 0;
            this.character0.Columns = 5;
            this.character0.DisplayName = "Custom 0";
            this.character0.Location = new System.Drawing.Point(15, 15);
            this.character0.Margin = new System.Windows.Forms.Padding(18);
            this.character0.MinimumSize = new System.Drawing.Size(5, 5);
            this.character0.Name = "character0";
            this.character0.Rows = 8;
            this.character0.Selected = true;
            this.character0.Size = new System.Drawing.Size(20, 32);
            this.character0.TabIndex = 99;
            this.character0.TabStop = false;
            this.character0.MouseClick += new System.Windows.Forms.MouseEventHandler(this.custArrayHighlight_Click);
            // 
            // customCharacter2
            // 
            this.customCharacter2.BorderWidth = 0;
            this.customCharacter2.Columns = 5;
            this.customCharacter2.DisplayName = "Custom Character";
            this.customCharacter2.Location = new System.Drawing.Point(15, 137);
            this.customCharacter2.Margin = new System.Windows.Forms.Padding(18);
            this.customCharacter2.MinimumSize = new System.Drawing.Size(5, 5);
            this.customCharacter2.Name = "customCharacter2";
            this.customCharacter2.Rows = 8;
            this.customCharacter2.Selected = false;
            this.customCharacter2.Size = new System.Drawing.Size(20, 32);
            this.customCharacter2.TabIndex = 7;
            this.customCharacter2.Visible = false;
            // 
            // hbg1
            // 
            this.hbg1.BorderWidth = 0;
            this.hbg1.Columns = 5;
            this.hbg1.DisplayName = "Custom Character";
            this.hbg1.Location = new System.Drawing.Point(53, 15);
            this.hbg1.Margin = new System.Windows.Forms.Padding(18);
            this.hbg1.MinimumSize = new System.Drawing.Size(5, 5);
            this.hbg1.Name = "hbg1";
            this.hbg1.Rows = 8;
            this.hbg1.Selected = false;
            this.hbg1.Size = new System.Drawing.Size(20, 32);
            this.hbg1.TabIndex = 99;
            this.hbg1.TabStop = false;
            this.hbg1.Visible = false;
            // 
            // character4
            // 
            this.character4.BorderWidth = 0;
            this.character4.Columns = 5;
            this.character4.DisplayName = "Custom 4";
            this.character4.Location = new System.Drawing.Point(15, 97);
            this.character4.Margin = new System.Windows.Forms.Padding(18);
            this.character4.MinimumSize = new System.Drawing.Size(5, 5);
            this.character4.Name = "character4";
            this.character4.Rows = 8;
            this.character4.Selected = false;
            this.character4.Size = new System.Drawing.Size(20, 32);
            this.character4.TabIndex = 99;
            this.character4.TabStop = false;
            this.character4.MouseClick += new System.Windows.Forms.MouseEventHandler(this.custArrayHighlight_Click);
            // 
            // customCharacter7
            // 
            this.customCharacter7.BorderWidth = 0;
            this.customCharacter7.Columns = 5;
            this.customCharacter7.DisplayName = "Custom Character";
            this.customCharacter7.Location = new System.Drawing.Point(53, 15);
            this.customCharacter7.Margin = new System.Windows.Forms.Padding(18);
            this.customCharacter7.MinimumSize = new System.Drawing.Size(5, 5);
            this.customCharacter7.Name = "customCharacter7";
            this.customCharacter7.Rows = 8;
            this.customCharacter7.Selected = false;
            this.customCharacter7.Size = new System.Drawing.Size(20, 32);
            this.customCharacter7.TabIndex = 7;
            this.customCharacter7.Visible = false;
            // 
            // vbg1
            // 
            this.vbg1.BorderWidth = 0;
            this.vbg1.Columns = 5;
            this.vbg1.DisplayName = "Custom Character";
            this.vbg1.Location = new System.Drawing.Point(53, 15);
            this.vbg1.Margin = new System.Windows.Forms.Padding(18);
            this.vbg1.MinimumSize = new System.Drawing.Size(5, 5);
            this.vbg1.Name = "vbg1";
            this.vbg1.Rows = 8;
            this.vbg1.Selected = false;
            this.vbg1.Size = new System.Drawing.Size(20, 32);
            this.vbg1.TabIndex = 21;
            this.vbg1.TabStop = false;
            this.vbg1.Visible = false;
            // 
            // hbg0
            // 
            this.hbg0.BorderWidth = 0;
            this.hbg0.Columns = 5;
            this.hbg0.DisplayName = "Custom Character";
            this.hbg0.Location = new System.Drawing.Point(15, 15);
            this.hbg0.Margin = new System.Windows.Forms.Padding(18);
            this.hbg0.MinimumSize = new System.Drawing.Size(5, 5);
            this.hbg0.Name = "hbg0";
            this.hbg0.Rows = 8;
            this.hbg0.Selected = false;
            this.hbg0.Size = new System.Drawing.Size(20, 32);
            this.hbg0.TabIndex = 11;
            this.hbg0.TabStop = false;
            this.hbg0.Visible = false;
            // 
            // hbg6
            // 
            this.hbg6.BorderWidth = 0;
            this.hbg6.Columns = 5;
            this.hbg6.DisplayName = "Custom Character";
            this.hbg6.Location = new System.Drawing.Point(15, 137);
            this.hbg6.Margin = new System.Windows.Forms.Padding(18);
            this.hbg6.MinimumSize = new System.Drawing.Size(5, 5);
            this.hbg6.Name = "hbg6";
            this.hbg6.Rows = 8;
            this.hbg6.Selected = false;
            this.hbg6.Size = new System.Drawing.Size(20, 32);
            this.hbg6.TabIndex = 17;
            this.hbg6.TabStop = false;
            this.hbg6.Visible = false;
            // 
            // vbg6
            // 
            this.vbg6.BorderWidth = 0;
            this.vbg6.Columns = 5;
            this.vbg6.DisplayName = "Custom Character";
            this.vbg6.Location = new System.Drawing.Point(15, 137);
            this.vbg6.Margin = new System.Windows.Forms.Padding(18);
            this.vbg6.MinimumSize = new System.Drawing.Size(5, 5);
            this.vbg6.Name = "vbg6";
            this.vbg6.Rows = 8;
            this.vbg6.Selected = false;
            this.vbg6.Size = new System.Drawing.Size(20, 32);
            this.vbg6.TabIndex = 26;
            this.vbg6.TabStop = false;
            this.vbg6.Visible = false;
            // 
            // customCharacter4
            // 
            this.customCharacter4.BorderWidth = 0;
            this.customCharacter4.Columns = 5;
            this.customCharacter4.DisplayName = "Custom Character";
            this.customCharacter4.Location = new System.Drawing.Point(15, 97);
            this.customCharacter4.Margin = new System.Windows.Forms.Padding(18);
            this.customCharacter4.MinimumSize = new System.Drawing.Size(5, 5);
            this.customCharacter4.Name = "customCharacter4";
            this.customCharacter4.Rows = 8;
            this.customCharacter4.Selected = false;
            this.customCharacter4.Size = new System.Drawing.Size(20, 32);
            this.customCharacter4.TabIndex = 7;
            this.customCharacter4.Visible = false;
            // 
            // hbg2
            // 
            this.hbg2.BorderWidth = 0;
            this.hbg2.Columns = 5;
            this.hbg2.DisplayName = "Custom Character";
            this.hbg2.Location = new System.Drawing.Point(15, 54);
            this.hbg2.Margin = new System.Windows.Forms.Padding(18);
            this.hbg2.MinimumSize = new System.Drawing.Size(5, 5);
            this.hbg2.Name = "hbg2";
            this.hbg2.Rows = 8;
            this.hbg2.Selected = false;
            this.hbg2.Size = new System.Drawing.Size(20, 32);
            this.hbg2.TabIndex = 13;
            this.hbg2.TabStop = false;
            this.hbg2.Visible = false;
            // 
            // vbg0
            // 
            this.vbg0.BorderWidth = 0;
            this.vbg0.Columns = 5;
            this.vbg0.DisplayName = "Custom Character";
            this.vbg0.Location = new System.Drawing.Point(15, 15);
            this.vbg0.Margin = new System.Windows.Forms.Padding(18);
            this.vbg0.MinimumSize = new System.Drawing.Size(5, 5);
            this.vbg0.Name = "vbg0";
            this.vbg0.Rows = 8;
            this.vbg0.Selected = false;
            this.vbg0.Size = new System.Drawing.Size(20, 32);
            this.vbg0.TabIndex = 20;
            this.vbg0.TabStop = false;
            this.vbg0.Visible = false;
            // 
            // customCharacter3
            // 
            this.customCharacter3.BorderWidth = 0;
            this.customCharacter3.Columns = 5;
            this.customCharacter3.DisplayName = "Custom Character";
            this.customCharacter3.Location = new System.Drawing.Point(53, 97);
            this.customCharacter3.Margin = new System.Windows.Forms.Padding(18);
            this.customCharacter3.MinimumSize = new System.Drawing.Size(5, 5);
            this.customCharacter3.Name = "customCharacter3";
            this.customCharacter3.Rows = 8;
            this.customCharacter3.Selected = false;
            this.customCharacter3.Size = new System.Drawing.Size(20, 32);
            this.customCharacter3.TabIndex = 7;
            this.customCharacter3.Visible = false;
            // 
            // hbg4
            // 
            this.hbg4.BorderWidth = 0;
            this.hbg4.Columns = 5;
            this.hbg4.DisplayName = "Custom Character";
            this.hbg4.Location = new System.Drawing.Point(15, 97);
            this.hbg4.Margin = new System.Windows.Forms.Padding(18);
            this.hbg4.MinimumSize = new System.Drawing.Size(5, 5);
            this.hbg4.Name = "hbg4";
            this.hbg4.Rows = 8;
            this.hbg4.Selected = false;
            this.hbg4.Size = new System.Drawing.Size(20, 32);
            this.hbg4.TabIndex = 15;
            this.hbg4.TabStop = false;
            this.hbg4.Visible = false;
            // 
            // customCharacter8
            // 
            this.customCharacter8.BorderWidth = 0;
            this.customCharacter8.Columns = 5;
            this.customCharacter8.DisplayName = "Custom Character";
            this.customCharacter8.Location = new System.Drawing.Point(15, 15);
            this.customCharacter8.Margin = new System.Windows.Forms.Padding(18);
            this.customCharacter8.MinimumSize = new System.Drawing.Size(5, 5);
            this.customCharacter8.Name = "customCharacter8";
            this.customCharacter8.Rows = 8;
            this.customCharacter8.Selected = false;
            this.customCharacter8.Size = new System.Drawing.Size(20, 32);
            this.customCharacter8.TabIndex = 7;
            this.customCharacter8.Visible = false;
            // 
            // customCharacter6
            // 
            this.customCharacter6.BorderWidth = 0;
            this.customCharacter6.Columns = 5;
            this.customCharacter6.DisplayName = "Custom Character";
            this.customCharacter6.Location = new System.Drawing.Point(15, 54);
            this.customCharacter6.Margin = new System.Windows.Forms.Padding(18);
            this.customCharacter6.MinimumSize = new System.Drawing.Size(5, 5);
            this.customCharacter6.Name = "customCharacter6";
            this.customCharacter6.Rows = 8;
            this.customCharacter6.Selected = false;
            this.customCharacter6.Size = new System.Drawing.Size(20, 32);
            this.customCharacter6.TabIndex = 7;
            this.customCharacter6.Visible = false;
            // 
            // vbg2
            // 
            this.vbg2.BorderWidth = 0;
            this.vbg2.Columns = 5;
            this.vbg2.DisplayName = "Custom Character";
            this.vbg2.Location = new System.Drawing.Point(15, 54);
            this.vbg2.Margin = new System.Windows.Forms.Padding(18);
            this.vbg2.MinimumSize = new System.Drawing.Size(5, 5);
            this.vbg2.Name = "vbg2";
            this.vbg2.Rows = 8;
            this.vbg2.Selected = false;
            this.vbg2.Size = new System.Drawing.Size(20, 32);
            this.vbg2.TabIndex = 22;
            this.vbg2.TabStop = false;
            this.vbg2.Visible = false;
            // 
            // character2
            // 
            this.character2.BorderWidth = 0;
            this.character2.Columns = 5;
            this.character2.DisplayName = "Custom 2";
            this.character2.Location = new System.Drawing.Point(15, 54);
            this.character2.Margin = new System.Windows.Forms.Padding(18);
            this.character2.MinimumSize = new System.Drawing.Size(5, 5);
            this.character2.Name = "character2";
            this.character2.Rows = 8;
            this.character2.Selected = false;
            this.character2.Size = new System.Drawing.Size(20, 32);
            this.character2.TabIndex = 3;
            this.character2.TabStop = false;
            this.character2.MouseClick += new System.Windows.Forms.MouseEventHandler(this.custArrayHighlight_Click);
            // 
            // vbg4
            // 
            this.vbg4.BorderWidth = 0;
            this.vbg4.Columns = 5;
            this.vbg4.DisplayName = "Custom Character";
            this.vbg4.Location = new System.Drawing.Point(15, 97);
            this.vbg4.Margin = new System.Windows.Forms.Padding(18);
            this.vbg4.MinimumSize = new System.Drawing.Size(5, 5);
            this.vbg4.Name = "vbg4";
            this.vbg4.Rows = 8;
            this.vbg4.Selected = false;
            this.vbg4.Size = new System.Drawing.Size(20, 32);
            this.vbg4.TabIndex = 24;
            this.vbg4.TabStop = false;
            this.vbg4.Visible = false;
            // 
            // customCharacter5
            // 
            this.customCharacter5.BorderWidth = 0;
            this.customCharacter5.Columns = 5;
            this.customCharacter5.DisplayName = "Custom Character";
            this.customCharacter5.Location = new System.Drawing.Point(53, 54);
            this.customCharacter5.Margin = new System.Windows.Forms.Padding(18);
            this.customCharacter5.MinimumSize = new System.Drawing.Size(5, 5);
            this.customCharacter5.Name = "customCharacter5";
            this.customCharacter5.Rows = 8;
            this.customCharacter5.Selected = false;
            this.customCharacter5.Size = new System.Drawing.Size(20, 32);
            this.customCharacter5.TabIndex = 7;
            this.customCharacter5.Visible = false;
            // 
            // hbg3
            // 
            this.hbg3.BorderWidth = 0;
            this.hbg3.Columns = 5;
            this.hbg3.DisplayName = "Custom Character";
            this.hbg3.Location = new System.Drawing.Point(53, 54);
            this.hbg3.Margin = new System.Windows.Forms.Padding(18);
            this.hbg3.MinimumSize = new System.Drawing.Size(5, 5);
            this.hbg3.Name = "hbg3";
            this.hbg3.Rows = 8;
            this.hbg3.Selected = false;
            this.hbg3.Size = new System.Drawing.Size(20, 32);
            this.hbg3.TabIndex = 14;
            this.hbg3.TabStop = false;
            this.hbg3.Visible = false;
            // 
            // vbg3
            // 
            this.vbg3.BorderWidth = 0;
            this.vbg3.Columns = 5;
            this.vbg3.DisplayName = "Custom Character";
            this.vbg3.Location = new System.Drawing.Point(53, 54);
            this.vbg3.Margin = new System.Windows.Forms.Padding(18);
            this.vbg3.MinimumSize = new System.Drawing.Size(5, 5);
            this.vbg3.Name = "vbg3";
            this.vbg3.Rows = 8;
            this.vbg3.Selected = false;
            this.vbg3.Size = new System.Drawing.Size(20, 32);
            this.vbg3.TabIndex = 23;
            this.vbg3.TabStop = false;
            this.vbg3.Visible = false;
            // 
            // character3
            // 
            this.character3.BorderWidth = 0;
            this.character3.Columns = 5;
            this.character3.DisplayName = "Custom 3";
            this.character3.Location = new System.Drawing.Point(53, 54);
            this.character3.Margin = new System.Windows.Forms.Padding(18);
            this.character3.MinimumSize = new System.Drawing.Size(5, 5);
            this.character3.Name = "character3";
            this.character3.Rows = 8;
            this.character3.Selected = false;
            this.character3.Size = new System.Drawing.Size(20, 32);
            this.character3.TabIndex = 4;
            this.character3.TabStop = false;
            this.character3.MouseClick += new System.Windows.Forms.MouseEventHandler(this.custArrayHighlight_Click);
            // 
            // hbg5
            // 
            this.hbg5.BorderWidth = 0;
            this.hbg5.Columns = 5;
            this.hbg5.DisplayName = "Custom Character";
            this.hbg5.Location = new System.Drawing.Point(53, 97);
            this.hbg5.Margin = new System.Windows.Forms.Padding(18);
            this.hbg5.MinimumSize = new System.Drawing.Size(5, 5);
            this.hbg5.Name = "hbg5";
            this.hbg5.Rows = 8;
            this.hbg5.Selected = false;
            this.hbg5.Size = new System.Drawing.Size(20, 32);
            this.hbg5.TabIndex = 16;
            this.hbg5.TabStop = false;
            this.hbg5.Visible = false;
            // 
            // vbg5
            // 
            this.vbg5.BorderWidth = 0;
            this.vbg5.Columns = 5;
            this.vbg5.DisplayName = "Custom Character";
            this.vbg5.Location = new System.Drawing.Point(53, 97);
            this.vbg5.Margin = new System.Windows.Forms.Padding(18);
            this.vbg5.MinimumSize = new System.Drawing.Size(5, 5);
            this.vbg5.Name = "vbg5";
            this.vbg5.Rows = 8;
            this.vbg5.Selected = false;
            this.vbg5.Size = new System.Drawing.Size(20, 32);
            this.vbg5.TabIndex = 25;
            this.vbg5.TabStop = false;
            this.vbg5.Visible = false;
            // 
            // character5
            // 
            this.character5.BorderWidth = 0;
            this.character5.Columns = 5;
            this.character5.DisplayName = "Custom 5";
            this.character5.Location = new System.Drawing.Point(53, 97);
            this.character5.Margin = new System.Windows.Forms.Padding(18);
            this.character5.MinimumSize = new System.Drawing.Size(5, 5);
            this.character5.Name = "character5";
            this.character5.Rows = 8;
            this.character5.Selected = false;
            this.character5.Size = new System.Drawing.Size(20, 32);
            this.character5.TabIndex = 7;
            this.character5.TabStop = false;
            this.character5.MouseClick += new System.Windows.Forms.MouseEventHandler(this.custArrayHighlight_Click);
            // 
            // customCharacter1
            // 
            this.customCharacter1.BorderWidth = 0;
            this.customCharacter1.Columns = 5;
            this.customCharacter1.DisplayName = "Custom Character";
            this.customCharacter1.Location = new System.Drawing.Point(53, 137);
            this.customCharacter1.Margin = new System.Windows.Forms.Padding(18);
            this.customCharacter1.MinimumSize = new System.Drawing.Size(5, 5);
            this.customCharacter1.Name = "customCharacter1";
            this.customCharacter1.Rows = 8;
            this.customCharacter1.Selected = false;
            this.customCharacter1.Size = new System.Drawing.Size(20, 32);
            this.customCharacter1.TabIndex = 7;
            this.customCharacter1.Visible = false;
            // 
            // vbg7
            // 
            this.vbg7.BorderWidth = 0;
            this.vbg7.Columns = 5;
            this.vbg7.DisplayName = "Custom Character";
            this.vbg7.Location = new System.Drawing.Point(53, 137);
            this.vbg7.Margin = new System.Windows.Forms.Padding(18);
            this.vbg7.MinimumSize = new System.Drawing.Size(5, 5);
            this.vbg7.Name = "vbg7";
            this.vbg7.Rows = 8;
            this.vbg7.Selected = false;
            this.vbg7.Size = new System.Drawing.Size(20, 32);
            this.vbg7.TabIndex = 27;
            this.vbg7.TabStop = false;
            this.vbg7.Visible = false;
            // 
            // hbg7
            // 
            this.hbg7.BorderWidth = 0;
            this.hbg7.Columns = 5;
            this.hbg7.DisplayName = "Custom Character";
            this.hbg7.Location = new System.Drawing.Point(53, 137);
            this.hbg7.Margin = new System.Windows.Forms.Padding(18);
            this.hbg7.MinimumSize = new System.Drawing.Size(5, 5);
            this.hbg7.Name = "hbg7";
            this.hbg7.Rows = 8;
            this.hbg7.Selected = false;
            this.hbg7.Size = new System.Drawing.Size(20, 32);
            this.hbg7.TabIndex = 18;
            this.hbg7.TabStop = false;
            this.hbg7.Visible = false;
            // 
            // character7
            // 
            this.character7.BorderWidth = 0;
            this.character7.Columns = 5;
            this.character7.DisplayName = "Custom 7";
            this.character7.Location = new System.Drawing.Point(53, 137);
            this.character7.Margin = new System.Windows.Forms.Padding(18);
            this.character7.MinimumSize = new System.Drawing.Size(5, 5);
            this.character7.Name = "character7";
            this.character7.Rows = 8;
            this.character7.Selected = false;
            this.character7.Size = new System.Drawing.Size(20, 32);
            this.character7.TabIndex = 9;
            this.character7.TabStop = false;
            this.character7.MouseClick += new System.Windows.Forms.MouseEventHandler(this.custArrayHighlight_Click);
            // 
            // emptyCharacterEditor
            // 
            this.emptyCharacterEditor.BorderWidth = 1;
            this.emptyCharacterEditor.Columns = 5;
            this.emptyCharacterEditor.DisplayName = "Custom Character";
            this.emptyCharacterEditor.Location = new System.Drawing.Point(20, 35);
            this.emptyCharacterEditor.Margin = new System.Windows.Forms.Padding(4);
            this.emptyCharacterEditor.MinimumSize = new System.Drawing.Size(5, 5);
            this.emptyCharacterEditor.Name = "emptyCharacterEditor";
            this.emptyCharacterEditor.Rows = 8;
            this.emptyCharacterEditor.Selected = false;
            this.emptyCharacterEditor.Size = new System.Drawing.Size(101, 161);
            this.emptyCharacterEditor.TabIndex = 38;
            this.emptyCharacterEditor.Visible = false;
            // 
            // CyCharLCDControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.customCharacterGroupBox);
            this.Controls.Add(this.groupBox4);
            this.Name = "CyCharLCDControl";
            this.Size = new System.Drawing.Size(470, 242);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.customCharacterGroupBox.ResumeLayout(false);
            this.customCharacterGroupBox.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private CharLCD_I2C_v1_5.CyCustomCharacter customCharacter8;
        private CharLCD_I2C_v1_5.CyCustomCharacter customCharacter1;
        private CharLCD_I2C_v1_5.CyCustomCharacter vbg4;
        private CharLCD_I2C_v1_5.CyCustomCharacter character1;
        private CharLCD_I2C_v1_5.CyCustomCharacter vbg7;
        private CharLCD_I2C_v1_5.CyCustomCharacter hbg5;
        private CharLCD_I2C_v1_5.CyCustomCharacter character6;
        private CharLCD_I2C_v1_5.CyCustomCharacter character0;
        private CharLCD_I2C_v1_5.CyCustomCharacter customCharacter6;
        private CharLCD_I2C_v1_5.CyCustomCharacter customCharacter5;
        private CharLCD_I2C_v1_5.CyCustomCharacter customCharacter2;
        private CharLCD_I2C_v1_5.CyCustomCharacter hbg1;
        private CharLCD_I2C_v1_5.CyCustomCharacter character4;
        private CharLCD_I2C_v1_5.CyCustomCharacter customCharacter7;
        private CharLCD_I2C_v1_5.CyCustomCharacter hbg7;
        private CharLCD_I2C_v1_5.CyCustomCharacter vbg1;
        private CharLCD_I2C_v1_5.CyCustomCharacter hbg3;
        private CharLCD_I2C_v1_5.CyCustomCharacter vbg5;
        private CharLCD_I2C_v1_5.CyCustomCharacter hbg0;
        private CharLCD_I2C_v1_5.CyCustomCharacter vbg3;
        private CharLCD_I2C_v1_5.CyCustomCharacter character3;
        private CharLCD_I2C_v1_5.CyCustomCharacter hbg6;
        private CharLCD_I2C_v1_5.CyCustomCharacter vbg6;
        private CharLCD_I2C_v1_5.CyCustomCharacter character5;
        private CharLCD_I2C_v1_5.CyCustomCharacter vbg2;
        private CharLCD_I2C_v1_5.CyCustomCharacter customCharacter4;
        private CharLCD_I2C_v1_5.CyCustomCharacter character7;
        private CharLCD_I2C_v1_5.CyCustomCharacter character2;
        private CharLCD_I2C_v1_5.CyCustomCharacter hbg2;
        private CharLCD_I2C_v1_5.CyCustomCharacter vbg0;
        private CharLCD_I2C_v1_5.CyCustomCharacter customCharacter3;
        private CharLCD_I2C_v1_5.CyCustomCharacter hbg4;
        private CharLCD_I2C_v1_5.CyCustomCharacter emptyCharacterEditor;
        private CharLCD_I2C_v1_5.CyCustomCharacter characterEditor;
        private System.Windows.Forms.CheckBox crCheckBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.RadioButton vbgRadioButton;
        private System.Windows.Forms.RadioButton hbgRadioButton;
        private System.Windows.Forms.RadioButton udRadioButton;
        private System.Windows.Forms.RadioButton noneRadioButton;
        private System.Windows.Forms.GroupBox groupBox4;
        private CyCustomCharacterGroupBox customCharacterGroupBox;
        private System.Windows.Forms.Label currentEditableCharacter;
    }
}
