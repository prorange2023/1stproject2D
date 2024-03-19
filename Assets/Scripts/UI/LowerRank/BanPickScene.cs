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
        // ������ �������� ���̾ �����ϰ� gameManager �� �� ����Ʈ�� �ֽ��ϴ�.

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
        button.interactable = false; // ���� ��ư�� ��Ȱ��ȭ�մϴ�.
        Destroy(button);
        //buttons.Remove(button); // ����Ʈ���� �ش� ��ư�� �����մϴ�.AI ���� ����Ʈ ���� ��ư�� ���� �ϰ� �;����� �ڵ带 �������س�
        Manager.Game.ActPoint = 0;
        Debug.Log("destroy");
    }
    public void ButtonDisable(Button button)
    {
        button.interactable = false; // ���� ��ư�� ��Ȱ��ȭ�մϴ�.
        Debug.Log("interactable false");
    }

}
