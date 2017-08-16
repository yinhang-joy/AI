using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum StateID
{
    NullStateID=0,
    PatroState=1,
    Chase
}

public enum Transition
{
    NullTRansition=0,
    SeePlayer,
    LostPlayer
}
public abstract class FSMState {

    protected StateID stateID;

    public StateID ID { get { return stateID; } }
    protected Dictionary<Transition, StateID> map = new Dictionary<Transition, StateID>();

    protected FSMSystem fsm;

    public FSMState(FSMSystem fsm)
    {
        this.fsm = fsm;
    }

    public void ADDTransition(Transition transition, StateID stateid)
    {
        if (transition==Transition.NullTRansition||stateid==StateID.NullStateID)
        {
            return;
        }
        if (map.ContainsKey(transition))
        {
            return;
        }
        map.Add(transition,stateid);
    }
    public void DeleteTransition(Transition transition)
    {
        if (transition==Transition.NullTRansition)
        {
            return;
        }
        if (map.ContainsKey(transition))
        {
            map.Remove(transition);
        }
    }
    public StateID GetOutputState(Transition transition)
    {
        if (map.ContainsKey(transition))
        {
            return map[transition];
        }
        else
        {
            return StateID.NullStateID;
        }
    }
    public virtual void DoBeforeEntering()
    {

    }
    public virtual void DoAfterLeaving() { }
    public abstract void Act(GameObject npc);//状态要执行的方法
    public abstract void Reason(GameObject npc);//判断转换条件
}
