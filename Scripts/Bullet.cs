using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    float alive_time = 5f;              //Time of alive of this bullet
    int dam = 20;                //Damage of this bullet
    float bullet_spd = 20f;             //Speed of this bullet
    GameObject enemyTrigger;            //Store enemy game object
    void Update()
    {
        alive_time -= Time.deltaTime;   //Countdown alive time
        if (alive_time <= 0)
        {
            Destroy(this.gameObject);   //Destroy this bullet when alive time is equal to 0 or less
        }
        //Make this bullet move forward
        this.transform.Translate(Vector3.forward * Time.deltaTime * bullet_spd);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")       //if this bullet hit the enemy
        {
            enemyTrigger = other.gameObject;
            enemyTrigger.GetComponent<EnemyHealth>().hp -= dam;     //Decrease enemy's health
            Destroy(this.gameObject);                               //Destroy this bullet
        }
    }
}
