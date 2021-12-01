using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelController : MonoBehaviour
{
    [SerializeField] Text healthText;
    [SerializeField] Text ammoLoadedText;
    [SerializeField] Text ammoReserveText;

    GameObject player;
    Health health;
    

    void Start()
    {
        player = GameObject.Find("Player");
        health = player.GetComponent<Health>();
    }

    void Update()
    {
        // Quit Application
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        // Reload scene if left clicked after player dies
        if (Input.GetKeyDown(KeyCode.Mouse0) && health.playerDead == true)
        {
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }
    }

    // UI Updates
    public void UIFire(int rocketsLoaded)
    {
        ammoLoadedText.text = rocketsLoaded.ToString();
    }

    public void UIReload(int rocketsLoaded, int rocketAmmo)
    {
        ammoLoadedText.text = rocketsLoaded.ToString();
        ammoReserveText.text = rocketAmmo.ToString();
    }

    public void UIHealth(int health)
    {
        if(health < 0)
        {
            health = 0;
        }
        healthText.text = health.ToString();
    }
}
