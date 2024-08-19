using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    public float laneSpeed;
    public float jumpLength;
    public float jumpHeight;
    public float MaxHealth;
    public Slider slide;
    public bool strike;

    private float currentHealth;
    private Animator anim;
    private Rigidbody rb;
    private int currentLane = 3;
    private Vector3 verticalTargetPosition;
    private bool jumpManager;
    private bool jumping = false;
    private float jumpStart;

    //Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        currentHealth = MaxHealth;
        slide.maxValue = MaxHealth;
        slide.value = MaxHealth;

        Application.targetFrameRate = 60;
    }

    //Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            ChangeLane(-3);
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            ChangeLane(3);
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        if (jumping)
        {
            float ratio = (transform.position.z - jumpStart) / jumpLength;
            if (ratio >= 1f)
            {
                jumping = false;
                anim.SetBool("Jumping", false);
            }
            else
            {
                verticalTargetPosition.y = Mathf.Sin(ratio * Mathf.PI) * jumpHeight;
            }
        }
        else
        {
            verticalTargetPosition.y = Mathf.MoveTowards(verticalTargetPosition.y, 0, 5 * Time.deltaTime);
        }
        Vector3 targetPosition = new Vector3(verticalTargetPosition.x, verticalTargetPosition.y, transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, laneSpeed * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        rb.velocity = Vector3.forward * speed;
    }

    void ChangeLane(int direction)
    {
        int targetLane = currentLane + direction;
        if (targetLane < 0 || targetLane > 6)
            return;
        currentLane = targetLane;
        verticalTargetPosition = new Vector3((currentLane - 3), 0, 0);
    }

    void Jump()
    {
        if (!jumping)
        {
            jumpStart = transform.position.z;
            anim.SetFloat("JumpSpeed", speed / jumpLength);
            anim.SetBool("Jumping", true);
            jumping = true;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "enemy")
        {
            TakeDamage(1f);
            anim.SetTrigger("Damage");
            strike = true;
            if (jumpManager == true)
            {
                Vector3 targetPosition = new Vector3(0, 0, -10);
                rb.MovePosition(targetPosition);
            }

        }
    }

    void TakeDamage(float Damage)
    {
        currentHealth = currentHealth - Damage;
        slide.value = currentHealth;
    }
}
