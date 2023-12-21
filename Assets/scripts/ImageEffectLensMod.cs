using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;



public class ImageEffectLensMod : MonoBehaviour
{
    [SerializeField] Material lensMat;
    [SerializeField] Material vMat;
    [SerializeField] Material caMat;
    [SerializeField] Material healthMat;
    [SerializeField] PlayerController playerController;

    //lens shader variable 
    string d = "_distortion";
    string r = "_vr";
    string ca = "_intensity";
    string t = "_threshold";
    string s = "_saturation";

    float maxDistort = -0.37f;
    float minDistort = -0.25f;
    float currentDistort;

    //vignette shader variable
    float maxRadius = 0.73f;
    float minRadius = 0.83f;
    float currentRadius;

    float saturation = 1f;

    //chromatic aberration variables
    float maxIntensity = 0.02f;
    float minIntensity = 0.01f;
    float currentIntensity;

    //health vars
    float threshold = 1.0f;

    void Start()
    {
        //get material componenet
        lensMat = GetComponent<ImageEffectLensMod>().lensMat;
        //return error if no property
        if (!lensMat.HasProperty(d))
        {
            Debug.LogError("the shader associated with the material on this game object is missing a necessary property. _distortion is required");
        }

        if (!vMat.HasProperty(r))
        {
            Debug.LogError("the shader associated with the material on this game object is missing a necessary property. _distortion is required");
        }

        currentDistort = -0.25f;
        currentRadius = 0.809f;
        currentIntensity = 0.01f;
        threshold = 1.0f;

        saturation = 1f;
    }
    void Update()
    {
        //if hit obstacle

        if (playerController.hitObstacle == true)
        {
            threshold = 0.23f;
            saturation = .4f;
        } 
        else if (playerController.health < playerController.healthMax && playerController.health > (playerController.health -1 ))
        {
            threshold = 0.15f;
            saturation = 0.75f;
        } 
        else if (playerController.health <= (playerController.healthMax - 1))
        {
            threshold = 0.015f;
            saturation = 0.6f;
        }
        else
        {
            threshold = 1.0f;
            saturation = 1.0f;
        }


        //if speeding up
        if (playerController.speedUp == true)
        {

            //lens
            if (currentDistort <= maxDistort)
            {
                currentDistort = maxDistort;
            }
            else
            {
                currentDistort -= 0.005f;
            }

            //vignette
            if (currentRadius <= maxRadius)
            {
                currentRadius = maxRadius;
            }
            else
            {
                currentRadius -= 0.005f;
            }

            //chromatic ab.
            if (currentIntensity <= maxIntensity)
            {
                currentIntensity = maxIntensity;
            }
            else
            {
                currentIntensity += 0.005f;
            }

        } 
        else
        {
            //reset to default values if !speedUp
  
            //lens
            if (currentDistort < minDistort)
            {
                currentDistort += 0.005f;
            }
            else if (currentDistort >= minDistort)
            {
                currentDistort = minDistort;
            }

            //vignette
            if (currentRadius < minRadius)
            {
                currentRadius += 0.005f;
            }
            else if (currentRadius >= minRadius)
            {
                currentRadius = minRadius;
            }

            //Chromatic ab
            if (currentIntensity < minIntensity)
            {
                currentIntensity -= 0.005f;
            }
            else if (currentIntensity >= minIntensity)
            {
                currentIntensity = minIntensity;
            }
        }

        //set distortion based on speed
        lensMat.SetFloat(d, currentDistort);
        vMat.SetFloat(r, currentRadius);
        vMat.SetFloat(s, saturation);
        caMat.SetFloat(ca, currentIntensity);
        healthMat.SetFloat(t, threshold);

    }
}
