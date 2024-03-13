using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BlueTeamPointViewer : MonoBehaviour
{
    public TMP_Text text;

    private void OnEnable()
    {
        UpdateView(Manager.Battle.BluePoint);
        Manager.Battle.OnredDied += UpdateView;
        Debug.Log(Manager.Battle.BluePoint);
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
