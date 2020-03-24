namespace MoonBurst.Core.Serializer
{
    public class SerializerBase<T, TY> : ISerializer<T, TY> where T : IFileSerializableType where TY : new()
    {
        public SerializerBase(string defaultPath)
        {
            Default = defaultPath;
        }

        private string Default { get; }

        public void Load(string path, T target)
        {
            ApplyData(XmlFileSerializer<TY>.Load(path ?? Default), target);
            target.CurrentPath = path ?? Default;
        }

        public void LoadDefault(T target)
        {
            Load(Default, target);
        }

        public void Save(T source, string path)
        {
            XmlFileSerializer<TY>.Save(path ?? Default, ExtractData(source));
            source.CurrentPath = path;
        }

        public void SaveDefault(T source)
        {
            Save(source, Default);
        }

        public virtual TY ExtractData(T source)
        {
            return new TY();
        }

        public virtual void ApplyData(TY source, T target)
        {

        }

        public object GetData(T source)
        {
            return ExtractData(source);
        }
    }
}