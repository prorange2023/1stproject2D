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
        //Debug.Log("æ¿ ¿Ãµø");
        //Manager.Scene.LoadScene("GameScene");
    }

    
}
