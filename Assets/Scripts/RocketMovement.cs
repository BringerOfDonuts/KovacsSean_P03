using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketMovement : MonoBehaviour
{
    private float rocketSpeed = 20f;

    private Vector3 explosionPos;
    private float power = 100.0f;
    private float radius = 3.0f;
    private float upForce = 1.0f;

    [SerializeField] GameObject explosion;
    [SerializeField] ParticleSystem explosionParticles;

    private float secondsUntilDestroy = 10f;

    public Health health;
    
    // Start is called before the first frame update
    void Start()
    {
        explosion = GameObject.Find("Explosion");
        explosionParticles = explosion.GetComponent<ParticleSystem>();
        
        Destroy(gameObject, secondsUntilDestroy);
    }

    private void FixedUpdate()
    {
        transform.Translate(Vector3.up * rocketSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        // TODO: ADD EXPLOSION PARTICLES, SOUNDS, AND EXPLOSIVE FORCE TO ENEMIES
        Explode();
        Destroy(gameObject);
    }

    private void Explode()
    {
        explosionPos = transform.position;
        explosion.transform.position = explosionPos;
        explosionParticles.Play();

        Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if(rb != null)
            {
                rb.AddExplosionForce(power, explosionPos, radius, upForce, ForceMode.Impulse);
            }

            health = hit.GetComponent<Health>();
            if(health != null)
            {
                float distanceToRocket = Vector3.Distance(gameObject.transform.position, hit.transform.position);
                health.TakeDamage(distanceToRocket);
            }
        }
    }
}
