using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    private int health = 200;
    public bool playerDead = false;

    [SerializeField] GameObject blood;
    [SerializeField] ParticleSystem bloodParticles;
    [SerializeField] GameObject rocketLauncher;

    void Start()
    {
        
    }

    
    void Update()
    {
        if(health <= 0)
        {
            Death();
            if(gameObject.tag == "Player")
            {
                Destroy(rocketLauncher);
                playerDead = true;
            }
        }
    }

    public void TakeDamage(float distanceToRocket)
    {
        int damageTaken = 90 - Mathf.RoundToInt((30 * distanceToRocket));
        Debug.Log("Damage taken: " + damageTaken);
        health -= damageTaken;
        Debug.Log("Health of " + gameObject + ": " + health);
    }

    // TODO: DISABLE MOVECAMERA SCRIPT ONCE PLAYER DIES TO PREVENT ERRORS
    private void Death()
    {
        blood.transform.position = gameObject.transform.position;
        bloodParticles.Play();
        
        Destroy(gameObject);
    }
}
