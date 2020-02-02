using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Summon : MonoBehaviour
{
    public Transform summonPoint;
    public GameObject SummonPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            InstantiateSummon();
        }
    }

    public void InstantiateSummon()
    {
        Instantiate(SummonPrefab, summonPoint.position, Quaternion.identity);
        Player.Instance.Summoning();
    }
}
