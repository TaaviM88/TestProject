using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Throw : MonoBehaviour
{
    private Rigidbody2D weaponRb2D;
    Weapon weaponScript;
    private float returnTime;
    Movement movement;
    private Vector3 origLocPos;
    private Vector3 origLocRot;
    private Vector3 pullPosition;

    [Header("Public References")]
    public Transform weapon;
    public Transform hand;
    public Transform curvePoint;

    [Space]
    [Header("Parameters")]
    public float throwPower = 30;
    public float warpDuration = 2f;

    [Space]
    [Header("Bools")]
    public bool walking = true;
    public bool aiming = false;
    public bool hasWeapon = true;
    public bool pulling = false;

    public int facing = 1;

    bool isWarping = false;
    Material material;
    

    // Start is called before the first frame update
    void Start()
    {
        movement = GetComponent<Movement>();
        weaponRb2D = weapon.GetComponent<Rigidbody2D>();
        weaponScript = weapon.GetComponent<Weapon>();
        origLocPos = weapon.localPosition;
        origLocRot = weapon.localEulerAngles;
        material = GetComponent<Renderer>().material;
        
        
    }

    // Update is called once per frame
    void Update()
    {
        if(hasWeapon)
        {
            if(Input.GetButtonDown("Fire1"))
            {
                weaponScript.activated = true;
            }

            if (Input.GetButtonUp("Fire1"))
            {
                WeaponThrow();
            }

        }
        else
        {
            if(Input.GetButtonDown("Fire1") && weaponScript.canPulled && !isWarping)
            {
                WeaponStartPull();
            }

            if(Input.GetButtonDown("Fire2") && weaponScript.canPulled && !isWarping)
            {
                WarpToWeapon();
                isWarping = true;
            }
        }

        if(pulling)
        {
            if(returnTime <1)
            {
                weapon.position = GetQuadraticCurvePoint(returnTime, pullPosition, curvePoint.position, hand.position);
                returnTime += Time.deltaTime * 1.5f;
                
            }
            else
            {
                WeaponCatch();
            }
        }
}

    public void WeaponThrow()
    {
        hasWeapon = false;
        weaponScript.activated = true;
        weaponRb2D.isKinematic = false;
        weaponRb2D.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        weaponRb2D.gravityScale = 0;
        weapon.parent = null;
        weapon.eulerAngles = new Vector3(0,0,0);
        //weaponScript.SetRopeLifeTime(600);
        weaponScript.ToggleColliderTrigger(false);
        weaponScript.SetCameraToFollowWeapon();
        weaponScript.StartInvokeReapeaterForMarkers(0.05f);
        // weapon.transform.position += transform.right / 5;

        if (movement.looking == 0)
        {
            //weaponRb2D.AddForce(new Vector2(movement.facing, movement.looking) * throwPower, ForceMode2D.Impulse);
            weaponScript.ThrowWeapon(new Vector2(movement.facing, movement.looking), throwPower);
        }
        else
        {
            //weaponRb2D.AddForce(new Vector2(movement.GetHorizontalInput(), movement.looking) * throwPower, ForceMode2D.Impulse);
            weaponScript.ThrowWeapon(new Vector2(movement.GetHorizontalInput(), movement.looking), throwPower);
        }
        weaponScript.ResetRangeTimer();
    }

    private void WeaponCatch()
    {
        returnTime = 0;
        pulling = false;
        weapon.parent = hand;
        weaponScript.activated = false;
        weapon.localEulerAngles = origLocRot;
        weapon.localPosition = origLocPos;
        hasWeapon = true;
        weapon.localScale = new Vector3(1, 1, 1);
        weaponScript.ToggleRopeOff(false);
        weaponScript.ToggleColliderTrigger(true);
        weaponScript.CancelIncokeReapterForMarkers();
        weaponScript.DestroyMarkers();
        Player.Instance.SetCameraToFollowPlayer();
        Debug.Log("pelaajalla on ase" + hasWeapon);
    }

    private void WeaponStartPull()
    {
        pullPosition = weapon.position;
        weaponRb2D.Sleep();
        weaponRb2D.collisionDetectionMode = CollisionDetectionMode2D.Discrete;
        weaponRb2D.isKinematic = true;
        weapon.DORotate(new Vector3(0, 0, -90),.2f).SetEase(Ease.InOutSine);
        weapon.DOBlendableLocalRotateBy(Vector2.right * 90, .5f);
        weaponScript.activated = true;
        pulling = true;
        weaponScript.SetRopeLifeTime(20f);
        weaponScript.StartInvokeReapeaterForMarkers(0.05f);
    }

    public Vector3 GetQuadraticCurvePoint(float t, Vector2 p0, Vector2 p1, Vector2 p2)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        return (uu * p0) + (2 * u * t * p1) + (tt * p2);
    }

    public void WarpToWeapon()
    {
        movement.canMove = false;
        material.DOFloat(1, "_DissolveAmount", warpDuration);
        transform.DOMove(weapon.position, warpDuration).SetEase(Ease.InExpo).OnComplete(() => FinishWarp());
        //weaponScript.SetRopeLifeTime(0.5f);
        
        Rigidbody2D rb = movement.GetRigidbody();
        rb.isKinematic = true;
        
    }
    public void FinishWarp()
    {
        material.DOFloat(0, "_DissolveAmount", 0.2f);
        movement.canMove = true;
        isWarping = false;
        Rigidbody2D rb = movement.GetRigidbody();
        rb.isKinematic = false;
        weaponScript.CancelIncokeReapterForMarkers();
        weaponScript.DestroyMarkers();
        WeaponStartPull();
    }

   
}
