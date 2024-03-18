using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleScene : BaseScene
{
    [SerializeField] Button NewGameButton;
    [SerializeField] Button LoadGameButton;
    [SerializeField] Button OptionButton;
    [SerializeField] SettingUI settingUIPrefab;
    [SerializeField] Button ExitGameButton;
    private void Start()
    {
        //bool exist = Manager.Data.ExistSaveData();
        //ContinueButton.interactable = exist;
    }
    public override IEnumerator LoadingRoutine()
    {
        yield return null;
    }

    public void NewGame()
    {
        Manager.Data.NewData();
        Manager.Scene.LoadScene("ManageMentScene");
        for (int i = 0; i < 2; i++)
        {
            Manager.Game.BluePopName.Add("BBB");
            Manager.Game.BluePopAtk.Add(7);
            Manager.Game.BluePopDef.Add(7);
        }
        for (int i = 0; i < 3; i++)
        {
            Manager.Game.RedPopName.Add("AAA");
            Manager.Game.RedPopAtk.Add(7);
            Manager.Game.RedPopDef.Add(7);
        }

    }

    //public void ContinueGame()
    //{
    //    Manager.Data.LoadData();
    //    Manager.Scene.LoadScene("GameScene");
    //}
    public void LoadGame()
    {

    }
    public void Option()
    {
        Manager.UI.ShowPopUpUI(settingUIPrefab);
    }
    public void ExitButton()
    {
        
    }
    public void GameSceneLoad()
    {
        //Debug.Log("¾À ÀÌµ¿");
        //Manager.Scene.LoadScene("GameScene");
    }

    
}
