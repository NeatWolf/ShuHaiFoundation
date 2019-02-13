using System.Xml.Linq;

namespace ShuHai.VSFiles.Projects.Elements
{
    public class PropertyGroup : Group
    {
        public PropertyGroup(Project project, XElement xml) : base(project, xml) { }
    }
}