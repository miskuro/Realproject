//need to add player sticking to wall if J is pressed while sliding

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


public class Player : MonoBehaviour
{

    public GameObject startingPoint;
    public GameObject deadEnd;

    public float IncreaseSpeed = 0.005f;
    public GameObject cubePrefab;
    private bool SizeIncrease = false;
    private bool SizeDecrease = false;
    public float DecreaceSpeed = 0.0005f;
    public Camera mainCamera;
    public int cameraSize = 5;
    public float speed = 10;
    private bool walking = false; 
    private bool falling = false; 
    private bool sliding = false; 
    private float horizontal;
    private bool isFacingRight = true;
    public float jumpPower = 7f;
    private bool canJump = false;
    private bool willJump = false;
    private bool wallSlide = false;
    private float wallslideSpeed = 2f;

    public Animator anim;   


    private bool isWallJumping;
    private float wallJumpDir;
    private float wallJumpTime = 0.2f;
    private float wallJumpCounter;
    private float wallJumpDuration = 0.4f;
    public Vector2 wallJumpingPower;
    private GameObject checkpoint;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundlayer;

    [SerializeField] private Transform WallCheck;
    [SerializeField] private LayerMask WallLayer;
    void Update()
    {
        if (CheckpointManager.checkpoints.Count > 0)
            {
                checkpoint = CheckpointManager.checkpoints[0];
            }
            
            
        if (isGrounded())
        {
            deadEnd.transform.position = new Vector3(transform.position.x, transform.position.y - 200f , 0);
        }
        else{deadEnd.transform.position = deadEnd.transform.position;}

        Vector3 localScale = transform.localScale;
        if (localScale.y >= 2f)
        {
            localScale.y = 2f;
            localScale.x = 2f;
            transform.localScale = localScale;

        }

        spawnCube();
        speed = Mathf.Abs(-transform.localScale.y * 1.5f + 12f);
        jumpPower = Mathf.Abs(-transform.localScale.y + 12f);
        wallJumpingPower.x =  Mathf.Abs(-transform.localScale.y + 100f);
        wallJumpingPower.y =  Mathf.Abs(-transform.localScale.y + 100f);
        //Debug.Log(DecreaceSpeed);
        crease();
        if (SizeIncrease == false)
        {
            chrink();
        }
        
        mainCamera.orthographicSize = cameraSize;
        wallslide();
        wallJump();
        //anim.SetBool("walking", walking);
        //anim.SetBool("notjumping", isGrounded());
        //anim.SetBool("falling", falling);
        //anim.SetBool("slidin", sliding);

        if(!isWallJumping)
        {
            flip();
        }

        horizontal = Input.GetAxis("Horizontal");

        if (isGrounded())
        {
            falling = false;
            rb.gravityScale = 1.3f;
        }
        if (horizontal != 0 && isGrounded())
        {
            walking = true;
        }
        else
        {
            walking = false;
        }


        if (Input.GetButtonDown("Jump") && canJump)
        {

            willJump = true;
            StartCoroutine(tooLateToJump());
        }

        if (isGrounded() && willJump)
        {
            walking = false;
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
            willJump = false;
        
        }

        if (Input.GetButtonUp("Jump") && !isGrounded())
        {
            walking = false;
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
            falling = true;
            rb.gravityScale = 3.2f;
        }

        if (cameraSize < 0)
        {
            horizontal *= -1f;
        }
        else
        { 
            horizontal = Input.GetAxis("Horizontal");
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (CheckpointManager.checkpoints.Count > 0)
            {transform.position = checkpoint.transform.position;}
        }





        




    }

    void FixedUpdate()
    {

        if (!isWallJumping){
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        }


        RaycastHit2D hitGround = Physics2D.Raycast(groundCheck.position, -Vector2.up , 2f, groundlayer);
        Debug.DrawRay(groundCheck.position,-Vector2.up * hitGround.distance, Color.red);

        if (hitGround.collider != null)
        {
            if(hitGround.distance <= 1f && hitGround)
            {
                canJump = true;
            }

            else
            {
                canJump = false;
            }
        }




    }
    


