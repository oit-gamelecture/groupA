using UnityEngine;
using System.Collections;
using UnityEngine.UI;
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
    private Text content;
    // Start is called before the first frame update
    void Start()
    {
        //fpsを60に固定
        Application.targetFrameRate = 60;


        originalPosition = new Vector3(0, -1110, 0); //image.rectTransform.localPosition;
        targetPosition1 = image.rectTransform.localPosition + new Vector3(0, 200, 0);
        targetPosition2 = image.rectTransform.localPosition + new Vector3(0, 1150, 0);
        audioSource = GetComponent<AudioSource>();
        content = textBox.GetComponent<Text>();
        //blindfold.SetActive(false);

        // コルーチンを開始
        StartCoroutine(CallMethodAtRandomIntervals());

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isMoving)
        {
            // コルーチンを開始して位置を移動する
            //StartCoroutine(MoveImage());
        }
    }

    private void RandomText()
    {
        int rad = Random.Range(1, 13);

        switch (rad)
        {
            case 1:
                content.text = "MVDAの株価が暴落か！！！" + rad;
                break;
            case 2:
                content.text = "" + rad;
                break;
            case 3:
                content.text = "" + rad;
                break;
            case 4:
                content.text = "" + rad;
                break;
            case 5:
                content.text = "" + rad;
                break;
            case 6:
                content.text = "" + rad;
                break;
            case 7:
                content.text = "" + rad;
                break;
            case 8:
                content.text = "" + rad;
                break;
            case 9:
                content.text = "" + rad;
                break;
            case 10:
                content.text = "" + rad;
                break;
            case 11:
                content.text = "" + rad;
                break;
            case 12:
                content.text = "" + rad;
                break;
            default:
                Debug.Log("rad関数で想定していない値が選ばれている。何かがおかしい。");
                break;
        }

    }

    public IEnumerator MoveImage()
    {
        isMoving = true;
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

            //売るか、保持するかの処理

            bool decisionMade = false;
            waitTime = 0f;

            while (waitTime < 5f && (!decisionMade || waitTime < 2f))
            {
                if (Input.GetKeyDown(KeyCode.W))
                {
                    Debug.Log("売った！！");
                    decisionMade = true;
                }
                else if (Input.GetKeyDown(KeyCode.S))
                {
                    Debug.Log("保持した！！");
                    decisionMade = true;
                }
                waitTime += Time.deltaTime;
                yield return null;
            }

            // 1秒待つ
            //yield return new WaitForSeconds(1f);
            audioSource.Stop();

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
