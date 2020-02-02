using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{

    Rigidbody2D _rb2D;
    [Header("Public References")]

    [Header("Parameters")]
    public float moveSpeed = 5f;
    public int facing = 1;
    public float looking = 0;
    public bool canMove = true;
    private float rawHorizontal = 0;
     private float originalScaleX;
    // Start is called before the first frame update
    void Start()
    {
        _rb2D = GetComponent<Rigidbody2D>();
        originalScaleX = transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        if(!canMove)
        {
            return;
        }

        float horizontalMovement = Input.GetAxis("Horizontal");
        rawHorizontal = Input.GetAxisRaw("Horizontal");
        float flipX = rawHorizontal;
        float flipY = Input.GetAxisRaw("Vertical");

        if(horizontalMovement !=0 && Input.GetButton("Fire2"))
        {
            Walk(horizontalMovement);
        }

        if(flipX != 0)
        {
            FlipPlayer(flipX);
        }
            looking = (int)flipY;
        
    }

    public void Walk(float dir)
    {
        Vector2 playerVelocity = new Vector2(dir * moveSpeed, _rb2D.velocity.y);
        _rb2D.velocity = playerVelocity;
    }

    public void FlipPlayer(float x)
    {
       
       transform.localScale = new Vector3(x * originalScaleX, transform.localScale.y, transform.localScale.z);
        facing = (int)x;

    }

    public float GetHorizontalInput()
    {
        return rawHorizontal;
    }

    public Rigidbody2D GetRigidbody()
    {
        return _rb2D;
    }
}
