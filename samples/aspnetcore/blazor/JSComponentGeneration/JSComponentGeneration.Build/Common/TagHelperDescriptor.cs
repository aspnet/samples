namespace JSComponentGeneration.Build.Common
{
    internal class TagHelperDescriptor
    {
        public string Kind { get; set; }
        public string Name { get; set; }
        public BoundAttributeDescriptor[] BoundAttributes { get; set; }
    }
}
