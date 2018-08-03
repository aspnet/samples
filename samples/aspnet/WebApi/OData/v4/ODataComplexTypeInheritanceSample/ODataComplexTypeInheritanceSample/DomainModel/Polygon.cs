using System.Collections.Generic;

namespace ODataComplexTypeInheritanceSample
{
    public class Polygon : Shape
    {
        public IList<Point> Vertexes { get; set; }
        public Polygon()
        {
            Vertexes = new List<Point>();
        }
    }
}
