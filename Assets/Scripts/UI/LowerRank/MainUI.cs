using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainUI : PopUpUI
{
    //[SerializeField] ShotCutUI ShotCutUIPrefab;
    protected override void Awake()
    {
        base.Awake();

        //GetUI<Button>("ShotCutButton").onClick.AddListener(ShotCut);
        GetUI<Button>("BackButton").onClick.AddListener(Close);
    }

    public void NewGame()
    {
        Manager.Data.NewData();
        Manager.Scene.LoadScene("BattleScene");
    }
    public void ShotCut()
    {
        //Manager.UI.ShowPopUpUI(ShotCutUIPrefab);
    }
}
