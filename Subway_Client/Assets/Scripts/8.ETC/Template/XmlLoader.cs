using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UnityEngine;

public class XmlLoader : MonoSingleton<XmlLoader>
{
    FileInfo fileInfo;
    XmlDocument xmlDoc;
    StreamReader reader;
    bool isInitialized = false;
    string path = string.Empty;

    public void Init()
    {
        path = Directory.GetParent(Application.dataPath).FullName;
        fileInfo = new FileInfo(path + "/ConnectionInfo.xml");
        reader = new StreamReader(fileInfo.Name);
        xmlDoc = new XmlDocument();

        xmlDoc.LoadXml(reader.ReadToEnd());
        isInitialized = true;
    }

    public string XmlLoad(string key, string parentNode = "root")
    {
        if (!isInitialized)
            Init();

        if (fileInfo.Exists)
        {
            string value = xmlDoc.SelectSingleNode(parentNode + "/" + key).InnerText;
            reader.Close();
            return value;
        }
        else
        {
            return string.Empty;
        }
    }

    public void XmlSave(string key, string value, string parentNode = "root")
    {
        if (!isInitialized)
            Init();

        if (xmlDoc.SelectSingleNode(parentNode + "/" + key) == null)
        {
            AddXmlNode(key, value, parentNode);
        }
        else
        {
            ModifyXmlValue(key, value, parentNode);
        }
    }


    private void AddXmlNode(string newNode, string value, string parentNode)
    {
        XmlNode xmlNode = xmlDoc.SelectSingleNode(parentNode);
        XmlElement xmlEle = xmlDoc.CreateElement(newNode);

        xmlNode.AppendChild(xmlEle);
        xmlEle.InnerText = value;

        reader.Close();

        xmlDoc.Save(fileInfo.Name);
    }

    private void ModifyXmlValue(string node, string value, string parentNode)
    {
        XmlNode xmlNode = xmlDoc.SelectSingleNode(parentNode + "/" + node);

        xmlNode.InnerText = value;

        reader.Close();

        xmlDoc.Save(fileInfo.Name);
    }
}