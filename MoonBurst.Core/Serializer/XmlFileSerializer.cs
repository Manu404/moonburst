using System;
using System.IO;
using System.Xml.Serialization;

namespace MoonBurst.Core.Serializer
{
    public class XmlFileSerializer<T> where T : new()
    {
        public static void Save(string filename, T obj)
        {
            using (var fs = new FileStream(filename, FileMode.Create))
            {
                var serializer = new XmlSerializer(typeof(T));
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
                    var serializer = new XmlSerializer(typeof(T));
                    return (T)serializer.Deserialize(stream);
                }
            }
            catch (Exception)
            {
                return new T();
            }
        }
    }
}