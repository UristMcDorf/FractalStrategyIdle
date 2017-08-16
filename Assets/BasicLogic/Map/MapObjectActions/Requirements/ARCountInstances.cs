using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARCountInstances : ActionRequirement
{
    [SerializeField]
    List<string> sysNameOfRequired = null;
    [SerializeField]
    int amount = 2; //by default, this can be used to prevent destroying the last instance of an improvement

    [SerializeField]
    bool moreOrEqual = true;

    [SerializeField]
    bool useLevel = false;
    [SerializeField]
    double amountModifierPerLevel = 1;

    [SerializeField]
    bool useRequiredLevel = false;
    [SerializeField]
    double amountModiferPerRequiredLevel = 1;

    public override bool IsMet(ILeveled instance)
    {
        double required = amount;

        if (useLevel)
            required *= System.Math.Pow(amountModifierPerLevel, instance.Level);

        int count = Tier.Instance.GetTotalCountOf(sysNameOfRequired, (useRequiredLevel ? CalculateLevelCompare(instance) : 0));

        return !moreOrEqual ^ count >= required;
    }

    public override string ToString()
    {
        string levelString = useRequiredLevel ? string.Format("of level {0}+ ", CalculateLevelCompare(UIManager.Instance.CurrentlySelected)) : "";

        return string.Format("Requirement: At least {0} {1} {2}built total.", amount, Tier.Instance.GetAsString(sysNameOfRequired), levelString); //TODO: make a MapObjectClass.ToString()
    }

    protected int CalculateLevelCompare(ILeveled instance)
    {
        return (int)System.Math.Floor(System.Math.Pow(amountModiferPerRequiredLevel, instance.Level));
    }
}
