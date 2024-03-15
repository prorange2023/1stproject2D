using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Buffers;

public class Athlete : Pilot
{
    [SerializeField] Pilot Pilot;
    [SerializeField] TMP_Text athleteName;
    [SerializeField] TMP_Text athleteAttack;
    [SerializeField] TMP_Text athleteDefence;
    

    public int attack;
    public int defence;
    public void OnEnable()
    {
        Debug.Log("start check");
        attack = Random.Range(7, 11);

        Debug.Log($"{attack}");
        defence = Random.Range(7, 11);
        Debug.Log($"{defence}");

        athleteName.text = ("ProGamer");
        Debug.Log(athleteName.text);
        athleteAttack.text = attack.ToString();
        athleteDefence.text = defence.ToString();
    }
}
