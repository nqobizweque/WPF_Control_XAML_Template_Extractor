using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml;

namespace ControlTemplateExtractor
{
    static class ControlCollections
    {
        private static Type[] GetAssemblyControls(string _assemblyPath, string _namespace)
        {
            return Assembly.LoadFile($@"{_assemblyPath}").GetTypes().Where(t => string.Equals(t.Namespace, _namespace, StringComparison.Ordinal)).ToArray();
        }

        public static List<Type> GetConfigControls()
        {
            List<Type> types = new List<Type>();

            XmlDocument config = new XmlDocument();
            config.Load("Config.xml");
            foreach (XmlNode node in config.DocumentElement.ChildNodes)
            {
                var _assembly = node.FirstChild.InnerText;
                var _namespace = node.LastChild.InnerText;
                types.AddRange(GetAssemblyControls(_assembly, _namespace));
            }
            return types;
        }

        
    }
}
