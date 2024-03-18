using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{


    [Header("Scout")]

    public List<Pilot> ourpilot = new List<Pilot>();
    public List<Pilot> enemypilot = new List<Pilot>();
    //blue
    public string bluegName;
    public int blueAttack;
    public int blueDefence;

    public List<SpriteRenderer> BlueRenderers = new List<SpriteRenderer>();
    public List<string> BluePopName = new List<string>();
    public List<int> BluePopAtk = new List<int>();
    public List<int> BluePopDef = new List<int>();
    //red
    public string redgName;
    public int redAttack;
    public int redDefence;

    public List<SpriteRenderer> RedRenderers = new List<SpriteRenderer>();
    public List<string> RedPopName = new List<string>();
    public List<int> RedPopAtk = new List<int>();
    public List<int> RedPopDef = new List<int>();

    [Header("BanPick")]
    public Dictionary<string, BattleAI> BlueTeam =  new Dictionary<string, BattleAI>();
    public Dictionary<string, BattleAI> RedTeam = new Dictionary<string, BattleAI>();


    private void OnEnable()
    {
        bluegName = ("AAA");
        blueAttack = Random.Range(7, 11);
        blueDefence = Random.Range(7, 11);

        redgName = ("BBB");
        redAttack = 7;
        redDefence = 7;
    }
    public void BlueAdd(string name, BattleAI BattleAI)
    {
        BlueTeam.Add(name,BattleAI);
    }
    public void RedAdd(string name, BattleAI BattleAI)
    {
        RedTeam.Add(name, BattleAI);
    }
}
