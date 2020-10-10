using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody rb;
    private Rigidbody ballRb;
    private Animator anim;
    private Vector3 moveInput;

    [Header("Stats")]
    [Range(4, 8)] public float moveSpeed = 2;
    [Range(30, 60)] public float jumpForce = 2;
    public float kickForce = 2;

    [Header("Debug - No Modify")]
    public bool isGround;
    public float distToGround;
    public bool canKick;

    void Start()
    {
        distToGround = GetComponent<Collider>().bounds.extents.y;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    void Update()
    {
        // Get Move Input
        moveInput = new Vector3(Input.GetAxis("Horizontal"), 0, 0);

        // Start Jump
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            Jump();
        }

        // K I C K
        if (Input.GetMouseButtonDown(0) && canKick)
        {
            anim.SetTrigger("Kick");

            if (ballRb != null)
            {
                Vector3 rebound = new Vector3(-(Random.Range(1.5f, 2f)), Random.Range(0.5f, 1f), 0);
                ballRb.AddForce(rebound * kickForce, ForceMode.Force);
                Debug.Log("Chute");
            }
        }
    }

    void FixedUpdate()
    {
        // Move
        transform.position += moveInput * moveSpeed * Time.fixedDeltaTime;
    }

    // Detect ground
    private bool IsGrounded()
    {
        isGround = Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f);
        return isGround;
    }

    // Add force to a rigidbody for jump
    private void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision other) 
    {
        if (other.gameObject.tag != "Ball") return;

        canKick = true;
        Vector3 rebound = new Vector3(-(Random.Range(0.5f, 1f)), Random.Range(0.5f, 1f), 0);

        // quando a bola só bate no jogador
        if (IsGrounded() && moveInput.x == 0)
        {
            rebound = new Vector3(-(Random.Range(0.5f, 1f)), Random.Range(0.5f, 1f), 0);
            Debug.Log("Kick 1");
        }
        // quando a bola bate no jogador e ele esta pulando
        else if (!IsGrounded() && moveInput.x == 0)
        {
            rebound = new Vector3(-(Random.Range(0.5f, 1f)), Random.Range(1f, 1.5f), 0);
            Debug.Log("Kick 2");
        }
        // quando a bola bate no jogador e ele não esta pulando mas se movendo
        else if (IsGrounded() && moveInput.x != 0)
        {
            rebound = new Vector3(-(Random.Range(1f, 1.5f)), Random.Range(0.5f, 1f), 0);
            Debug.Log("Kick 3");
        }
        // quando a bola bate no jogador e ele esta pulando e se movendo
        else if (!IsGrounded() && moveInput.x != 0)
        {
            rebound = new Vector3(-(Random.Range(1f, 1.5f)), Random.Range(0.5f, 1f), 0);
            Debug.Log("Kick 4");
        }

        // kick
        if (other.gameObject.tag == "Ball")
        {
            if (ballRb == null)
                ballRb = other.gameObject.GetComponent<Rigidbody>();

            ballRb.AddForce(rebound * kickForce, ForceMode.Force);
        }
    }

    private void OnCollisionExit(Collision other) 
    {
        if (other.gameObject.tag == "Ball")
        {
            StartCoroutine("KickTimer");
        }
    }

    private IEnumerator KickTimer()
    {
        yield return new WaitForSeconds(0.1f);
        canKick = false;
    }
}
