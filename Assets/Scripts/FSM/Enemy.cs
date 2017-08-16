using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    private FSMSystem fsm; 

	// Use this for initialization
	void Start () {
        InitFsm();
    }
	void InitFsm()
    {
        fsm = new FSMSystem();
        PatrolState patrolState = new PatrolState(fsm);
        patrolState.ADDTransition(Transition.SeePlayer,StateID.Chase);
        ChaseState chaseState = new ChaseState(fsm);
        chaseState.ADDTransition(Transition.LostPlayer, StateID.PatroState);
        fsm.AddState(patrolState);
        fsm.AddState(chaseState);
    }
	// Update is called once per frame
	void Update () {
        fsm.Update(this.gameObject);
	}
}
