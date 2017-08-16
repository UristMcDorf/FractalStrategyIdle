using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Action))]
public class ActionRequirement : MonoBehaviour
{
    public bool preventsShowing = false;
    public bool isShown = true;
    public int requirementIndex = 0;

    public virtual bool IsMet(ILeveled instance)
    {
        return true;
    }

    public virtual bool TryDo(ILeveled instance)
    {
        return IsMet(instance);
    }

    public override string ToString()
    {
        return "undefined requirement";
    }
}
