using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StreetLightManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(transform.position.x, 0f, transform.position.z);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, 90, transform.eulerAngles.z);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 position = transform.position;
        if (position.x == 0 || position.x == 3)
        {
            position.x = -3.0f;
            transform.position = position;
        }
    }
}
