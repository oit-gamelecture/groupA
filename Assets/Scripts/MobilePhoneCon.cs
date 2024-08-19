using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Unity.VisualScripting;

public class MobilePhoneCon : MonoBehaviour
{
    public GameObject blindfold;  //目隠しを入れるオブジェクト
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
        originalPosition = image.rectTransform.localPosition;
        targetPosition1 = image.rectTransform.localPosition + new Vector3(0, 200, 0);
        targetPosition2 = image.rectTransform.localPosition + new Vector3(0, 550, 0);
        audioSource = GetComponent<AudioSource>();
        content = textBox.GetComponent<Text>();
        blindfold.SetActive(false);

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
        float elapsedTime = 0f; //経過した時間
        audioSource.PlayOneShot(notification);
        RandomText();

        while (elapsedTime < duration)
        {
            image.rectTransform.localPosition = Vector3.Lerp(originalPosition, targetPosition1, elapsedTime / duration * 5);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        audioSource.PlayOneShot(heartBeat);
        blindfold.SetActive(true);

        image.rectTransform.localPosition = targetPosition1;

        yield return new WaitForSeconds(3f);
        elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            image.rectTransform.localPosition = Vector3.Lerp(targetPosition1, targetPosition2, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        audioSource.Stop();
        // 画像を新しい位置に移動する
        image.rectTransform.localPosition = targetPosition2;

        // 3秒待つ
        yield return new WaitForSeconds(3f);

        elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            image.rectTransform.localPosition = Vector3.Lerp(targetPosition2, originalPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 元の位置に戻す
        image.rectTransform.localPosition = originalPosition;

        blindfold.SetActive(false);
        isMoving = false;
    }

    private IEnumerator CallMethodAtRandomIntervals()
    {
        while (true)
        {
            // 15秒から30秒の間でランダムな時間を待機
            float randomInterval = Random.Range(15f, 30f);
            yield return new WaitForSeconds(randomInterval);
            StartCoroutine(MoveImage());
            // メソッドを呼び出し

        }
    }
}
