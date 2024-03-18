using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BanPickBlue00: MonoBehaviour
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
        pilotName = Manager.Game.BluePopName[0];
        attack = Manager.Game.BluePopAtk[0];
        defence = Manager.Game.BluePopDef[0];


        athleteName.text = pilotName;
        athleteAttack.text = attack.ToString();
        athleteDefence.text = defence.ToString();
    }

}
