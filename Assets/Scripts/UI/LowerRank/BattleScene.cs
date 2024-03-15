using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleScene : BaseScene
{
    [SerializeField] Button LoadNextSceneButton;
    public override IEnumerator LoadingRoutine()
    {
        yield return null;
    }
    public void BattleEnd()
    {
        Manager.Scene.LoadScene("EndScene");
    }

}
