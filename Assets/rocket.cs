﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    
    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 50f;

    Rigidbody rigidBody;
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
        Thrust();
        Rotate();
    }

    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
        case "Friendly":
            //do nothing
            print("ok");
            break;
        case "Fuel":
            print("fuel");
            break;
        default:
            print("die");
            break;
        }
    }

    private void Rotate()
    {
        
        rigidBody.freezeRotation = true; //take manual control of rotation
        
        
        float rotationThisFrame = rcsThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
        {
            
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }
        rigidBody.freezeRotation = false; 
    }

    private void Thrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            if (!audioSource.isPlaying)
                audioSource.Play();
            rigidBody.AddRelativeForce(Vector3.up * mainThrust);
        }
        else
        {
            audioSource.Stop();
        }
    }
}
