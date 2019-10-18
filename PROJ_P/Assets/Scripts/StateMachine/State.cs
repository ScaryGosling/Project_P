//Author: Paschalis Tolios

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State : ScriptableObject
{
    public virtual void EnterState() { }
    public virtual void ExitState() { }
    public virtual void ToDo() { }
    public virtual void InitializeState(StateMachine owner) { }
    public virtual void TakeDamage(float damage) { }

}
