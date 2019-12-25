using System;
using System.IO;
using System.Xml.Serialization;
using Microsoft.Win32;
using MoonBurst.Model;

namespace MoonBurst.ViewModel
{
    public class DataSerializer<T, TY> where T : IFileSerializableType<TY>, new()
        where TY : IFileSerializableData, new()
    {
        public static void SaveToFile(T data)
        {
            XmlFileSerializer<TY>.Save(data.Path, (TY)data.GetData());
        }

        public static T LoadFromFile(string path)
        {
            if (String.IsNullOrEmpty(path)) return LoadDefault();
            var newObject = (T)new T().CreateFromData(XmlFileSerializer<TY>.Load(path),  path);
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

    public class XmlFileSerializer<T> where T : IFileSerializableData, new()
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