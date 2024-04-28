/*

  TempHum.cpp - Library for utility method for http webserver.
  Created by Paolo F. April 4, 2024.
  Released into the public domain.

*/
#include "Arduino.h"
#include "TempHum.h"

TempHum::TempHum(){
  _temperature = 0;
  _humidity = 0;
  _valid_TempValue = false;
  _valid_HumValue = false;
}

void TempHum::setTemperature(float temp) {
  _temperature= temp;
  _valid_TempValue = !isnan(_temperature);
}
void TempHum::setHumidity(float hum) {
  _humidity= hum;
  _valid_HumValue = !isnan(_humidity);
}

void TempHum::setTemperatureHumidity(float temp, float hum) {
  setTemperature(temp);
  setHumidity(hum);
}

float TempHum::getTemperature() {
  return _temperature;
}
float TempHum::getHumidity() {
  return _humidity;
}
bool TempHum::hasInvalidValues() {
  return !_valid_HumValue || !_valid_TempValue;
}

void TempHum::serialPrint() {
  
  if (hasInvalidValues()) {
    Serial.println("Impossibile leggere i dati del sensore DHT, controllare il cablaggio.");
    return;
  }

  Serial.print("Humidity: ");
  Serial.print(_humidity);  
  Serial.print(" %, Temp: ");
  Serial.print(_temperature);
  Serial.print(" Celsius ");
  Serial.println();
}


