using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Buffers;

public class Athlete : Human
{
    [SerializeField] Human human;
    [SerializeField] TMP_Text athleteName;
    [SerializeField] TMP_Text athleteAttack;
    [SerializeField] TMP_Text athleteDefence;
    
    public void Start()
    {
        Debug.Log("start check");
        int attack = Random.Range(7, 11);

        Debug.Log($"{attack}");
        int defence = Random.Range(7, 11);
        Debug.Log($"{defence}");

        athleteName.text = ("ProGamer");
        Debug.Log(athleteName.text);
        athleteAttack.text = attack.ToString();
        athleteDefence.text = defence.ToString();

    }

}
