using System;

[Serializable]
public struct GroupMember
{
    public string name { get; set; }
    public int birthYear { get; set; }
    public string favColour { get; set; }

    public GroupMember(string name, int birthYear, string favColour)
    {
        this.name = name;
        this.birthYear = birthYear;
        this.favColour = favColour;
    }
}