using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

[System.Obsolete]
public class PL_Ctrl : NetworkBehaviour
{
    public float spd;           //How fast this character moves
    public float rotateSpd;     //How fast this character rotates
    public float rotate;        //The variable store this character rotation
    public float gravity = 8;   //Gravity affect this character
    public float hp = 100;      //Max health point of this character

    public int maxAmmo;         //Max ammo of this character
    public int currentAmmo;     //Currnet ammo of this character

    public float firerate = 0.2f;   //Fire rate of this character
    private float nextfire = 0;     //The variable for storing delay of fire rate
    private float reloadSpd = 2.8f; //Speed of reloading time of this character

    public GameObject bullet;       //Bullet prefab
    public GameObject muzzleFlash;  //Flash effect prefab
    public GameObject shells;       //Shell prefab
    public GameObject bullet_spawn; //Spawn point for spawning bullets
    public GameObject muzzle_spawn; //Spawn point for spawning flash
    public GameObject shells_spawn; //Spawn point for spawning shell

    public AudioSource shot;        //Fire gun sound
    public AudioSource reload;      //Reload gun sound

    Vector3 moveDir = Vector3.zero;

    Animator anim;
    CharacterController controller;

    public int ammo = 40;           //Start amount of ammo
    Text ammoText;                  //Display amount of ammo
    void Start()
    {
        anim = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        ammoText = GameObject.FindGameObjectWithTag("AmmoCount").GetComponent<Text>();
        currentAmmo = maxAmmo;      //Set current ammo equal to max ammo
    }

    void Update()
    {
        DisplayAmmo();      //Update amount of ammo in UI
        
        #region Movements
        //Move foward.
        if (Input.GetKey(KeyCode.W))
        {
            anim.SetBool("IsRun", true);
            anim.SetBool("L", false);
            anim.SetBool("R", false);

            moveDir = new Vector3(0, 0, 1);
            moveDir *= spd;
            moveDir = transform.TransformDirection(moveDir);
        }
        //Move backward.
        else if (Input.GetKey(KeyCode.S))
        {
            anim.SetBool("IsBack", true);
            anim.SetBool("L", false);
            anim.SetBool("R", false);

            moveDir = new Vector3(0, 0, -0.5f);
            moveDir *= spd-1.5f;
            moveDir = transform.TransformDirection(moveDir);
        }
        //Stop moving.
        else
        {
            anim.SetBool("IsRun", false);
            anim.SetBool("IsBack", false);
            moveDir = Vector3.zero;
        }

        //Make this character moves
        controller.Move(moveDir * Time.deltaTime);
        //Make this character falls
        controller.Move(new Vector3(0, -gravity * Time.deltaTime, 0));
        #endregion

        #region Rotation
        //Play animation when rotate clockwise
        if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.W))
        {
            anim.SetBool("L", true);
        }
        //Play animation when rotate counter clockwise
        else if (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.W))
        {
            anim.SetBool("R", true);
        }
        //Stop play animation
        else
        {
            anim.SetBool("L", false);
            anim.SetBool("R", false);
        }

        //Make this character rotates
        rotate += Input.GetAxis("Horizontal") * rotateSpd * Time.deltaTime;
        transform.eulerAngles = new Vector3(0, rotate, 0);
        #endregion

        #region Fire
        if (Input.GetKey(KeyCode.Space))
        {
            anim.SetBool("IsShoot", true);
            anim.SetBool("IsRun", false);
            if (currentAmmo >= 1)           //When amount of ammo equal to 1 or more
            {
                if (Time.time > nextfire)
                {
                    if (isServer)
                        RpcFire();
                    else
                        CmdFire();
                    nextfire = Time.time + firerate;
                    currentAmmo -= 1;
                }
            }
            else if (currentAmmo == 0)      //When amount of ammo equal to 0
            {
                anim.SetBool("IsShoot", false);     //Play firing animation
            }
        }
        else
        {
            anim.SetBool("IsShoot", false);         //Stop play firing animation
        }
        #endregion

        //Reload when press R and current ammo is not equal to max ammo
        if (Input.GetKey(KeyCode.R) && currentAmmo != maxAmmo) 
        {
            Reload();
        }
        else if (currentAmmo == maxAmmo)
        {
            anim.SetBool("IsReload", false);
        }
    }

    [ClientRpc]
    void RpcFire()  //Method for commanding server to fire
    {
        //Spawn flash effect
        Instantiate(muzzleFlash, muzzle_spawn.transform);
        shot.Play();        //Play fire sound
        //Spawn bullet
        Transform _bullet = Instantiate(bullet.transform, bullet_spawn.transform.position, Quaternion.identity);
        _bullet.rotation = bullet_spawn.transform.rotation;
        //Spawn shell
        Transform _shells = Instantiate(shells.transform, shells_spawn.transform.position, Quaternion.identity);
        _shells.rotation = shells_spawn.transform.rotation;
    }
    [Command]
    void CmdFire()  //Method for commanding client to fire
    {
        RpcFire();
    }
    void Reload()   //Method for reloading gun
    {
        reload.Play();                      //Play reload sound
        anim.SetBool("IsReload", true);
        currentAmmo = maxAmmo;              //Set current ammo equal to max ammo
        nextfire = Time.time + reloadSpd;   //Set delay of firing
    }
    void DisplayAmmo()  //Method for updating amount of ammo in UI
    {
        ammoText.text = currentAmmo.ToString();
    }
}
