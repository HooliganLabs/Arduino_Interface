# Arduino_Interface
Connection between Arduino and Unity.

Made with the Arduino MEGA2560 "Most Complete Starter Kit", for the board and ultrasonic sensor. Based on the tutorial by https://www.alanzucconi.com/2015/10/07/how-to-integrate-arduino-with-unity/

Here is what it looks like: https://twitter.com/HooliganLabs/status/1037036578768650240
You can ignore the also connected passive speaker you may see connected to the Arduino in this video, it is not used for this.

The .ide file to flash to your Arduino is in the "Unity_Arduino" folder. The .ide requires two .zip files with the serial communication and ultrasonic sensor libraries. These zip files are included in the folder as well. The ultrasonic sensor is connected as per the instructions in the MEGA2560 kit manual. In short, that is VCC to 5V, Trig to D12 PWM, Echo to D11 PWM, and GND to ground.

To connect with Unity, open the project folder with Unity. Open "SampleScene" and run it. If your Arduino is connected to COM4 it should work immediately, with the target vehicle moving as on a (adjustable) scale to the measured ultrasonic distance.

Please let me know if you find anything to be incomplete or not working. :)
