using NetTopologySuite.Geometries;
using System.ComponentModel.DataAnnotations.Schema;

namespace spatial
{
    [Table("City")]
    public class City
    {
        public int ID { get; set; }

        public string CityName { get; set; }

        public Point Location { get; set; }
    }
}
