# 96kHz Stereo USB Audio with PSoC 5LP
This is an improved version of [CE95390 - USB Audio with PSoC 3/5LP](http://www.cypress.com/documentation/code-examples/ce95390-usb-audio-psoc-35lp) which is an 8-bit, 32-kHz, mono USB Audio.

Your PSoC5LP will be appeared as an audio interface with the following features:
- Sampling rate: 32kHz - 96kHz
- Bit depth: 16-bit (But actual audio output via internal DAC is 8bit.)
- Audio channel: Stereo (No mono support.)

# Included community components
This project contains [CharLCD_I2C_v1_5](http://japan.cypress.com/forum/psoc-community-components/charlcdi2c-component-interface-pcf8574at-hd44780-combo?page=2) which is a "CharLCD_I2C Component interface for PCF8574AT / HD44780 Combo a.k.a 'LCD1602 with I2C backpack' designed for Arduino" posted by Michael Bey.
