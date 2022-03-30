using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ RequireComponent(typeof(Rigidbody), typeof(AudioSource))]
public class BoxSound : MonoBehaviour
{
    private Rigidbody rb;
    private AudioSource dragSound;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        dragSound = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if(rb.velocity.x != 0 || rb.velocity.z !=0)
        {
            if(!dragSound.isPlaying)
            dragSound.Play();
        }

        else
        {
            dragSound.Stop();
        }
    }
}
