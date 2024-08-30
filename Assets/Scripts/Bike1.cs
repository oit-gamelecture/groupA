using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bike1 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
       transform.position = new Vector3(transform.position.x, 0f, transform.position.z);

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 position = transform.position;
        position.y = 0f;
        transform.position = position;
    }
}
