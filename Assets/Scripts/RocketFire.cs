using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketFire : MonoBehaviour
{
    private bool canFire = true;
    private float fireDelay = 1f;
    private int rocketsLoaded = 4;
    private int rocketAmmo = 16;
    private bool reloading = false;

    [SerializeField] GameObject rocket;
    [SerializeField] Transform barrel;
    

    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Mouse0) && canFire)
        {
            fireDelay = 1f;
            canFire = false;
            rocketsLoaded -= 1;
            FireRocket();
            Debug.Log("Ammo Loaded: " + rocketsLoaded);
            Debug.Log("Ammo in Reserves: " + rocketAmmo);
        }

        if (rocketsLoaded <= 0 && reloading == false)
        {
            canFire = false;
            reloading = true;
            InvokeRepeating("Reload", 1.0f, 1.0f);
        }

        if(fireDelay <= 0 && rocketsLoaded > 0)
        {
            // TODO: Maybe change to require full ammo before firing
            canFire = true;
        }

        fireDelay = fireDelay - Time.deltaTime;
    }

    private void FireRocket()
    {
        Instantiate(rocket, barrel.position, barrel.rotation);
    }

    private void Reload()
    {
        // TODO: ADD RELOAD ANIMATION AND SOUNDS
        if(rocketAmmo > 0)
        {
            rocketsLoaded += 1;
            rocketAmmo -= 1;
        }

        if(rocketsLoaded == 4)
        {
            canFire = true;
            reloading = false;
            CancelInvoke();
        }
        
    }
}
