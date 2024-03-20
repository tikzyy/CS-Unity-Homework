using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;
using System.Xml.Serialization;

public class DataManager : MonoBehaviour
{
    private string _dataPath;

    private string _xmlMembers;
    private string _jsonMembers;


    //page.337
    private List<Members> memberList = new List<Members>
    {
        new Members("Clara",1998,"Black"),
        new Members("Astrid",2003,"Orange"),
        new Members("Benjamin",1998,"Blue"),
        new Members("Oliver", 2001,"Purple"),
        new Members("Christoffer",2002,"Yellow")
    };
    void Awake()
    {
        _dataPath = Application.persistentDataPath+"/Player_Data/";
        Debug.Log(_dataPath);


        _xmlMembers = _dataPath + "MembersData.xml";
        _jsonMembers = _dataPath + "MembersData.json";
    }

    void Start()
    {
     Initialize();
    }

    public void Initialize()
    {
        NewDirectory();
        SerializeXML();
        DeserializeXML();
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
    }
    //page.336
    public void SerializeXML()
    {
        var xmlSerializer = new XmlSerializer(typeof(List<Members>));

        using (FileStream stream = File.Create(_xmlMembers))
        {
            xmlSerializer.Serialize(stream, memberList);
        }
    }
    //page.336
    public void DeserializeXML()
    {
        if (File.Exists(_xmlMembers))
        {
            var xmlSerializer = new XmlSerializer(typeof(List<Members>));

            using (FileStream stream = File.OpenRead(_xmlMembers))
            {
                var members = (List<Members>)xmlSerializer.Deserialize(stream);

                Members.Member ListOfMembers = new Members.Member();
                ListOfMembers.list = members;
                SerializeJSON();
            }
            
        }
    }

    //page.341 (This is NOT taking the data from XML and turning it into JSON, just making JSON from the origial data)
    public void SerializeJSON(Members group)
    {
        string jsonString = JsonUtility.ToJson(group, true);

        using (StreamWriter stream = File.CreateText(_jsonMembers))
        {
            stream.WriteLine(jsonString);
        }
    }
}
