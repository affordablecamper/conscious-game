using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using EZCameraShake;
using System;
//script by Daniel Bryant


public class ConsciousGun : MonoBehaviour {
    

    //[SerializeField]
    //variables!
    [Header("CameraShake")]
    public float Magnitude = 2f;
    public float Roughness = 10f;
    public float FadeOutTime = 5f;

    [SerializeField]


    [Header("Misc")]

    public Camera FPSCamera;                //Camera
    
    public LayerMask enviormentMask;
    
    public LayerMask enemyMask;               //enemy mask
    
    [Space]

    [Header("Integers/Values")]
    public int magAmmo = 30;            //Total mag ammo.

    [Space]
    [Header("Floats/Values")]
    
    public float weaponRange = 100f;    //Range of the weapon.
    public float tracerPower;               //The thrust of the tracer.
    public float muzzleflashtime = 0.1f;//How fast the muzzle flash goes off for.
    public float recoilAmount = 2f;         //Physical recoil stuff.
    public float bulletCasingForce;     //The force of the bullet casing.
    


   


    //privates
    private float fireCountdown = 0f;   //Something to do with firerate.
    private float muzzleFlashTimerStart;    //When the muzzle flash should start again.

    [Header("GameObjects")]
    [Space]
    
    public GameObject weaponObject;         //The weapon its self


    public GameObject muzzleFlash;          //The light
    //public GameObject bulletCasing;         //The bullet casing
    public GameObject tracer;            //The tracer

    


    [Space]
    [Header("Booleans")]
    public bool canShoot;              //If the weapon can shoot or not.
    public bool muzzleFlashEnabled = false;//If the muzzle flash is enabled.
    public bool magEmpty;              //If the mag is empty.
    
    [Space]
    [Header("Transforms")]
    //public Transform bulletCasingSpawn; //The location of the "bulletcasingspawn"
    public Transform fwd;             //The forward working direction that the "tracer" gameobject comes out from

    [Space]
    [Header("Vectors")]
    public Vector3 normalPosition;          //The normal position of the view model.




    [Space]
    [Header("Audio")]

    private AudioSource Audio;         //The audio source

    public AudioClip gunshotClip;      //Audio stuff
    [Space]
    [Header("Impacts")]
    public GameObject concreteImpactEffect;
    public GameObject waterImpactEffect;
    //public GameObject playerImpactEffect;

    //animation stuff




    //public Animation Reload;
    //public Text ammoText;










    private void Start()
    {


        canShoot = true;
        Audio = GetComponent<AudioSource>(); // finding the audio source
        muzzleFlashTimerStart = muzzleflashtime;

        if (FPSCamera == null)
        {

            Debug.LogError(Color.cyan + "No FPS Cam is attached");
            this.enabled = false;
        }

    }



    // Update is called once per frame
    //[Client]




    void Update()
    {




        if (Input.GetButtonDown("Fire1") && magEmpty == false)
        {

            Shoot();

        }








        if (muzzleFlashEnabled == true)
        {


            muzzleflashtime -= Time.deltaTime;
            muzzleFlash.SetActive(true);

        }


        if (muzzleflashtime <= 0)
        {

            muzzleFlashEnabled = false;
            muzzleflashtime = muzzleFlashTimerStart;
            muzzleFlash.SetActive(false);


        }








        if (magAmmo <= 0)
        {

            magEmpty = true;


        }
        else magEmpty = false;


    }




    
    public void Shoot()
    {

        {



            Ray ray = FPSCamera.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
            RaycastHit hitInfo;
            //ammoText.text = ("Ammo:" + magAmmo + "/" + reserveAmmo); // set the ammo text to the desired ammounts set and done by the vars
            Debug.DrawRay(ray.origin, ray.direction * weaponRange, Color.green);
          

                GameObject newProjectile = Instantiate(tracer, fwd.transform.position, fwd.transform.rotation) as GameObject;
                newProjectile.GetComponent<Rigidbody>().AddForce(FPSCamera.transform.forward.normalized * tracerPower);
                // Instantiate(bulletCasing, bulletCasingSpawn.position, bulletCasing.transform.rotation);
                muzzleFlashEnabled = true;

                magAmmo -= 1;

               
                CameraShaker.Instance.ShakeOnce(Magnitude, Roughness, 0, FadeOutTime);
                Audio.PlayOneShot(gunshotClip); //playing audio from audio source

                Vector3 weaponObjectLocalPosition = weaponObject.transform.localPosition;
                weaponObjectLocalPosition.z = weaponObjectLocalPosition.z - recoilAmount;
                weaponObject.transform.localPosition = weaponObjectLocalPosition;
                







                if (Physics.Raycast(ray, out hitInfo, weaponRange, enviormentMask))
                {
                    if (hitInfo.collider.tag == "Concrete")
                    {
                        Instantiate(concreteImpactEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));

                    }

                    if (hitInfo.collider.tag == "Water")
                    {
                        //Instantiate(waterImpactEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));

                    }
                }


                if (Physics.Raycast(ray, out hitInfo, weaponRange, enemyMask))
                {
                    if (hitInfo.collider.tag == "Enemy")
                    {
                     
                        ConsciousSwitch _switch = hitInfo.collider.gameObject.GetComponent<ConsciousSwitch>();
                        _switch.OnSwitch();
                        //ConsciousSwitchBack playerSwitch = GameObject.FindGameObjectWithTag("Player").GetComponent<ConsciousSwitchBack>();
                        //playerSwitch.OnPlayerSwitch();
                    }




                }


            




        }
    }








    private void FixedUpdate()
    {



          
            weaponObject.transform.localPosition = Vector3.Lerp(weaponObject.transform.localPosition, normalPosition, Time.deltaTime);
           
            
        
    




    }
}