using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace ShuHai.VSFiles.Projects.Elements
{
    public class Root : Element
    {
        public Root(Project project, XElement xml) : base(project, xml)
        {
            defaultPropertyGroup = new Lazy<PropertyGroup>(FindDefaultPropertyGroup);
        }

        #region Attributes

        public string ToolsVersion
        {
            get => GetAttributeValue(ref toolsVersionAttribute, ToolsVersionAttributeName);
            set => SetAttributeValue(ref toolsVersionAttribute, ToolsVersionAttributeName, value);
        }

        public string Sdk
        {
            get => GetAttributeValue(ref sdkAttribute, SdkAttributeName);
            set => SetAttributeValue(ref sdkAttribute, SdkAttributeName, value);
        }

        private XAttribute toolsVersionAttribute;
        private XAttribute sdkAttribute;

        private const string ToolsVersionAttributeName = "ToolsVersion";
        private const string SdkAttributeName = "Sdk";

        #endregion Attributes

        #region Elements

        #region Properties

        public string PropertyValue(string name) => Property(name)?.Value;

        public Element Property(string name) => Property(e => e.Name == name);

        public Element Property(Func<Element, bool> predicate) => Properties().FirstOrDefault(predicate);

        public IEnumerable<Element> Properties() => PropertyGroups().SelectMany(g => g.Children());

        public IEnumerable<Element> Properties(Func<Element, bool> predicate)
            => Properties().Where(predicate);

        #region Groups

        #region Default

        /// <summary>
        ///     The property group that contains one or more of the following properties: &lt;ProjectGuid&gt;,
        ///     &lt;TargetFramework&gt;, &lt;OutputType&gt;, &lt;RootNamespace&gt;, etc.
        /// </summary>
        public PropertyGroup DefaultPropertyGroup => defaultPropertyGroup.Value;

        private readonly Lazy<PropertyGroup> defaultPropertyGroup;

        private PropertyGroup FindDefaultPropertyGroup()
        {
            return PropertyGroup(g => g.Children()
                .Any(e => e.Name == "ProjectGuid" || e.Name == "TargetFramework"));
        }

        #endregion Default

        public PropertyGroup PropertyGroup(Func<PropertyGroup, bool> predicate) => Child(predicate);

        public IEnumerable<PropertyGroup> PropertyGroups() => Children<PropertyGroup>();

        public IEnumerable<PropertyGroup> PropertyGroups(Condition condition)
            => PropertyGroups(g => g.Conditions != null && g.Conditions.Contains(condition));

        public IEnumerable<PropertyGroup> PropertyGroups(Conditions conditions)
            => PropertyGroups(g => g.Conditions == conditions);

        public IEnumerable<PropertyGroup> PropertyGroups(Func<PropertyGroup, bool> predicate)
            => Children<PropertyGroup>().Where(predicate);

        #endregion Groups

        #endregion Properties

        #region Items

        public Element Item(Func<Element, bool> predicate) => Items().FirstOrDefault(predicate);

        public IEnumerable<Element> Items() => ItemGroups().SelectMany(g => g.Children());

        public IEnumerable<Element> Items(Func<Element, bool> predicate) => Items().Where(predicate);

        #region Groups

        public ItemGroup ItemGroup(Func<ItemGroup, bool> predicate) => Child(predicate);

        public IEnumerable<ItemGroup> ItemGroups() => Children<ItemGroup>();

        public IEnumerable<ItemGroup> ItemGroups(Condition condition)
            => ItemGroups(g => g.Conditions != null && g.Conditions.Contains(condition));

        public IEnumerable<ItemGroup> ItemGroups(Conditions conditions)
            => ItemGroups(g => g.Conditions == conditions);

        public IEnumerable<ItemGroup> ItemGroups(Func<ItemGroup, bool> predicate)
            => Children<ItemGroup>().Where(predicate);

        #endregion Groups

        #endregion Items

        #endregion Elements
    }
}