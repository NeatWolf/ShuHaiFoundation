using System.Xml.Linq;

namespace ShuHai.VSFiles.Projects.Elements
{
    public class Import : Element
    {
        public Import(Project project, XElement xml) : base(project, xml) { }

        #region Attributes

        public string Project
        {
            get => GetAttributeValue(ref projectAttribute, ProjectAttributeName);
            set => SetAttributeValue(ref projectAttribute, ProjectAttributeName, value);
        }

        private XAttribute projectAttribute;

        private const string ProjectAttributeName = "Project";

        #endregion Attributes
    }
}