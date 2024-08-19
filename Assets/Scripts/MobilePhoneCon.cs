using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Unity.VisualScripting;

public class MobilePhoneCon : MonoBehaviour
{
    public Image image;  // コントロールするImageオブジェクト
    public float duration = 1.0f;  // アニメーションの時間
    private Vector3 originalPosition;  // Imageの初期位置
    private Vector3 targetPosition1;  // Imageの目標座標
    private Vector3 targetPosition2;
    private bool isMoving = false;  // 位置が移動中かどうか
    // Start is called before the first frame update
    void Start()
    {
        originalPosition = image.rectTransform.localPosition;
        targetPosition1 = image.rectTransform.localPosition + new Vector3(0, 200, 0);
        targetPosition2 = image.rectTransform.localPosition + new Vector3(0, 550, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isMoving)
        {
            // コルーチンを開始して位置を移動する
            StartCoroutine(MoveImage());

        }
    }

    public IEnumerator MoveImage()
    {
        isMoving = true;
        float elapsedTime = 0f; //経過した時間

        while (elapsedTime < duration)
        {
            image.rectTransform.localPosition = Vector3.Lerp(originalPosition, targetPosition1, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        image.rectTransform.localPosition = targetPosition1;

        yield return new WaitForSeconds(3f);
        elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            image.rectTransform.localPosition = Vector3.Lerp(targetPosition1, targetPosition2, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

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

        isMoving = false;
    }
}
