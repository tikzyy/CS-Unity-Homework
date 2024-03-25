using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Xml;
using System.Xml.Serialization;
using System.Text;

public class DataManager : MonoBehaviour
{
    private string _dataPath;
    private string _xmlGroupMembers;
    private string _JSONMembers;
    
    private string _state;
    public string State
    {
        get { return _state; }
        set { _state = value; }
    }
    
    // list with names, birthdate and favourite colour
    private List<GroupMember> _GroupMember = new List<GroupMember>
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
        _dataPath = Directory.GetCurrentDirectory() + "/Logs/Files/";

        Debug.Log(_dataPath);
        
        _xmlGroupMembers = _dataPath + "GroupMember_Data.xml";
        _JSONMembers = _dataPath + "JSONMembers.json";
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
        DeserializeXML();
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
            Debug.Log("Directory already exists...");
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
            if (!File.Exists(_JSONMembers))
            {
                DeserializeXML();
            }
        }
    }
    
    public void DeserializeXML()
    {
        if (File.Exists(_xmlGroupMembers))
        {
            var xmlSerializer = new XmlSerializer(typeof(List<GroupMember>));

            using (FileStream stream = File.OpenRead(_xmlGroupMembers))
            {
                var people = (List<GroupMember>)xmlSerializer.Deserialize(stream);

                People membersToJson = new People();
                membersToJson.members = people;
                SerializeJSON(membersToJson);
            }
        }
    }
    public void SerializeJSON(People members)
    {
        string jsonString = JsonUtility.ToJson(members, true);
        using (StreamWriter stream = File.CreateText(_JSONMembers))
        {
            stream.WriteLine(jsonString);
        }
    }
    
}
