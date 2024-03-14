using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BanPickNinja : MonoBehaviour
{

    [SerializeField] Animator animator;
    public void OnEnable()
    {
        animator.SetLayerWeight(1, 1);
    }
    public void OnMouseEnter()
    {
        Debug.Log("OnMouseEnter");
        animator.SetBool("Enter", true);
    }
    public void OnMouseExit()
    {
        Debug.Log("OnMouseExit");
        animator.SetBool("Enter", false);
    }
    public void OnMouseDown()
    {
        Debug.Log("mouseDown");
        animator.SetBool("Down", true);
    }
    public void TeamChoice()
    {
        
    }
}
