using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoutUI : PopUpUI
{
    [SerializeField] Button ExitButton;
    [SerializeField] Button ScoutedAthlete;
    [SerializeField] Pilot pilot01;
    [SerializeField] Pilot pilot02;
    [SerializeField] Pilot pilot03;
    [SerializeField] Pilot pilot04;

    public void ExitScout()
    {
        Manager.UI.ClosePopUpUI();
    }
    


}
