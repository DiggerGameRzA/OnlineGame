using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

[System.Obsolete]
public class SetupLocalPlayer : NetworkBehaviour
{
    [SyncVar]
    public string name = "Player1";             //Default player's name
    [SerializeField] TextMesh textName;         //Display name
    [SerializeField] GameObject canvasUI;       //Find UI canvas

    [SerializeField] GameObject cam;
    [SerializeField] Transform camPivot;
    Image img;
    Health health;
    void Start()
    {
        textName = transform.GetChild(0).GetComponent<TextMesh>();
        canvasUI = transform.GetChild(1).gameObject;
        //img = GameObject.FindGameObjectWithTag("Health").GetComponent<Image>();
        health = GetComponent<Health>();

        if (isLocalPlayer)      //If this is a local player
        {
            canvasUI.SetActive(true);                               //Active UI canvas
            GetComponent<PL_Ctrl>().enabled = true;                 //Enable this PL_Ctrl script
            camPivot = GameObject.Find("CamPivot").transform;       //Find camera pivot
            camPivot.transform.parent = this.gameObject.transform;  //Set camera pivot parent is this game object

            textName.gameObject.SetActive(false);                   //Deactive this text name
        }

        Invoke("AddToTarget", 0.5f);        //Use AddToTraget method in 0.5 second
    }

    void Update()
    {
        img = GameObject.FindGameObjectWithTag("Health").GetComponent<Image>();     //Find health image UI
        textName.text = name;       //Set text name to name

        if (isLocalPlayer)      //If this is a local player
        {
            img.fillAmount = ((health.currentHP / health.maxHp) * 0.5f) + 0.5f;     //Update health in UI
        }

        if (isLocalPlayer && health.currentHP > 0)          //If this is a local player and HP is more than 0
        {
            GetComponent<PL_Ctrl>().enabled = true;         //Enable this PL_Ctrl script
        }
    }
    void AddToTarget()
    {
        foreach (GameObject i in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            //Add this player to be target of all enemies
            i.GetComponent<EnemyCtrl>().target.Add(this.gameObject);
        }
    }
}
