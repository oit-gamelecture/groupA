using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class MobilePhoneCon : MonoBehaviour
{
    //public GameObject blindfold;  //目隠しを入れるオブジェクト
    public Image image;  // コントロールするImageオブジェクト
    public float duration = 1.0f;  // アニメーションの時間
    private Vector3 originalPosition;  // Imageの初期位置
    private Vector3 targetPosition1;  // Imageの目標座標
    private Vector3 targetPosition2;
    private bool isMoving = false;  // 位置が移動中かどうか

    public AudioSource audioSource;
    public AudioClip notification; //通知音
    public AudioClip heartBeat;
    public AudioClip goodNews;
    public AudioClip badNews;

    public GameObject textBox;
    public GameObject subjectBox;
    private Text content;
    private Text subject;


    private List<int> remainingValues;  // 未使用の値のリスト
    private int[] allValues = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };  // 使用するすべての値

    // Start is called before the first frame update


    private float scoreTime;    //スコアに時間をかける用の変数
    public float pastTime;
    public GameObject scoreTextBox;
    private Text scoreText;
    private float decreaseManager = 0.1f;

    void Start()
    {
        //fpsを60に固定
        Application.targetFrameRate = 60;

        ResetRemainingValues(); //乱数リストをリセット
        originalPosition = new Vector3(0, -1050, 0); //image.rectTransform.localPosition;
        targetPosition1 = image.rectTransform.localPosition + new Vector3(0, 190, 0);
        targetPosition2 = image.rectTransform.localPosition + new Vector3(0, 935 + 190, 0);
        audioSource = GetComponent<AudioSource>();
        content = textBox.GetComponent<Text>();
        subject = subjectBox.GetComponent<Text>();
        //blindfold.SetActive(false);

        // コルーチンを開始
        image.rectTransform.localPosition = originalPosition;
        StartCoroutine(CallMethodAtRandomIntervals());

        ScoreManager.Instance.ResetScore();
        scoreTime = Time.time;
        scoreText = scoreTextBox.GetComponent<Text>();
        scoreText.text = "個人資産：＄" + ScoreManager.Instance.Score;


    }

    // Update is called once per frame
    void Update()
    {
        pastTime = Time.time - scoreTime;
        scoreText.text = "個人資産$" + ScoreManager.Instance.Score;
        ScoreManager.Instance.AddScore(-1 * pastTime * pastTime * decreaseManager);

        if (ScoreManager.Instance.Score < 0)
        {
            SceneManager.LoadScene("GameOverMoney");
        }
        if (Input.GetKeyDown(KeyCode.Space) && !isMoving)
        {
            // コルーチンを開始して位置を移動する
            //StartCoroutine(MoveImage());
        }
    }

    private void ResetRemainingValues()
    {
        // 全ての値をリストに追加し、シャッフル
        remainingValues = new List<int>(allValues);
        ShuffleList(remainingValues);
    }

    private void ShuffleList(List<int> list)
    {
        // Fisher-Yatesアルゴリズムでリストをシャッフル
        for (int i = 0; i < list.Count; i++)
        {
            int randomIndex = Random.Range(i, list.Count);
            int temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
    private void RandomText()
    {
        if (remainingValues.Count == 0)
        {
            // すべての値が使用された場合、リセットして再シャッフル
            ResetRemainingValues();
        }
        int rad = remainingValues[0];
        remainingValues.RemoveAt(0);

        switch (rad)
        {
            case 1:
                subject.text = "MVDA株の再検討について";
                content.text = "MVDAの株価が設定した損切りラインを下回りました。ポートフォリオの再検討をお勧めします。";
                break;
            case 2:
                subject.text = "Amazone株価急落について";
                content.text = "Amazoneの株価が急落しました。速やかな対応が必要です。";
                break;
            case 3:
                subject.text = "Googol株式の緊急売却推奨";
                content.text = "Googolの株が目標価格に達しませんでした。売却を検討してください。";
                break;
            case 4:
                subject.text = "Microhard株価の急変動について";
                content.text = "Microhardの株が急落し、損失が発生しています。ポートフォリオの調整を推奨します。";
                break;
            case 5:
                subject.text = "Tessle株式の損切りライン到達";
                content.text = "Tessleの株価が大幅に下落しました。至急対策を講じる必要があります。";
                break;
            case 6:
                subject.text = "Faceblock株の大幅下落について";
                content.text = "Faceblockの株が急落しました。損失を最小限に抑えるための行動が求められます。";
                break;
            case 7:
                subject.text = "April株の異常な下落について";
                content.text = "Aprilの株価が設定した基準を下回りました。売却を検討してください。";
                break;
            case 8:
                subject.text = "Netfreedom株価の急落の件";
                content.text = "Netfreedomの株が目標価格に届かず、損失が拡大しています。対策が必要です。";
                break;
            case 9:
                subject.text = "Distiny株式の価格変動について";
                content.text = "Distinyの株価が設定ラインを下回りました。早急な対応をお勧めします。";
                break;
            case 10:
                subject.text = "Starbox株の急激な価格変動";
                content.text = "Starboxの株が急落しています。適切な対応が必要です。";
                break;
            case 11:
                subject.text = "WcDonalds株価下落のお知らせ";
                content.text = "WcDonaldsの株価が急落しました。速やかな行動を推奨します。";
                break;
            case 12:
                subject.text = "CocaCall株の再検討を推奨";
                content.text = "CocaCallの株が目標価格に達していません。ポートフォリオの調整を検討してください。";
                break;
            default:
                Debug.Log("rad関数で想定していない値が選ばれている。何かがおかしい。");
                break;
        }

    }

    public IEnumerator MoveImage()
    {
        isMoving = true;
        bool scoreAdded = false;  // スコアが加算されたかどうかのフラグ
        float elapsedTime = 0f; // 経過時間
        audioSource.PlayOneShot(notification);
        RandomText();

        // 最初の目標位置に移動
        while (elapsedTime < duration)
        {
            image.rectTransform.localPosition = Vector3.Lerp(originalPosition, targetPosition1, elapsedTime / duration * 5);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        audioSource.PlayOneShot(heartBeat);
        image.rectTransform.localPosition = targetPosition1;

        // 3秒待ってWキーが押されたか確認
        float waitTime = 0f;
        bool movedToSecondPosition = false;

        while (waitTime < 5f)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                movedToSecondPosition = true;
                break;
            }
            waitTime += Time.deltaTime;
            yield return null;
        }

        if (movedToSecondPosition)
        {
            // Wキーが押されたので次の目標位置に移動
            elapsedTime = 0f;


            while (elapsedTime < duration)
            {
                image.rectTransform.localPosition = Vector3.Lerp(targetPosition1, targetPosition2, elapsedTime / duration * 3);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            image.rectTransform.localPosition = targetPosition2;

            // 売るか、保持するかの処理
            bool decisionMade = false;
            waitTime = 0f;

            while (waitTime < 5f && (!decisionMade || waitTime < 1f))
            {
                if (Input.GetKeyDown(KeyCode.W) && !decisionMade)
                {
                    StartCoroutine(PlayGoodNewsSound());
                    Debug.Log("売った！！");
                    decisionMade = true;
                    ScoreManager.Instance.AddScore(-20 * pastTime * decreaseManager);  // スコアを加算
                    decreaseManager *= 0.5f;
                }
                waitTime += Time.deltaTime;
                yield return null;
            }

            // 元の位置に戻る
            elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                image.rectTransform.localPosition = Vector3.Lerp(targetPosition2, originalPosition, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            image.rectTransform.localPosition = originalPosition;
        }
        else
        {
            ScoreManager.Instance.AddScore(-1000 * pastTime * decreaseManager);  // スコアを加算

            // 何も押されなかったので元の位置に戻る
            elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                image.rectTransform.localPosition = Vector3.Lerp(targetPosition1, originalPosition, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            image.rectTransform.localPosition = originalPosition;
            audioSource.Stop();
        }

        isMoving = false;
    }

    // "goodNews" 音を再生するコルーチン
    private IEnumerator PlayGoodNewsSound()
    {
        audioSource.PlayOneShot(goodNews);
        yield return new WaitForSeconds(2f); // 2秒間音を鳴らし続ける
        audioSource.Stop();
    }




    private IEnumerator CallMethodAtRandomIntervals()
    {
        while (true)
        {
            // 15秒から30秒の間でランダムな時間を待機
            float randomInterval = Random.Range(15f, 30f);
            yield return new WaitForSeconds(20);
            StartCoroutine(MoveImage());
            // メソッドを呼び出し

        }
    }

}
