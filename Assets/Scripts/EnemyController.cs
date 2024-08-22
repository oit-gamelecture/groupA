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
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").transform;
        int r = Random.Range(1, 3);
        if(r == 1)
        {
            transform.position = new Vector3(-3, transform.position.y, transform.position.z);
        }

        if(r==2)
        {
            transform.position = new Vector3(0, transform.position.y, transform.position.z);
        }
        else
        {
            transform.position = new Vector3(3, transform.position.y, transform.position.z);
        }


    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!GameObject.Find("Player").GetComponent<PlayerMovement>().strike)
        {
            if(Vector3.Distance(transform.position, player.position) <= distance)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - speed * Time.deltaTime);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
           
            StartCoroutine("MoveDestroyCoroutine");
            StartCoroutine("DisableCollidersTemporarily");
           
        }
    }

    IEnumerator MoveDestroyCoroutine()
    {
        float timeElapsed = 0.0f;

        while(timeElapsed < duration)
        {
            transform.Translate(Vector3.left * destroySpeed * Time.deltaTime);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }

    IEnumerator DisableCollidersTemporarily()
    {
        GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag("enemy");

        foreach(GameObject obj in taggedObjects)
        {
            if(obj.GetComponent<Collider>() != null)
            {
                obj.GetComponent<Collider>().enabled = false;
            }
        }

        yield return new WaitForSeconds(disableDuration);

        foreach(GameObject obj in taggedObjects)
        {
            if(obj.GetComponent<Collider>()!=null)
            {
                obj.GetComponent<Collider>().enabled = true;
            }
        }
    }


}
