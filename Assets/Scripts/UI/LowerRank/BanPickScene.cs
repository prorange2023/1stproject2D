using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BanPickScene : BaseScene
{
    [SerializeField] BanpickRunner runner;  
    public override IEnumerator LoadingRoutine()
    {
        yield return null;
    }
    
    public void Choice()
    {
        if (runner.state == BanpickRunner.State.BanTurn01 || runner.state == BanpickRunner.State.BanTurn02)
        {
            
        }
    }
}
