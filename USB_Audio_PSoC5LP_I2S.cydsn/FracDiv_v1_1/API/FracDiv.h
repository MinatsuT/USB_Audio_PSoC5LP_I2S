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
#include "`$INSTANCE_NAME`_CtrlReg_1.h"

/***************************************
*        Function Prototypes
***************************************/
void `$INSTANCE_NAME`_Start(void);
void `$INSTANCE_NAME`_Stop(void);
void `$INSTANCE_NAME`_Write(uint32 Y, uint32 X);
void `$INSTANCE_NAME`_Init(void);

/* Control register bits */
#define `$INSTANCE_NAME`_EN         (0x01u)

/* Control register bits */
#define `$INSTANCE_NAME`_X 	        (*(reg32 *) `$INSTANCE_NAME`_Div32_u0__D0_REG)
#define `$INSTANCE_NAME`_X_PTR      ((reg32 *) `$INSTANCE_NAME`_Div32_u0__D0_REG)
#define `$INSTANCE_NAME`_Y 	        (*(reg32 *) `$INSTANCE_NAME`_Div32_u0__D1_REG)
#define `$INSTANCE_NAME`_Y_PTR 	    ((reg32 *) `$INSTANCE_NAME`_Div32_u0__D1_REG)
#define `$INSTANCE_NAME`_A0         (*(reg32 *) `$INSTANCE_NAME`_Div32_u0__A0_REG)
#define `$INSTANCE_NAME`_A0_PTR     ((reg32 *) `$INSTANCE_NAME`_Div32_u0__A0_REG)
#define `$INSTANCE_NAME`_A1         (*(reg32 *) `$INSTANCE_NAME`_Div32_u0__A1_REG)
#define `$INSTANCE_NAME`_A1_PTR     ((reg32 *) `$INSTANCE_NAME`_Div32_u0__A1_REG)

/* [] END OF FILE */
