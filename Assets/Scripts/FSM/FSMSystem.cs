using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMSystem  {
    private Dictionary<StateID, FSMState> states;
    private StateID currentStateID;
    private FSMState currentState;

    public StateID CurrentStateID
    {
        get
        {
            return currentStateID;
        }

        set
        {
            currentStateID = value;
        }
    }

    public FSMState CurrentState
    {
        get
        {
            return currentState;
        }

        set
        {
            currentState = value;
        }
    }
    public FSMSystem()
    {
        states = new Dictionary<StateID, FSMState>();
    }
    public void Update(GameObject npc)
    {
        currentState.Act(npc);
        currentState.Reason(npc);
    }
    public void AddState(FSMState s)
    {
        if (s==null)
        {
            return;
        }
        if (currentState == null)
        {
            currentState = s;
            currentStateID = s.ID;
        }
        if (states.ContainsKey(s.ID))
        {
            Debug.LogError("状态"+s.ID+"状态已经存在，无法重复添加");
        }
        states.Add(s.ID, s);
    }
    public void DeleteState(StateID id)
    {
        if (id==StateID.NullStateID)
        {
            return;
        }
        if (states.ContainsKey(id)==false)
        {
            Debug.LogError("无法输出不存在的状态");
        }
        states.Remove(id);
    }
    /// <summary>
    /// 执行转换条件
    /// </summary>
    /// <param name="trans"></param>
    public void PerformTransition(Transition trans)
    {
        if (trans==Transition.NullTRansition)
        {
            Debug.LogError("无法执行空的转换条件");
            return;
        }
        StateID id = currentState.GetOutputState(trans);
        Debug.Log(id);
        if (id==StateID.NullStateID)
        {
            Debug.LogWarning("当前状态"+currentState+"无法发生条件转换"+trans);
            return;
        }
        if (states.ContainsKey(id) == false)
        {
            Debug.LogError("在状态机里不存在状态"+id);
        }
        FSMState state = states[id];
        currentState.DoAfterLeaving();
        currentState = state;
        currentStateID = state.ID;
        currentState.DoBeforeEntering();
    }
}
