using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameClearCon : MonoBehaviour
{
    public GameObject buttonUi;
    public GameObject scoreUi;
    private CanvasGroup canvasGroup;
    private Text scoreText;

    public AudioSource audioSource;
    public AudioClip buttonAudioClip;
    private bool isTransitioning = false;

    void Start()
    {
        canvasGroup = buttonUi.GetComponent<CanvasGroup>();
        scoreText = scoreUi.GetComponent<Text>();
        audioSource = GetComponent<AudioSource>();

        scoreText.text = "個人資産:$" + ScoreManager.Instance.Score;
    }

    void Update()
    {
        float sin = Mathf.Sin(Time.time);
        float absSin = Mathf.Abs(sin);
        canvasGroup.alpha = absSin;

        if (Input.GetKey(KeyCode.Space) && !isTransitioning)
        {
            isTransitioning = true;  // シーン遷移が二重に実行されないようにする
            StartCoroutine(PlaySoundAndTransition());
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
        yield return new WaitForSeconds(1.5f);  // 効果音が鳴り終わるまで待機
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
