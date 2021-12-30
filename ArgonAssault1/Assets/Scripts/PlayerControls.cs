using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    [Header("General Setup Setting")]
    [Tooltip("How fast ship moves up and down based upon player input")]
    [SerializeField] float controlSpeed = 30f;
    [Tooltip("How far player moves horizontally")][SerializeField] float xRange = 1f;
    [Tooltip("How far player moves vertically")][SerializeField] float yRange = 2f;

    [Header("Laser gun array")]
    [Tooltip("Add all playerlasers here ")]
    [SerializeField] GameObject[] lasers;


    [Header("Screen position based tuning")]
    [SerializeField] float positionPitchFactor = -2f;
    [SerializeField] float controlPitchFactor = -15f;

    [Header("Player input based tuning")]
    [SerializeField] float positionYawFactor = 2f;
    [SerializeField] float controlRollFactor = -70f;

    

    float xThrow;
    float yThrow;
    // Start is called before the first frame update


    // Update is called once per frame
    void Update()
    {
        ProcessTranslation();
        ProcessRotation();
        ProcessFiring();
    }

    void ProcessFiring()
    {
        
        if (Input.GetButton("Fire1"))
        {
            SetActiveLasers(true);
        }
        else
        {
            SetActiveLasers(false);
        }
    }
    void SetActiveLasers(bool isActive)
    {
        foreach (var laser in lasers)
        {
            var emissionModule = laser.GetComponent<ParticleSystem>().emission;
            emissionModule.enabled = isActive;
        }
        
    }

    void ProcessRotation()
    {
        float pitchDueToPosition = transform.localPosition.y * positionPitchFactor;
        float pitchDueToControlThrow = yThrow * controlPitchFactor;


        float pitch = pitchDueToPosition + pitchDueToControlThrow;
        float yaw = transform.localPosition.y * positionYawFactor;
        float roll = xThrow * controlRollFactor;
        transform.localRotation = Quaternion.Euler(pitch, yaw, roll);
    }

    void ProcessTranslation()
    {
         xThrow = Input.GetAxis("Horizontal");
         yThrow = Input.GetAxis("Vertical");

        float xOffset = xThrow * Time.deltaTime * controlSpeed;
        float rawXPos = transform.localPosition.x + xOffset;
        float clampedXPos = Mathf.Clamp(rawXPos, -xRange, xRange);

        float yOffset = yThrow * Time.deltaTime * controlSpeed;
        float rawYPos = transform.localPosition.y + yOffset;
        float clampedYPos = Mathf.Clamp(rawYPos, -yRange, yRange);

        transform.localPosition = new Vector3(clampedXPos, clampedYPos, transform.localPosition.z);
    }
}
