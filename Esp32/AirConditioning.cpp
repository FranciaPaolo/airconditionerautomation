/*

  AirConditioning.cpp - Library for utility method for http webserver.
  Created by Paolo F. April 4, 2024.
  Released into the public domain.

*/
#include "Arduino.h"
#include "AirConditioning.h"

#include <IRremoteESP8266.h>
#include <ir_Mitsubishi.h>

AirConditioning::AirConditioning(IRMitsubishiAC* ac) {
  _ac = ac;

  _temperature = 20;
}

void AirConditioning::setDefault() {
  (*_ac).off();
  (*_ac).setFan(2);
  (*_ac).setMode(kMitsubishiAcCool);
  (*_ac).setTemp(24);
  (*_ac).setVane(kMitsubishiAcVaneAuto);
}

void AirConditioning::setTemperature(String temperature) {
  float f_temp = temperature.toFloat();
  if (!isnan(f_temp)) {
    _temperature = f_temp;
    (*_ac).setTemp(_temperature);
  }
}

void AirConditioning::setMode(String mode) {
  if (mode == "1") {
    (*_ac).setMode(kMitsubishiAcHeat);
  } else if (mode == "3") {
    (*_ac).setMode(kMitsubishiAcCool);
  } else {
    (*_ac).setMode(kMitsubishiAcHeat);
  }
}

void AirConditioning::send() {
  (*_ac).send();
}

void AirConditioning::on() {
  (*_ac).on();
}

void AirConditioning::off() {
  (*_ac).off();
}


void AirConditioning::serialPrint() {
  // Display the settings.
  Serial.println("Mitsubishi A/C remote is in the following state:");
  Serial.printf("  %s\n", (*_ac).toString().c_str());

  // Display the encoded IR sequence.
  unsigned char* ir_code = (*_ac).getRaw();
  Serial.print("IR Code: 0x");
  for (uint8_t i = 0; i < kMitsubishiACStateLength; i++)
    Serial.printf("%02X", ir_code[i]);
  Serial.println();
}

String AirConditioning::html_home(float temperature, float humidity) {
  String html_text = "<!DOCTYPE html><html>";
  html_text += "<head><meta name=\"viewport\" content=\"width=device-width, initial-scale=1\">";
  html_text += "<link rel=\"icon\" href=\"data:,\">";
  // CSS to style the on/off buttons
  // Feel free to change the background-color and font-size attributes to fit your preferences
  html_text += "<style>html { font-family: Helvetica; display: inline-block; margin: 0px auto; text-align: center;}";
  html_text += ".statusbox { background-color: #4D2F50; border: none; color: white; padding: 16px 40px; display:inline-block; width: 500px; font-size: large;";
  html_text += "text-decoration: none; font-size: 30px; margin: 2px; cursor: pointer;}";
  html_text += ".smalllist { width: 600px; margin-left: auto; margin-right: auto; text-align: left; }</style></head>";

  // Web Page Heading
  html_text += "<body><h1>ESP32 Web Server Francia</h1>";
  html_text += "<p class=\"statusbox\">Environment<br /> Temperature: " + String(temperature, 1) + ", Humidity: " + String(humidity, 1) + "</p><br />";
  html_text += "<p class=\"statusbox\">Commands<br />" + text_airconditioning() + "</p>";
  html_text += "<h1>Api methods:</h1>";
  html_text += "<p><ul class=\"smalllist\">";
  html_text += "<li>HttpGet return environment temperature and humidity: <a target=\"_blank\" href=\"/dht22\">/dht22</a></li>";
  html_text += "<li>HttpGet return status of the command to the air conditioning: <a target=\"_blank\" href=\"/airconditioning\">/airconditioning</a></li>";
  html_text += "<li>HttpGet set turnon airconditioning: <a target=\"_blank\" href=\"/climateon?temperature=24&mode=1\">/climateon?temperature=24&mode=1</a><br />";
  html_text += "query params:<br />";
  html_text += "temperature (float)<br />";
  html_text += "mode (1 heat, 3 cool)";
  html_text += "</li>";
  html_text += "<li>HttpGet set turnoff airconditioning: <a target=\"_blank\" href=\"/climateoff\">/climateoff</a></li>";
  html_text += "</ul></p>";
  html_text += "</body></html>";

  return html_text;
}

String AirConditioning::json_airconditioning() {
  String json = "{";
  json += "\"mode\": " + String((*_ac).getMode()) + " ,";
  json += "\"temperature\": " + String((*_ac).getTemp()) + " ,";
  json += "\"power\": " + String((*_ac).getPower()) + " ";
  json += "}";
  return json;
}

String AirConditioning::text_airconditioning() {
  String text = "";

  if (String((*_ac).getMode()) == "1") {
    text += "Mode: Heat";
  } else if (String((*_ac).getMode()) == "3") {
    text += "Mode: Cool";
  } else
    text += "Mode: " + String((*_ac).getMode());


  text += ", Temperature: " + String((*_ac).getTemp(), 1);
  text += ", Power: " + String((*_ac).getPower());
  return text;
}
