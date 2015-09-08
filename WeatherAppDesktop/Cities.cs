using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherAppDesktop
{
    public class Cities
    {
        public static List<City> CityList { get; set; }
        StreamReader reader;

        public Cities ()
        {
            reader = new StreamReader("city.list.json");
            CityList = new List<City>();
            while (!reader.EndOfStream)
            {
                CityList.Add(JsonHelper.JsonDeserialize<City>(reader.ReadLine()));
            }
        }
    }
}
