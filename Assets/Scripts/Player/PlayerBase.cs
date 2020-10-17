using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public abstract class PlayerBase : MonoBehaviour
{
    protected Rigidbody ballRb;
    protected Vector3 moveInput;
    protected bool canKick;

    [Header("Stats")]
    [Range(4, 8)] public float moveSpeed = 7;
    [Range(30, 60)] public float jumpForce = 40;
    [Range(140, 240)] public float kickForce = 180;

    [Header("Audios")]
    public AudioClip kickSound;
    public AudioClip headSound;

    protected virtual void Update()
    {
        // Get Move Input
        moveInput = new Vector3(CrossPlatformInputManager.GetAxis("Horizontal"), 0, 0);

        // Start Jump
        if (Input.GetKeyDown(KeyCode.Z))
        {
            JumpEvent();
        }

        // K I C K
        if (Input.GetKeyDown(KeyCode.X))
        {
            KickEvent();
        }
    }

    protected virtual void FixedUpdate()
    {
        // Can Move
        if (!GameController.get.waitScore)
        {
            // Move
            transform.position += moveInput * moveSpeed * Time.fixedDeltaTime;
        }
    }

    protected virtual void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag != "Ball") return;

        AudioSource.PlayClipAtPoint(headSound, Camera.main.transform.position);

        canKick = true;
        Vector3 rebound = new Vector3(Random.Range(0.5f, 1f), Random.Range(-0.5f, -1f), 0);

        // quando a bola só bate no jogador
        if (IsGrounded() && moveInput.x == 0)
        {
            rebound = new Vector3(Random.Range(0.5f, 1f), Random.Range(-0.5f, -1f), 0);
            Debug.Log("Kick 1");
        }
        // quando a bola bate no jogador e ele esta pulando
        else if (!IsGrounded() && moveInput.x == 0)
        {
            rebound = new Vector3(Random.Range(0.5f, 1f), Random.Range(-1f, -1.5f), 0);
            Debug.Log("Kick 2");
        }
        // quando a bola bate no jogador e ele não esta pulando mas se movendo
        else if (IsGrounded() && moveInput.x != 0)
        {
            rebound = new Vector3(Random.Range(1f, 1.5f), Random.Range(-0.5f, -1f), 0);
            Debug.Log("Kick 3");
        }
        // quando a bola bate no jogador e ele esta pulando e se movendo
        else if (!IsGrounded() && moveInput.x != 0)
        {
            rebound = new Vector3(Random.Range(1f, 1.5f), Random.Range(-0.5f, -1f), 0);
            Debug.Log("Kick 4");
        }

        // R E B O U N D
        if (other.gameObject.tag == "Ball")
        {
            if (ballRb == null)
                ballRb = other.gameObject.GetComponent<Rigidbody>();

            BallRebound(rebound);
        }
    }

    protected virtual void OnCollisionExit(Collision other)
    {
        if (other.gameObject.tag == "Ball")
        {
            StartCoroutine(KickTimer());
        }
    }

    #region Abstract Methods

    /// <summary> Kick called through inputs or button events </summary>
    public abstract void JumpEvent();

    /// <summary> Kick called through inputs or button events </summary>
    public abstract void KickEvent();

    /// <summary> Add rebound to ball </summary>
    public abstract void BallRebound(Vector3 rebound);

    /// <summary> Detect contact with ground </summary>
    protected abstract bool IsGrounded();

    /// <summary> Tolerance to kick after collision exit  </summary>
    protected abstract IEnumerator KickTimer();

    #endregion    
}
