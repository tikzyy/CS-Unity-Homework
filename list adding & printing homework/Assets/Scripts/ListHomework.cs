using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListHomework : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        string name = "Oliver the Idiot";
        int number = 2;
        FindPartyMember(name, number-1);
    }
    public void FindPartyMember(string myName, int myNumber)
    {
        List<string> questPartyMembers = new List<string>()
        {
            "Grim the Barbarian",
            "Merlin the Wise",
            "Blu the Brazilian"
        };
        int listLength = questPartyMembers.Count;

        questPartyMembers.Add("Margo the Sleeper");
        questPartyMembers.Insert(myNumber, myName);

        int partySize = questPartyMembers.Count;

        foreach (string name in questPartyMembers)
        {
            print(name);
        }

        /// I think it would have been easier to learn lists by asking us
        /// to write a list from scratch; it was a little bit confusing
        /// navigating through a larger piece of code when we barely know
        /// anything...

    }
}