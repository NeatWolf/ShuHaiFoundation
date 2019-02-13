using System;
using System.IO;
using ShuHai.VSFiles.Projects.Elements;

namespace ShuHai.VSFiles.Projects
{
    public class Resolver
    {
        /// <summary>
        ///     Resolve full output path of specified <see cref="PropertyGroup" />.
        /// </summary>
        /// <param name="projectPath">Path of the owner project.</param>
        /// <param name="propertyGroup">Conditional property group that represents a project configurations.</param>
        public static string ResolveOutputPath(string projectPath, PropertyGroup propertyGroup)
        {
            if (propertyGroup == null)
                throw new ArgumentNullException(nameof(propertyGroup));

            var subPath = "bin";
            var prop = propertyGroup.Child(PropertyNames.OutputPath);
            if (prop != null)
            {
                subPath = prop.Value;
            }
            else
            {
                var baseProp = propertyGroup.Child(PropertyNames.BaseOutputPath)
                    ?? propertyGroup.Parent.DefaultPropertyGroup.Child(PropertyNames.BaseOutputPath);

                if (baseProp != null)
                {
                    if (propertyGroup.Conditions.TryGetValue(Condition.Names.Configuration, out var configuration))
                        subPath = baseProp.Value + "\\" + configuration;
                    else
                        subPath = baseProp.Value;
                }
            }

            var dir = Path.GetDirectoryName(projectPath);
            return Path.Combine(dir, subPath);
        }
    }
}