using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : FSMState
{
    private Transform playerTransform;

    public ChaseState(FSMSystem fsm) : base(fsm)
    {
        this.stateID = StateID.Chase;
        playerTransform = GameObject.Find("Player").transform;
        
    }
    public override void Act(GameObject npc)
    {
        npc.transform.LookAt(playerTransform.position);
        npc.transform.Translate(Vector3.forward*Time.deltaTime);
    }

    public override void Reason(GameObject npc)
    {
        if (Vector3.Distance(npc.transform.position,playerTransform.transform.position)>6)
        {
            fsm.PerformTransition(Transition.LostPlayer);
        }
    }
}
