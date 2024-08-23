using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountDown : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip seClip;
    public AudioClip startClip;
    // Start is called before the first frame update
    void Start()
    {
       
        StartCoroutine(PlaySoundMultipleTimes());
    }

    IEnumerator PlaySoundMultipleTimes()
    {
        for(int i = 0; i < 5; i++)
        {
            audioSource.PlayOneShot(seClip);
            yield return new WaitForSeconds(seClip.length);
        }
        audioSource.PlayOneShot(startClip);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
