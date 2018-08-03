namespace ODataComplexTypeInheritanceSample
{
    public class Circle : Shape
    {
        public Point Center { get; set; }
        public int Radius { get; set; }

        public override string ToString()
        {
            // {centerX, centerY, radius}
            return "{" + Center.X + "," + Center.Y + "," + Radius + "}";
        }
    }
}
