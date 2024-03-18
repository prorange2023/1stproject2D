using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Buffers;

public class Athlete : Pilot
{
    [SerializeField] TMP_Text athleteName;
    [SerializeField] TMP_Text athleteAttack;
    [SerializeField] TMP_Text athleteDefence;

    public string gName;
    public int attack;
    public int defence;
    public void OnEnable()
    {

        gName = ("boxer");
        attack = Manager.Game.blueAttack;
        defence = Manager.Game.blueDefence;


        athleteName.text = gName;
        
        athleteAttack.text = attack.ToString();
        athleteDefence.text = defence.ToString();
    }
}
