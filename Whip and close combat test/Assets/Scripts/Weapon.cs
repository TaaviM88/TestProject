using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class Weapon : MonoBehaviour
{
    [Header("Public References")]
    public TrailRenderer rope;
    public GameObject hitEffect;
    public GameObject marker;
    [Space]

    [Header("Parameters")]
    public bool activated;
    public float rotationSpeed;
    public float damage;
    public bool canPulled = false;
    public float rangeTime = 2f;
    float orginalRangeTime;
    Rigidbody2D _rb2d;
    PolygonCollider2D polygonCollider;
    List<GameObject> markers = new List<GameObject>();
    Vector3 throwDirection = Vector2.zero;
    public void Start()
    {
        _rb2d = GetComponent<Rigidbody2D>();
        _rb2d.isKinematic = true;
        orginalRangeTime = rangeTime;
        ToggleRopeOff(false);
        hitEffect.gameObject.SetActive (false);
        polygonCollider = GetComponent<PolygonCollider2D>();
        ToggleColliderTrigger(true);
        markers.Clear();
    }
    // Update is called once per frame
    void Update()
    {
        if (activated)
        {
            transform.localEulerAngles += Vector3.forward * rotationSpeed * Time.deltaTime;
            canPulled = false;

            if(rangeTime > 0 )
            {
                rangeTime -= Time.deltaTime;
                
            }
            else
            {
                _rb2d.gravityScale = 1;
                canPulled = true;
            }
            //rope.gameObject.SetActive(true);
        }

        if(canPulled )
        {
            ResetRangeTimer();

        }

        if(rope.gameObject.activeInHierarchy)
        {
            //rope.SetPosition(0, gameObject.transform.position);
            //rope.SetPosition(1, Player.Instance.gameObject.transform.position);
        }
    }

    public void ThrowWeapon(Vector2 direction, float power)
    {
        throwDirection = direction;
        _rb2d.AddForce(new Vector2(direction.x, direction.y) * power, ForceMode2D.Impulse);
        ResetRangeTimer();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //default layer(0)
        if (collision.gameObject.layer == 0)
        {
            print(collision.gameObject.name);
            SpawnHitEffect();
            _rb2d.Sleep();
            _rb2d.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            _rb2d.isKinematic = true;
            activated = false;
            canPulled = true;
            CancelIncokeReapterForMarkers();
        }
        

        if(collision.gameObject.layer == 10)
        {
            if(collision.gameObject.GetComponent<Breakable>() != null)
            collision.gameObject.GetComponent<Breakable>().Break();
        }

        else
        {
            _rb2d.gravityScale = 2;
        }
    }

    public void SpawnHitEffect()
    {
        GameObject hitEffectClone = Instantiate(hitEffect, transform.position, Quaternion.identity);
        hitEffectClone.SetActive(true);
        hitEffectClone.GetComponent<ParticleSystem>().Play(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //break-layer(10)
        if(collision.gameObject.layer == 10)
        {
            if (collision.gameObject.GetComponent<Breakable>() != null)
                collision.gameObject.GetComponent<Breakable>().Break();
            SpawnHitEffect();
        }
    }

 /*   private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Breakable"))
        {
            //Add breakable  objects if needed
            if (other.GetComponent<BreakBoxScript>() != null)
            {
                other.GetComponent<BreakBoxScript>().Break();
            }
        }
    }*/

    public void ResetRangeTimer()
    {
        rangeTime = orginalRangeTime;
    }

    public void ToggleRopeOff(bool b)
    {
        rope.Clear();
        //rope.gameObject.SetActive(b);
        
    }

    public void SetRopeLifeTime(float f)
    {
        rope.time = f;
    }

   public void ToggleColliderTrigger(bool newBool)
    {
        polygonCollider.isTrigger = newBool;
    }

    public void SetCameraToFollowWeapon()
    {
        var vcam = GameObject.FindObjectOfType<CinemachineVirtualCamera>();
        vcam.Follow = this.gameObject.transform;
    }


    //Trajectery stuff

    public void StartInvokeReapeaterForMarkers(float t)
    { 
        //0.05f
        InvokeRepeating("CreateNewMark", 0, t);
    }

    public void CancelIncokeReapterForMarkers()
    {
        CancelInvoke();
    }

    public void CreateNewMark()
    {
        float rot_z = Mathf.Atan2(throwDirection.y, throwDirection.x) * Mathf.Rad2Deg;
        GameObject newMarker = Instantiate(marker, transform.position ,Quaternion.Euler(0f,0f,rot_z));
        newMarker.transform.SetParent(null);
        newMarker.transform.localScale =new Vector3(0.8f, 0.2f, 1f);
        markers.Add(newMarker);
    }

    public void DestroyMarkers()
    {
        foreach (GameObject mark in markers)
        {
            Destroy(mark);
        }

        markers.Clear();
    }
}
