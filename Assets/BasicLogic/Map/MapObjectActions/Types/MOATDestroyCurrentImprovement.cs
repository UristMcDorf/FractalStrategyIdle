using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MOATDestroyCurrentImprovement : ActionType
{
    //Requires MapObjectInstance
    public override void Perform(ILeveled instance)
    {
        if (instance.GetType() == typeof(ImprovementInstance))
        {
            ImprovementInstance improvement = (ImprovementInstance)instance;
            EventManager.Instance.QueueEvent(new NewSelectedInstanceEvent(improvement.ParentTile));
            improvement.DestroyThis();
        }
        else
        {
            Debug.LogError("You're trying to directly destroy a tile or something else! This is not supported.");
        }

        base.Perform(instance);            
    }

    public override string ToString()
    {
        return string.Format("Destroy the selected improvement.");
    }
}
