using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace WeatherAppDesktop
{
    public class Weather
    {
        public string City { get; set; }
        public string Country { get; set; }
        public string Temperature { get; set; }
        public string MaxTemperature { get; set; }
        public string MinTemperature { get; set; }
        public string Pressure { get; set; }
        public string Humidity { get; set; }
        public string WindSpeed { get; set; }
        public bool Downfalls { get; set; }
        public string Sky { get; set; }
        public string Sunset { get; set; }
        public string Sunrise { get; set; }
        public string Time { get; set; }
        public string Date { get; set; }

        //public Weather(string city, string country, string temperature, string maxTemperature, string minTemperature, string pressure, string humidity,
        //    string windSpeed, string sky, DateTime sunset, DateTime sunrise)
        //{
        //    City = city;
        //    Country = country;
        //    Temperature = temperature;
        //    MaxTemperature = maxTemperature;
        //    MinTemperature = minTemperature;
        //    Pressure = pressure;
        //    Humidity = humidity;
        //    WindSpeed = windSpeed;
        //    Sky = sky;
        //    Sunset = sunset;
        //    Sunrise = sunrise;
        //}

        public void GetWeatherById(int id)
        {
            try
            {
                var path = "http://api.openweathermap.org/data/2.5/weather?id=" + id.ToString() + "&APPID=4355fd6b34d30979fb0e05c52642bf31" + "&mode=xml" + "&units=metric" + "&lang=ru";
                XDocument doc = XDocument.Load(path);


                City = doc.Element("current").Element("city").Attribute("name").Value;

                Country = doc.Element("current").Element("city").Element("country").Value;

                var sunrise = doc.Element("current").Element("city").Element("sun").Attribute("rise").Value;
                var sunset = doc.Element("current").Element("city").Element("sun").Attribute("set").Value;

                Sunrise = Convert.ToDateTime(sunrise).ToLocalTime().ToShortTimeString();
                Sunset = Convert.ToDateTime(sunset).ToLocalTime().ToShortTimeString();

                Temperature = doc.Element("current").Element("temperature").Attribute("value").Value;

                MaxTemperature = doc.Element("current").Element("temperature").Attribute("max").Value;

                MinTemperature = doc.Element("current").Element("temperature").Attribute("min").Value;

                Humidity = doc.Element("current").Element("humidity").Attribute("value").Value + doc.Element("current").Element("humidity").Attribute("unit").Value;

                Pressure = doc.Element("current").Element("pressure").Attribute("value").Value + doc.Element("current").Element("pressure").Attribute("unit").Value;

                WindSpeed = doc.Element("current").Element("wind").Element("speed").Attribute("value").Value;

                Sky = doc.Element("current").Element("weather").Attribute("value").Value;

                var time = doc.Element("current").Element("lastupdate").Attribute("value").Value;
                Time = Convert.ToDateTime(time).ToLocalTime().ToShortTimeString();
                Date = Convert.ToDateTime(time).ToShortDateString();

                int rain = Convert.ToInt32(doc.Element("current").Element("weather").Attribute("number").Value);
                if (rain >= 500 || rain <= 531 || rain >= 600 || rain <= 622)
                {
                    Downfalls = true;
                }
                else
                {
                    Downfalls = false;
                }
            }
            catch (Exception ex)
            {
                ShowInfo sg = new ShowInfo("Ошибка", "К сожалению, информация о данном городе на сервере не найдена.");
                sg.ShowDialog();
            }
        }
    }
}
