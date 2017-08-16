using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MOATBuildImprovement : ActionType
{
    [SerializeField]
    string improvementSysName = "default";

    //Requires MapObjectInstance
    public override void Perform(ILeveled instance)
    {
        MapObjectInstance newInstance = Tier.Instance.GetSingularBySysName<ImprovementClass>(improvementSysName).BuildAt((MapObjectInstance)instance);

        EventManager.Instance.QueueEvent(new NewSelectedInstanceEvent(newInstance));

        base.Perform(instance);
    }

    public override string ToString()
    {
        return string.Format("Build a {0} in this space.", Tier.Instance.GetAsString(improvementSysName));
    }
}
