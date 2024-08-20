using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    /// <summary>
    /// �Q�[���X�^�[�g
    /// </summary>
    
    public enum PlayState
    {
        None,
        Ready,
        Play,
        Finish,
    }

    public PlayState CurrentState = PlayState.None;

    [SerializeField] int countStartTime = 5;

    [SerializeField] Text countdownText = null;

    [SerializeField] Text timerText = null;

    float currentCountDown = 0;

    float timer = 0;
    // Start is called before the first frame update
    void Start()
    {
        CountDownStart();
    }

    // Update is called once per frame
    void Update()
    {
        timerText.text = "Time : 000.0s";
        //�X�e�[�g��Ready�̂Ƃ�
        if(CurrentState == PlayState.Ready)
        {
            currentCountDown -= Time.deltaTime;

            int intNum = 0;

            if(currentCountDown <= (float)countStartTime && currentCountDown > 0)
            {
                intNum = (int)Mathf.Ceil(currentCountDown);
                countdownText.text = intNum.ToString();
            }
            else if(currentCountDown <= 0)
            {
                StartPlay();
                intNum = 0;
                countdownText.text = "Start!!";

                StartCoroutine(WaitErase());
            }
        }

        else if (CurrentState == PlayState.Play)
        {
            timer += Time.deltaTime;
            timerText.text = "Time : " + timer.ToString("000.0") + "s";
        }
        else
        {
            timer = 0;
            timerText.text = "TIme : 000.0s";
        }
    }

    ///<summary>
    ///�J�E���g�_�E���X�^�[�g
    /// </summary>
    
    void CountDownStart()
    {
        currentCountDown = (float)countStartTime;
        SetPlayState(PlayState.Ready);
        countdownText.gameObject.SetActive(true);
        GameObject.Find("Player").GetComponent<PlayerMovement>().enabled = false;
    }

    ///<summary>
    ///�Q�[���X�^�[�g
    /// </summary>

    void StartPlay()
    {
        Debug.Log("Start!!!");
        SetPlayState(PlayState.Play);
        GameObject.Find("Player").GetComponent<PlayerMovement>().enabled = true;
    }

    ///<summary>
    ///�����҂��Ă���Start�\��������
    /// </summary>
     
    IEnumerator WaitErase()
    {
        yield return new WaitForSeconds(2f);
        countdownText.gameObject.SetActive(false);
    }

    ///<summary>
    ///���݂̃X�^�[�g�ݒ�
    /// </summary>
    
    void SetPlayState(PlayState state)
    {
        CurrentState = state;
    }
}
