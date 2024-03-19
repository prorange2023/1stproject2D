using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickButton : MonoBehaviour
{
    public void Pick(GameObject prefab)
    {
        // 생성된 프리펩의 레이어를 변경합니다.
        
        if (Manager.Game.blueTurn ==true)
        {
            prefab.layer = 8;
            Manager.Game.BlueTeam.Add(prefab);
        }
        else
        {
            prefab.layer = 9;
            Manager.Game.RedTeam.Add(prefab);
        }
        Manager.Game.ActPoint = 0;
    }
}
