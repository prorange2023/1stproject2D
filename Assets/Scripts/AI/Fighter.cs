using System.Collections;
using UnityEngine;


public class Fighter : BattleAI, IDamagable
{
    public enum State { Idle, Trace, Avoid, Battle, Die }

    [Header("Component")]
    [SerializeField] Animator animator;
    [SerializeField] SpriteRenderer render;
    [SerializeField] Rigidbody2D rigid;
    [SerializeField] BattleAI battleAI;

    [Header("Attack")]
    [SerializeField] bool debug;
    [SerializeField] LayerMask layerMask;
    [SerializeField] float attackRange;
    [SerializeField, Range(0, 360)] float angle;
    [SerializeField] int deal;
    [SerializeField] int attackCost;
    [SerializeField] float attackCooltime;

    private float cosRange;

    Collider[] colliders = new Collider[20];


    [Header("Spec")]
    [SerializeField] float moveSpeed;
    [SerializeField] float avoidRange;
    [SerializeField] float hp;
    [SerializeField] bool isDied;

    [Header("Manager")]
    [SerializeField] BattleManager battleManager;



    private StateMachine stateMachine;
    private Transform firstTarget;
    private Transform secondTarget;
    private Transform enemyUlti;
    private Vector2 gravePos;

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

    private class FighterState : BaseState
    {
        protected Fighter owner;
        protected Transform transform => owner.transform;
        protected bool isDead => owner.isDied;
        protected float moveSpeed => owner.moveSpeed;
        protected float attackRange => owner.attackRange;
        protected float avoidRange => owner.avoidRange;
        protected float hp => owner.hp;

        protected Animator animator => owner.animator;
        protected Transform firstTarget => owner.firstTarget;
        protected Transform enemyUlti => owner.enemyUlti;
        protected Vector2 gravePos => owner.gravePos;

        public FighterState(Fighter owner)
        {
            this.owner = owner;
        }

        public void FindTarget()
        {
            // 태그 변경하는것도 만들어야되네?! 오마이갓뜨!
            owner.firstTarget = GameObject.FindWithTag("EnemyLongRange").transform;
            //secondTarget = GameObject.FindWithTag("EnemyShortRange").transform;
            owner.enemyUlti = GameObject.FindWithTag("EnemyUlti").transform;

            //GameObject[] Enemy = GameObject.FindGameObjectsWithTag("EnemyLongRange");
            owner.gravePos = transform.position;
        }
    }

    private class IdleState : FighterState
    {
        public IdleState(Fighter owner) : base(owner) { }
        
        public override void Enter()
        {
            
        }
        public override void Update()
        {
            FindTarget();
        }
        public override void Transition()
        {
            if (Vector2.Distance(firstTarget.position, transform.position) > attackRange)
            {
                ChangeState(State.Trace);
                animator.SetBool("Run", true);
            }
            else if (Vector2.Distance(enemyUlti.position, transform.position) < avoidRange)
            {
                ChangeState(State.Avoid);
                animator.SetBool("Run", true);

            }
            else if (Vector2.Distance(firstTarget.position, transform.position) <= attackRange && Vector2.Distance(enemyUlti.position, transform.position) > avoidRange)
            {
                ChangeState(State.Battle);
                animator.SetBool("Battle", true);

            }
            else if (owner.hitPoint <= 0)
            {
                ChangeState(State.Die);
                animator.SetBool("Die", true);
            }
        }
    }

    private class TraceState : FighterState
    {
        public TraceState(Fighter owner) : base(owner) { }

        public override void Enter()
        {
            
        }
        public override void Update()
        {
            Vector2 dir = (firstTarget.position - transform.position).normalized;
            transform.Translate(dir * moveSpeed * Time.deltaTime, Space.World);
            FindTarget();
            owner.Diretion();
        }

