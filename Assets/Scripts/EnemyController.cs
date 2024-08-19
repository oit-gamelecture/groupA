using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Transform player;
    public int speed = 5;
    public int distance = 100;
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
            transform.position = new Vector3(0, transform.position.y, transform.position.z);
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
}
