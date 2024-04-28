/*

  TempHum.h - Library for utility method for http webserver.
  Created by Paolo F. April 4, 2024.
  Released into the public domain.

*/
#include "Arduino.h"

class TempHum
{
  public:
    TempHum();

    void setTemperature(float temp);
    void setHumidity(float hum);
    void setTemperatureHumidity(float temp, float hum);

    float getTemperature();
    float getHumidity();
    bool hasInvalidValues();
    void serialPrint();

  private:
    float _temperature;
    float _humidity;
    bool _valid_TempValue;
    bool _valid_HumValue;
};
