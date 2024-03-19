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
    public void Start()
    {
        
    }

    public void BattleEnd()
    {
        Manager.Scene.LoadScene("EndScene");
    }
    //Vector2 RandomPosition(Vector2 minBounds, Vector2 maxBounds)
    //{
    //    Vector2 newPos = new Vector2();
    //    newPos.x = Random.Range(minBounds.x, maxBounds.x);
    //    newPos.y = Random.Range(minBounds.y, maxBounds.y);
    //}
    public void BlueSpawn()
    {
        for (int i = 0; i < Manager.Game.BlueTeam.Count; i++)
        {

            Vector3 randomPosition = new Vector2();
            randomPosition.x = Random.Range(-7, -14);
            randomPosition.y = Random.Range(1, 10);
            randomPosition.z = 0;
            Instantiate(Manager.Game.BlueTeam[i], randomPosition, Quaternion.identity);
        }
    }
    public void RedSpawn()
    {
        for (int i = 0; i < Manager.Game.RedTeam.Count; i++)
        {

            Vector3 randomPosition = new Vector2();
            randomPosition.x = Random.Range(-7, -14);
            randomPosition.y = Random.Range(1, 10);
            randomPosition.z = 0;
            Instantiate(Manager.Game.BlueTeam[i], randomPosition, Quaternion.identity);
        }
    }

}
