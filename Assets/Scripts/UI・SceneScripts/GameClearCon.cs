using System.Collections;
using System.Collections.Generic;
using UnityEditor.AssetImporters;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameClearCon : MonoBehaviour
{
    public GameObject buttonUi;
    public GameObject scoreUi;
    CanvasGroup canvasGroup;
    Text scoreText;
    // Start is called before the first frame update
    void Start()
    {
        canvasGroup = buttonUi.GetComponent<CanvasGroup>();
        scoreText = scoreUi.GetComponent<Text>();

        scoreText.text = "個人資産" + ScoreManager.Instance.Score + "＄";
    }

    // Update is called once per frame
    void Update()
    {
        float sin = Mathf.Sin(Time.time);
        float absSin = Mathf.Abs(sin);
        canvasGroup.alpha = absSin;

        if (Input.GetKey(KeyCode.Space))
        {
            SceneManager.LoadScene("Title");
            ScoreManager.Instance.ResetScore();

        }
    }
}
