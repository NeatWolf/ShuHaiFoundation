using System.Xml.Linq;

namespace ShuHai.VSFiles.Projects.Elements
{
    public abstract class Group : Element
    {
        public new Root Parent => (Root)base.Parent;

        protected Group(Project project, XElement xml) : base(project, xml) { }
    }
}