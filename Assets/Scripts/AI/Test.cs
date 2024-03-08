using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : BattleAI
{

    public enum State { Idle, Trace, Battle, Die }

    [Header("Component")]
    [SerializeField] Animator animator;
    [SerializeField] SpriteRenderer render;
    [SerializeField] Rigidbody2D rigid;


    [Header("Attack")]
    [SerializeField] bool debug;
    [SerializeField] LayerMask layerMask;
    [SerializeField] float range;
    [SerializeField, Range(0, 360)] float angle;
    [SerializeField] int deal;
    [SerializeField] int attackCost;
    [SerializeField] float attackCooltime;

    private float cosRange;

    Collider[] atkColliders = new Collider[20];


    [Header("Spec")]
    [SerializeField] float moveSpeed;
    [SerializeField] float attackRange;
    [SerializeField] float avoidRange;
    [SerializeField] float hp;


    //private Vector2 moveDir;
    //private float xSpeed;
    //[Header("Manager")]
    //[SerializeField] BattleManager battleManager;

    private StateMachine stateMachine;
    private Transform firstTarget;
    private Transform secondTarget;
    private Transform enemyUlti;
    private Vector2 startPos;
    //private float preAngle;


    private void Awake()
    {
        stateMachine = gameObject.AddComponent<StateMachine>();
        stateMachine.AddState(State.Idle, new IdleState(this));
        stateMachine.AddState(State.Trace, new TraceState(this));

        stateMachine.AddState(State.Battle, new BattleState(this));
        stateMachine.AddState(State.Die, new DieState(this));
        stateMachine.InitState(State.Idle);
    }


    private void Start()
    {

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
        Gizmos.DrawWireSphere(transform.position, range);
    }




    private class TestState : BaseState
    {
        protected Test owner;
        protected Transform transform => owner.transform;

        protected float moveSpeed => owner.moveSpeed;
        protected float attackRange => owner.attackRange;
        protected float avoidRange => owner.avoidRange;
        protected float hp => owner.hp;

        protected Animator animator => owner.animator;
        protected Transform firstTarget => owner.firstTarget;

        protected Transform enemyUlti => owner.enemyUlti;
        protected Vector2 startPos => owner.startPos;

        public TestState(Test owner)
        {
            this.owner = owner;

        }

        public void FindTarget()
        {
            // 태그 변경하는것도 만들어야되네?! 오마이갓뜨!
            owner.firstTarget = GameObject.FindWithTag("EnemyLongRange").transform;
            //owner.firstTarget = owner.battleManager.gameObject.GetComponent<>
            //secondTarget = GameObject.FindWithTag("EnemyShortRange").transform;
            owner.enemyUlti = GameObject.FindWithTag("EnemyUlti").transform;

            //GameObject[] Enemy = GameObject.FindGameObjectsWithTag("EnemyLongRange");
            owner.startPos = transform.position;
        }


    }
    private class IdleState : TestState
    {

        public IdleState(Test owner) : base(owner) { }
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

            else if (Vector2.Distance(firstTarget.position, transform.position) <= attackRange && owner.attackCost == 1)
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

    private class TraceState : TestState
    {
        public TraceState(Test owner) : base(owner) { }


        public override void Update()
        {
            Vector2 dir = (firstTarget.position - transform.position).normalized;
            transform.Translate(dir * moveSpeed * Time.deltaTime, Space.World);
            owner.Diretion();
            FindTarget();
        }

        public override void Transition()
        {

            if (firstTarget == null || (owner.attackCost == 0))
            {
                ChangeState(State.Idle);
                animator.SetBool("Battle", false);
            }
            else if (Vector2.Distance(firstTarget.position, transform.position) <= attackRange)
            {
                ChangeState(State.Battle);
                animator.SetBool("Run", false);
                animator.SetBool("Battle", true);
            }

            else if (hp <= 0)
            {
                ChangeState(State.Die);
                animator.SetBool("Run", false);
                animator.SetBool("Die", true);
            }
        }
    }


    private class BattleState : TestState
    {
        public BattleState(Test owner) : base(owner) { }

        IEnumerator AttackCostCoroutine()
        {
            yield return new WaitForSeconds(owner.attackCooltime);
            owner.attackCost = 1;
        }
        public void Attack()
        {
            if (owner.attackCost == 1)
            {
                owner.StopCoroutine(AttackCostCoroutine());
                int size = Physics.OverlapSphereNonAlloc(transform.position, owner.range, owner.atkColliders, owner.layerMask);
                for (int i = 0; i < size; i++)
                {
                    Vector3 dirToTarget = (owner.atkColliders[i].transform.position - transform.position).normalized;

                    if (Vector3.Dot(dirToTarget, transform.forward) < owner.cosRange)
                        continue;

                    IDamagable damagable = owner.atkColliders[i].GetComponent<IDamagable>();
                    damagable?.TakeDamage(owner.deal);
                }
                owner.attackCost--;
                owner.StartCoroutine(AttackCostCoroutine());
            }
        }

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
            if (firstTarget == null || (owner.attackCost == 0))
            {
                ChangeState(State.Idle);
                animator.SetBool("Battle", false);
            }
            else if (Vector2.Distance(firstTarget.position, transform.position) > attackRange)
            {
                ChangeState(State.Trace);
                animator.SetBool("Battle", false);
                //owner.StopCoroutine(owner.AttackCostCoroutine());
                animator.SetBool("Run", true);
            }
            else if (hp <= 0)
            {
                ChangeState(State.Die);
                animator.SetBool("Battle", false);
                //owner.StopCoroutine(owner.AttackCostCoroutine());
                animator.SetBool("Die", true);
            }
        }
    }

    private class DieState : TestState
    {
        public DieState(Test owner) : base(owner) { }

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
            }
        }
    }
}
