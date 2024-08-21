using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlphaCon : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float sin = Mathf.Sin(Time.time);
        float absSin = Mathf.Abs(sin);
        canvasGroup.alpha = absSin;
    }
}
