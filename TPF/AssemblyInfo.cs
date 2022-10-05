using System.Windows;
using System.Windows.Markup;

// Namespaces als ein XAML-Namespace vereinen
// Um weitere Namespaces hinzuzufügen muss jeweils eine XmlnsDefinition hinzugefügt werden
[assembly: XmlnsDefinition("http://schemas.tpf.com/xaml/presentation", "TPF.Controls")]
[assembly: XmlnsDefinition("http://schemas.tpf.com/xaml/presentation", "TPF.MarkupExtensions")]
[assembly: XmlnsDefinition("http://schemas.tpf.com/xaml/presentation", "TPF.Converter")]
[assembly: XmlnsDefinition("http://schemas.tpf.com/xaml/presentation", "TPF.DragDrop")]
[assembly: XmlnsDefinition("http://schemas.tpf.com/xaml/presentation", "TPF.DragDrop.Behaviors")]
[assembly: XmlnsPrefix("http://schemas.tpf.com/xaml/presentation", "tpf")]

[assembly: ThemeInfo(
    ResourceDictionaryLocation.None, //where theme specific resource dictionaries are located
                                     //(used if a resource is not found in the page,
                                     // or application resource dictionaries)
    ResourceDictionaryLocation.SourceAssembly //where the generic resource dictionary is located
                                              //(used if a resource is not found in the page,
                                              // app, or any theme specific resource dictionaries)
)]
