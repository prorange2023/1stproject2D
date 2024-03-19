using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BanButton : MonoBehaviour
{
    public void ButtonPressed(Button button)
    {
        button.interactable = false; // 눌린 버튼을 비활성화합니다.
        Destroy(button);
        //buttons.Remove(button); // 리스트에서 해당 버튼을 제거합니다.
        Manager.Game.ActPoint = 0;
    }

}
