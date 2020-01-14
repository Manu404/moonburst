using System;
using System.IO;
using System.Xml.Serialization;
using Microsoft.Win32;
using MoonBurst.Model;

namespace MoonBurst.Core
{
    public class DataSerializer<T> where T : IFileSerializableType, new()
    {
        public static void SaveToFile(T data)
        {
            if (String.IsNullOrEmpty(data.Path)) SaveDefault(data);
            else XmlFileSerializer<T>.Save(data.Path, data);
        }

        public static T LoadFromFile(string path)
        {
            if (String.IsNullOrEmpty(path)) return LoadDefault();
            var newObject = XmlFileSerializer<T>.Load(path);
            newObject.Path = path;
            return newObject;
        }

        public static T LoadDefault()
        {
            return LoadFromFile(new T().Default);
        }

        public static void SaveDefault(T data)
        {
            data.Path = data.Default;
            SaveToFile(data);
        }
    }

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
                    return (T) serializer.Deserialize(stream);
                }
            }
            catch (Exception ex)
            {
                return new T();
            }
        }
    }
}