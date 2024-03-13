using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RedTeamPointViewer : MonoBehaviour
{
    public TMP_Text text;
    public BattleManager battleManager;

    private void OnEnable()
    {
        UpdateView(battleManager.RedPoint);
        Manager.Battle.OnblueDied += UpdateView;
        Debug.Log(battleManager.RedPoint);
    }
    private void OnDisable()
    {
        Manager.Battle.OnredDied -= UpdateView;
    }
    private void UpdateView(int value)
    {
        text.text = value.ToString();
    }
}