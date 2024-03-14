using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] TMP_Text text;
    [SerializeField] float extratime;
    [SerializeField] PopUpUI pauseUIPrefab;

    private void OnEnable()
    {
        UpdateView(Manager.Battle.BattleTime);
        Manager.Battle.OntimeChanged += UpdateView;
    }
    private void OnDisable()
    {
        //Manager.Battle.BattleEnd(); 왜 작동 안하는지는 나중에...
        Manager.Battle.OntimeChanged -= UpdateView;
        Manager.UI.ShowPopUpUI(pauseUIPrefab);
    }
    private void Update()
    {
        if (Manager.Battle.BattleTime>0)
        {
            Manager.Battle.BattleTime -= Time.deltaTime;
        }
        else if(Manager.Battle.BattleTime <= 0)
        {
            Manager.Battle.BattleTime = 0.0f;
        }
    }
    
    private void UpdateView(float value)
    {
        int minutes = Mathf.FloorToInt(Manager.Battle.BattleTime / 60);
        int seconds = Mathf.FloorToInt(Manager.Battle.BattleTime % 60);
        text.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
