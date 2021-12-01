using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    [SerializeField] Transform cameraPosition;
    GameObject Player;
    Health health;

    private void Start()
    {
        Player = GameObject.Find("Player");
        health = Player.GetComponent<Health>();
    }

    void Update()
    {
        // Move camera as long as the player is not dead
        if(health.playerDead == false)
        {
            transform.position = cameraPosition.position;
        }
        
    }
}
