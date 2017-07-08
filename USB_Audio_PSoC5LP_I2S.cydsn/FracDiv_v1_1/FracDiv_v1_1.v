//`#start header` -- edit after this line, do not edit this line
// ========================================
//
// Copyright YOUR COMPANY, THE YEAR
// All Rights Reserved
// UNPUBLISHED, LICENSED SOFTWARE.
//
// CONFIDENTIAL AND PROPRIETARY INFORMATION
// WHICH IS THE PROPERTY OF your company.
//
// ========================================
`include "cypress.v"

// Component: CyControlReg_v1_80
`ifdef CY_BLK_DIR
`undef CY_BLK_DIR
`endif

`ifdef WARP
`define CY_BLK_DIR "$CYPRESS_DIR\..\psoc\content\CyPrimitives\cyprimitives.cylib\CyControlReg_v1_80"
`include "$CYPRESS_DIR\..\psoc\content\CyPrimitives\cyprimitives.cylib\CyControlReg_v1_80\CyControlReg_v1_80.v"
`else
`define CY_BLK_DIR "C:\Program Files (x86)\Cypress\PSoC Creator\4.1\PSoC Creator\psoc\content\CyPrimitives\cyprimitives.cylib\CyControlReg_v1_80"
`include "C:\Program Files (x86)\Cypress\PSoC Creator\4.1\PSoC Creator\psoc\content\CyPrimitives\cyprimitives.cylib\CyControlReg_v1_80\CyControlReg_v1_80.v"
`endif

// Component: CyStatusReg_v1_90
`ifdef CY_BLK_DIR
`undef CY_BLK_DIR
`endif

`ifdef WARP
`define CY_BLK_DIR "$CYPRESS_DIR\..\psoc\content\CyPrimitives\cyprimitives.cylib\CyStatusReg_v1_90"
`include "$CYPRESS_DIR\..\psoc\content\CyPrimitives\cyprimitives.cylib\CyStatusReg_v1_90\CyStatusReg_v1_90.v"
`else
`define CY_BLK_DIR "C:\Program Files (x86)\Cypress\PSoC Creator\4.1\PSoC Creator\psoc\content\CyPrimitives\cyprimitives.cylib\CyStatusReg_v1_90"
`include "C:\Program Files (x86)\Cypress\PSoC Creator\4.1\PSoC Creator\psoc\content\CyPrimitives\cyprimitives.cylib\CyStatusReg_v1_90\CyStatusReg_v1_90.v"
`endif
//`#end` -- edit above this line, do not edit this line

