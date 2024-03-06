using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Fighter : BattleAI
{
    public enum State { Idle, Trace, Avoid, Battle, Die }

    [SerializeField] float moveSpeed;
    [SerializeField] float attackRange;
    [SerializeField] float avoidRange;
    [SerializeField] float hp;

    private StateMachine stateMachine;
    private Transform target;
    
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
    }

    private void Start()
    {
        // 태그 변경하는것도 만들어야되네?! 오마이갓뜨!
        target = GameObject.FindWithTag("Enemy").transform;
        
        startPos = transform.position;
    }

    private class FighterState : BaseState
    {
        protected Fighter owner;
        protected Transform transform => owner.transform;

        protected float moveSpeed => owner.moveSpeed;
        protected float attackRange => owner.attackRange;
        protected float avoidRange => owner.avoidRange;
        protected float hp => owner.hp;
        protected Transform target => owner.target;
        protected Vector2 startPos => owner.startPos;

        public FighterState(Fighter owner)
        {
            this.owner = owner;
        }
    }

    private class IdleState : FighterState
    {
        public IdleState(Fighter owner) : base(owner) { }

        public override void Transition()
        {
            if (Vector2.Distance(target.position, transform.position) > attackRange)
            {
                ChangeState(State.Trace);
                Debug.Log("idle to trace");
            }
            else if (Vector2.Distance(target.position, transform.position) < avoidRange && Vector2.Distance(target.position, transform.position) < attackRange)
            {
                ChangeState(State.Avoid);
                Debug.Log("idle to avoid");
            }
            else if (Vector2.Distance(target.position, transform.position) <= attackRange && Vector2.Distance(target.position, transform.position) > attackRange)
            {
                ChangeState(State.Battle);
                Debug.Log("idle to battle");
            }
            else if (hp <= 0)
            {
                ChangeState(State.Die);
                Debug.Log("idle to avoid");
            }
        }
    }

    private class TraceState : FighterState
    {
        public TraceState(Fighter owner) : base(owner) { }

        public override void Update()
        {
            Vector2 dir = (target.position - transform.position).normalized;
            transform.Translate(dir * moveSpeed * Time.deltaTime, Space.World);
            Debug.Log("Trace");
        }

        public override void Transition()
        {
            if (Vector2.Distance(target.position, transform.position) <= attackRange && Vector2.Distance(target.position, transform.position) > avoidRange)
            {
                ChangeState(State.Battle);
                Debug.Log("trace to battle");
            }
            else if (Vector2.Distance(target.position, transform.position) < avoidRange)
            {
                ChangeState(State.Avoid);
                Debug.Log("trace to avoid");
            }
            else if (hp <= 0)
            {
                ChangeState(State.Die);
                Debug.Log("trace to die");
            }
        }
    }
    private class AvoidState : FighterState
    {


        public AvoidState(Fighter owner) : base(owner) { }

        public override void Update()
        {
            //Transform enemyUlti = GameObject.FindWithTag("EnemyUlti").transform;
            //// 도망치는걸 여기다 구현
            //Vector2 dir = (enemyUlti.position - transform.position).normalized;
            //transform.Translate(-dir * moveSpeed * Time.deltaTime, Space.World);
            //Debug.Log("why continue avoid???");
        }

        public override void Transition()
        {
            if (Vector2.Distance(target.position, transform.position) <= attackRange && Vector2.Distance(target.position, transform.position) >= avoidRange)
            {
                ChangeState(State.Battle);
                Debug.Log("avoid to battle");
            }
            else if (Vector2.Distance(target.position, transform.position) > attackRange)
            {
                ChangeState(State.Trace);
                Debug.Log("avoid to trace");
            }
            else if (hp <= 0)
            {
                ChangeState(State.Die);
                Debug.Log("avoid to die");
            }
        }
    }

    private class BattleState : FighterState
    {
        public BattleState(Fighter owner) : base(owner) { }

        public override void Update()
        {
            Debug.Log("arrowattack");
        }

        public override void Transition()
        {
            if (Vector2.Distance(target.position, transform.position) < avoidRange)
            {
                ChangeState(State.Avoid);
                Debug.Log("battle to avoid");
            }
            else if (Vector2.Distance(target.position, transform.position) > attackRange)
            {
                ChangeState(State.Trace);
                Debug.Log("battle to trace");
            }
            else if (hp <= 0)
            {
                ChangeState(State.Die);
                Debug.Log("battle to avoid");
            }
        }
    }

    private class DieState : FighterState
    {
        public DieState(Fighter owner) : base(owner) { }

        public override void Update()
        {

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

