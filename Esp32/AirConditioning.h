/*

  AirConditioning.h - Library for utility method for http webserver.
  Created by Paolo F. April 4, 2024.
  Released into the public domain.

*/
#include "Arduino.h"

#include <IRremoteESP8266.h>
#include <ir_Mitsubishi.h>

class AirConditioning
{
  public:
    AirConditioning(IRMitsubishiAC* ac);
    void setDefault();
    void setTemperature(String temperature);
    void setMode(String mode); // 1 Heat, 3 Cool
    void send();
    void on();
    void off();
    void serialPrint();

    String html_home(float temperature, float humidity);
    String json_airconditioning();
    String text_airconditioning();

  private:
    IRMitsubishiAC* _ac;
    float _temperature;
    bool _coolMode; // if false -> hotMode
    bool _on; // if false -> off
    
    //void _setDefaultMode();

};
