using System.Xml.Linq;

namespace ShuHai.VSFiles.Projects.Elements
{
    public class ItemGroup : Group
    {
        public ItemGroup(Project project, XElement xml) : base(project, xml) { }
    }
}