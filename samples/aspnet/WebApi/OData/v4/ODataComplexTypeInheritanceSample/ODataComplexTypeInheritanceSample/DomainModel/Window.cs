using System.Collections.Generic;

namespace ODataComplexTypeInheritanceSample
{
    public class Window
    {
        public Window()
        {
            OptionalShapes = new List<Shape>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public Window Parent { get; set; }
        public Shape CurrentShape { get; set; }
        public IList<Shape> OptionalShapes { get; set; }
    }
}
