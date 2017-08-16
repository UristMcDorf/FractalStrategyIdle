using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ATIncreaseLevel : ActionType
{
    public override void Perform(ILeveled instance)
    {
        instance.IncrementLevel();

        base.Perform(instance);
    }

    public override string ToString()
    {
        return string.Format("Increase level by 1.");
    }
}
