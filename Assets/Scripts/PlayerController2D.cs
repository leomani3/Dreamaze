using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController2D : MonoBehaviour
{
    public float speed;
    public float jumpForce;
    public float wallJumpForce;
    public float gravity;
    public int wallGripDivide;
    public bool canWallJump = false;

    public Transform groundCheck;
    public Transform wallCheckRight;
    public Transform wallCheckLeft;

    private Rigidbody2D rb;
    private float x;
    private float y;

    private Vector2 MoveVector;

    private bool grounded = false;
    private bool againstWallRight = false;
    private bool againstWallLeft = false;
    private bool isWallJumping = false;
    private bool jumping = false;
    private bool lastWallJumpedRight = false;
    private bool lastWallJumpedLeft = false;


    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        GroundCheck(); //check si le personnage est au sol est stock le résultat dans la variable grounded
        WallCheckLeft();
        WallCheckRight();

        if (!grounded) //si le personnage est en l'air en applique une force vers le bas pour simuler la gravité
        {
            if (((againstWallLeft || againstWallRight) && rb.velocity.y < 0) && canWallJump) //on veut que le personnage tombe moins vite quand il est contre un mur
            {
                MoveVector.y -= gravity / wallGripDivide * Time.deltaTime;
            }
            else
            {
                MoveVector.y -= gravity * Time.deltaTime;
            }
        }
        else
        {
            if (!jumping)
            {
                MoveVector.y = 0;
                jumping = false;
            }
        }

        Debug.Log(MoveVector);
        rb.velocity = MoveVector;
    }

    /// <summary>
    /// fait sauter le personnage d'une force de "jump force". Si le personnage est en l'air et contre un mur il fait un wall jmup
    /// </summary>
    public void Jump()
    {
        if (grounded)
        {
            MoveVector = new Vector2(0, jumpForce);
            jumping = true;
        }
        else
        {
            if (canWallJump)
            {
                if (againstWallLeft)
                {
                    isWallJumping = true;
                    MoveVector = new Vector2(wallJumpForce, jumpForce);
                    lastWallJumpedLeft = true;
                }
                if (againstWallRight)
                {
                    isWallJumping = true;
                    MoveVector = new Vector2(-wallJumpForce, jumpForce);
                    lastWallJumpedRight = true;
                }
            }
        }
    }

    /// <summary>
    /// Permet de déplacer le personnage sur l'axe horizontal selon la vitesse fixée par "speed"
    /// </summary>
    public void HorizontalMove(float dir)
    {
        float x = dir * speed;
        if (!isWallJumping)
        {
            MoveVector.x = x;
        }
        else
        {
            if (lastWallJumpedLeft && x < 0)
            {
                MoveVector.x += x / 7;
            }
            else if (lastWallJumpedRight && x > 0)
            {
                MoveVector.x += x / 7;
            }
        }
    }






    private void GroundCheck()
    {
        grounded = false;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, 0.01f);

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                grounded = true;
                isWallJumping = false;
                lastWallJumpedRight = false;
                lastWallJumpedLeft = false;
            }
        }
    }

    private void WallCheckRight()
    {
        againstWallRight = false;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(wallCheckRight.position, 0.01f);

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                if (lastWallJumpedLeft)
                {
                    lastWallJumpedLeft = false;
                    isWallJumping = false;
                }
                againstWallRight = true;
            }
        }
    }

    private void WallCheckLeft()
    {
        againstWallLeft = false;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(wallCheckLeft.position, 0.01f);

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                if (lastWallJumpedRight)
                {
                    lastWallJumpedRight = false;
                    isWallJumping = false;
                }
                againstWallLeft = true;
            }
        }
    }
}