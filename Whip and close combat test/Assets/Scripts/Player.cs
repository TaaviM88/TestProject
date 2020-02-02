using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class Player : MonoBehaviour
{
    public static Player Instance;
    private enum AttackMode {SicleThrow,SummonSeel};
    private AttackMode attackMode = AttackMode.SicleThrow;
    bool throwing = true;
    bool summoning = false;
    bool summoned = false;
    Throw throwScript;
    Movement move;
    Summon summon;
    // Start is called before the first frame update
    void Start()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        throwScript = GetComponent<Throw>();
        move = GetComponent<Movement>();
        summon = GetComponent<Summon>();
        SetCameraToFollowPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        if(summoned)
        {
            return;
        }

        if(Input.GetKeyDown(KeyCode.M))
        {
            ToggleAttackMode();
        }

        if(!throwing && attackMode == AttackMode.SummonSeel)
        {
            throwScript.enabled = false;
        }
        else
        {
            if(!throwScript.enabled)
            {
                throwScript.enabled = true;
            }
        }

        if(!summoning && attackMode == AttackMode.SicleThrow)
        {
            summon.enabled = false;
        }
        else
        {
            summon.enabled = true;
        }

        Debug.Log(attackMode);
    }

    public void ToggleAttackMode()
    {
        attackMode = attackMode != AttackMode.SicleThrow ? AttackMode.SicleThrow : AttackMode.SummonSeel;

        if(attackMode == AttackMode.SicleThrow)
        {
            throwing = true;
            summoning = false;
        }
        else
        {
            throwing = false;
            summoning = true;
        }
    }

    public void SetThrowing (bool newBool)
    {
        throwing = newBool;
        
    }

    public void Summoning()
    {
        SetSummoning(true);
        move.enabled = false;
        throwScript.enabled = false;
        summon.enabled = false;
        summoned = true;
    }

    public void EndSummon()
    {
        SetSummoning(false);
        move.enabled = true;
        summon.enabled = true;
        summoned = false;
        SetCameraToFollowPlayer();
    }

    public void SetSummoning(bool newbool)
    {
        summoning = newbool; 
    }

    public bool GetIsThrowing()
    {
        return throwing;
    }

    public bool GetIsSummoning()
    {
        return summoning;
    }

    public void SetCameraToFollowPlayer()
    {
        var vcam = GameObject.FindObjectOfType<CinemachineVirtualCamera>();
        vcam.Follow = this.gameObject.transform;
    }
}
