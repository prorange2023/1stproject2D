using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SettingUI : PopUpUI
{
    //[SerializeField] ShotCutUI ShotCutUIPrefab;
    protected override void Awake()
    {
      //base.Awake();

      //GetUI<Button>("ShotCutButton").onClick.AddListener(ShotCut);
      GetUI<Button>("BackButton").onClick.AddListener(Close);
    }


    public void ShotCut()
    {
      //Manager.UI.ShowPopUpUI(ShotCutUIPrefab);
    }
}
