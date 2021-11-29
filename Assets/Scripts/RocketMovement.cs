using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketMovement : MonoBehaviour
{
    private float rocketSpeed = 10f;
    private Vector3 explosionPos;

    private float secondsUntilDestroy = 10f;
    
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, secondsUntilDestroy);
    }

    private void FixedUpdate()
    {
        transform.Translate(Vector3.up * rocketSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        // TODO: ADD EXPLOSION PARTICLES, SOUNDS, AND EXPLOSIVE FORCE TO PLAYER + ENEMIES
        explosionPos = transform.position;
        Debug.Log("Rocket had collided");
        Destroy(gameObject);
    }
}
