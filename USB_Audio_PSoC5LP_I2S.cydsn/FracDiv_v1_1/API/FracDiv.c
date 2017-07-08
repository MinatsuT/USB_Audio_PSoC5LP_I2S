/* ========================================
 *
 * Copyright YOUR COMPANY, THE YEAR
 * All Rights Reserved
 * UNPUBLISHED, LICENSED SOFTWARE.
 *
 * CONFIDENTIAL AND PROPRIETARY INFORMATION
 * WHICH IS THE PROPERTY OF your company.
 *
 * ========================================
*/
#include "`$INSTANCE_NAME`.h"

void `$INSTANCE_NAME`_Start(void) {
    `$INSTANCE_NAME`_CtrlReg_1_Control |= `$INSTANCE_NAME`_EN ;
}

void `$INSTANCE_NAME`_Stop(void) {
    `$INSTANCE_NAME`_CtrlReg_1_Control &= (uint8)~`$INSTANCE_NAME`_EN ;
}

void `$INSTANCE_NAME`_Write(uint32 Y, uint32 X) {
    `$INSTANCE_NAME`_Y = Y;
    `$INSTANCE_NAME`_X = X;
    `$INSTANCE_NAME`_Init();
}

void `$INSTANCE_NAME`_Init() {
    `$INSTANCE_NAME`_A0 = 0u;
    `$INSTANCE_NAME`_A1 = 0u;
}

/* [] END OF FILE */
