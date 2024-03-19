using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;

public class BanpickRunner : MonoBehaviour
{
    public enum State
    {
        Start, BanTurn01, BanTurn02, PickTurn01, PickTurn02, PickTurn03, PickTurn04
    }

    public State state;
    
    private StateMachine stateMachine;
    

    [Header("BanPickButton")]
    public Button[] BanButton;
    public Button[] PickButton;

    private void Awake()
    {
        state = State.Start;
        stateMachine = gameObject.AddComponent<StateMachine>();
        stateMachine.AddState(State.Start, new StartState(this));
        stateMachine.AddState(State.BanTurn01, new BanTurn01State(this));
        stateMachine.AddState(State.BanTurn02, new BanTurn02State(this));
        stateMachine.AddState(State.PickTurn01, new PickTurn01State(this));
        stateMachine.AddState(State.PickTurn02, new PickTurn02State(this));
        stateMachine.AddState(State.PickTurn03, new PickTurn03State(this));
        stateMachine.AddState(State.PickTurn04, new PickTurn04State(this));
        stateMachine.InitState(State.Start);
        BanPickStart();
    }
    void BanPickStart()
    {
        state = State.BanTurn01;
    }

    
    public void Pick()
    {
        if (Manager.Game.blueTurn == true)
        {
            Manager.Game.BlueTeam.Add(gameObject);
        }
    }

    private class BanpickRunnerState : BaseState
    {
        protected BanpickRunner owner;
        
        public BanpickRunnerState(BanpickRunner owner)
        {
            this.owner = owner;
        }

        public void EnablePickButton()
        {
            foreach (var button in owner.PickButton)
            {
                button.interactable = true; // 배열에 있는 모든 버튼을 활성화합니다.
            }
        }
        public void DisablePickButton()
        {
            foreach (var button in owner.PickButton)
            {
                button.interactable = false; // 배열에 있는 모든 버튼을 비활성화합니다.
            }
        }
        public void EnableBanButton()
        {
            foreach (var button in owner.BanButton)
            {
                button.interactable = true; // 배열에 있는 모든 버튼을 활성화합니다.
            }
        }
        public void DisableBanButton()
        {
            foreach (var button in owner.BanButton)
            {
                button.interactable = false; // 배열에 있는 모든 버튼을 비활성화합니다.
            }
        }


    }
    private class StartState : BanpickRunnerState
    {

        public StartState(BanpickRunner owner) : base(owner) { }
        public override void Enter()
        {
            Manager.Game.ActPoint = 1;
        }
        public override void Update()
        {
            
        }
        public override void Transition()
        {
            if (Manager.Game.ActPoint == 0)
            {
                ChangeState(State.BanTurn01);
            }
        }
    }

    private class BanTurn01State : BanpickRunnerState
    {

        public BanTurn01State(BanpickRunner owner) : base(owner) { }
        public override void Enter()
        {
            Manager.Game.blueTurn = true;
            Manager.Game.ActPoint = 1;
            EnableBanButton();
            
            
            DisablePickButton();
        }
        public override void Update()
        {

        }
        public override void Transition()
        {
            if (Manager.Game.ActPoint == 0)
            {
                ChangeState(State.BanTurn01);

            }
        }
    }
    private class BanTurn02State : BanpickRunnerState
    {

        public BanTurn02State(BanpickRunner owner) : base(owner) { }
        public override void Enter()
        {
            Manager.Game.blueTurn = false;
            Manager.Game.ActPoint = 1;
            EnableBanButton();
            
            
            DisablePickButton();
        }
        public override void Update()
        {

        }
        public override void Transition()
        {
            if (Manager.Game.ActPoint == 0)
            {
                ChangeState(State.PickTurn01);
                Manager.Game.ActPoint = 1;
            }
        }
    }

    private class PickTurn01State : BanpickRunnerState
    {

