using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BanPickUnit : MonoBehaviour, ISelectable
{

    [SerializeField] Animator animator;
    [SerializeField] Button mybutton;
    [SerializeField] GameObject ChampPrefab;
    [SerializeField] SpriteRenderer renderer;
    public void OnEnable()
    {
        animator.SetLayerWeight(1, 1);
    }
    public void OnMouseEnter()
    {
        
        animator.SetBool("Enter", true);
    }
    public void OnMouseExit()
    {
        
        animator.SetBool("Enter", false);
    }
    public void OnMouseDown()
    {
        
        animator.SetBool("Down", true);
    }
    public void Buttonstop(SpriteRenderer sprite, Button button)
    {
        sprite = renderer;
        button = mybutton;
        sprite.color = new Color(118, 118, 118);
        button.enabled = false;
    }
    public void AddChamp(GameObject gameobject)
    {
        gameobject = ChampPrefab;
        
    }
}
