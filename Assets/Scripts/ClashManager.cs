using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClashManager : MonoBehaviour
{
    private AudioSource audioSource;
  

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }


    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "enemy" || collision.gameObject.tag == "trip")
        {
            audioSource.Play();
           
        }

    }
}
