using System;
using System.IO;
using System.Xml.Serialization;
using MoonBurst.Model;

namespace MoonBurst.Core
{
    public class SerializerBase<T, Y> : ISerializer<T, Y> where T : IFileSerializableType where Y : new()
    {
        public virtual string Default { get; }

        public void Load(string path, T target)
        {
            ApplyData(XmlFileSerializer<Y>.Load(path ?? Default), target);
            target.CurrentPath = path ?? Default;
        }

        public void LoadDefault(T target)
        {
            Load(Default, target);
        }

        public void Save(T source, string path)
        {
            XmlFileSerializer<Y>.Save(path ?? Default, ExtractData(source));
            source.CurrentPath = path;
        }

        public void SaveDefault(T source)
        {
            Save(source, Default);
        }

        public virtual Y ExtractData(T source)
        {
            return new Y();
        }

        public virtual void ApplyData(Y source, T target)
        {

        }
    }
}