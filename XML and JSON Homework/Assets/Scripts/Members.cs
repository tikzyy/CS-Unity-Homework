using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct Members
{
    public string name;
    public int year;
    public string color;

    public Members(string name, int year, string color)
    {
        this.name = name;
        this.year = year;
        this.color = color;
    }

    [Serializable]
    public class Member
    {
        public List<Members> list;
    }
}