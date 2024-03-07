using System.Diagnostics;
using UnityEngine;

public class Archer : BattleAI
{
    public enum State { Idle, Trace, Avoid, Battle ,Die }

    [Header("Component")]
    [SerializeField] Animator animator;

    [Header("Attack")]
    [SerializeField] bool debug;
    [SerializeField] LayerMask layerMask;
    [SerializeField] float range;
    [SerializeField, Range(0, 360)] float angle;
    [SerializeField] int deal;
    [SerializeField] int attackCost;
    [SerializeField] float attackCooltime;


    [Header("Spec")]
    [SerializeField] float moveSpeed;
    [SerializeField] float attackRange;
    [SerializeField] float avoidRange;
    [SerializeField] float hp;
    

    private StateMachine stateMachine;
    private Transform firstTarget;
    private Transform secondTarget;
    private Transform enemyUlti;
    private Vector2 startPos;

    
    
    private void Awake()
    {
        stateMachine = gameObject.AddComponent<StateMachine>();
        stateMachine.AddState(State.Idle, new IdleState(this));
        stateMachine.AddState(State.Trace, new TraceState(this));
        stateMachine.AddState(State.Avoid, new AvoidState(this));
        stateMachine.AddState(State.Battle, new BattleState(this));
        stateMachine.AddState(State.Die, new DieState(this));
        stateMachine.InitState(State.Idle);
        //���̷�
    }

    private void Start()
    {
        // �±� �����ϴ°͵� �����ߵǳ�?! �����̰���!
        firstTarget = GameObject.FindWithTag("EnemyLongRange").transform;
        //secondTarget = GameObject.FindWithTag("EnemyShortRange").transform;
        enemyUlti = GameObject.FindWithTag("EnemyUlti").transform;
        startPos = transform.position;
    }

    private void OnDrawGizmosSelected()
    {
        if (debug == false)
            return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }


    private class ArcherState : BaseState
    {
        protected Archer owner;
        protected Transform transform => owner.transform;

        protected float moveSpeed => owner.moveSpeed;
        protected float attackRange => owner.attackRange;
        protected float avoidRange => owner.avoidRange;
        protected float hp => owner.hp;

        protected Animator animator => owner.animator;
        protected Transform firstTarget => owner.firstTarget;
        protected Vector2 startPos => owner.startPos;

        public ArcherState(Archer owner)
        {
            this.owner = owner;
        }

        
    }

    private class IdleState : ArcherState
    {
        public IdleState(Archer owner) : base(owner) { }

        public override void Transition()
        {
            
            if (Vector2.Distance(firstTarget.position, transform.position) > attackRange)
            {
                ChangeState(State.Trace);
                animator.SetBool("Run", true);
            }
            else if (Vector2.Distance(firstTarget.position, transform.position) < avoidRange && Vector2.Distance(firstTarget.position, transform.position) < attackRange)
            {
                ChangeState(State.Avoid);
                animator.SetBool("Run", true);

            }
            else if (Vector2.Distance(firstTarget.position, transform.position) <= attackRange && Vector2.Distance(firstTarget.position, transform.position) > attackRange)
            {
                ChangeState(State.Battle);
                animator.SetBool("Battle", true);
            }
            else if (hp <= 0)
            {
                ChangeState(State.Die);
                animator.SetBool("Die", true);
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
        }
        
        public override void Transition()
        {
            if (Vector2.Distance(firstTarget.position, transform.position) <= attackRange && Vector2.Distance(firstTarget.position, transform.position) > avoidRange)
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
            else if (hp <= 0)
            {
                ChangeState(State.Die);
                animator.SetBool("Run", false);
                

            }
        }
    }
    private class AvoidState : ArcherState
    {
        public AvoidState(Archer owner) : base(owner) { }

        public override void Update()
        {
            // ����ġ�°� ����� ����
            Vector2 dir = (firstTarget.position - transform.position).normalized;
            transform.Translate(-dir * moveSpeed * Time.deltaTime, Space.World);
            
        }

        public override void Transition()
        {
            if (Vector2.Distance(firstTarget.position, transform.position) <= attackRange && Vector2.Distance(firstTarget.position, transform.position) >= avoidRange)
            {
                ChangeState(State.Battle);
                animator.SetBool("Run", false);
                animator.SetBool("Battle", true);

            }
            else if (Vector2.Distance(firstTarget.position, transform.position) > attackRange)
            {
                ChangeState(State.Trace);
                animator.SetBool("Run", true);
            }
            else if (hp <= 0)
            {
                ChangeState(State.Die);
                animator.SetBool("Run", false);
                animator.SetBool("Die", true);
            }
        }
    }

    private class BattleState : ArcherState
    {
        public BattleState(Archer owner) : base(owner) { }

        public override void Update()
        {
            Debug.Log("arrowattack");
            
        }

        public override void Transition()
        {
            if (Vector2.Distance(firstTarget.position, transform.position) < avoidRange)
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
            else if (hp <= 0)
            {
                ChangeState(State.Die); 
                animator.SetBool("Battle", false);
                animator.SetBool("Die", true);

            }
        }
    }
    
    private class DieState : ArcherState
    {
        public DieState(Archer owner) : base(owner) { }

        public override void Update()
        {
            
        }
        
        //���⼭ �ڷ�ƾ���� ��Ȱ �����ؾߵɰͰ����� �ϴ� ���߿�
        public override void Transition()
        {
            if (Vector2.Distance(startPos, transform.position) < 0.1f)
            {
                ChangeState(State.Idle);

            }
        }
    }

}
    
