using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Random = UnityEngine.Random;

public class PlayerSolo : PlayerBase
{
    private Rigidbody rb;
    private Animator anim;

    private bool isGround;
    private float distToGround;

    void Start()
    {
        distToGround = GetComponent<Collider>().bounds.extents.y;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    /// <summary> Kick called through inputs or button events </summary>
    public override void JumpEvent()
    {
        if (IsGrounded())
        {
            // Add force to a rigidbody for jump
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    /// <summary> Kick called through inputs or button events </summary>
    public override void KickEvent()
    {
        anim.SetTrigger("Kick");

        if (canKick && ballRb != null)
        {
            AudioSource.PlayClipAtPoint(kickSound, Camera.main.transform.position);
            Vector3 rebound = new Vector3(Random.Range(1.5f, 2f), Random.Range(-0.5f, -1f), 0);
            ballRb.AddForce(rebound * kickForce, ForceMode.Force);
            Debug.Log("Chute");
        }
    }

    /// <summary> Add rebound to ball </summary>
    public override void BallRebound(Vector3 rebound)
    {
        ballRb.AddForce(rebound * kickForce, ForceMode.Force);
    }

    /// <summary> Detect contact with ground </summary>
    protected override bool IsGrounded()
    {
        isGround = Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f);
        return isGround;
    }

    /// <summary> Tolerance to kick after collision exit  </summary>
    protected override IEnumerator KickTimer()
    {
        yield return new WaitForSeconds(0.15f);
        canKick = false;
    }
}
