using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMoveController : MonoBehaviour
{
    //[SerializeField]
    //private int maxJumpTimes = 2;
    //private int currentJumpTimes;
    //[SerializeField]
    //private Transform groundCheckCenterTrans;
    [SerializeField]
    private float groundCheckRadius;
    [SerializeField]
    private LayerMask whatIsGround;
    private bool isGround;
    
    [SerializeField]
    private float jumpSpeed = 10f;
    [SerializeField]
    private float onceJumpMaxDuration = 0.5f;
    private float currentJumpDuration;
    private bool isJumping;
    [SerializeField] public float moveSpeed = 3f;
    [SerializeField] private float addSpeed = 0.05f;
    private float keepWalk;
    [SerializeField] private Rigidbody2D rb;
    private Collider2D coll;
    private float minusSpeed;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        keepWalk = 0f;
        //isFacingRight = true;
        //currentJumpTimes = 0;
        isGround = true;
        currentJumpDuration = 0f;
        isJumping = false;
    }

    void FixedUpdate()
    {
        CheckGround();
        if (isJumping)
            if (ShouldStopJump())
                StopJump();
            else
                JumpContinuously();
    }
    public void Move(float faceDir){
        transform.Translate(keepWalk * moveSpeed * Time.deltaTime, 0, 0);        
        if (faceDir != 0)//轉方向
        {
            GetComponent<Animator>().SetBool("start", true);
            keepWalk = faceDir;
            transform.localScale = new Vector3(faceDir,1,1);
        }
    }
    public void Jump()
    {
        /*if (!CanJump())
        {
            UnityEngine.Debug.Log("達到最大跳躍次數！");
            return;
        }*/
        //自己增加的
        if (!isGround){
            return;
        }
        moveSpeed += addSpeed;
        rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
        //currentJumpTimes++;
        isJumping = true;
    }

    /*void ResetJumpTimes()
    {
        currentJumpTimes = 0;
    }*/

    /*bool CanJump()
    {
        return currentJumpTimes < maxJumpTimes;
    }*/
    void CheckGround()
    {
        Vector2 vector = new Vector2 (transform.position.x, transform.position.y - 0.5f);
        if (isGround && rb.velocity.y > 0){
            //UnityEngine.Debug.Log("notGround");
            isGround = false;
        }
        else if (!isGround && rb.velocity.y <= 0 && Physics2D.OverlapCircle(vector, groundCheckRadius, whatIsGround))
        {
            //UnityEngine.Debug.Log("isGround");
            //UnityEngine.Debug.Log(Physics2D.OverlapCircle(vector, groundCheckRadius, whatIsGround));
            isGround = true;
            GetComponent<Animator>().SetBool("jump", false);
            //ResetJumpTimes();
        }
        /*if (coll.IsTouchingLayers(whatIsGround))
            isGround = true;
        else
            isGround = false;*/

    }
    bool ShouldStopJump()
    {
        return (currentJumpDuration < onceJumpMaxDuration) ? false : true;       
    }

    public void StopJump()
    {
        if (!isJumping)
            return;
        isJumping = false;
        currentJumpDuration = 0;
    }

    void JumpContinuously()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
        currentJumpDuration += Time.deltaTime;
    }

    // 轉身
    /*void TurnBack()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1f;
        transform.localScale = scale;
    }*/


    

}
