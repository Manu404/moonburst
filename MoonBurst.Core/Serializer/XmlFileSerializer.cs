using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Serialization;

namespace MoonBurst.Core.Serializer
{
    public class XmlFileSerializer<T> where T : new()
    {
        public static void Save(string filename, T obj)
        {
            using (var fs = new FileStream(filename, FileMode.Create))
            {
                Type[] extraTypes = (typeof(T)).GetProperties()
                    .Where(p => p.PropertyType.IsInterface)
                    .Select(p => p.GetValue(obj, null).GetType())
                    .ToArray();

                DataContractSerializer serializer = new DataContractSerializer(typeof(T), extraTypes);
                StringWriter sw = new StringWriter();
                XmlTextWriter xw = new XmlTextWriter(sw);
                serializer.WriteObject(fs, obj);
                fs.Close();
                //return XElement.Parse(sw.ToString());
            }
        }

        public static T Load(string path)
        {
            try
            {
                using (var stream = new FileStream(path, FileMode.Open))
                {
                    DataContractSerializer serializer = new DataContractSerializer(typeof(T));
                    StringWriter sw = new StringWriter();
                    XmlTextWriter xw = new XmlTextWriter(sw);
                    return (T)serializer.ReadObject(stream);
                }
            }
            catch (Exception)
            {
                return new T();
            }
        }
    }
}