using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickButton : MonoBehaviour
{
    public void Pick(GameObject prefab)
    {
        // ������ �������� ���̾ �����մϴ�.
        
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
