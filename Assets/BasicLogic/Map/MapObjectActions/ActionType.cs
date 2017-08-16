using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Action))]
public class ActionType : MonoBehaviour
{
    public virtual void Perform(ILeveled instance)
    {
        EventManager.Instance.QueueEvent(new ActionPerformedEvent(this.GetComponent<Action>()));
    }
}
