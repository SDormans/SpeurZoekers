using Speurzoekers.Common.Domain;
using Speurzoekers.Common.Domain.Enums;
using Speurzoekers.Data;
using Speurzoekers.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataSeeder
{
    /// <summary>
    /// Use https://www.latlong.net/ to determine coords
    /// </summary>
    static class RegionSeeder
    {
        public static Task SeedRegion(SpeurzoekersDbContext context)
        {
            var result = new List<Task>
            {
                context.Regions.AddAsync(new RegionEntity
                {
                    Name = "Nationaal Park De Meinweg",
                    Biotope = Biotope.Bos,
                    Latitude = 51.162936f,
                    Longitude = 6.122879f,
                    MinPositions = 8,
                    MaxPositions = 15,
                    Polygon =
                        "[[51.169956,6.090256],[51.194060, 6.165461],[51.186412, 6.180612],[51.173310, 6.139085],[51.158342, 6.175228],[51.141748, 6.117990]]"
                }),
                context.Regions.AddAsync(new RegionEntity
                {
                    Name = "Goffertpark Nijmegen",
                    Biotope = Biotope.Park,
                    Latitude = 51.824314f,
                    Longitude = 5.835259f,
                    MinPositions = 8,
                    MaxPositions = 15,
                    Polygon =
                        "[[51.825322, 5.827454],[51.829715, 5.834389],[51.823492, 5.841044],[51.818521, 5.839689]]"
                }),
                context.Regions.AddAsync(new RegionEntity
                {
                    Name = "Werk",
                    Biotope = Biotope.Stad,
                    Latitude = 51.816013f,
                    Longitude = 5.824441f,
                    MinPositions = 4,
                    MaxPositions = 8,
                    Polygon = new Coordinate(5.824441f, 51.816013f)
                        .GetRectangleString(0.015f, 0.009f)
                }),
                context.Regions.AddAsync(new RegionEntity
                {
                    Name = "Test",
                    Biotope = Biotope.Water,
                    Latitude = 51.759083f,
                    Longitude = 5.860751f,
                    MinPositions = 3,
                    MaxPositions = 7,
                    Polygon =
                        "[[51.759083, 5.860751],[51.754480, 5.759797],[51.753072, 5.759454],[51.757064, 5.865678],[51.801059, 5.821813],[51.790832, 5.835075],[51.790281, 5.833691],[51.801468, 5.819389]]"
                }),
                context.Regions.AddAsync(new RegionEntity
                {
                    Name = "Stadspark Veenendaal",
                    Biotope = Biotope.Park,
                    Latitude = 52.022472f,
                    Longitude = 5.564472f,
                    MinPositions = 12,
                    MaxPositions = 20,
                    Polygon =
                        "[[52.022472, 5.564472],[52.020616, 5.564433],[52.020332, 5.565270],[52.019942, 5.564744],[52.019651, 5.564969],[52.018667, 5.564433],[52.018509, 5.564497],[52.017116, 5.563424],[52.016575, 5.565205],[52.015897, 5.566421],[52.016815, 5.567172],[52.016274, 5.568889],[52.016835, 5.569329],[52.017007, 5.569007],[52.017568, 5.569576],[52.018413, 5.567065],[52.018103, 5.566722],[52.018638, 5.565359],[52.021220, 5.567827],[52.022052, 5.565842]]"
                }),
                context.Regions.AddAsync(new RegionEntity
                {
                    Name = "Nijmegen Hatert",
                    Biotope = Biotope.Bos,
                    Latitude = 51.806518f,
                    Longitude = 5.818097f,
                    MinPositions = 8,
                    MaxPositions = 15,
                    Polygon =
                        "[[51.806518, 5.818097],[51.798758, 5.825858],[51.805103, 5.839856],[51.810788, 5.830835]]"
                }),
                context.Regions.AddAsync(new RegionEntity
                {
                    Name = "Nijmegen Centrum e.o.",
                    Biotope = Biotope.Stad,
                    Latitude = 51.850520f,
                    Longitude = 5.856484f,
                    MinPositions = 8,
                    MaxPositions = 15,
                    Polygon =
                        "[[51.850520, 5.856484],[51.850227, 5.831372],[51.836479, 5.814760],[51.826637, 5.857607],[51.853241, 5.870309]]"
                }),
                context.Regions.AddAsync(new RegionEntity
                {
                    Name = "Nijmegen West",
                    Biotope = Biotope.Weide,
                    Latitude = 51.837982f,
                    Longitude = 5.803091f,
                    MinPositions = 8,
                    MaxPositions = 15,
                    Polygon =
                        "[[51.837982, 5.803091],[51.846147, 5.761794],[51.777871, 5.740859],[51.806041, 5.814993]]"
                }),
                context.Regions.AddAsync(new RegionEntity
                {
                    Name = "Arnhem",
                    Biotope = Biotope.Bos,
                    Latitude = 51.940383f,
                    Longitude = 5.811036f,
                    MinPositions = 15,
                    MaxPositions = 30,
                    Polygon =
                        "[[51.940383, 5.811036],[51.926665, 5.879059],[51.969637, 5.995992],[52.017576, 5.925745],[52.026316, 5.827743]]"
                })
            };
            return Task.WhenAll(result);
        }
    }
}
