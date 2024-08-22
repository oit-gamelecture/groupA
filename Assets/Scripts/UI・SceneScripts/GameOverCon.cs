using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameOverCon : MonoBehaviour
{
    public GameObject scoreUi;
    Text scoreText;
    // Start is called before the first frame update
    void Start()
    {
        scoreText = scoreUi.GetComponent<Text>();

        scoreText.text = "個人資産" + ScoreManager.Instance.Score + "＄";

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey && !Input.GetMouseButton(0) && !Input.GetMouseButton(1) && !Input.GetMouseButton(2))
        {
            SceneManager.LoadScene("main");
        }
    }
}
