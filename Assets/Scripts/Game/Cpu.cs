using UnityEngine;
using System.Collections;

public class Cpu : MonoBehaviour {

	/// <summary>
	/// Main CPU controller. 
	/// This class ia a compact AI manager which moves the cpu opponent and manage its decisions.
	/// </summary>

	// A dropdown option to select the AI level
	// The different AI levels uses different parameters for the speed and accuracy of the cpu.
	public enum CpuLevels { easy, normal, hard, veryHard }
	public CpuLevels cpuLevel = CpuLevels.easy;

	private Rigidbody rb;
    private Rigidbody ballRb;
	private GameObject ball;
    private Animator anim;

	private bool canJump = true;
	private float adjustingPosition;
	private bool canMove = true;

	[Header("private AI parameters")]
	[SerializeField] float moveSpeed;
	[SerializeField] float jumpSpeed;
	[SerializeField] float minJumpDistance;
	[SerializeField] float jumpDelay;
	[SerializeField] float accuracy;
    [SerializeField] float maxProtectedDistance;
    [SerializeField] float kickForce ;

    [Header("Move Rules")]
	public Vector2 cpuFieldLimits = new Vector2(1f, 8.5f);

    [Header("Audios")]
    public AudioClip kickSound;
    public AudioClip headSound;

    [Header("Debug - No Modify")]
    public bool isGround;
    public float distToGround;

	void Awake () 
    {
        distToGround = GetComponent<Collider>().bounds.extents.y;
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

		// Find and cache ball object
		ball = GameObject.FindGameObjectWithTag("Ball");

		// StartCoroutine(ResetPosition());
		SetCpuLevel();
	}

	//change the AI configuration based on the enum selection
	public void SetCpuLevel() 
    {
		switch(cpuLevel) 
        {
            case CpuLevels.easy:
                moveSpeed = 6.0f;
                jumpSpeed = 40.0f;
                minJumpDistance = 2.5f;
                accuracy = 0.4f;
                jumpDelay = 0.3f;
                maxProtectedDistance = -1.5f;
                kickForce = 180;
                break;

            case CpuLevels.normal:
                moveSpeed = 8.0f;
                jumpSpeed = 50.0f;
                minJumpDistance = 3.5f;
                accuracy = 0.3f;
                jumpDelay = 0.2f;
                maxProtectedDistance = -2.25f;
                kickForce = 220;
                break;

            case CpuLevels.hard:
                moveSpeed = 10.0f;
                jumpSpeed = 60.0f;
                minJumpDistance = 3.5f;
                accuracy = 0.2f;
                jumpDelay = 0.1f;
                maxProtectedDistance = -3.25f;
                kickForce = 260;
                break;
            case CpuLevels.veryHard:
                moveSpeed = 13.0f;
                jumpSpeed = 60.0f;
                minJumpDistance = 3.5f;
                accuracy = 0.15f;
                jumpDelay = 0.05f;
                maxProtectedDistance = -4f;
                kickForce = 280;
                break;
		}
	}
	
	void FixedUpdate () 
    {
		if(canMove && !GameController.get.waitScore) // && !GameController.isGoalTransition && !GameController.gameIsFinished)
			CpuMoveToBall();
	}

	/// <summary> Main cpu play routines </summary>
	void CpuMoveToBall() 
    {
		if(ball.transform.position.x < cpuFieldLimits.x && ball.transform.position.x < cpuFieldLimits.y) 
        {
			//move the cpu towards the ball

			transform.position = new Vector3(Mathf.SmoothStep(transform.position.x, ball.transform.position.x + adjustingPosition, Time.fixedDeltaTime * moveSpeed),
			                                 transform.position.y,
			                                 transform.position.z);

			//if cpu is close enough to the ball, make it jump
			if(Vector3.Distance(transform.position, ball.transform.position) < minJumpDistance && IsGrounded() && canJump) {
				canJump = false;
				Vector3 jumpPower = new Vector3(0, jumpSpeed - Random.Range(0, 20), 0);
				rb.AddForce(jumpPower, ForceMode.Impulse);
			}
		}

        if(ball.transform.position.x > cpuFieldLimits.x)
        {
			transform.position = new Vector3(Mathf.SmoothStep(transform.position.x, maxProtectedDistance, Time.fixedDeltaTime * moveSpeed),
			                                 transform.position.y,
			                                 transform.position.z);
        }
	}

	// Take the object to its initial position
	public IEnumerator ResetPosition() 
    {
		canMove = false;

		rb.sleepThreshold = 0.005f;
		rb.velocity = Vector3.zero;
		rb.angularVelocity = Vector3.zero;
        
		adjustingPosition = Random.Range(0.1f, accuracy);

		yield return new WaitForSeconds(0.75f);
		canMove = true;
	}

    // Detect ground
    private bool IsGrounded()
    {
        isGround = Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f);
        return isGround;
    }

	/// <summary> enable jump ability again  </summary>
	private IEnumerator JumpActivation() 
    {
		yield return new WaitForSeconds(jumpDelay);
		canJump = true;
	}

	/// <summary> Shoot the ball upon collision </summary>
	void OnCollisionEnter (Collision other) 
    {	
        if (other.gameObject.tag != "Ball") return;

        Vector3 rebound = new Vector3(-(Random.Range(0.5f, 1f)), Random.Range(0.5f, 1f), 0);

        // quando a bola só bate no jogador
        if (IsGrounded())
        {
            rebound = new Vector3(Random.Range(1.5f, 2f), Random.Range(0.5f, 1f), 0);
            AudioSource.PlayClipAtPoint(kickSound, Camera.main.transform.position);
            Debug.Log("Kick 1");
        }

        // quando a bola bate no jogador e ele esta pulando
        else if (!IsGrounded())
        {
            rebound = new Vector3(Random.Range(1f, 1.5f), Random.Range(1.2f, 1.8f), 0);
            AudioSource.PlayClipAtPoint(headSound, Camera.main.transform.position);
            Debug.Log("Kick 2");
        }

        // R E B O U N D
        if (other.gameObject.tag == "Ball")
        {
            anim.SetTrigger("Kick");

            // rand chance of enable jump
            if(IsGrounded() && Random.Range(0, 2) == 0)
                StartCoroutine(JumpActivation());
                
            if (ballRb == null)
                ballRb = other.gameObject.GetComponent<Rigidbody>();

            ballRb.AddForce(rebound * kickForce, ForceMode.Force);
        }
	}
}
