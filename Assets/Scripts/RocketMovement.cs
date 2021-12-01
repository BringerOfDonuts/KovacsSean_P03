using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketMovement : MonoBehaviour
{
    private float rocketSpeed = 25f;

    private Vector3 explosionPos;
    private float power = 100.0f;
    private float radius = 3.0f;
    private float upForce = 1.0f;

    [SerializeField] GameObject explosion;
    [SerializeField] ParticleSystem explosionParticles;
    public GameObject explosionSoundObject;
    public AudioSource explosionSource;

    private float secondsUntilDestroy = 10f;

    public Health health;
    
    void Start()
    {
        // Instantiate Particle System and Audio Source
        explosion = GameObject.Find("Explosion");
        explosionParticles = explosion.GetComponent<ParticleSystem>();
        explosionSoundObject = GameObject.Find("ExplosionSound");
        explosionSource = explosionSoundObject.GetComponent<AudioSource>();
        
        // Destroy rocket after certain number of seconds
        Destroy(gameObject, secondsUntilDestroy);
    }

    private void FixedUpdate()
    {
        // Movement
        transform.Translate(Vector3.up * rocketSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Explodes when it is Triggered by any object
        Explode();
        Destroy(gameObject);
    }

    private void Explode()
    {
        // Move Explosion Particle System to rocket's location and play particles and sound
        explosionPos = transform.position;
        explosion.transform.position = explosionPos;
        explosionParticles.Play();
        explosionSource.Play();

        // Check any colliders for rigidbodies and add explosion force to them
        Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if(rb != null)
            {
                rb.AddExplosionForce(power, explosionPos, radius, upForce, ForceMode.Impulse);
            }

            // Check for the player or an enemy and deal damage to them based on how close the explosion was
            health = hit.GetComponent<Health>();
            if(health != null)
            {
                float distanceToRocket = Vector3.Distance(gameObject.transform.position, hit.transform.position);
                health.TakeDamage(distanceToRocket, hit);
                
            }
        }
    }
}