module FracDiv_v1_1 (
    input wire clock, 
    input wire reset, 
    input wire en, 
    output wire div,
    output wire en_out,
    output wire reset_out
);

    parameter X = 2;
    parameter Y = 1;
    localparam XminusY = (X-Y);
    localparam XminusY0 = (XminusY & 8'hff);
    localparam XminusY1 = ((XminusY>>8) & 8'hff);
    localparam XminusY2 = ((XminusY>>16) & 8'hff);
    localparam XminusY3 = ((XminusY>>24) & 8'hff);
    localparam Y0 = (Y & 8'hff);
    localparam Y1 = ((Y>>8) & 8'hff);
    localparam Y2 = ((Y>>16) & 8'hff);
    localparam Y3 = ((Y>>24) & 8'hff);

//`#start body` -- edit after this line, do not edit this line

/* ==================== Wire and Register Declarations ==================== */
wire A_LT_X;
wire Div32_d0_load;
wire Div32_d1_load;
wire Div32_f0_load;
wire Div32_f1_load;
wire Div32_route_si;
wire Div32_route_ci;
wire  [2:0] Div32_select;
wire ctrl_en;
wire ctrl_1;
wire ctrl_2;
wire ctrl_3;
wire ctrl_4;
wire ctrl_5;
wire ctrl_6;
wire ctrl_7;
wire StatusReg_1_status0;
wire StatusReg_1_status1;
wire StatusReg_1_status2;
wire StatusReg_1_status3;
wire StatusReg_1_status4;
wire StatusReg_1_status5;
wire StatusReg_1_status6;
wire StatusReg_1_status7;

/* ==================== Assignment of Combinatorial Variables ==================== */
assign div = (!A_LT_X);
assign Div32_d0_load = (1'b0);
assign Div32_d1_load = (1'b0);
assign Div32_f0_load = (1'b0);
assign Div32_f1_load = (1'b0);
assign Div32_route_si = (1'b0);
assign Div32_route_ci = (1'b0);
assign Div32_select[0] = (!A_LT_X);
assign Div32_select[1] = (reset);
assign Div32_select[2] = (!(ctrl_en && en));

assign StatusReg_1_status0 = (ctrl_en);
assign StatusReg_1_status1 = (en);
assign StatusReg_1_status2 = (reset);
assign StatusReg_1_status3 = (1'b0);
assign StatusReg_1_status4 = (1'b0);
assign StatusReg_1_status5 = (1'b0);
assign StatusReg_1_status6 = (1'b0);
assign StatusReg_1_status7 = (1'b0);

assign en_out = en;
assign reset_out = reset;

/* ==================== Div32 (Width: 32) Instantiation ==================== */
parameter Div32_dpconfig0 = 
{
    `CS_ALU_OP__ADD, `CS_SRCA_A0, `CS_SRCB_D1, `CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE, `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA, `CS_CMP_SEL_CFGA,  /*CFGRAM0: Add Y */
    `CS_ALU_OP__SUB, `CS_SRCA_A0, `CS_SRCB_D0, `CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE, `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA, `CS_CMP_SEL_CFGA,  /*CFGRAM1: Sub X */
    `CS_ALU_OP_PASS, `CS_SRCA_A1, `CS_SRCB_D0, `CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE, `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA, `CS_CMP_SEL_CFGA,  /*CFGRAM2: Reset */
    `CS_ALU_OP_PASS, `CS_SRCA_A1, `CS_SRCB_A0, `CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE, `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA, `CS_CMP_SEL_CFGA,  /*CFGRAM3: Reset */
    `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0, `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE, `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA, `CS_CMP_SEL_CFGA,  /*CFGRAM4:  */
    `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0, `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE, `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA, `CS_CMP_SEL_CFGA,  /*CFGRAM5:  */
    `CS_ALU_OP_PASS, `CS_SRCA_A1, `CS_SRCB_D0, `CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE, `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA, `CS_CMP_SEL_CFGA,  /*CFGRAM6: Reset */
    `CS_ALU_OP_PASS, `CS_SRCA_A1, `CS_SRCB_A0, `CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE, `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA, `CS_CMP_SEL_CFGA,  /*CFGRAM7: Reset */
    8'hFF, 8'h00, /* CFG9 */
    8'hFF, 8'hFF, /* CFG11-10 */
    `SC_CMPB_A1_D1, `SC_CMPA_A0_D1, `SC_CI_B_ARITH, `SC_CI_A_ARITH, `SC_C1_MASK_DSBL, `SC_C0_MASK_DSBL, `SC_A_MASK_DSBL, `SC_DEF_SI_0, `SC_SI_B_CHAIN, `SC_SI_A_DEFSI, /* CFG13-12 */
    `SC_A0_SRC_ACC, `SC_SHIFT_SL, 1'b0, `SC_SR_SRC_REG, `SC_FIFO1_BUS, `SC_FIFO0_BUS, `SC_MSB_DSBL, `SC_MSB_BIT0, `SC_MSB_NOCHN, `SC_FB_NOCHN, `SC_CMP1_NOCHN, `SC_CMP0_NOCHN, /* CFG15-14 */
    3'b000, `SC_FIFO_SYNC__ADD, 2'b000, `SC_FIFO1_DYN_OF, `SC_FIFO0_DYN_OF, `SC_FIFO_CLK1_POS, `SC_FIFO_CLK0_POS, `SC_FIFO_CLK__DP, `SC_FIFO_CAP_AX, `SC_FIFO_LEVEL, `SC_FIFO__SYNC, `SC_EXTCRC_DSBL, `SC_WRK16CAT_DSBL /* CFG17-16 */
};
parameter Div32_dpconfig1 = 
{
    `CS_ALU_OP__ADD, `CS_SRCA_A0, `CS_SRCB_D1, `CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE, `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA, `CS_CMP_SEL_CFGA,  /*CFGRAM0: Add Y */
    `CS_ALU_OP__SUB, `CS_SRCA_A0, `CS_SRCB_D0, `CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE, `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA, `CS_CMP_SEL_CFGA,  /*CFGRAM1: Sub X */
    `CS_ALU_OP_PASS, `CS_SRCA_A1, `CS_SRCB_D0, `CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE, `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA, `CS_CMP_SEL_CFGA,  /*CFGRAM2: Reset */
    `CS_ALU_OP_PASS, `CS_SRCA_A1, `CS_SRCB_A0, `CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE, `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA, `CS_CMP_SEL_CFGA,  /*CFGRAM3: Reset */
    `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0, `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE, `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA, `CS_CMP_SEL_CFGA,  /*CFGRAM4:  */
    `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0, `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE, `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA, `CS_CMP_SEL_CFGA,  /*CFGRAM5:  */
    `CS_ALU_OP_PASS, `CS_SRCA_A1, `CS_SRCB_D0, `CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE, `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA, `CS_CMP_SEL_CFGA,  /*CFGRAM6: Reset */
    `CS_ALU_OP_PASS, `CS_SRCA_A1, `CS_SRCB_A0, `CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE, `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA, `CS_CMP_SEL_CFGA,  /*CFGRAM7: Reset */
    8'h00, 8'h00, /* CFG9 */
    8'h00, 8'h00, /* CFG11-10 */
    `SC_CMPB_A1_D1, `SC_CMPA_A0_D1, `SC_CI_B_CHAIN, `SC_CI_A_CHAIN, `SC_C1_MASK_DSBL, `SC_C0_MASK_DSBL, `SC_A_MASK_DSBL, `SC_DEF_SI_0, `SC_SI_B_CHAIN, `SC_SI_A_CHAIN, /* CFG13-12 */
    `SC_A0_SRC_ACC, `SC_SHIFT_SL, 1'b0, `SC_SR_SRC_REG, `SC_FIFO1_BUS, `SC_FIFO0_BUS, `SC_MSB_DSBL, `SC_MSB_BIT0, `SC_MSB_CHNED, `SC_FB_CHNED, `SC_CMP1_CHNED, `SC_CMP0_CHNED, /* CFG15-14 */
    3'b000, `SC_FIFO_SYNC__ADD, 2'b000, `SC_FIFO1_DYN_OF, `SC_FIFO0_DYN_OF, `SC_FIFO_CLK1_POS, `SC_FIFO_CLK0_POS, `SC_FIFO_CLK__DP, `SC_FIFO_CAP_AX, `SC_FIFO_LEVEL, `SC_FIFO__SYNC, `SC_EXTCRC_DSBL, `SC_WRK16CAT_DSBL /* CFG17-16 */
};
parameter Div32_dpconfig2 = 
{
    `CS_ALU_OP__ADD, `CS_SRCA_A0, `CS_SRCB_D1, `CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE, `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA, `CS_CMP_SEL_CFGA,  /*CFGRAM0: Add Y */
    `CS_ALU_OP__SUB, `CS_SRCA_A0, `CS_SRCB_D0, `CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE, `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA, `CS_CMP_SEL_CFGA,  /*CFGRAM1: Sub X */
    `CS_ALU_OP_PASS, `CS_SRCA_A1, `CS_SRCB_D0, `CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE, `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA, `CS_CMP_SEL_CFGA,  /*CFGRAM2: Reset */
    `CS_ALU_OP_PASS, `CS_SRCA_A1, `CS_SRCB_A0, `CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE, `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA, `CS_CMP_SEL_CFGA,  /*CFGRAM3: Reset */
    `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0, `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE, `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA, `CS_CMP_SEL_CFGA,  /*CFGRAM4:  */
    `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0, `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE, `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA, `CS_CMP_SEL_CFGA,  /*CFGRAM5:  */
    `CS_ALU_OP_PASS, `CS_SRCA_A1, `CS_SRCB_D0, `CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE, `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA, `CS_CMP_SEL_CFGA,  /*CFGRAM6: Reset */
    `CS_ALU_OP_PASS, `CS_SRCA_A1, `CS_SRCB_A0, `CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE, `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA, `CS_CMP_SEL_CFGA,  /*CFGRAM7: Reset */
    8'h00, 8'h00, /* CFG9 */
    8'h00, 8'h00, /* CFG11-10 */
    `SC_CMPB_A1_D1, `SC_CMPA_A0_D1, `SC_CI_B_CHAIN, `SC_CI_A_CHAIN, `SC_C1_MASK_DSBL, `SC_C0_MASK_DSBL, `SC_A_MASK_DSBL, `SC_DEF_SI_0, `SC_SI_B_CHAIN, `SC_SI_A_CHAIN, /* CFG13-12 */
    `SC_A0_SRC_ACC, `SC_SHIFT_SL, 1'b0, `SC_SR_SRC_REG, `SC_FIFO1_BUS, `SC_FIFO0_BUS, `SC_MSB_DSBL, `SC_MSB_BIT0, `SC_MSB_CHNED, `SC_FB_CHNED, `SC_CMP1_CHNED, `SC_CMP0_CHNED, /* CFG15-14 */
    3'b000, `SC_FIFO_SYNC__ADD, 2'b000, `SC_FIFO1_DYN_OF, `SC_FIFO0_DYN_OF, `SC_FIFO_CLK1_POS, `SC_FIFO_CLK0_POS, `SC_FIFO_CLK__DP, `SC_FIFO_CAP_AX, `SC_FIFO_LEVEL, `SC_FIFO__SYNC, `SC_EXTCRC_DSBL, `SC_WRK16CAT_DSBL /* CFG17-16 */
};
parameter Div32_dpconfig3 = 
{
    `CS_ALU_OP__ADD, `CS_SRCA_A0, `CS_SRCB_D1, `CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE, `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA, `CS_CMP_SEL_CFGA,  /*CFGRAM0: Add Y */
    `CS_ALU_OP__SUB, `CS_SRCA_A0, `CS_SRCB_D0, `CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE, `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA, `CS_CMP_SEL_CFGA,  /*CFGRAM1: Sub X */
    `CS_ALU_OP_PASS, `CS_SRCA_A1, `CS_SRCB_D0, `CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE, `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA, `CS_CMP_SEL_CFGA,  /*CFGRAM2: Reset */
    `CS_ALU_OP_PASS, `CS_SRCA_A1, `CS_SRCB_A0, `CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE, `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA, `CS_CMP_SEL_CFGA,  /*CFGRAM3: Reset */
    `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0, `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE, `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA, `CS_CMP_SEL_CFGA,  /*CFGRAM4:  */
    `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0, `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE, `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA, `CS_CMP_SEL_CFGA,  /*CFGRAM5:  */
    `CS_ALU_OP_PASS, `CS_SRCA_A1, `CS_SRCB_D0, `CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE, `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA, `CS_CMP_SEL_CFGA,  /*CFGRAM6: Reset */
    `CS_ALU_OP_PASS, `CS_SRCA_A1, `CS_SRCB_A0, `CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE, `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA, `CS_CMP_SEL_CFGA,  /*CFGRAM7: Reset */
    8'h00, 8'h00, /* CFG9 */
    8'h00, 8'h00, /* CFG11-10 */
    `SC_CMPB_A1_D1, `SC_CMPA_A0_D1, `SC_CI_B_CHAIN, `SC_CI_A_CHAIN, `SC_C1_MASK_DSBL, `SC_C0_MASK_DSBL, `SC_A_MASK_DSBL, `SC_DEF_SI_0, `SC_SI_B_DEFSI, `SC_SI_A_CHAIN, /* CFG13-12 */
    `SC_A0_SRC_ACC, `SC_SHIFT_SL, 1'b0, `SC_SR_SRC_REG, `SC_FIFO1_BUS, `SC_FIFO0_BUS, `SC_MSB_ENBL, `SC_MSB_BIT7, `SC_MSB_CHNED, `SC_FB_CHNED, `SC_CMP1_CHNED, `SC_CMP0_CHNED, /* CFG15-14 */
    3'b000, `SC_FIFO_SYNC__ADD, 2'b000, `SC_FIFO1_DYN_OF, `SC_FIFO0_DYN_OF, `SC_FIFO_CLK1_POS, `SC_FIFO_CLK0_POS, `SC_FIFO_CLK__DP, `SC_FIFO_CAP_AX, `SC_FIFO_LEVEL, `SC_FIFO__SYNC, `SC_EXTCRC_DSBL, `SC_WRK16CAT_DSBL /* CFG17-16 */
};
cy_psoc3_dp32 #(
    .cy_dpconfig_a( Div32_dpconfig0 ), .cy_dpconfig_b( Div32_dpconfig1 ), .cy_dpconfig_c( Div32_dpconfig2 ), .cy_dpconfig_d( Div32_dpconfig3 ),
    .d0_init_a( XminusY0 ), .d0_init_b( XminusY1 ), .d0_init_c( XminusY2 ), .d0_init_d( XminusY3 ),
    .d1_init_a( Y0 ), .d1_init_b( Y1 ), .d1_init_c( Y2 ), .d1_init_d( Y3 ),
    .a0_init_a( 8'b00000000 ), .a0_init_b( 8'b00000000 ), .a0_init_c( 8'b00000000 ), .a0_init_d( 8'b00000000 ),
    .a1_init_a( 8'b00000000 ), .a1_init_b( 8'b00000000 ), .a1_init_c( 8'b00000000 ), .a1_init_d( 8'b00000000 ))
    Div32(
        .clk( clock ),
        .cs_addr( Div32_select ),
        .route_si( Div32_route_si ),
        .route_ci( Div32_route_ci ),
        .f0_load( Div32_f0_load ),
        .f1_load( Div32_f1_load ),
        .d0_load( Div32_d0_load ),
        .d1_load( Div32_d1_load ),
        .ce0(  ), 
        .cl0( { A_LT_X, Div32_cl0_3, Div32_cl0_2, Div32_cl0_1 } ), 
        .z0(  ), 
        .ff0(  ), 
        .ce1(  ), 
        .cl1(  ), 
        .z1(  ), 
        .ff1(  ), 
        .ov_msb(  ), 
        .co_msb(  ), 
        .cmsb(  ), 
        .so(  ), 
        .f0_bus_stat(  ), 
        .f0_blk_stat(  ), 
        .f1_bus_stat(  ), 
        .f1_blk_stat(  )
    );

/* ==================== CtrlReg_1 ==================== */
    CyControlReg_v1_80 CtrlReg_1 (
        .control_1(hardware_en),
        .control_2(ctrl_2),
        .control_3(ctrl_3),
        .control_0(ctrl_en),
        .control_4(ctrl_4),
        .control_5(ctrl_5),
        .control_6(ctrl_6),
        .control_7(ctrl_7),
        .clock(clock),
        .reset(1'b0));
    defparam CtrlReg_1.Bit0Mode = 1;
    defparam CtrlReg_1.Bit1Mode = 1;
    defparam CtrlReg_1.Bit2Mode = 1;
    defparam CtrlReg_1.Bit3Mode = 1;
    defparam CtrlReg_1.Bit4Mode = 1;
    defparam CtrlReg_1.Bit5Mode = 1;
    defparam CtrlReg_1.Bit6Mode = 1;
    defparam CtrlReg_1.Bit7Mode = 1;
    defparam CtrlReg_1.BitValue = 1;
    defparam CtrlReg_1.BusDisplay = 0;
    defparam CtrlReg_1.ExtrReset = 1;
    defparam CtrlReg_1.NumOutputs = 8;

/* ==================== StatusReg_1 ==================== */
    CyStatusReg_v1_90 StatusReg_1 (
        .status_0(StatusReg_1_status0),
        .status_1(StatusReg_1_status1),
        .status_2(StatusReg_1_status2),
        .status_3(StatusReg_1_status3),
        .clock(clock),
        .status_4(StatusReg_1_status4),
        .status_5(StatusReg_1_status5),
        .status_6(StatusReg_1_status6),
        .status_7(StatusReg_1_status7),
        .intr(Net_2),
        .status_bus(8'b0));
    defparam StatusReg_1.Bit0Mode = 0;
    defparam StatusReg_1.Bit1Mode = 1;
    defparam StatusReg_1.Bit2Mode = 1;
    defparam StatusReg_1.Bit3Mode = 1;
    defparam StatusReg_1.Bit4Mode = 1;
    defparam StatusReg_1.Bit5Mode = 1;
    defparam StatusReg_1.Bit6Mode = 1;
    defparam StatusReg_1.Bit7Mode = 1;
    defparam StatusReg_1.BusDisplay = 0;
    defparam StatusReg_1.Interrupt = 0;
    defparam StatusReg_1.MaskValue = 0;
    defparam StatusReg_1.NumInputs = 8;

//`#end` -- edit above this line, do not edit this line
endmodule
//`#start footer` -- edit after this line, do not edit this line
//`#end` -- edit above this line, do not edit this line


//[] END OF FILE
