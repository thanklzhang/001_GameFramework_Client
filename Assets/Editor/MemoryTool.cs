using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;
using System.Text;
using LitJson;
using System;
using System.Runtime.Serialization.Formatters.Binary;

public class MemoryTool
{
    public static T DeepCopyByBinary<T>(T obj)
    {
        object retval;
        using (MemoryStream ms = new MemoryStream())
        {
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(ms, obj);
            ms.Seek(0, SeekOrigin.Begin);
            retval = bf.Deserialize(ms);
            ms.Close();
        }
        return (T)retval;
    }

    //public static T DeepCopyByBinary<T>(T obj)
    //{
    //    System.Xml.Serialization.XmlSerializer xmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
    //    System.IO.MemoryStream stream = new System.IO.MemoryStream();
    //    xmlSerializer.Serialize(stream, obj);
    //    //主要是把网络上的添加了转成字符串的过程
    //    string temp = System.Text.Encoding.Default.GetString(stream.ToArray());
    //    stream = new System.IO.MemoryStream(System.Text.Encoding.Default.GetBytes(temp));
    //    System.Xml.XmlReaderSettings xmlReaderSettings = new System.Xml.XmlReaderSettings();
    //    xmlReaderSettings.IgnoreComments = true;
    //    System.Xml.XmlReader xmlReader = System.Xml.XmlReader.Create(stream, xmlReaderSettings);
    //    if (xmlReader != null)
    //    {
    //        T t = (T)xmlSerializer.Deserialize(xmlReader);
    //        return t;
    //    }
    //    return default(T);
    //}



}
