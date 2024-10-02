using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChelikController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    public float speed;

    void Update()
    {    
        speed = Mathf.Abs(Input.GetAxis("Vertical"));
        animator.SetFloat("Speed", speed);
    }
}
