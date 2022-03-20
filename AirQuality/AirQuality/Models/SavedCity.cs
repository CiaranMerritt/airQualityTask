using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AirQuality.Models
{
    public class SavedCity
    {
        public int CityId { get; set; }

        public string CityName { get; set; }

        public string CountryName { get; set; }

        public string LastUpdated { get; set; }

        public string Parameters { get; set; }
    }
}
