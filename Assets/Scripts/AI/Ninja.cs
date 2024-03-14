using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Ninja : BattleAI, IDamagable
{
    public enum State { Idle, Trace, Avoid, Battle, Die, Gameover }

    [Header("Component")]
    [SerializeField] Animator animator;
    [SerializeField] SpriteRenderer render;
    [SerializeField] Rigidbody2D rigid;
    [SerializeField] GameObject self;

    [Header("Attack")]
    [SerializeField] bool debug;
    [SerializeField] LayerMask layerMask = 0;
    [SerializeField] float attackRange;
    [SerializeField, Range(0, 360)] float angle;
    [SerializeField] int deal;
    [SerializeField] int attackCost;
    [SerializeField] float attackCooltime;
    [SerializeField] BattleAI targetBattleAI;

    private float cosRange;

    Collider2D[] atkColliders = new Collider2D[20];


    [Header("Spec")]
    [SerializeField] float moveSpeed;
    [SerializeField] float avoidRange;
    [SerializeField] float hp;
    [SerializeField] bool isDied;


    //private Vector2 moveDir;
    //private float xSpeed;



    private StateMachine stateMachine;
    private Transform firstTarget;
    private Transform secondTarget;
    private Transform enemyUlti;
    private Vector3 gravePos = new Vector3(60, 60, 0);
    private List<BattleAI> enemyList;
    //private float preAngle;


    private void Awake()
    {
        stateMachine = gameObject.AddComponent<StateMachine>();
        stateMachine.AddState(State.Idle, new IdleState(this));
        stateMachine.AddState(State.Trace, new TraceState(this));
        stateMachine.AddState(State.Avoid, new AvoidState(this));
        stateMachine.AddState(State.Battle, new BattleState(this));
        stateMachine.AddState(State.Die, new DieState(this));
        stateMachine.AddState(State.Gameover, new GameoverState(this));
        stateMachine.InitState(State.Idle);


    }


    private void Start()
    {
        this.hitPoint = hp;
        ListChoice();
    }
    public void ListChoice()
    {
        if (gameObject.layer == 8)
        {
            this.enemyList = Manager.Battle.redAI;
            layerMask = 512;
            gravePos = new Vector3(-12.9f, 5.74f, 0);
        }
        else if (gameObject.layer == 9)
        {
            this.enemyList = Manager.Battle.blueAI;
            layerMask = 256;
            gravePos = new Vector3(12.9f, -5.74f, 0);
        }
    }
    public void Diretion()
    {

        float ax = transform.position.x;
        float bx = firstTarget.position.x;

        if (ax > bx)
        {
            render.flipX = true;
            gameObject.transform.rotation = Quaternion.identity;
            //gameObject.transform.rotation = new Vector3(0, 180, 0);
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




    private class NinjaState : BaseState
    {
        protected Ninja owner;
        protected Transform transform => owner.transform;

        protected float moveSpeed => owner.moveSpeed;
        protected float attackRange => owner.attackRange;
        protected float avoidRange => owner.avoidRange;
        protected float hp => owner.hitPoint;

        protected Animator animator => owner.animator;
        protected Transform firstTarget => owner.firstTarget;
        protected Transform enemyUlti => owner.enemyUlti;
        protected Vector2 gravePos => owner.gravePos;
        protected List<BattleAI> redAI => Manager.Battle.redAI;
        protected List<BattleAI> blueAI => Manager.Battle.redAI;

        public NinjaState(Ninja owner)
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
            // 3월 12일 와서 적 울티 리스트찾는거 해놔 일단 생각나는게 그거뿐이다.
            //owner.StopCoroutine(FindCoroutine());
            //owner.StartCoroutine(FindCoroutine());
        }
        IEnumerator AttackCoroutine()
        {
            while (owner.attackCost == 0)
            {
                yield return new WaitForSeconds(owner.attackCooltime);
                owner.attackCost = 1;
            }
        }
        public void Attack()
        {
            if (owner.attackCost == 1)
            {
                owner.StopCoroutine(AttackCoroutine());
                int size = Physics2D.OverlapCircleNonAlloc(transform.position, owner.attackRange, owner.atkColliders, owner.layerMask);
                for (int i = 0; i < size; i++)
                {
                    Vector2 dirToTarget = (owner.atkColliders[i].transform.position - transform.position).normalized;

                    if (Vector2.Dot(dirToTarget, transform.right) < owner.cosRange)
                        continue;

                    IDamagable damagable = owner.atkColliders[i].GetComponent<IDamagable>();
                    damagable?.TakeDamage(owner.deal);
                }
                owner.attackCost--;
                owner.StartCoroutine(AttackCoroutine());
            }


        }
    }
    private class IdleState : NinjaState
    {

        public IdleState(Ninja owner) : base(owner) { }
        public override void Enter()
        {
            if (owner.gameObject.layer == 8)
            {
                Manager.Battle.StopRezen(owner.gameObject);
            }
            else if (owner.gameObject.layer == 9)
            {
                Manager.Battle.StopRezen(owner.gameObject);
            }
        }
        public override void Update()
        {
            FindTarget();
        }
        public override void Transition()
        {
            if (Manager.Battle.BattleTime <= 0)
            {
                ChangeState(State.Gameover);
                animator.SetBool("Battle", false);
                animator.SetBool("Run", false);
                //owner.StopCoroutine(owner.AttackCostCoroutine());
                animator.SetBool("Die", false);
            }
            else if(hp <= 0)
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
            }
            else if (Vector2.Distance(firstTarget.position, transform.position) > attackRange)
            {
                ChangeState(State.Trace);
                animator.SetBool("Run", true);
            }

            else if (Vector2.Distance(firstTarget.position, transform.position) <= attackRange && owner.attackCost == 1)
            {
                ChangeState(State.Battle);
                animator.SetBool("Battle", true);

            }
        }


    }

    private class TraceState : NinjaState
    {
        public TraceState(Ninja owner) : base(owner) { }


        public override void Update()
        {
            Vector2 dir = (firstTarget.position - transform.position).normalized;
            transform.Translate(dir * moveSpeed * Time.deltaTime, Space.World);
            owner.Diretion();
            FindTarget();
        }

        public override void Transition()
        {
            if (Manager.Battle.BattleTime <= 0)
            {
                ChangeState(State.Gameover);
                animator.SetBool("Battle", false);
                animator.SetBool("Run", false);
                //owner.StopCoroutine(owner.AttackCostCoroutine());
                animator.SetBool("Die", false);
            }
            else if(hp <= 0)
            {
                ChangeState(State.Die);
                animator.SetBool("Run", false);
                animator.SetBool("Battle", false);
                animator.SetBool("Die", true);
            }
            else if (firstTarget == null || (owner.attackCost == 0))
            {
                ChangeState(State.Idle);
                animator.SetBool("Battle", false);
                animator.SetBool("Run", false);
            }
            else if (Vector2.Distance(firstTarget.position, transform.position) <= attackRange)
            {
                ChangeState(State.Battle);
                animator.SetBool("Run", false);
                animator.SetBool("Battle", true);
            }
        }
    }

    private class AvoidState : NinjaState
    {


        public AvoidState(Ninja owner) : base(owner) { }

        public override void Enter()
        {

        }
        public override void Update()
        {
            // 도망치는걸 여기다 구현
            Vector2 dir = (enemyUlti.position - transform.position).normalized;
            transform.Translate(-dir * moveSpeed * Time.deltaTime, Space.World);
            Attack();
            FindTarget();

        }

        public override void Transition()
        {
            if (Manager.Battle.BattleTime <= 0)
            {
                ChangeState(State.Gameover);
                animator.SetBool("Battle", false);
                animator.SetBool("Run", false);
                //owner.StopCoroutine(owner.AttackCostCoroutine());
                animator.SetBool("Die", false);
            }
            else if(hp <= 0)
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
            else if (Vector2.Distance(firstTarget.position, transform.position) <= attackRange && Vector2.Distance(enemyUlti.position, transform.position) > avoidRange)
            {
                ChangeState(State.Battle);
                animator.SetBool("Run", false);
                animator.SetBool("Battle", true);
            }
            else if (Vector2.Distance(firstTarget.position, transform.position) > attackRange && Vector2.Distance(enemyUlti.position, transform.position) > avoidRange)
            {
                ChangeState(State.Trace);
                animator.SetBool("Run", false);
                animator.SetBool("Die", true);
            }
        }
    }


    private class BattleState : NinjaState
    {
        public BattleState(Ninja owner) : base(owner) { }



        public void Start()
        {

        }

        public override void Update()
        {
            FindTarget();
            Attack();
            owner.Diretion();
        }

        public override void Transition()
        {
            if (Manager.Battle.BattleTime <= 0)
            {
                ChangeState(State.Gameover);
                animator.SetBool("Battle", false);
                animator.SetBool("Run", false);
                //owner.StopCoroutine(owner.AttackCostCoroutine());
                animator.SetBool("Die", false);
            }
            else if (hp <= 0)
            {
                ChangeState(State.Die);
                animator.SetBool("Battle", false);
                animator.SetBool("Run", false);
                //owner.StopCoroutine(owner.AttackCostCoroutine());
                animator.SetBool("Die", true);
            }
            else if (firstTarget == null || (owner.attackCost == 0))
            {
                ChangeState(State.Idle);
                animator.SetBool("Battle", false);
                animator.SetBool("Run", false);
            }
            else if (Vector2.Distance(firstTarget.position, transform.position) > attackRange)
            {
                ChangeState(State.Trace);
                animator.SetBool("Battle", false);
                //owner.StopCoroutine(owner.AttackCostCoroutine());
                animator.SetBool("Run", true);
            }

        }
    }

    private class DieState : NinjaState
    {
        public DieState(Ninja owner) : base(owner) { }

        public override void Enter()
        {
            if (owner.gameObject.layer == 8)
            {
                Manager.Battle.RedPoint++;
                Manager.Battle.MoveToblueGrave(owner.gameObject);
                owner.gameObject.transform.position = new Vector3(60, 60, 0);
                owner.hitPoint = owner.hp;
            }
            else if (owner.gameObject.layer == 9)
            {
                Manager.Battle.BluePoint++;
                Manager.Battle.MoveToRedGrave(owner.gameObject);
                owner.gameObject.transform.position = new Vector3(-60, 60, 0);
                owner.hitPoint = owner.hp;
            }
        }
        public override void Update()
        {
            owner.Diretion();
        }
        //여기서 코루틴으로 부활 구현해야될것같은데 일단 나중에
        public override void Transition()
        {
            if (Vector2.Distance(gravePos, transform.position) < 0.1f)
            {
                ChangeState(State.Idle);
            }
        }
    }

    private class GameoverState : NinjaState
    {
        public GameoverState(Ninja owner) : base(owner) { }


    }
}
