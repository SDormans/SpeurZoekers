using System;
using System.Drawing;
using System.Globalization;

namespace Speurzoekers.Common.Domain
{
    public class Coordinate : IEquatable<Coordinate>
    {
        public const char Separator = ',';

        public static readonly NumberFormatInfo Format = new NumberFormatInfo
        {
            NumberDecimalSeparator = ".",
            NumberDecimalDigits = 6
        };

        public float Longitude { get; set; }
        public float Latitude { get; set; }
        public PointF Point
        {
            get => new PointF(Latitude, Longitude);
            set
            {
                Longitude = value.Y;
                Latitude = value.X;
            }
        }

        public Coordinate(float longitude, float latitude)
        {
            Longitude = longitude;
            Latitude = latitude;
        }

        /// <summary>String as longitude, latitude</summary>
        public Coordinate(string valueString)
        {
            string[] values = valueString.Split(Separator);

            Longitude = float.Parse(values[0], Format);
            Latitude = float.Parse(values[1], Format);
        }

        /// <summary>Print longitude, latitude</summary>
        public override string ToString()
        {
            return ToString(Longitude, Latitude);
        }

        /// <summary>Print latitude, longitude</summary>
        public string ToStringInverted()
        {
            return ToString(Latitude, Longitude);
        }

        private string ToString(float x, float y)
        {
            return $"{string.Format(Format, "{0:N}", x)}" +
                    $"{Separator} " +
                    $"{string.Format(Format, "{0:N}", y)}";
        }

        public Coordinate[] GetRectangle(float longLength, float latLength)
        {
            float lon = longLength / 2, lat = latLength / 2;

            return new[] 
            {
                new Coordinate(Longitude - lon, Latitude - lat),
                new Coordinate(Longitude + lon, Latitude - lat),
                new Coordinate(Longitude + lon, Latitude + lat),
                new Coordinate(Longitude - lon, Latitude + lat)
            };
        }

        public string GetRectangleString(float longLength, float latLength)
        {
            float lon = longLength / 2, lat = latLength / 2;

            return $"[[{GetMin(Latitude, lat)},{GetMin(Longitude, lon)}], [{GetMin(Latitude, lat)},{GetPlus(Longitude, lon)}], " +
                $"[{GetPlus(Latitude, lat)},{GetPlus(Longitude, lon)}], [{GetPlus(Latitude, lat)},{GetMin(Longitude, lon)}]]";
        }

        private static string GetMin(float x, float y)
        {
            return string.Format(Format, "{0:N}", x - y);
        }

        private static string GetPlus(float x, float y)
        {
            return string.Format(Format, "{0:N}", x + y);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Coordinate);
        }

        public bool Equals(Coordinate other)
        {
            return other != null &&
                   Longitude == other.Longitude &&
                   Latitude == other.Latitude;
        }

        public override int GetHashCode()
        {
            int hashCode = 1209561475;
            hashCode = hashCode * -1521134295 + Longitude.GetHashCode();
            hashCode = hashCode * -1521134295 + Latitude.GetHashCode();
            return hashCode;
        }
    }
}
