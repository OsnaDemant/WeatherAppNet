# WeatherAppNet
To run the program, you must have the Api key that can be found on this website. 
https://openweathermap.org/api?msclkid=f5934583b58611ec9853fe0372227ef7
Put api key in "ApiKey.json" document next to .exe 

put the city name as the first argument, and optionally as the second argument /C , /F or /K to set the scale type.

 - /C - celsius
 - /F - Fahrenheit
 - /K - kelvin

if you want to refresh the data stored in the database add as third argument /R 
typical command line:  **"New York" /C /R**

For WebApi, we used Swagger to elegantly present the operation of the code. 

 - **/WeatherForecast/{cityName}** - returns weather information in this city
 - **/WeatherForecast/{cityName}/temperature** returns the temperature in Celsius, Fahrenhaits and Kelvin using first letter 
 - **/cache** - return the cache 

you can also set refresh on the database by setting refresh to true

example WebApi link: https://localhost:7173/WeatherForecast/Szczecin/temperature?scaleTemperature=C&refresh=true
