using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BanButton : MonoBehaviour
{
    public void ButtonPressed(Button button)
    {
        button.interactable = false; // ���� ��ư�� ��Ȱ��ȭ�մϴ�.
        Destroy(button);
        //buttons.Remove(button); // ����Ʈ���� �ش� ��ư�� �����մϴ�.
        Manager.Game.ActPoint = 0;
    }

}