    private void flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
            anim.SetTrigger("flip");
        }

    }

    private bool isGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundlayer);
    }


    private bool onWall()
    {
        return Physics2D.OverlapCircle(WallCheck.position, 0.3f, WallLayer);
    }

    private void wallslide()
    {
        if (onWall() && !isGrounded())// && rb.velocity.y < 0)
        {
            wallSlide = true;
            
            sliding = true;


        }
        else {wallSlide = false;
        sliding = false;}

        if (wallSlide)
        {
            rb.velocity = new Vector2(rb.velocity.x , Mathf.Clamp(rb.velocity.y, -wallslideSpeed, float.MaxValue));
        }

    }
    
    private void wallJump()
    {
        if (wallSlide)
        {
            isWallJumping = false;
            if (transform.localScale.x > 0f)
            {
                wallJumpDir = -1;
            }
            else{wallJumpDir = 1;}
            
            wallJumpCounter = wallJumpTime;

            CancelInvoke(nameof(stopWallJump));

        }
        else
        {
            wallJumpCounter -= Time.deltaTime;

        }

        if (Input.GetButtonDown("Jump") && wallJumpCounter > 0f)
        {
            isWallJumping = true;
            rb.velocity = new Vector2(wallJumpDir * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpCounter = 0f;


            if (transform.localScale.x != wallJumpDir)
            {
                isFacingRight = !isFacingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }

            Invoke(nameof(stopWallJump), wallJumpDuration); 

        }
    }


    private void stopWallJump()
    {
        isWallJumping = false;
    }



    private void chrink()
    {
        Vector3 localScale = transform.localScale;
        if (localScale.x > 0)
        {
            localScale.x -= DecreaceSpeed;
        }
        else{localScale.x += DecreaceSpeed;}
        
        localScale.y -= DecreaceSpeed;
        transform.localScale = localScale;
        if (localScale.x <= 0 && localScale.y <= 0)
        {
            //Destroy(gameObject);
            localScale.x = 1f;
            localScale.y = 1f;
            if (CheckpointManager.checkpoints.Count > 0)
            transform.position = checkpoint.transform.position;
            else{transform.position = startingPoint.transform.position;}
        }
        
    }

    private void crease()
    {
        if (SizeIncrease)
        {

            Vector3 localScale = transform.localScale;
            if (localScale.x > 0)
            {
                localScale.x += IncreaseSpeed;
            }
            else{localScale.x -= IncreaseSpeed;}
            
            localScale.y += IncreaseSpeed;
            
            transform.localScale = localScale;
        }
        else if (SizeDecrease)
        {
            DecreaceSpeed = 0.001f;

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Collectible"))
        {
            SizeIncrease = true;
        }
        else if (collision.CompareTag("decrease"))
        {
            SizeDecrease = true;
        }
        else if (collision.CompareTag("miniCube"))
        {
            Vector3 localScale = transform.localScale;
            localScale.y += 0.2f;
            if (localScale.x > 0)
            {
                localScale.x += 0.2f;
            }
            else{localScale.x -= 0.2f;}
            Debug.Log("hi");
        }
        else if (collision.CompareTag("launcher"))
        {
            rb.velocity = new Vector2 (rb.velocity.x, jumpPower * 2f);
        }
        else if (collision.CompareTag("launcher1"))
        {
            rb.velocity = new Vector2 (rb.velocity.x, jumpPower * 8f);
        }
        else if (collision.CompareTag("end"))
        {
            SceneManager.LoadScene(5);
        }
        else if (collision.CompareTag("death"))
        {
            
            transform.position = startingPoint.transform.position;
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Collectible"))
        {
            SizeIncrease = false;
        }
        else if (collision.CompareTag("decrease"))
        {
            SizeDecrease = false;
            DecreaceSpeed = 0.0005f;
        }
    }
    
    private IEnumerator tooLateToJump()
    {
        yield return new WaitForSeconds(0.5f);
        willJump = false;
    }
    private void spawnCube()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            Vector3 localScale = transform.localScale;
            Debug.Log("pressed J");
            localScale.y -= 0.2f;
            if (localScale.x > 0)
            {
                localScale.x -= 0.2f;
            }
            else{localScale.x += 0.2f;}
            transform.localScale = localScale;
            Instantiate(cubePrefab, new Vector3(transform.position.x , transform.position.y + 1.5f, 0) , Quaternion.identity);
        }
    }
    public void test()
    {
        Debug.Log("test test");
    }
}
