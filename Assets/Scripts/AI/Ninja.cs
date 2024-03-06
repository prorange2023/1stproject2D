using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class Ninja : MonoBehaviour
{
    public enum State { Idle, Trace, Battle, Die }

    [Header("Component")]
    [SerializeField] Animator animator;

    [Header("Spec")]
    [SerializeField] float moveSpeed;
    [SerializeField] float attackRange;
    [SerializeField] float avoidRange;
    [SerializeField] float hp;
    [SerializeField] int damage;

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
        
        stateMachine.AddState(State.Battle, new BattleState(this));
        stateMachine.AddState(State.Die, new DieState(this));
        stateMachine.InitState(State.Idle);
    }

    private void Start()
    {

        // 태그 변경하는것도 만들어야되네?! 오마이갓뜨!
        firstTarget = GameObject.FindWithTag("EnemyLongRange").transform;
        //secondTarget = GameObject.FindWithTag("EnemyShortRange").transform;
        enemyUlti = GameObject.FindWithTag("EnemyUlti").transform;

        //GameObject[] Enemy = GameObject.FindGameObjectsWithTag("EnemyLongRange");
        startPos = transform.position;
    }

    private class NinjaState : BaseState
    {
        protected Ninja owner;
        protected Transform transform => owner.transform;

        protected float moveSpeed => owner.moveSpeed;
        protected float attackRange => owner.attackRange;
        protected float avoidRange => owner.avoidRange;
        protected float hp => owner.hp;
        protected Transform firstTarget => owner.firstTarget;

        protected Transform enemyUlti => owner.enemyUlti;
        protected Vector2 startPos => owner.startPos;

        public NinjaState(Ninja owner)
        {
            this.owner = owner;
        }
    }
    private class IdleState : NinjaState
    {
        public IdleState(Ninja owner) : base(owner) { }
        public override void Update()
        {

        }
        public override void Transition()
        {
            if (Vector2.Distance(firstTarget.position, transform.position) > attackRange)
            {
                ChangeState(State.Trace);
                Debug.Log("Ninja idle to trace");
            }
            
            else if (Vector2.Distance(firstTarget.position, transform.position) <= attackRange)
            {
                ChangeState(State.Battle);
                Debug.Log("Ninja idle to battle");
            }
            else if (hp <= 0)
            {
                ChangeState(State.Die);
                Debug.Log("Ninja idle to avoid");
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
            Debug.Log("Trace");
        }

        


        public override void Transition()
        {
            if (Vector2.Distance(firstTarget.position, transform.position) <= attackRange)
            {
                ChangeState(State.Battle);
                Debug.Log("Ninja trace to battle");
            }
            
            else if (hp <= 0)
            {
                ChangeState(State.Die);
                Debug.Log("Ninja trace to die");
            }
        }
    }
    

    private class BattleState : NinjaState
    {
        public BattleState(Ninja owner) : base(owner) { }

        public override void Update()
        {
            Debug.Log("knife knife");
        }

        public override void Transition()
        {
            if (Vector2.Distance(firstTarget.position, transform.position) > attackRange)
            {
                ChangeState(State.Trace);
                Debug.Log("Ninja to trace");
            }
            else if (hp <= 0)
            {
                ChangeState(State.Die);
                Debug.Log("Ninja to avoid");
            }
        }
    }

    private class DieState : NinjaState
    {
        public DieState(Ninja owner) : base(owner) { }

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
