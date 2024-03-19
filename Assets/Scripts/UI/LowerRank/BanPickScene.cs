using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BanPickScene : BaseScene
{
    [SerializeField] BanpickRunner runner;  
    public override IEnumerator LoadingRoutine()
    {
        yield return null;
    }

    public void Pick(GameObject prefab)
    {
        // 생성된 프리펩의 레이어를 변경하고 gameManager 의 팀 리스트에 넣습니다.

        if (Manager.Game.blueTurn == true)
        {
            prefab.layer = 8;
            Manager.Game.BlueTeam.Add(prefab);
            Debug.Log("blue+");
        }
        else
        {
            prefab.layer = 9;
            Manager.Game.RedTeam.Add(prefab);
            Debug.Log("red+");
        }
        Manager.Game.ActPoint = 0;
        
    }
    public void ButtonDestroy(Button button)
    {
        button.interactable = false; // 눌린 버튼을 비활성화합니다.
        Destroy(button);
        //buttons.Remove(button); // 리스트에서 해당 버튼을 제거합니다.AI 한테 리스트 안의 버튼중 고르게 하고 싶었으나 코드를 못생각해냄
        Manager.Game.ActPoint = 0;
        Debug.Log("destroy");
    }
    public void ButtonDisable(Button button)
    {
        button.interactable = false; // 눌린 버튼을 비활성화합니다.
        Debug.Log("interactable false");
    }

}
