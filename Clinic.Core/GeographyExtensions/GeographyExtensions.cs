using System;
using System.Data.Entity.Spatial;
using System.Globalization;

namespace Clinic.Core.GeographyExtensions
{
    public static class GeographyExtensions
    {
        public static DbGeography CreatePoint(double longitude, double latitude)
        {
            if (latitude <= -90 && latitude >= 90) return null;
            if (longitude <= -180 && longitude >= 180) return null;

            string wellKnownText = String.Format(CultureInfo.InvariantCulture.NumberFormat, "POINT({0} {1})", longitude, latitude);
            return DbGeography.FromText(wellKnownText);
        }
    }
}
