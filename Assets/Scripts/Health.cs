using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    private int health = 200;
    public bool playerDead = false;
    [SerializeField] int playerDamageMultiplier = 1;

    [SerializeField] GameObject blood;
    [SerializeField] ParticleSystem bloodParticles;
    [SerializeField] GameObject rocketLauncher;

    public AudioSource hurtSource;
    public AudioClip[] hurtClipArray;

    LevelController levelController;

    void Start()
    {
        levelController = GameObject.Find("Level Controller").GetComponent<LevelController>();
    }

    
    void Update()
    {
        if(health <= 0)
        {
            // Destroys gameObject with 0 health. Checks if the player died and destroys additional gameObjects.
            Death();
            if(gameObject.tag == "Player")
            {
                Destroy(rocketLauncher);
                playerDead = true;
            }
        }

        // Developer Cheat to Refill Ammo and Health
        if (Input.GetKeyDown(KeyCode.H))
        {
            health = 200;
            levelController.UIHealth(health);
        }
    }

    public void TakeDamage(float distanceToRocket, Collider hit)
    {
        // Checks if the player is damaged, can apply playerDamageMultiplier for reduced damage taken
        if (hit.tag == "Player")
        {
            // Updates Player UI and applies multiplier
            Debug.Log("Player has been hit");
            int playerDamageTaken = (90 - Mathf.RoundToInt((30 * distanceToRocket))) / playerDamageMultiplier;
            health -= playerDamageTaken;
            levelController.UIHealth(health);
            Debug.Log("Health of Player: " + health);
        }
        else
        {
            Debug.Log("Enemy has been hit");
            int damageTaken = 90 - Mathf.RoundToInt((30 * distanceToRocket));
            health -= damageTaken;
            Debug.Log("Health of " + gameObject + ": " + health);
        }
        // Play a random hurt sound from the array
        AudioSource.PlayClipAtPoint(hurtClipArray[Random.Range(0, hurtClipArray.Length)], transform.position, 1f);
    }


    private void Death()
    {
        // Destroy gameObject and play blood particles
        blood.transform.position = gameObject.transform.position;
        bloodParticles.Play();
        
        Destroy(gameObject);
    }
}
