using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConeManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Vector3 position = transform.position;
        position.y = 0;
        transform.position = position;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