        public override void Transition()
        {
            if (Vector2.Distance(firstTarget.position, transform.position) <= attackRange && Vector2.Distance(enemyUlti.position, transform.position) > avoidRange)
            {
                ChangeState(State.Battle);
                animator.SetBool("Run", false);
                animator.SetBool("Battle", true);

            }
            else if (Vector2.Distance(enemyUlti.position, transform.position) < avoidRange)
            {
                ChangeState(State.Avoid);
                animator.SetBool("Run", true);
                
            }
            else if (owner.hitPoint <= 0)
            {
                ChangeState(State.Die);
                animator.SetBool("Run", false);
                animator.SetBool("Die", true);
            }
        }
    }
    private class AvoidState : FighterState
    {


        public AvoidState(Fighter owner) : base(owner) { }

        public override void Enter()
        {
            
        }
        public override void Update()
        {
            // 도망치는걸 여기다 구현
            Vector2 dir = (enemyUlti.position - transform.position).normalized;
            transform.Translate(-dir * moveSpeed * Time.deltaTime, Space.World);
            FindTarget();

        }

        public override void Transition()
        {
            if (Vector2.Distance(firstTarget.position, transform.position) <= attackRange && Vector2.Distance(enemyUlti.position, transform.position) > avoidRange)
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
            else if (owner.hitPoint <= 0)
            {
                ChangeState(State.Die);
                animator.SetBool("Run", false);
                animator.SetBool("Die", true);
                
            }
        }
    }

    private class BattleState : FighterState
    {
        public BattleState(Fighter owner) : base(owner) { }

        IEnumerator AttackCostCoroutine()
        {
            yield return new WaitForSeconds(owner.attackCooltime);
            owner.attackCost = 1;
        }
        public void Attack()
        {
            if (owner.attackCost == 1)
            {
                owner.StopAllCoroutines();
                int size = Physics.OverlapSphereNonAlloc(transform.position, owner.attackRange, owner.colliders, owner.layerMask);
                for (int i = 0; i < size; i++)
                {
                    Vector3 dirToTarget = (owner.colliders[i].transform.position - transform.position).normalized;

                    if (Vector3.Dot(dirToTarget, transform.forward) < owner.cosRange)
                        continue;

                    IDamagable damagable = owner.colliders[i].GetComponent<IDamagable>();
                    damagable?.TakeDamage(owner.deal);
                }
                owner.attackCost--;
                owner.StartCoroutine(AttackCostCoroutine());
            }
        }
        public override void Enter()
        {
            
        }
        public override void Update()
        {
            Attack();
            FindTarget();
            owner.Diretion();
        }

        public override void Transition()
        {
            if (Vector2.Distance(enemyUlti.position, transform.position) < avoidRange && Vector2.Distance(firstTarget.position, transform.position) < attackRange)
            {
                ChangeState(State.Avoid);
                animator.SetBool("Battle", false);
                animator.SetBool("Run", true);
            }
            else if (Vector2.Distance(firstTarget.position, transform.position) > attackRange)
            {
                ChangeState(State.Trace);
                animator.SetBool("Battle", false);
                animator.SetBool("Run", true);
                
            }
            else if (owner.hitPoint <= 0)
            {
                ChangeState(State.Die);
                animator.SetBool("Battle", false);
                animator.SetBool("Die", true);
            }
        }
    }

    private class DieState : FighterState
    {
        public DieState(Fighter owner) : base(owner) { }

        public override void Enter()
        {
            Debug.Log("Enter Die");
            if (owner.gameObject.layer == 8)
            {
                owner.battleManager.OnRedUnitDead();
                owner.hitPoint = hp;
                owner.gameObject.transform.position = new Vector3(60, 60, 0);
            }
            else if (owner.gameObject.layer == 9)
            {
                owner.battleManager.OnBlueUnitDead();
                owner.hitPoint = hp;
                owner.gameObject.transform.position = new Vector3(-60, 60, 0);
            }
        }

        public override void Update()
        {

        }
        //여기서 코루틴으로 부활 구현해야될것같은데 일단 나중에
        public override void Transition()
        {
            if (Vector2.Distance(gravePos, transform.position) < 0.1f)
            {
                ChangeState(State.Idle);
                animator.SetBool("Die", false);
            }
        }
    }
}

