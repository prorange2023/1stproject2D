using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomStat : MonoBehaviour
{
    List<int> attack = new List<int>();
    List<int> defence = new List<int>();
    public void OnEnable()
    {
        for (int i = 0; i < 8; i++)
        {
            attack[i] = Random.Range(7, 11);
            defence[i] = Random.Range(7, 11);
        }
    }
}
