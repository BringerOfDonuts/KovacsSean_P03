using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketFire : MonoBehaviour
{
    private bool canFire = true;
    private float fireDelay = 1f;
    private int rocketsLoaded = 4;
    private int rocketAmmo = 20;
    private bool reloading = false;

    [SerializeField] GameObject rocket;
    [SerializeField] Transform barrel;

    public AudioSource fireSource;
    public AudioSource reloadSource;

    private Animator anim;

    LevelController levelController;

    private void Start()
    {
        levelController = GameObject.Find("Level Controller").GetComponent<LevelController>();
        anim = gameObject.GetComponent<Animator>();
    }

    void Update()
    {
        if(rocketsLoaded <= 0)
        {
            canFire = false;
        }
        
        // Fire rocket if player is able to and clicked left-mouse. Update UI and play sound and animation.
        if (Input.GetKeyDown(KeyCode.Mouse0) && canFire)
        {
            fireDelay = 1f;
            canFire = false;
            reloading = false;
            rocketsLoaded -= 1;
            FireRocket();
            fireSource.Play();
            anim.Play("FireAnimation");
            levelController.UIFire(rocketsLoaded);
        }

        // Check if player can reload and if they are not already reloading
        if (Input.GetKeyDown(KeyCode.R) && rocketsLoaded < 4 && reloading == false && rocketAmmo > 0)
        {
            canFire = false;
            reloading = true;
            anim.Play("ReloadStart");
            InvokeRepeating("ReloadSound", 0.5f, 1.0f);
            InvokeRepeating("Reload", 1.0f, 1.0f);
        }

        if(fireDelay <= 0 && rocketsLoaded > 0)
        {
            canFire = true;
        }

        fireDelay = fireDelay - Time.deltaTime;

        // Developer Cheat to Refill Ammo and Health
        if (Input.GetKeyDown(KeyCode.H))
        {
            rocketAmmo = 20;
            levelController.UIReload(rocketsLoaded, rocketAmmo);
        }
    }

    private void FireRocket()
    {
        Instantiate(rocket, barrel.position, barrel.rotation);
    }

    private void Reload()
    {
        // TODO: BUG WHERE CANCELING A RELOAD BY FIRING AND RELOADING AGAIN CAUSES FASTER RELOAD (Overlapping Invokes)
        if(reloading == true)
        {
            if (rocketAmmo > 0)
            {
                rocketsLoaded += 1;
                rocketAmmo -= 1;
                anim.Play("ReloadLoop");
                levelController.UIReload(rocketsLoaded, rocketAmmo);
            }

            if (rocketsLoaded == 4 || rocketAmmo == 0)
            {
                canFire = true;
                reloading = false;
                anim.Play("ReloadEnd");
                CancelInvoke();
            }
        }
        else
        {
            CancelInvoke();
        }
        
        
    }

    private void ReloadSound()
    {
        if(reloading == true)
        {
            reloadSource.Play();
        }
        else
        {
            CancelInvoke();
        }
    }
}
