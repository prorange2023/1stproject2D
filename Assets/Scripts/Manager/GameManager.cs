using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public int PlayerTeam;

    
    public void Test()
    {
        Debug.Log(GetInstanceID());
    }

}
