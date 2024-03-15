using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoutUI : PopUpUI
{
    [SerializeField] Button ExitButton;
    [SerializeField] Button ScoutedAthlete;

    public void ExitScout()
    {
        Manager.UI.ClosePopUpUI();
    }

}
