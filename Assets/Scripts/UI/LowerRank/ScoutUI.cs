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
    public void AddPilot(Athlete randomPilot)
    {
        Manager.Game.ourpilot.Add(randomPilot);

        if (Manager.Game.ourpilot.Count <4)
        {
            //Manager.Game.renderers.Add(randomPilot.sprite);
            Manager.Game.BluePopName.Add(randomPilot.gName);
            Manager.Game.BluePopAtk.Add(randomPilot.attack);
            Manager.Game.BluePopDef.Add(randomPilot.defence);

            Debug.Log("added");

            Debug.Log(Manager.Game.BluePopName[0]);
            Debug.Log(Manager.Game.BluePopAtk[0]);
            Debug.Log(Manager.Game.BluePopDef[0]);
            Debug.Log(Manager.Game.ourpilot[0].name);
        }
        
    }
}
