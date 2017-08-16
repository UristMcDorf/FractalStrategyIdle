using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ATResearch : ActionType
{
    public override void Perform(ILeveled instance)
    {
        Research research = (Research)instance;

        research.Complete();

        base.Perform(instance);
    }

    public override string ToString()
    {
        return string.Format("Research this.");
    }
}
