using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PrologueCon : MonoBehaviour
{

    public AudioSource audioSource;
    public AudioClip buttonAudioClip;
    private bool isTransitioning = false;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey && !Input.GetMouseButton(0) && !Input.GetMouseButton(1) && !Input.GetMouseButton(2) && !isTransitioning)
        {
            isTransitioning = true;  // シーン遷移が二重に実行されないようにする
            StartCoroutine(PlaySoundAndTransition());
            //SceneManager.LoadScene("main");
        }
    }

    private IEnumerator PlaySoundAndTransition()
    {
        audioSource.PlayOneShot(buttonAudioClip);
        yield return new WaitForSeconds(1.5f);  // 効果音が鳴り終わるまで待機
        SceneManager.LoadScene("main");
        ScoreManager.Instance.ResetScore();
    }
}
