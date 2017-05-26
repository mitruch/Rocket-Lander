﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UI;

[System.Serializable]
public class Rocket : MonoBehaviour
{
    //from json
    public float gravity;
    public float rocketMass;
    public float thrustForce;
    public float turningForce;
    public float fuel;

    public static float powering;
    public static int success = 0;
    public static int fails = 0;

    public Text scoreText, fuelText;

    private string jsonString;

    public static bool isWasted = false; 
    public static bool isLanding = false; 
    public static bool splashOnce = false;
    public bool newGame = false;

    public new Rigidbody2D rigidbody2D;
   
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        jsonString = File.ReadAllText(Application.dataPath + "/Resources/rocketAttributes.json");
      //  Debug.Log(jsonString);
        Load(jsonString);
      //  Debug.Log(fuel);

        isWasted = false;
        splashOnce = false;
        isLanding = false;

        rigidbody2D.gravityScale = gravity;
        rigidbody2D.drag = thrustForce;
        rigidbody2D.angularDrag = turningForce;

        powering = fuel;

        UpdateUI();
    }

    public void Load(string savedData)
    {
        JsonUtility.FromJsonOverwrite(savedData, this);
    }

    void Splash()
    {
        if (splashOnce == false && isLanding == false)
        {
            isLanding = true;
            splashOnce = true;
            Invoke("Die", 0.2f);
        }
    }

    void Die()
    {
        isWasted = true;
        fails++;
        Invoke("RestartLevel", 1.5f);
        Debug.Log("die collision");
    }

    public void RestartLevel()
    {     
        int scene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(scene, LoadSceneMode.Single);
       // Application.LoadLevel(Application.loadedLevel);
    }

    public void RestartScore()
    {
        success = 0;
        fails = 0;

    }

    void Land()
    {
        isLanding = false;
        splashOnce = true;
        success++;
        Debug.Log("Score = " + success);
        Invoke("RestartLevel", 0.5f);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        
        Debug.Log(collision.gameObject.tag);
        if (collision.gameObject.tag != "landingspace")
        {
            if (splashOnce == false && isLanding == false)
            {
                Splash();
            }
        }
    }
  
    void UpdateUI()
    {
        scoreText.text = "Success: " + success + " Fails: " + fails + " Total: " + (success + fails);
        fuelText.text = "fuel: " + fuel.ToString();
    }

    void Update()
    {
        //Debug.Log(gameObject.tag);

        if(powering < 0.0f)
        {
            isWasted = true;
        }

        if (isLanding == true && splashOnce == false)
        {
            Land();
        }
        else if (isWasted)
        {
            Splash();
        }

        UpdateUI();

    }
}
