using System;

namespace TPF.Demo.Net461.Controls
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class SearchMetadataAttribute : Attribute
    {
        public string Name { get; set; }

        public SearchMetadataAttribute(string name)
        {
            Name = name;
        }
    }
}