        public PickTurn01State(BanpickRunner owner) : base(owner) { }
        public override void Enter()
        {
            Manager.Game.blueTurn = true;
            Manager.Game.ActPoint = 1;
           
            DisableBanButton();
            EnablePickButton();
           
        }
        public override void Update()
        {

        }
        public override void Transition()
        {
            if (Manager.Game.ActPoint == 0)
            {
                ChangeState(State.PickTurn02);
                
            }
        }
    }
    private class PickTurn02State : BanpickRunnerState
    {

        public PickTurn02State(BanpickRunner owner) : base(owner) { }
        public override void Enter()
        {
            Manager.Game.blueTurn = false;
            Manager.Game.ActPoint = 1;
            
            DisableBanButton();
            EnablePickButton();

        }
        public override void Update()
        {

        }
        public override void Transition()
        {
            if (Manager.Game.ActPoint == 0)
            {
                ChangeState(State.PickTurn03);
                
            }
        }
    }
    private class PickTurn03State : BanpickRunnerState
    {

        public PickTurn03State(BanpickRunner owner) : base(owner) { }
        public override void Enter()
        {
            Manager.Game.blueTurn = true;
            Manager.Game.ActPoint = 1;
            
            DisableBanButton();
            EnablePickButton();
        }
        public override void Update()
        {

        }
        public override void Transition()
        {
            if (Manager.Game.ActPoint == 0)
            {
                ChangeState(State.PickTurn04);
                
            }
        }
    }
    private class PickTurn04State : BanpickRunnerState
    {
        public PickTurn04State(BanpickRunner owner) : base(owner) { }
        public override void Enter()
        {
            Manager.Game.blueTurn = false;
            Manager.Game.ActPoint = 1;
            
            DisableBanButton();
            EnablePickButton();
            
        }
        public override void Update()
        {

        }
        public override void Transition()
        {
            if (Manager.Game.ActPoint == 0)
            {
                Manager.Scene.LoadScene("battleScene");
            }
        }
    }











    //public GameManager gameManager
    //{
    //    set
    //    {
    //        _battleStageManager = value.battleStageManager;
    //        _battleStageDataTable = value.dataTableManager.battleStageDataTable;

    //        _globalData = value.gameGlobalData.banpickStageGlobalData;
    //        _levelMaxCount = _globalData.stagesDataContainer.Length;
    //        _detailLevelMaxCount = _globalData.stagesDataContainer[_curLevel].orders.Count;

    //        int banpickLevelMaxCount = 0;
    //        int totalBanChampCount = 0;
    //        for (int i = 0; i < _levelMaxCount; ++i)
    //        {
    //            banpickLevelMaxCount += _globalData.stagesDataContainer[i].orders.Count;
    //            if (_globalData.stagesDataContainer[i].kind == BanpickStageKind.Ban)
    //                totalBanChampCount += _globalData.stagesDataContainer[i].orders.Count;
    //        }

    //        _battleStageDataTable.maxBanpickLevel = banpickLevelMaxCount;
    //        _battleStageDataTable.totalBanChampCount = totalBanChampCount / 2;
    //    }
    //}

    //private BattleStageManager _battleStageManager;
    //private BattleStageDataTable _battleStageDataTable;

    //private int _curLevel = 0;
    //private int _curDetailLevel = 0;
    //private int _detailLevelMaxCount = 0;
    //private int _progressStageCount = 1;
    //private int _levelMaxCount = 0;

    //[SerializeField] private BanpickStageGlobalData _globalData;

    //private int _curBanStage = 0;
    //private int _curPickStage = 0;
    //private readonly int BATTLE_TEAM_COUNT = (int)BattleTeamKind.End;

    //BanpickStageKind _curStageKind;
    //BattleTeamKind _curSelectTeamKind;

    //private AudioSource _audioSource;

    //private void Start()
    //{
    //    _audioSource = gameObject.AddComponent<AudioSource>();

    //    _battleStageDataTable.StartBanpick(_globalData.stagesDataContainer[0].kind, _globalData.stagesDataContainer[0].orders[0]);

