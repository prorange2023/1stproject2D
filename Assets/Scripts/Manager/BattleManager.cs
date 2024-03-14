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
    public List<GameObject> AllAI = new List<GameObject>();

    public UnityAction<int>/*�׼�*/ OnblueDied;
    public UnityAction<int>/*�׼�*/ OnredDied;
    public UnityAction<float> OntimeChanged;

    [Header("Judgement")]
    [SerializeField] int bluePoint;
    [SerializeField] int redPoint;
    [SerializeField] float battleTime;
    [SerializeField] float RezenTime;

    public List<BattleAI> redGrave = new List<BattleAI>();
    public List<BattleAI> blueGrave = new List<BattleAI>();

    

    public int BluePoint/*������Ƽ*/{ set { bluePoint = value; OnredDied?.Invoke(value); } get { return bluePoint; } }
    // blue layer == 8
    public int RedPoint/*������Ƽ*/{ set { redPoint = value; OnblueDied?.Invoke(value); } get { return redPoint; } }
    // red layer == 9
    public float BattleTime { set { battleTime = value; OntimeChanged?.Invoke(value); } get { return battleTime; } }

    private void Start()
    {
        
    }

    public void Update()
    {
        
    }
   
    
    public void BattleEnd()
    {
        if ( AllAI != null)
        {
            for (int i = 0; i < AllAI.Count; i++)
            {
                AllAI[i].layer = 8;
            }
        }
    }
    //�� ������ �� �۵����ϴ����� ���߿� Ȯ������
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
        if (((1 << other.gameObject.layer) & redTeam) != 0)
        {
            GameObject gameibjectAI = other.gameObject.GetComponent<GameObject>();
            AllAI.Add(gameibjectAI);
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
        if (((1 << other.gameObject.layer) & redTeam) != 0)
        {
            GameObject gameibjectAI = other.gameObject.GetComponent<GameObject>();
            AllAI.Remove(gameibjectAI);
        }
    }

}
