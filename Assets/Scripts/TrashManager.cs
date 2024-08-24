using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashManager : MonoBehaviour
{
    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

            Rigidbody rb = GetComponent<Rigidbody>();

          
                rb.AddForce(Vector3.forward * 10f, ForceMode.Impulse);
            
        }
    }

}
