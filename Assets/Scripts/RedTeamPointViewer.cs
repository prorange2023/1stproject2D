using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RedTeamPointViewer : MonoBehaviour
{
    public TMP_Text text;

    private void OnEnable()
    {
        UpdateView(Manager.Battle.RedPoint);
        Manager.Battle.OnblueDied += UpdateView;
        Debug.Log(Manager.Battle.RedPoint);
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