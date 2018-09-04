// Serial to Unity
// Based on https://www.alanzucconi.com/2015/10/07/how-to-integrate-arduino-with-unity/
#include <SerialCommand.h>

SerialCommand sCmd;

//www.elegoo.com
//2016.12.08
#include <SR04.h>
#define TRIG_PIN 12
#define ECHO_PIN 11
SR04 sr04 = SR04(ECHO_PIN,TRIG_PIN);
long a;

void setup() {
  Serial.begin(9600);
  while (!Serial);

  sCmd.addCommand("PING", pingHandler);
  sCmd.addCommand("ECHO", echoHandler);
  // sCmd.setDefaultHandler(errorHandler);

  delay(10);
}

void loop() {
  a=sr04.Distance();
   Serial.print(a);
   Serial.println("cm");
   
  if (Serial.available() > 0) {
    sCmd.readSerial();
  }

  delay(50);  
}

void pingHandler ()
{
  Serial.println("PONG");
}

void echoHandler ()
{
  char *arg;
  arg = sCmd.next();
  if (arg != NULL)
    Serial.println(arg);
  else
    Serial.println("nothing to echo");
}

/*
arg errorHandler (const char *command)
{
  // Error handling
}
*/
