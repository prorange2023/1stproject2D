using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class Archer : BattleAI, IDamagable
{
    public enum State { Idle, Trace, Avoid, Battle ,Die }

    [Header("Component")]
    [SerializeField] Animator animator;
    [SerializeField] SpriteRenderer render;
    [SerializeField] Rigidbody2D rigid;
    [SerializeField] GameObject self;

    [Header("Attack")]
    [SerializeField] bool debug;
    [SerializeField] LayerMask layerMask;
    [SerializeField] float attackRange;
    [SerializeField, Range(0, 360)] float angle;
    [SerializeField] int deal;
    [SerializeField] int attackCost;
    [SerializeField] float attackCooltime;
    [SerializeField] Transform firePoint;
    [SerializeField] Arrow arrowPrefab;
    [SerializeField] BattleAI targetBattleAI;
    [SerializeField] bool Attacking;
    private float cosRange;

    Collider[] atkcolliders = new Collider[20];

    [Header("Spec")]
    [SerializeField] float moveSpeed;
    [SerializeField] float avoidRange;
    [SerializeField] float hp;
    [SerializeField] float impactTime;
    //[SerializeField] bool isDied;


    //private Vector2 moveDir;
    //private float xSpeed;
    [Header("Manager")]
    [SerializeField] BattleManager battleManager;



    private StateMachine stateMachine;
    private Transform firstTarget;
    private Transform secondTarget;
    private Transform enemyUlti;
    private Vector3 gravePos = new Vector3(60, 60, 0);
    private List<BattleAI> enemyList;
    private float atktime;
    //private float preAngle;



    private void Awake()
    {
        stateMachine = gameObject.AddComponent<StateMachine>();
        stateMachine.AddState(State.Idle, new IdleState(this));
        stateMachine.AddState(State.Trace, new TraceState(this));
        stateMachine.AddState(State.Avoid, new AvoidState(this));
        stateMachine.AddState(State.Battle, new BattleState(this));
        stateMachine.AddState(State.Die, new DieState(this));
        stateMachine.InitState(State.Idle);
    }

    private void Start()
    {
        this.hitPoint = hp;
        ListChoice();
        this.atktime = animator.GetCurrentAnimatorStateInfo(0).length;
    }
    
    public void ListChoice()
    {
        if(gameObject.layer == 8)
        {
            this.enemyList = battleManager.redAI;
        }
        else if (gameObject.layer == 9)
        {
            this.enemyList = battleManager.blueAI;
        }
    }
    public void Diretion()
    {
        float ax = transform.position.x;
        float bx = firstTarget.position.x;

        if (ax > bx)
        {
            render.flipX = true;
        }
        else if (bx > ax)
        {
            render.flipX = false;
        }
        
    }
    private void OnDrawGizmosSelected()
    {
        if (debug == false)
            return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    

    private class ArcherState : BaseState
    {
        protected Archer owner;
        protected Transform transform => owner.transform;

        protected float moveSpeed => owner.moveSpeed;
        protected float attackRange => owner.attackRange;
        protected float avoidRange => owner.avoidRange;
        protected float hp => owner.hitPoint;

        protected Animator animator => owner.animator;
        protected Transform firstTarget => owner.firstTarget;
        protected Transform enemyUlti => owner.enemyUlti;
        protected Vector2 startPos => owner.gravePos;
        protected BattleManager battlemanager => owner.battleManager;
        protected List<BattleAI> redAI => owner.battleManager.redAI;
        protected List<BattleAI> blueAI => owner.battleManager.redAI;

        public ArcherState(Archer owner)
        {
            this.owner = owner;
        }
        public void FindTarget()
        {
            
            if (owner.enemyList != null && owner.enemyList.Count > 0)
            {
                float shortDis = Vector2.Distance(owner.gameObject.transform.position, owner.enemyList[0].transform.position);
                owner.firstTarget = owner.enemyList[0].transform;

                for (int i = 0; i < owner.enemyList.Count; i++)
                {
                    BattleAI battleai = (BattleAI)owner.enemyList[i];
                    float distance = Vector2.Distance(transform.position, battleai.transform.position);

                    if (distance < shortDis)
                    {
                        owner.firstTarget = battleai.transform;
                        shortDis = distance;
                        owner.targetBattleAI = battleai;
                    }
                }
            }
            else
            {
                owner.firstTarget = null;
                owner.targetBattleAI = null;
            }
        }
        // 3월 12일 와서 적 울티 리스트찾는거 해놔 일단 생각나는게 그거뿐이다.
        //owner.StopCoroutine(FindCoroutine());
        //owner.StartCoroutine(FindCoroutine());
        
        IEnumerator AttackCostCoroutine()
        {
            yield return new WaitForSeconds(owner.attackCooltime - owner.atktime);
            animator.SetBool("Battle", false);
            animator.SetBool("Idle", true);
            yield return new WaitForSeconds(owner.attackCooltime);
            owner.attackCost = 1;
        }
        public void Attack(BattleAI battleAI)
        {
            if (owner.attackCost == 1)
            {
                owner.StopCoroutine(AttackCostCoroutine());
                //Instantiate(owner.arrowPrefab, owner.firePoint.transform);
                //owner.arrowPrefab.SetTarget(owner.targetBattleAI);
                //owner.arrowPrefab.SetDamage(owner.deal);
                owner.attackCost--;
                owner.StartCoroutine(AttackCostCoroutine());
            }
        }
        IEnumerator AttackMotionCoRoutine()
        {       
            yield return new WaitForSeconds(owner.atktime);
        }
        public void AtkMotionRoutine()
        {
            owner.StopCoroutine(AttackMotionCoRoutine());
        }
        public void AniCheck()
        {
            if (owner.animator.GetCurrentAnimatorStateInfo(0).IsName("ArcherAttack") == true)
            {
                
                float animTime = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
                if (animTime >= 1.0f)
                {
                    owner.Attacking = false;
                }
                else
                {
                    owner.Attacking = true;
                }
            }
        }
    }

    private class IdleState : ArcherState
    {
        public IdleState(Archer owner) : base(owner) { }

        public override void Enter()
        {
            
        }

        public override void Update()
        {
            FindTarget();
        }
        public override void Transition()
        {
            if (hp <= 0)
            {
                ChangeState(State.Die);
                animator.SetBool("Die", true);
                animator.SetBool("Idle", false);
            }
            else if (firstTarget == null || owner.attackCost == 0)
            {
                ChangeState(State.Idle);
                animator.SetBool("Run", false);
                animator.SetBool("Idle", false);
            }
            else if (Vector2.Distance(firstTarget.position, transform.position) > attackRange)
            {
                ChangeState(State.Trace);
                animator.SetBool("Run", true);
                animator.SetBool("Idle", false);
            }
            else if (Vector2.Distance(firstTarget.position, transform.position) < avoidRange && owner.attackCost == 0)
            {
                Debug.Log("idle to Avoid");
                ChangeState(State.Avoid);
                animator.SetBool("Run", true);
            }
            else if (Vector2.Distance(firstTarget.position, transform.position) <= attackRange && Vector2.Distance(firstTarget.position, transform.position) > avoidRange)
            {
                Debug.Log("idle to Battle");
                ChangeState(State.Battle);
                animator.SetBool("Battle", true);
                animator.SetBool("Idle", false);
            }
        }
    }

    private class TraceState : ArcherState
    {
        public TraceState(Archer owner) : base(owner) { }

        //public override void TakeDamage(int damage) => hp -= damage;

        public override void Enter()
        {
            
        }
        public override void Update()
        {
            Vector2 dir = (firstTarget.position - transform.position).normalized;
            transform.Translate(dir * moveSpeed * Time.deltaTime, Space.World);
            owner.Diretion();
            FindTarget();
        }
        
        public override void Transition()
        {
            if (hp <= 0)
            {
                ChangeState(State.Die);
                animator.SetBool("Die", true);
                animator.SetBool("Run", false);
                animator.SetBool("Battle", false);
            }
            else if (firstTarget == null)
            {
                ChangeState(State.Idle);
                animator.SetBool("Run", false);
                animator.SetBool("Idle", true);
            }
            else if (Vector2.Distance(firstTarget.position, transform.position) <= attackRange && Vector2.Distance(firstTarget.position, transform.position) > avoidRange)
            {
                ChangeState(State.Battle);
                animator.SetBool("Run", false);
                animator.SetBool("Battle", true);
            }
            else if(Vector2.Distance(firstTarget.position, transform.position) < avoidRange)
            {
                ChangeState(State.Avoid);
                animator.SetBool("Run", false);
            }
            
        }
    }
    private class AvoidState : ArcherState
    {
        public AvoidState(Archer owner) : base(owner) { }

        public override void Enter()
        {
            
        }


        public override void Update()
        {
            // 도망치는걸 여기다 구현
            Vector2 dir = (firstTarget.position - transform.position).normalized;
            transform.Translate(-dir * moveSpeed * Time.deltaTime, Space.World);
            FindTarget();
            owner.Diretion();
            // 회피 도중의 공격을 어디다 해야할까....
        }

        public override void Transition()
        {

            if (hp <= 0)
            {
                ChangeState(State.Die);
                animator.SetBool("Run", false);
                animator.SetBool("Die", true);
            }
            else if (firstTarget == null)
            {
                ChangeState(State.Idle);
                animator.SetBool("Run", false);
            }
            else if (owner.attackCost == 1)
            {
                Debug.Log("avoid to battle");
                ChangeState(State.Battle);
                animator.SetBool("Run", false);
                animator.SetBool("Battle", true);
            }
            else if (Vector2.Distance(firstTarget.position, transform.position) > attackRange)
            {
                ChangeState(State.Trace);
                animator.SetBool("Run", true);
            }
            
        }
    }

    private class BattleState : ArcherState
    {
        public BattleState(Archer owner) : base(owner) { }

        
        public override void Enter()
        {
            
        }
        public override void Update()
        {
            
            FindTarget();
            Attack(owner.targetBattleAI);
            AniCheck();
            owner.Diretion();
            
        }

        public override void Transition()
        {
            if (hp <= 0)
            {
                ChangeState(State.Die);
                animator.SetBool("Battle", false);
                animator.SetBool("Die", true);
                Debug.Log("Battle to Die ");
            }
            else if (owner.targetBattleAI == null || owner.attackCost == 0)
            {
                ChangeState(State.Idle);
                animator.SetBool("Battle", false);
                animator.SetBool("Run", false);
                animator.SetBool("Idle", true);
                Debug.Log("Battle to idle");
            }
            else if (owner.Attacking == false && Vector2.Distance(firstTarget.position, transform.position) < avoidRange)
            {
                ChangeState(State.Avoid);
                animator.SetBool("Battle", false);
                animator.SetBool("Run", true);
                Debug.Log("Battle to avoid");
            }
            else if (Vector2.Distance(firstTarget.position, transform.position) > attackRange)
            {
                ChangeState(State.Trace);
                animator.SetBool("Battle", false);
                animator.SetBool("Run", true);
                Debug.Log("Battle to trace ");
                //제발 좀 되라
            }
            
        }
    }
    
    private class DieState : ArcherState
    {
        public DieState(Archer owner) : base(owner) { }

        public override void Enter()
        {
            if (owner.gameObject.layer == 8)
            {
                owner.battleManager.OnBlueUnitDead();
                //owner.battleManager.MoveToblueGrave();
                owner.gameObject.transform.position = new Vector3(60, 60, 0);
            }
            else if (owner.gameObject.layer == 9)
            {
                owner.battleManager.OnRedUnitDead();
                //owner.battleManager.MoveToRedGrave();
                owner.gameObject.transform.position = new Vector3(-60, 60, 0);
            }
        }
        public override void Update()
        {
            owner.Diretion();
        }
        
        //여기서 코루틴으로 부활 구현해야될것같은데 일단 나중에
        public override void Transition()
        {
            if (Vector2.Distance(startPos, transform.position) < 0.1f)
            {
                ChangeState(State.Idle);
                animator.SetBool("Die", false);
            }
        }
    }

}
    
