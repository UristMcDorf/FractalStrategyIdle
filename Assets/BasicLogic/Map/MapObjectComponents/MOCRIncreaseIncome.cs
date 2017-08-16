using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MOCRIncreaseIncome : MOCInRadius
{
    [SerializeField]
    List<string> resourcesAffectedSysNames = new List<string>();
    [SerializeField]
    List<string> mapObjectsAffectedSysNames = new List<string>();
    [SerializeField]
    double modifierPerLevel = 1;

    public override double GetModifier(MapObjectInstance instance)
    {
        return System.Math.Pow(modifierPerLevel, instance.Level);
    }

    public override bool IsApplicableForComponent(MapObjectComponent component)
    {
        MOCIncome incomeComponent = component as MOCIncome;

        if (incomeComponent == null)
            return false;

        return (mapObjectsAffectedSysNames.Count == 0 || mapObjectsAffectedSysNames.Contains(incomeComponent.GetComponent<MapObjectClass>().SysName)) && (resourcesAffectedSysNames.Count == 0 || resourcesAffectedSysNames.Contains(incomeComponent.ResourceSysName));
    }

    public override string GetDescription(MapObjectInstance instance)
    {
        return string.Format("Multiplies {0} income by {1} for {2} within {3} tiles.", Tier.Instance.GetAsString(resourcesAffectedSysNames), GetModifier(instance), Tier.Instance.GetAsString(mapObjectsAffectedSysNames), CalculateRadius(instance));
    }
}
