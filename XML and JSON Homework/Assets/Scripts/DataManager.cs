using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

public class DataManager : MonoBehaviour
{
    private string _dataPath;
    private string _xmlGroupMembers;
    private string _jsonMembers;
    
    private string _state;

    // list with names, birthdate and favourite colour
    private readonly List<GroupMember> _GroupMember = new List<GroupMember>
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
        
        _xmlGroupMembers = _dataPath + "GroupMember_Data.xml";
        _jsonMembers = _dataPath + "JSONMembers.json";
    }

    void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        _state = "Data Manager initialized..";
        Debug.Log(_state);

        FileSystemInfo();
        NewDirectory();
    }

    public void FileSystemInfo()
    {
        Debug.LogFormat("Path separator character: {0}", Path.PathSeparator);
        Debug.LogFormat("Directory separator character: {0}", Path.DirectorySeparatorChar);
        Debug.LogFormat("Current directory: {0}",
            Directory.GetCurrentDirectory());
        Debug.LogFormat("Temporary path: {0}", Path.GetTempPath());
    }

    public void NewDirectory()
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
    
    // not used
    public void WriteToXML(string filename)
    {
        if (!File.Exists(filename))
        {
            FileStream xmlStream = File.Create(filename);
            XmlWriter xmlWriter = XmlWriter.Create(xmlStream);

            xmlWriter.WriteStartDocument();
            xmlWriter.WriteStartElement("GroupMembers");
            foreach (var i in _GroupMember)
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
                DeserializeXML();
            }
        }
    }
    
    // I used ChatGPT for a few things in this portion as I had issues using the deserializing the XML properly -
    // I think the issue might be that some of the names in the XML file and for my variables overlap
    // but on further testing unfortunately that did not solve my issue
    // I genuinely am at loss for what have gone wrong
    public void DeserializeXML()
    {
        if (File.Exists(_xmlGroupMembers))
        {
            var xmlSerializer = new XmlSerializer(typeof(GroupMembersWrapper)); // Wrapper class (GPT contribution)

            using FileStream stream = File.OpenRead(_xmlGroupMembers);
            var wrapper = (GroupMembersWrapper)xmlSerializer.Deserialize(stream);
            var people = wrapper.Members;
            SerializeJSON(wrapper);
        }
    }

    [XmlRoot("GroupMembers")] // (GPT contribution)
    public class GroupMembersWrapper
    {
        [XmlElement("member")]
        public List<GroupMember> Members { get; set; }
    }


    public void SerializeJSON(GroupMembersWrapper members)
    {
        string jsonString = JsonUtility.ToJson(members, true);
        using (StreamWriter stream = File.CreateText(_jsonMembers))
        {
            stream.WriteLine(jsonString);
        }
    }
    
}
