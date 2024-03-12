using JetBrains.Annotations;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : Singleton<BattleManager>
{
    [Header("Rezen")]
    [SerializeField] bool debug;
    [SerializeField] float range;
    [SerializeField, Range(0, 360)] float angle;
    [SerializeField] LayerMask redTeam;
    [SerializeField] LayerMask blueTeam;
    [SerializeField] LayerMask redUlti;
    [SerializeField] LayerMask blueUlti;

    [SerializeField] LayerMask obstacleMask;

    public List<BattleAI> blueAI = new List<BattleAI>();
    public List<BattleAI> redAI = new List<BattleAI>();

    public List<BattleAI> redGrave = new List<BattleAI>();
    public List<BattleAI> blueGrave = new List<BattleAI>();

    [SerializeField] float RezenTime;
    
    int bluePoint;
    // blue layer == 8
    int redPoint;
    // red layer == 9
    private void Start()
    {
        
    }

    public void Update()
    {
        
    }
   
    

    public void OnBlueUnitDead()
    {
        bluePoint++;
        Debug.Log(bluePoint);
    }
    public void OnRedUnitDead()
    {
        redPoint++;
        Debug.Log(redPoint);
    }
    public void MoveToRedGrave(BattleAI battleAI)
    {
        
        Debug.Log("move to Redgrave");
    }

    public void MoveToblueGrave()
    {
        Debug.Log("move to bluegrave");
        StartCoroutine(RezenTimer());
    }

    Coroutine rezen;
    IEnumerator RezenTimer()
    {
        
        yield return new WaitForSeconds(RezenTime);
        gameObject.SetActive(true);
    }



    //blueTeam1 = blueAI[0];
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & blueTeam) != 0)
        {
            BattleAI battleAI = other.gameObject.GetComponent<BattleAI>();
          
            blueAI.Add(battleAI);
        }

        if (((1 << other.gameObject.layer) & redTeam) != 0)
        {
            BattleAI battleAI = other.gameObject.GetComponent<BattleAI>();
            redAI.Add(battleAI);
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & blueTeam) != 0)
        {
            BattleAI battleAI = other.gameObject.GetComponent<BattleAI>();
            blueAI.Remove(battleAI);
        }
        if (((1 << other.gameObject.layer) & redTeam) != 0)
        {
            BattleAI battleAI = other.gameObject.GetComponent<BattleAI>();
            redAI.Remove(battleAI);
        }
    }


    
}
