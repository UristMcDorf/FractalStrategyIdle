using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Requires MapObjectInstance
public class MOARWithinRadius : ActionRequirement
{
    [SerializeField]
    List<string> sysNamesOfRequired = null;
    [SerializeField]
    int amount = 0;
    [SerializeField]
    int radius = 0;

    public override bool IsMet(ILeveled instance)
    {
        return Tier.Instance.GetCountInRange((MapObjectInstance)instance, sysNamesOfRequired, radius) >= amount;
    }

    public override string ToString()
    {
        return string.Format("Requirement: at least {0} {1} within {2} tiles.", amount, Tier.Instance.GetAsString(sysNamesOfRequired), radius);
    }
}
