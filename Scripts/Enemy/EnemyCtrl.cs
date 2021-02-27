using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyCtrl : MonoBehaviour
{
    public float visionRange;       //Enemy's detection range.
    public float atkRange;          //Attack range of this enemy
    public float atkRate;           //Attack rate of this enemy
    public float damage;            //Attack damage of this enemy
    float cooldown;                 //Cooldown of enemy's attack time
    public float gravity = 8;       //Gravity affect this character.

    public List<GameObject> target;     //List of targets(Players)
    GameObject currentTarget = null;    //Current target(A Player)
    NavMeshAgent agent;
    Animator anim;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
    }
    void Update()
    {
        ChangeTarget();
        if (currentTarget != null)      //If there's a player
        {
            if (currentTarget.GetComponent<Health>().currentHP >= 0)    //If player's hp is equal to 0 or more
            {
                //Distance of this enemy and player
                float distance = Vector3.Distance(currentTarget.transform.position, transform.position);

                //If there is a player in vision range and NOT in attack range
                if (distance < visionRange && distance > atkRange)
                {
                    agent.SetDestination(currentTarget.transform.position); //Run into a player
                    anim.SetBool("IsMove", true);       //Play move animation
                }
                //If there is a player in attack range
                else if (distance < atkRange)
                {
                    agent.SetDestination(transform.position);   //Stop moving
                    anim.SetBool("IsAttack", true);             //Play attack animation
                    anim.SetBool("IsMove", false);              //Stop play move animation
                    if (Time.time > cooldown)
                    {
                        Attack();
                    }
                }
            }
            else
            {
                anim.SetBool("IsAttack", false);    //Stop play attack animation
                anim.SetBool("IsMove", false);      //Stop play move animation
            }
        }
    }
    void Attack()
    {
        anim.SetBool("IsAttack", false);        //Stop play attack animation
        cooldown = Time.time + atkRate;         //Reset cooldown
        currentTarget.GetComponent<Health>().TakeDamage(damage);    //Decrease player's hp
    }
    private void OnDrawGizmos()             //Draw gizmos guide
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, visionRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, atkRange);

    }
    void ChangeTarget()                     //Method for changing target
    {
        float minDist = Mathf.Infinity;
        foreach (GameObject i in target)
        {
            //Distance between this enemy and all players
            float distance = Vector3.Distance(transform.position, i.transform.position);
            if(distance < minDist)          //If distance is less than minimum distance
            {
                currentTarget = i;          //Change target
                minDist = distance;         //Reset minimum distance
            }
        }
    }
}
