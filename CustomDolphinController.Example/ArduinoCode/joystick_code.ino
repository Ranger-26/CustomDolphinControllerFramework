 /*
 * Created by ArduinoGetStarted.com
 *
 * This example code is in the public domain
 *
 * Tutorial page: https://arduinogetstarted.com/tutorials/arduino-joystick
 */

#include <ezButton.h>

#define VRX_PIN  A0 // Arduino pin connected to VRX pin
#define VRY_PIN  A1 // Arduino pin connected to VRY pin
#define SW_PIN   2  // Arduino pin connected to SW  pin

#define BUTTON_A_PIN 8
#define BUTTON_B_PIN 9

//buttons
ezButton button(SW_PIN);

int xValue = 0; // To store value of the X axis
int yValue = 0; // To store value of the Y axis
int bValue = 0; // To store value of the button

void setup() {
  Serial.begin(9600) ;
  button.setDebounceTime(1); // set debounce time to 50 milliseconds
  //seupp button pins
  pinMode(BUTTON_A_PIN, INPUT_PULLUP);
  pinMode(BUTTON_B_PIN, INPUT_PULLUP);
}

void loop() {
  button.loop(); // MUST call the loop() function first

  // read analog X and Y analog values
  xValue = analogRead(VRX_PIN);
  yValue = analogRead(VRY_PIN);

  // Read the button value
  bValue = button.getState();

  // print data to Serial Monitor on Arduino IDE
  Serial.print("X = ");
  Serial.print(xValue);
  Serial.print(", Y = ");
  Serial.print(yValue);
  Serial.print(", button_A_state = ");
  Serial.print(isButtonPressed(BUTTON_A_PIN));
  Serial.print(", button_B_state = ");
  Serial.print(isButtonPressed(BUTTON_B_PIN));
  Serial.print(", button_L_state = ");
  Serial.print(bValue);
  Serial.println("");
}

bool isButtonPressed(int pin){
  return digitalRead(pin) == LOW;
}
