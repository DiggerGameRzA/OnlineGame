using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    Animator anim;
    public int hp;
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (hp <= 0)        //When this character's hp is equal to 0 or less
        {
            anim.SetBool("IsDead", true);       //Play dead animation
            anim.SetBool("IsMove", false);      //Stop play move animation
            anim.SetBool("IsAttack", false);    //Stop play attack animation
            GetComponent<EnemyCtrl>().agent.SetDestination(transform.position);     //Stop moving
            GetComponent<EnemyCtrl>().enabled = false;                              //Disable this script
        }
    }
}
