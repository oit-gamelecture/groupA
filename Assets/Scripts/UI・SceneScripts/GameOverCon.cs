using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameOverCon : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip buttonAudioClip;
    private bool isTransitioning = false;
    //public GameObject scoreUi;
    //Text scoreText;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        //scoreText = scoreUi.GetComponent<Text>();

        //scoreText.text = "個人資産" + ScoreManager.Instance.Score + "＄";

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space) && !isTransitioning)
        {
            //SceneManager.LoadScene("main");
            ScoreManager.Instance.ResetScore();
            isTransitioning = true;  // シーン遷移が二重に実行されないようにする
            StartCoroutine(PlaySoundAndTransition());
        }
        if (Input.GetKey(KeyCode.Tab) && !isTransitioning)
        {
            //SceneManager.LoadScene("main");
            ScoreManager.Instance.ResetScore();
            isTransitioning = true;  // シーン遷移が二重に実行されないようにする
            StartCoroutine(PlaySoundAndTransitionTitle());
        }
        if (Input.GetKey(KeyCode.Escape) && !isTransitioning)
        {
            //SceneManager.LoadScene("main");
            ScoreManager.Instance.ResetScore();
            isTransitioning = true;  // シーン遷移が二重に実行されないようにする
            StartCoroutine(PlaySoundAndTransitionEnd());
        }
    }

    private IEnumerator PlaySoundAndTransition()
    {
        audioSource.PlayOneShot(buttonAudioClip);
        yield return new WaitForSeconds(1f);  // 効果音が鳴り終わるまで待機
        SceneManager.LoadScene("main");
        ScoreManager.Instance.ResetScore();
    }
    private IEnumerator PlaySoundAndTransitionTitle()
    {
        audioSource.PlayOneShot(buttonAudioClip);
        yield return new WaitForSeconds(1f);  // 効果音が鳴り終わるまで待機
        SceneManager.LoadScene("Title");
        ScoreManager.Instance.ResetScore();
    }
    private IEnumerator PlaySoundAndTransitionEnd()
    {
        audioSource.PlayOneShot(buttonAudioClip);
        yield return new WaitForSeconds(1f);  // 効果音が鳴り終わるまで待機
        Application.Quit();
    }
}
