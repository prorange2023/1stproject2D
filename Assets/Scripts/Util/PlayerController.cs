using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.InputSystem;
using Unity.VisualScripting;
using UnityEngine.Windows;

public class PlayerController : MonoBehaviour
{
    public enum State { Idle, Jump, Move, Attack, Hit, Die }


    private int hp;

    public int HP { get { return hp; } set { hp = value; } }

    private StateMachine<State> stateMachine = new StateMachine<State>();

    private void Start()
    {
        stateMachine.AddState(State.Idle, new IdleState());
        stateMachine.AddState(State.Jump, new JumpState());
        stateMachine.AddState(State.Move, new MoveState());

        stateMachine.Start(State.Idle);
    }


    private void Update()
    {
        stateMachine.Update();
    }

    private void LateUpdate()
    {
        stateMachine.LateUpdate();
    }

    private void FixedUpdate()
    {
        stateMachine.FixedUpdate();
    }
    private class PlayerState : BaseState<State>
    {
        protected PlayerController controller;

        public PlayerInput input; // 인풋시스템의 다른 활용
    }

    private class IdleState : PlayerState
    {
        public override void Enter()
        {
            int value = controller.HP;

        }
    }

    private void OnMove(InputValue input)
    {

    }


    private class JumpState : PlayerState
    {

        public override void Update()
        {


            if (input.actions["Jump"].IsPressed() && input.actions["Jump"].triggered)
            {

            }
        }
    }
    private class MoveState : PlayerState
    {

    }
}
