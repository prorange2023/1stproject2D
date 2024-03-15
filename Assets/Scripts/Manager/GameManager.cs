using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public List<Pilot> ourpilot = new List<Pilot>();
    public List<Pilot> enemypilot = new List<Pilot> ();

    public void Scout(Pilot pilot)
    {
        if (ourpilot.Count < 3) 
        {              
            ourpilot.Add(pilot);
            Debug.Log($"{ourpilot[0]}");
        }
    }
}
