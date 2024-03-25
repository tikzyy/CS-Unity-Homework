using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Newtonsoft.Json;


public class DataManager : MonoBehaviour
{
    private string _dataPath;
    private string _xmlGroupMembers;
    private string _jsonMembers;
    
    private string _state;

    // list with names, birthdate and favourite colour
    private readonly List<GroupMember> _groupMember = new List<GroupMember>
    {
        new GroupMember("memberOne", 2001, "Lilac"),
        new GroupMember("memberTwo", 1999, "blue"),
        new GroupMember("memberThree", 2003, "yellow"),
        new GroupMember("memberFour", 2004, "orange"),
        new GroupMember("memberFive", 2000, "gray"),
        new GroupMember("memberSix", 1997, "red"),
    };
    
    void Awake()
    {
        // _dataPath = Application.persistentDataPath + "/Player_Data/";
        
        // created a new directory in root folder "/Logs/" in order to easily access the XML file
        _dataPath = Directory.GetCurrentDirectory() + "/Logs/Files/XLM_json_Files/";

        Debug.Log(_dataPath);
        
        _xmlGroupMembers = _dataPath + "XMLMembers.xml";
        _jsonMembers = _dataPath + "JsonMembers.json";
    }

    public void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        _state = "Data Manager initialized..";
        Debug.Log(_state);

        FileSystemInfo();
        NewDirectory();
    }

    private void FileSystemInfo()
    {
        Debug.LogFormat("Path separator character: {0}", Path.PathSeparator);
        Debug.LogFormat("Directory separator character: {0}", Path.DirectorySeparatorChar);
        Debug.LogFormat("Current directory: {0}",
            Directory.GetCurrentDirectory());
        Debug.LogFormat("Temporary path: {0}", Path.GetTempPath());
    }

    private void NewDirectory()
    {
        if (Directory.Exists(_dataPath))
        {
            Debug.Log("Directory already exists, creating XML file");
            WriteToXML(_xmlGroupMembers);
            return;
        }

        Directory.CreateDirectory(_dataPath);
        Debug.Log("New directory created!");
        Debug.Log(_dataPath);
    }
    
    private void WriteToXML(string filename)
    {
        if (!File.Exists(filename))
        {
            FileStream xmlStream = File.Create(filename);
            XmlWriter xmlWriter = XmlWriter.Create(xmlStream);

            xmlWriter.WriteStartDocument();
            xmlWriter.WriteStartElement("GroupMembers");
            foreach (var i in _groupMember)
            {
                xmlWriter.WriteStartElement(i.name);
                xmlWriter.WriteElementString("BirthYear", i.birthYear.ToString());
                xmlWriter.WriteElementString("FavouriteColour", i.favColour);
                xmlWriter.WriteEndElement();
            }

            xmlWriter.WriteEndElement();
            xmlWriter.Close();
            xmlStream.Close();
            Debug.Log("XML File created");
        }
        else
        {
            Debug.Log("XML File already exists; Deserializing XML and serializing as json");
            if (!File.Exists(_jsonMembers))
            {
                XMLtoJson();
            }
        }
    }
    
    // I used ChatGPT for a few things in this portion as I had issues using the deserializing the XML properly -
    // I think the issue might be that some of the names in the XML file and for my variables overlap
    // but on further testing unfortunately that did not solve my issue
    // I genuinely am at loss for what have gone wrong
    //
    // update: upon further research I have decided to abandon the DeserializeXML method as it seems redundant. Instead, I will utilise another method.
    
    // private void DeserializeXML()
    // {
    //     if (!File.Exists(_xmlGroupMembers)) return;
    //     var xmlSerializer = new XmlSerializer(typeof(GroupMembersWrapper)); // Wrapper class (GPT contribution)

    //     using var stream = File.OpenRead(_xmlGroupMembers);
    //    var wrapper = (GroupMembersWrapper)xmlSerializer.Deserialize(stream);
    //    SerializeJson("placeholder");
    // }

    // [XmlRoot("GroupMembers")] // (GPT contribution)
    // public class GroupMembersWrapper
    // {
    //     [XmlElement("member")]
    //     public List<GroupMember> members { get; set; }
    // }

    // <summary>
    // these new methods simply reads the xml file as a string, "loads" it as using LoadXml and thereafter converts it to json
    // by initialising a string (jsonText) using JsonConvert.SerializeXmlNode from Newtonsoft.Json.
    // Finally the SerializeJson method is called, which simply creates the Json using the aforementioned data.
    // <summary>
    
    private void XMLtoJson()
    {
        string xml = File.ReadAllText(_xmlGroupMembers);
        
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(xml);
        
        string jsonText = JsonConvert.SerializeXmlNode(xmlDoc);
        
        File.WriteAllText(_jsonMembers, jsonText);
        Debug.LogFormat("json file created!");
    }
}
