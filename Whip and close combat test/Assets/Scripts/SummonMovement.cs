using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class SummonMovement : MonoBehaviour
{
    public float moveSpeed;
    Rigidbody2D _rb2D;
    float moveHorizontal;
    // Start is called before the first frame update
    void Start()
    {
        _rb2D = GetComponent<Rigidbody2D>();
        var vcam = GameObject.FindObjectOfType<CinemachineVirtualCamera>();
        vcam.Follow = this.gameObject.transform;
    }

    // Update is called once per frame
    void Update()
    {
         moveHorizontal = Input.GetAxis("Horizontal");

        if(Input.GetButtonDown("Fire1"))
        {
            Player.Instance.EndSummon();
            DestroyObject();
        }
    }

    private void FixedUpdate()
    {
        _rb2D.AddForce(new Vector2(moveHorizontal * moveSpeed, _rb2D.velocity.y));
    }

    public void DestroyObject()
    {
        Destroy(gameObject);
    }
}
