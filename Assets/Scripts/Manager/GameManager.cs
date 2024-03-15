using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public List<Human> ourHuman = new List<Human>();
    public List<Human> enemyHunan = new List<Human> (); 


    public void Scout(Human human)
    {
        if (ourHuman.Count < 3) 
        {              
            ourHuman.Add(human);
        }
    }

}
