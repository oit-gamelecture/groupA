using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialPositionRotation : MonoBehaviour
{
   
    // Start is called before the first frame update
    void Start()
    {
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, 90, transform.eulerAngles.z);
    }
}
