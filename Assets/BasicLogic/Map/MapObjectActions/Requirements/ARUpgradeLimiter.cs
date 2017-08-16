using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: less horseshit, move to CountInstances after all
public class ARUpgradeLimiter : ActionRequirement
{
    [SerializeField]
    List<string> sysNamesOfRequired = null;
    [SerializeField]
    int requiredPerLevelToUpgradeTo = 2;

    public override bool IsMet(ILeveled instance)
    {
        int count = Tier.Instance.GetTotalCountOf(sysNamesOfRequired, instance.Level + 1);

        return count < CalculateMaxInstances(instance);
    }

    public override string ToString()
    {
        int amount = CalculateMaxInstances(UIManager.Instance.CurrentlySelected);
        return string.Format("Requirement: Less than {0} {1} of level {2}+ built.", amount, Tier.Instance.GetAsString(sysNamesOfRequired), (UIManager.Instance.CurrentlySelected.Level + 1)); //TODO: make a MapObjectClass.ToString()
    }

    protected int CalculateMaxInstances(ILeveled instance)
    {
        int requiredPerInstance = (int)System.Math.Ceiling(System.Math.Pow(requiredPerLevelToUpgradeTo, instance.Level));

        return Tier.Instance.GetTotalCountOf(sysNamesOfRequired) / requiredPerInstance;
    }
}
