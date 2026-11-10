using Speurzoekers.Common.Domain;

namespace RegionUploadTool
{
    public static class Provinces
    {
        public static Province[] Dutch = new[]
        {
            new Province
            {
                Name = "Noord-Brabant",
                Coordinates = new []
                {
                    new Coordinate(4.5656587f, 51.7592519f),
                    new Coordinate(5.5723719f, 51.3683953f)
                }
            },
            new Province
            {
                Name = "Drenthe",
                Coordinates = new []
                {
                    new Coordinate(6.2473115f, 53.1546493f),
                    new Coordinate(6.9023344f, 52.6907578f)
                }
            },
            new Province
            {
                Name = "Flevoland",
                Coordinates = new []
                {
                    new Coordinate(5.8548093f, 52.7723537f),
                    new Coordinate(5.2239651f, 52.2962882f)
                }
            },
            new Province
            {
                Name = "Fryslân",
                Coordinates = new []
                {
                    new Coordinate(5.3812744f, 52.8450554f),
                    new Coordinate(6.2253307f, 53.4148817f)
                }
            },
            new Province
            {
                Name = "Gelderland",
                Coordinates = new []
                {
                    new Coordinate(5.1570721f, 51.8296006f),
                    new Coordinate(6.6297746f, 52.3536726f)
                }
            },
            new Province
            {
                Name = "Groningen",
                Coordinates = new []
                {
                    new Coordinate(6.269292f, 53.4148817f),
                    new Coordinate(7.1001599f, 52.9670409f)
                }
            },
            new Province
            {
                Name = "Noord-Holland",
                Coordinates = new []
                {
                    new Coordinate(5.1459476f, 52.2303168f),
                    new Coordinate(4.7283155f, 52.917044f)
                }
            },
            new Province
            {
                Name = "Zuid-Holland",
                Coordinates = new []
                {
                    new Coordinate(4.7949546f, 52.193184f),
                    new Coordinate(4.0256324f, 51.78433f)
                }
            },
            new Province
            {
                Name = "Limburg",
                Coordinates = new []
                {
                    new Coordinate(6.083679f, 51.5293551f),
                    new Coordinate(5.8331909f, 50.8201139f)
                }
            },
            new Province
            {
                Name = "Overijssel",
                Coordinates = new []
                {
                    new Coordinate(5.9395826f, 52.7892523f),
                    new Coordinate(6.8012235f, 52.1679812f)
                }
            },
            new Province
            {
                Name = "Utrecht",
                Coordinates = new []
                {
                    new Coordinate(4.8656807f, 52.2680444f),
                    new Coordinate(5.5844474f, 51.9656554f)
                }
            },
            new Province
            {
                Name = "Zeeland",
                Coordinates = new []
                {
                    new Coordinate(3.3985632f, 51.2855762f),
                    new Coordinate(4.2162428f, 51.6894087f)
                }
            }
        };

        public class Province
        {
            public string Name { get; set; }
            public Coordinate[] Coordinates { get; set; }

            public Province()
            { }

            public Province(string name, Coordinate[] coordinates)
            {
                Name = name;
                Coordinates = coordinates;
            }
        }
    }
}
