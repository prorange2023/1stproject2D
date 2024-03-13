using JetBrains.Annotations;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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

    public UnityAction<int>/*액션*/ OnblueDied;
    public UnityAction<int>/*액션*/ OnredDied;

    [SerializeField] int bluePoint;
    [SerializeField] int redPoint;


    public List<BattleAI> redGrave = new List<BattleAI>();
    public List<BattleAI> blueGrave = new List<BattleAI>();

    [SerializeField] float RezenTime;
    
    public int BluePoint/*프로퍼티*/{ set { bluePoint = value; OnredDied?.Invoke(value); } get { return bluePoint; } }

    // blue layer == 8
    public int RedPoint/*프로퍼티*/{ set { redPoint = value; OnblueDied?.Invoke(value); } get { return redPoint; } }
    // red layer == 9
    private void Start()
    {
        
    }

    public void Update()
    {
        
    }
   
    

    public void OnBlueUnitDead()
    {
        redPoint++;
        Debug.Log(bluePoint);
    }
    public void OnRedUnitDead()
    {
        bluePoint++;
        Debug.Log(redPoint);
    }
    public void MoveToRedGrave(GameObject battleai)
    {
        StartCoroutine(RezenTimer(battleai));
    }
    public void StopRezen(GameObject battleai)
    {
        StopCoroutine(RezenTimer(battleai));
    }

    public void MoveToblueGrave(GameObject battleai)
    {   
        StartCoroutine(RezenTimer(battleai));
    }
    IEnumerator RezenTimer(GameObject battleai)
    {
        Debug.Log("will rezen");
        if (battleai.gameObject.layer == 8)
        {
            Debug.Log("will move");
            yield return new WaitForSeconds(RezenTime);
            battleai.gameObject.transform.position = new Vector3(-12.9f, 5.74f, 0);
        }
        else if (battleai.gameObject.layer == 9)
        {
            Debug.Log("will move");
            yield return new WaitForSeconds(RezenTime);
            battleai.gameObject.transform.position = new Vector3(12.9f, -5.74f, 0);
        }
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
