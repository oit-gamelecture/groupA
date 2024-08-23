using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Transform player;
    public int speed = 5;
    public float destroySpeed = 3f;
    public float duration = 3.0f;
    public int distance = 100;
    public float disableDuration = 2.0f;
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(transform.position.x, 0f, transform.position.z);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, 180, transform.eulerAngles.z);
        anim = GetComponent<Animator>();
        player = GameObject.Find("Player").transform;
        int r = Random.Range(1, 3);
        if(r == 1)
        {
            transform.position = new Vector3(-3, 0, transform.position.z);
        }

        if(r==2)
        {
            transform.position = new Vector3(0, 0, transform.position.z);
        }
        else
        {
            transform.position = new Vector3(3,0, transform.position.z);
        }


    }

    // Update is called once per frame
    void FixedUpdate()
    {
      
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - speed * Time.deltaTime);
        anim.SetBool("Walk", true);
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
           
            StartCoroutine("MoveDestroyCoroutine");
            
           
        }
    }

    IEnumerator MoveDestroyCoroutine()
    {
        float timeElapsed = 0.0f;

        while(timeElapsed < duration)
        {
            transform.Translate(Vector3.right * destroySpeed * Time.deltaTime);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }

    


}
