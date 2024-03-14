using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleEndPopUpUI : PopUpUI
{
    public void BattleEnd()
    {
        Manager.Scene.LoadScene("EndScene");
    }
}
