using System.Collections.Generic;
using System.Xml.Linq;
using ShuHai.VSFiles.Projects.Elements;

namespace ShuHai.VSFiles.Projects
{
    public class Project
    {
        public string Name { get; private set; }

        public string Path { get; private set; }

        public Project(string path) { Load(path); }

        public Project(XElement root, string name = null) { Load(root, name); }

        public override string ToString() { return Path; }

        #region Xml

        public Root XmlRoot { get; private set; }

        private XDocument XmlDocument;

        internal readonly Dictionary<XElement, Element> xmlElements = new Dictionary<XElement, Element>();

        #endregion Xml

        #region Persistency

        public void Load(string path)
        {
            Path = path;
            Name = System.IO.Path.GetFileNameWithoutExtension(path);
            XmlDocument = XDocument.Load(path);
            XmlRoot = new Root(this, XmlDocument.Root);
        }

        public void Load(XElement root, string name = null)
        {
            Path = string.Empty;
            Name = name ?? string.Empty;
            XmlDocument = new XDocument(root);
            XmlRoot = new Root(this, XmlDocument.Root);
        }

        public void Save() { XmlDocument.Save(Path); }

        public void Save(string path) { XmlDocument.Save(path); }

        #endregion Persistency
    }
}