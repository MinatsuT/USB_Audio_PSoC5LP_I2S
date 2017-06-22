# CY8CKIT-059(PSoC5LP) USB Audio with Internal DAC and I2S Dual Out
This is an improved version of [CE95390 - USB Audio with PSoC 3/5LP](http://www.cypress.com/documentation/code-examples/ce95390-usb-audio-psoc-35lp) which is an 8-bit, 32-kHz, mono USB Audio.

Features:
- Internal DAC and I2S outputs are supported. Both of them sound simultaneously.
- Sampling rate: 44.1kHz - 96kHz
- Bit depth: 16-bit (Actual audio output via internal DAC is 8bit.)
- Audio channel: Stereo (No mono support.)

# Required parts
- PSoC5LP Prototyping Kit (CY8CKIT-059)
- Capacitors: C1(100pF), C2(1uF)
- Resister: R1(100k Ohm)
- External I2S DAC with self MASTER CLOCK generation capability [optional]
- 16x2 Character LCD (LCD1602) with I2C backpack [optional]

# Pin assignment and parts connection
<pre>
P0[6] --- Speaker L
P0[7] --- Speaker R

P1[6] --- LCD1602 I2C SDA
P1[7] --- LCD1602 I2C SCL

P12[2] --- I2S SCK (a.k.a. BCLK)
P12[3] --- I2S WS (a.k.a. LRCLK)
P12[4] --- I2S SDO (a.k.a. SDATA)

P15[0] --- C1(100pF) --- GND

P15[2] --+
         R1(100k Ohm)
P15[5] --+-- C2(1uF) --- GND
</pre>

# Included community components
This project contains [CharLCD_I2C_v1_5](http://japan.cypress.com/forum/psoc-community-components/charlcdi2c-component-interface-pcf8574at-hd44780-combo?page=2) which is a "CharLCD_I2C Component interface for PCF8574AT / HD44780 Combo a.k.a 'LCD1602 with I2C backpack' designed for Arduino" posted by Michael Bey.
