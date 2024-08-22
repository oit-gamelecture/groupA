using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PasserManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(transform.position.x, 0f,transform.position.z);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, 90, transform.eulerAngles.z);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
