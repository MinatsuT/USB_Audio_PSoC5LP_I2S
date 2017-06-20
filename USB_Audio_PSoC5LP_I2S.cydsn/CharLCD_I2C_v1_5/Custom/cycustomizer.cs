/*******************************************************************************
* Copyright 2008-2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
********************************************************************************/

using System.Collections.Generic;
using CyDesigner.Extensions.Gde;
using CyDesigner.Extensions.Common;
using System.Windows.Forms;


namespace CharLCD_I2C_v1_5
{
    [CyCompDevCustomizer]
    public partial class CyCustomizer : ICyParamEditHook_v1
    {
        #region ICyParamEditHook_v1 Members

        // Type of dialog box.  Advanced opens with a button on the generic form.  Complete opens immediately
        private const CyCompDevParamEditorMode MODE = CyCompDevParamEditorMode.COMPLETE;
        // Run Instance Customizer on Instance Selection
        private const bool EDIT_ON_DROP = false;
        private CyCharacterLCDEditingControl m_editControl;

        /// <summary>
        /// Edit Params initializes the data in the customizer as well as 
        /// instantiating the dialog.  
        /// </summary>
        /// <param name="obj"> Is a component object containing the parameter information for the object. </param>
        /// <returns>A DialogResult object associated with the form.  
        /// OK means accept changes.  Cancel means reject changes.</returns>
        DialogResult ICyParamEditHook_v1.EditParams(ICyInstEdit_v1 obj, ICyTerminalQuery_v1 termQuery, 
            ICyExpressMgr_v1 mgr)
        {
            ICyTabbedParamEditor editor = obj.CreateTabbedParamEditor();
            m_editControl = new CyCharacterLCDEditingControl(obj);
            editor.AddCustomPage("General", m_editControl, ExpView_ParamChanged, "General");
            editor.AddDefaultPage("Built-in", "Built-in");
            System.Windows.Forms.DialogResult result = editor.ShowDialog();
            return result;
        }

        CyCompDevParamEditorMode ICyParamEditHook_v1.GetEditorMode()
        {
            return MODE;
        }

        bool ICyParamEditHook_v1.EditParamsOnDrop
        {
            get { return EDIT_ON_DROP; }
        }
        #endregion

        private void ExpView_ParamChanged(ICyParamEditor custEditor, CyCompDevParam param)
        {
            if (param.ErrorCount == 0)
            {
                ((CyCharLCDControl)(m_editControl.DisplayControl)).InitializeParameters();                
            }
        }
    }

    public class CyCharacterLCDEditingControl : ICyParamEditingControl
    {
        private CyCharLCDControl m_control;

        public CyCharacterLCDEditingControl(ICyInstEdit_v1 obj)
        {
            this.m_control = new CyCharLCDControl(obj);
            this.m_control.Dock = DockStyle.Fill;
        }

        #region ICyParamEditingControl Members

        public Control DisplayControl
        {
            get { return m_control; }
        }

        public IEnumerable<CyCustErr> GetErrors()
        {
            return new CyCustErr[] { };
        }

        #endregion
    }
}
