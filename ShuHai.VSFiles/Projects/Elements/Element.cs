using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace ShuHai.VSFiles.Projects.Elements
{
    /// <summary>
    ///     A wrapper class that represents a special case of <see cref="XElement" /> in VS projects.
    /// </summary>
    public class Element
    {
        public static Element Create(Project project, XElement xml)
        {
            switch (xml.Name.LocalName)
            {
                case "Project":
                    return new Root(project, xml);
                case "PropertyGroup":
                    return new PropertyGroup(project, xml);
                case "ItemGroup":
                    return new ItemGroup(project, xml);
                case "Import":
                    return new Import(project, xml);
                default:
                    return new Element(project, xml);
            }
        }

        public readonly Project Project;

        public readonly XElement Xml;

        public string Name => Xml.Name.LocalName;

        public string Value { get => Xml.Value; set => Xml.Value = value; }

        public Element(Project project, XElement xml)
        {
            Project = project ?? throw new ArgumentNullException(nameof(project));
            Xml = xml ?? throw new ArgumentNullException(nameof(xml));

            xml.Changed += OnXmlChanged;

            ResetConditions();

            CreateChildren();
            instances.Add(xml, this);
        }

        private Dictionary<XElement, Element> instances => Project.xmlElements;

        private void OnXmlChanged(object sender, XObjectChangeEventArgs args)
        {
            var element = sender as XElement;
            switch (args.ObjectChange)
            {
                case XObjectChange.Add:
                    if (element != null)
                        Create(Project, element);
                    break;
                case XObjectChange.Name:
                    break;
                case XObjectChange.Remove:
                    if (element != null)
                        instances.Remove(element);
                    break;
                case XObjectChange.Value:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        #region Attributes

        #region Condition

        public Conditions Conditions
        {
            get => conditions;
            set
            {
                if (value != null)
                {
                    if (conditionAttribute == null)
                    {
                        conditionAttribute = new XAttribute(ConditionAttributeName, value.ValuesString);
                        Xml.Add(conditionAttribute);
                    }
                    else
                    {
                        conditionAttribute.Value = value.ValuesString;
                    }
                }
                else
                {
                    conditionAttribute?.Remove();
                    conditionAttribute = null;
                }
                conditions = value;
            }
        }

        private Conditions conditions;

        private XAttribute conditionAttribute;

        private const string ConditionAttributeName = "Condition";

        private void ResetConditions()
        {
            ResetConditionAttribute();

            if (conditionAttribute != null)
                Conditions.TryParse(conditionAttribute.Value, out conditions);
            else
                conditions = null;
        }

        private void ResetConditionAttribute() { conditionAttribute = Xml.Attribute(ConditionAttributeName); }

        #endregion Condition

        #endregion Attributes

        #region Hierarchy

        public Element Parent => Xml.Parent != null ? instances[Xml.Parent] : null;

        public bool HasChildren => Xml.HasElements;

        public T Child<T>(string name) where T : Element
        {
            var xmlElement = Xml.Element(name);
            var projElement = xmlElement != null ? instances[xmlElement] : null;
            return projElement as T;
        }

        public T Child<T>(Func<T, bool> predicate) where T : Element => Children<T>().FirstOrDefault(predicate);

        public IEnumerable<T> Children<T>() where T : Element
            => Children().Select(e => e as T).Where(e => e != null);

        public IEnumerable<T> Children<T>(Func<T, bool> predicate) where T : Element
            => Children<T>().Where(predicate);

        public Element Child(string name)
        {
            var xmlElement = Xml.Element(name);
            return xmlElement != null ? instances[xmlElement] : null;
        }

        public Element Child(Func<Element, bool> predicate) => Children().FirstOrDefault(predicate);

        public IEnumerable<Element> Children() => Xml.Elements().Select(e => instances[e]);

        public IEnumerable<Element> Children(Func<Element, bool> predicate)
            => Children().Where(predicate);

        public void SetChild(string name, string value)
        {
            var child = Xml.Element(name);
            if (child == null)
            {
                child = new XElement(name);
                Xml.Add(child);
            }
            child.Value = value;
        }

        public void AddChild(string name, string value) { Xml.Add(new XElement(name, value)); }

        private void CreateChildren()
        {
            foreach (var e in Xml.Elements())
                Create(Project, e);
        }

        #endregion Hierarchy

        #region Utilities

        protected string GetAttributeValue(ref XAttribute attribute, string name)
        {
            attribute = attribute ?? Xml.Attribute(name);
            return attribute?.Value;
        }

        protected void SetAttributeValue(ref XAttribute attribute, string name, string value)
        {
            if (value != null)
            {
                if (attribute == null)
                {
                    attribute = new XAttribute(name, value);
                    Xml.Add(attribute);
                }
                else
                {
                    attribute.Value = value;
                }
            }
            else
            {
                attribute?.Remove();
                attribute = null;
            }
        }

        #endregion Utilities
    }
}