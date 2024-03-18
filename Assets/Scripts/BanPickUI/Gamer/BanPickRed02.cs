using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BanPickRed02 : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] TMP_Text athleteName;
    [SerializeField] TMP_Text athleteAttack;
    [SerializeField] TMP_Text athleteDefence;

    public string pilotName;
    public int attack;
    public int defence;
    public void OnEnable()
    {
        //spriteRenderer.sprite = Manager.Game.renderers[0];
        pilotName = Manager.Game.RedPopName[2];
        attack = Manager.Game.RedPopAtk[2];
        defence = Manager.Game.RedPopDef[2];


        athleteName.text = pilotName;
        athleteAttack.text = attack.ToString();
        athleteDefence.text = defence.ToString();
    }

}
