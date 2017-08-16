using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ATProvideIncomeOnClick : ActionType
{
    [SerializeField]
    List<string> resourceSysNames = null;
    [SerializeField]
    int basicAmount = 1;
    [SerializeField]
    double levelModifier = 1;

    public override void Perform(ILeveled instance)
    {
        foreach (string resource in resourceSysNames)
            EventManager.Instance.QueueEvent(new ProduceImmediateIncomeEvent(resource, CalculateAmount(instance)));

        base.Perform(instance);
    }

    public override string ToString()
    {
        return string.Format("Gain {0} {1} per click.", CalculateAmount(UIManager.Instance.CurrentlySelected), Tier.Instance.GetAsString(resourceSysNames));
    }

    double CalculateAmount(ILeveled instance)
    {
        return basicAmount * System.Math.Pow(levelModifier, instance.Level);
    }
}
