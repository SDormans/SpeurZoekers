using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Speurzoekers.Common.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Speurzoekers.Common.Domain
{
    public class Region
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public float Longitude { get; set; }
        public float Latitude { get; set; }
        public string Polygon { get; set; }
        public Biotope Biotope { get; set; }
        public int MinPositions { get; set; }
        public int MaxPositions { get; set; }

        public Guid? ParentId { get; set; }
        public List<Region> Childeren { get; set; }

        public Coordinate[] PolygonVertices
        {
            get
            {
                if (_polygonVertices == null)
                {
                    var vertices = JsonConvert.DeserializeObject<JArray>(Polygon);
                    _polygonVertices = vertices
                        .Select(vertex => new Coordinate(vertex.Last().Value<float>(), vertex.First().Value<float>()))
                        .ToArray();
                }

                return _polygonVertices;
            }
            set
            {
                _polygonVertices = value;

                IEnumerable<string> values = value
                    .Select(p => $"[{p.ToStringInverted()}]");
                Polygon = $"[{string.Join(",", values)}]";
            }
        }

        private Coordinate[] _polygonVertices;
    }
}
