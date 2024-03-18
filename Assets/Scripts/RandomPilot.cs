using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class RandomPilot : Pilot
{
    public SpriteRenderer sprite;
    public string gName;
    public int atk;
    public int def;
    private void OnEnable()
    {
        makeRandom();
    }


    private void makeRandom()
    {
        sprite = GetComponent<SpriteRenderer>();
        gName = ("boxer");
        atk = Random.Range(7, 11);
        def = Random.Range(7, 11);
    }
}
