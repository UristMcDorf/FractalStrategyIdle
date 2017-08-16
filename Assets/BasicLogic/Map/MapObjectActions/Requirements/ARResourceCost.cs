using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARResourceCost : ActionRequirement
{
    [SerializeField]
    List<string> resourceSysNames = null;
    [SerializeField]
    List<double> basicCosts = null;

    Dictionary<Resource, double> bakedCosts = null;

    [SerializeField]
    bool useAmount = false;
    [SerializeField]
    List<string> sysNamesToCompare = null;
    [SerializeField]
    double costModifierPerInstance = 1;

    //Requires MapObjectInstance
    [SerializeField]
    bool useDistance = false;
    [SerializeField]
    List<string> sysNamesOfDistanceHubs = null;
    [SerializeField]
    double costModifierPerDistance = 1;

    [SerializeField]
    bool useLevel = false;
    [SerializeField]
    double costModifierPerLevel = 1;

    public void Start()
    {
        bakedCosts = new Dictionary<Resource, double>();
        List<Resource> resources = Tier.Instance.GetBySysNames<Resource>(resourceSysNames);

        for (int i = 0; i < resources.Count; i++)
        {
            bakedCosts.Add(resources[i], basicCosts[i]);
        }
    }

    public override bool IsMet(ILeveled instance)
    {
        foreach (Resource resource in bakedCosts.Keys)
        {
            if (resource.Amount < CalculateCost(instance, resource))
                return false;
        }

        return true;
    }

    public override bool TryDo(ILeveled instance)
    {
        foreach (Resource resource in bakedCosts.Keys)
        {
            if (!resource.TrySpend(CalculateCost(instance, resource)))
                return false;
        }

        return true;
    }

    public override string ToString()
    {
        List<string> resourceStrings = new List<string>();

        foreach (KeyValuePair<Resource, double> pair in bakedCosts)
        {
            resourceStrings.Add(string.Format("{0} {1}", FSIUtility.NumberToString(CalculateCost(UIManager.Instance.CurrentlySelected, pair.Key)), pair.Key.InGameName));
        }

        return string.Format("Cost: {0}.", string.Join(", ", resourceStrings.ToArray()));
    }

    double CalculateCost(ILeveled instance, Resource resource)
    {
        double cost = bakedCosts[resource];

        if (useAmount)
            cost *= System.Math.Pow(costModifierPerInstance, Tier.Instance.GetTotalCountOf(sysNamesToCompare));

        if (useDistance)
            cost *= System.Math.Pow(costModifierPerDistance, Map.Instance.GetDistanceToNearest((MapObjectInstance)instance, sysNamesOfDistanceHubs));

        if (useLevel)
            cost *= System.Math.Pow(costModifierPerLevel, instance.Level);

        return cost;
    }
}