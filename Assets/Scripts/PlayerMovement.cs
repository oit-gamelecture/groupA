using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    public float laneSpeed;
    public float jumpLength;
    public float jumpHeight;
    public float MaxHealth;
    public Slider slide;
    public bool strike = false;
    public bool isFeatureActive = true;
    public float jumpCooldown = 5f; // �W�����v�ł��Ȃ����ԁi�b�j
    private bool isJumpRestricted = false; // �W�����v���������ǂ���


    private bool isReadyGoCoroutineRunning = false;
    private float currentHealth;
    private Animator anim;
    private Rigidbody rb;
    private int currentLane = 3;
    private Vector3 verticalTargetPosition;
    private bool jumping = false;
    private float jumpStart;
    private bool canMove = false;
    private bool canJump = false;
    private bool canMoveLane = false;


    //Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        currentHealth = MaxHealth;
        slide.maxValue = MaxHealth;
        slide.value = MaxHealth;

        StartCoroutine(ReadyGoCoroutine());
    }


    //Update is called once per frame
    void Update()
    {
        if (canMove)
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
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            rb.velocity = Vector3.forward * speed;
        }
    }

    void ChangeLane(int direction)
    {
        if (canMoveLane)
        {
            int targetLane = currentLane + direction;
            if (targetLane < 0 || targetLane > 6)
                return;
            currentLane = targetLane;
            verticalTargetPosition = new Vector3((currentLane - 3), 0, 0);
        }
    }

    void Jump()
    {
        if (canJump && !isJumpRestricted)
        {
            if (!jumping)
            {
                jumpStart = transform.position.z;
                anim.SetFloat("JumpSpeed", speed / jumpLength);
                anim.SetBool("Jumping", true);
                jumping = true;
                StartCoroutine(JumpRestrictionCoroutine());
            }
        }

    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "enemy" || other.gameObject.tag == "MoveEnemy")
        {
            TakeDamage(1f);
            anim.SetTrigger("Damage");
            strike = true;
            StartCoroutine("DisableFeatureCoroutine");
        }

        if (other.gameObject.tag == "trip")
        {
            TakeDamage(0.5f);
            anim.SetTrigger("Trip");
            anim.SetTrigger("Up");
            anim.SetTrigger("Back");
            strike = true;
            StartCoroutine("TripFeatureCoroutine");
        }
    }

    IEnumerator DisableFeatureCoroutine()
    {

        speed = -6;
        yield return new WaitForSeconds(1.7f);
        speed = 10;
    }

    IEnumerator TripFeatureCoroutine()
    {
        speed = 0;

        yield return new WaitForSeconds(1.0f);

        speed = 10;
    }

    IEnumerator ReadyGoCoroutine()
    {
        speed = 0;
        canMove = false;
        canJump = false;
        canMoveLane = false;
        isJumpRestricted = false;
        yield return new WaitForSeconds(0);
        canMove = true;
        canJump = true;
        canMoveLane = true;
        anim.SetTrigger("idle");
        speed = 10;
    }

    IEnumerator JumpRestrictionCoroutine()
    {
        isJumpRestricted = true; // �W�����v�𐧌�����
        yield return new WaitForSeconds(jumpCooldown); // �������ԑ҂�
        isJumpRestricted = false; // �W�����v��������
    }


    void TakeDamage(float Damage)
    {
        currentHealth = currentHealth - Damage;
        slide.value = currentHealth;
        if (slide.value == 0)
        {
            SceneManager.LoadScene("GameOver");
        }
    }
}
