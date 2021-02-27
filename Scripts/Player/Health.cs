using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    GameObject canvasUI;                        //UI canvas
    [SerializeField] Image img;
    Animator anim;
    [SerializeField] public float currentHP;    //current health point
    public float maxHp = 100f;                  //Max health point
    private void Awake()
    {
        currentHP = maxHp;
    }
    private void Start()
    {
        currentHP = maxHp;
        anim = GetComponent<Animator>();
        canvasUI = transform.GetChild(1).gameObject;
        img = canvasUI.transform.GetChild(3).GetComponent<Image>();
    }
    void Update()
    {
        if(currentHP <= 0f)                     //If hp is equal to 0 or less
        {
            anim.SetBool("IsShoot", false);     //Stop play fire animation
            anim.SetBool("Dead",true);          //Play dead animation
            anim.Play("Die");                   //Play dead animation
            foreach (GameObject i in GameObject.FindGameObjectsWithTag("Enemy"))
            {
                //Remove this player as targer for all of enemyies
                i.GetComponent<EnemyCtrl>().target.Remove(this.gameObject);
            }
            GetComponent<PL_Ctrl>().enabled = false;    //Disable PL_Ctrl script
        }
    }
    public void TakeDamage(float dmg)
    {
        currentHP -= dmg;
    }
}
