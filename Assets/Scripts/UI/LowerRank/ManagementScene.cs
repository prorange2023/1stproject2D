using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManagementScene : BaseScene
{
    [SerializeField] Button ManageButton;
    [SerializeField] Button ScoutButton;
    [SerializeField] PopUpUI ScoutUI;
    [SerializeField] Button NextPhaseButton;

    public override IEnumerator LoadingRoutine()
    {
        yield return null;
    }

    public void NextPhase()
    {
        Manager.Scene.LoadScene("BanPickScene");
    }
    public void Scout()
    {
        Manager.UI.ShowPopUpUI(ScoutUI);
    }

}
