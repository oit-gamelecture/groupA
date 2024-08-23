using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleCon : MonoBehaviour
{
    public GameObject ui;
    CanvasGroup canvasGroup;

    public AudioSource audioSource;
    public AudioClip buttonAudioClip;
    private bool isTransitioning = false;
    // Start is called before the first frame update
    void Start()
    {
        canvasGroup = ui.GetComponent<CanvasGroup>();
        audioSource = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
        float sin = Mathf.Sin(Time.time);
        float absSin = Mathf.Abs(sin);
        canvasGroup.alpha = absSin;

        if (Input.anyKey && !Input.GetMouseButton(0) && !Input.GetMouseButton(1) && !Input.GetMouseButton(2) && !isTransitioning)
        {
            //SceneManager.LoadScene("Prologue");
            isTransitioning = true;  // シーン遷移が二重に実行されないようにする
            StartCoroutine(PlaySoundAndTransition());
        }
    }

    private IEnumerator PlaySoundAndTransition()
    {
        audioSource.PlayOneShot(buttonAudioClip);
        yield return new WaitForSeconds(1f);  // 効果音が鳴り終わるまで待機
        SceneManager.LoadScene("Prologue");
        ScoreManager.Instance.ResetScore();
    }
}