    //    StartCoroutine(DelayBanpickStart());
    //}

    //IEnumerator DelayBanpickStart()
    //{
    //    yield return YieldInstructionStore.GetWaitForSec(1f);

    //    ProgressBanpick();
    //}

    //public void OnSelectChampion(string championName)
    //{
    //    BanpickStageKind tmpCurStageKind = _curStageKind;
    //    BattleTeamKind tmpCurSelectTeamKind = _curSelectTeamKind;
    //    int curStage = 0;

    //    switch (_curStageKind)
    //    {
    //        case BanpickStageKind.Ban:
    //            _audioSource.PlayOneShot(SoundStore.GetAudioClip("BanChampion"), 1f);
    //            curStage = _curBanStage / BATTLE_TEAM_COUNT;
    //            ++_curBanStage;
    //            break;
    //        case BanpickStageKind.Pick:
    //            _audioSource.PlayOneShot(SoundStore.GetAudioClip("PickChampion"), 1f);
    //            curStage = _curPickStage / BATTLE_TEAM_COUNT;
    //            ++_curPickStage;
    //            _battleStageManager.PickChampion(tmpCurSelectTeamKind, curStage, championName);
    //            break;
    //    }

    //    ++_progressStageCount;
    //    ++_curDetailLevel;
    //    if (_detailLevelMaxCount <= _curDetailLevel)
    //    {
    //        ++_curLevel;
    //        if (_levelMaxCount <= _curLevel)
    //        {
    //            SetReceiveButtonEventState(false);
    //            _battleStageDataTable.EndBanpick();
    //        }
    //        else
    //        {
    //            _detailLevelMaxCount = _globalData.stagesDataContainer[_curLevel].orders.Count;
    //            _curDetailLevel = 0;

    //            _battleStageDataTable.StartBanpickOneStage(_globalData.stagesDataContainer[_curLevel].kind);
    //        }
    //    }

    //    if (_levelMaxCount > _curLevel)
    //    {
    //        _battleStageDataTable.curBanpickStageInfo.Set(
    //            _globalData.stagesDataContainer[_curLevel].kind,
    //            _globalData.stagesDataContainer[_curLevel].orders[_curDetailLevel], curStage, _progressStageCount);

    //        ProgressBanpick();
    //    }

    //    switch (tmpCurStageKind)
    //    {
    //        case BanpickStageKind.Ban:
    //            _battleStageDataTable.UpdateBanpickData(championName, tmpCurStageKind, tmpCurSelectTeamKind, curStage);
    //            break;
    //        case BanpickStageKind.Pick:
    //            _battleStageDataTable.UpdateBanpickData(championName, tmpCurStageKind, tmpCurSelectTeamKind, curStage);
    //            break;
    //    }
    //}

    //public void ProgressBanpick()
    //{
    //    StartCoroutine(CheckPauseBanpick());
    //}

    //IEnumerator CheckPauseBanpick()
    //{
    //    while (_battleStageDataTable.isPauseBanpick)
    //    {
    //        yield return null;
    //    }

    //    _curStageKind = _globalData.stagesDataContainer[_curLevel].kind;
    //    _curSelectTeamKind = _globalData.stagesDataContainer[_curLevel].orders[_curDetailLevel];

    //    _battleStageManager.ProgressBanpick(this, _curSelectTeamKind);
    //}

    //// 버튼 이벤트를 받을건지 안받을건지 인자의 값에 따라 정하는 함수..
    //public void SetReceiveButtonEventState(bool isOnReceive)
    //{
    //    if (true == isOnReceive)
    //    {
    //        _battleStageDataTable.OnClickedSelectChampionButton -= OnSelectChampion;
    //        _battleStageDataTable.OnClickedSelectChampionButton += OnSelectChampion;
    //    }
    //    else
    //    {
    //        _battleStageDataTable.OnClickedSelectChampionButton -= OnSelectChampion;
    //    }
    //}
}
