using System;
using System.IO;
using System.Xml.Serialization;

namespace MoonBurst.Core
{
    public class XmlFileSerializer<T> where T : new()
    {
        public static void Save(string filename, T obj)
        {
            using (FileStream fs = new FileStream(filename, FileMode.Create))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                serializer.Serialize(fs, obj);
                fs.Close();
            }
        }

        public static T Load(string path)
        {
            try
            {
                using (var stream = new StreamReader(path))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(T));
                    return (T)serializer.Deserialize(stream);
                }
            }
            catch (Exception ex)
            {
                return new T();
            }
        }
    }
}