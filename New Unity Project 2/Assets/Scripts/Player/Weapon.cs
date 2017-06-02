using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    private Animator myAnimator;

    private bool swordAttack;

    // Use this for initialization
    void Start()
    {
        myAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
    }

    private void FixedUpdate()
    {
        HandleSwordAttack();
        ResetAttack();
    }

    private void HandleSwordAttack()
    {
        if (swordAttack)
        {
            myAnimator.SetTrigger("attack");
        }
    }

    private void HandleInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            swordAttack = true;
        }
    }

    private void ResetAttack()
    {
        swordAttack = false;
    }
}
