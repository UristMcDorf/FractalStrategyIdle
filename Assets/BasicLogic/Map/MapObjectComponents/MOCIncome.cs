using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MOCIncome : MapObjectComponent
{
    [SerializeField]
    string _resourceSysName = "default";
    public string ResourceSysName
    {
        get
        {
            return _resourceSysName;
        }
    }

    [SerializeField]
    double amountPerSecond = 0;
    [SerializeField]
    double levelModifier = 2;

    public override void UpdateOnInstance(MapObjectInstance instance)
    {
        if (EventManager.Instance != null)
        {
            EventManager.Instance.QueueEvent(new ProduceIncomeEvent(ResourceSysName, CalculateIncome(instance, Time.deltaTime)));
        }
	}

    public double CalculateIncome(MapObjectInstance instance, double deltaTime)
    {
        double amount = amountPerSecond * System.Math.Pow(levelModifier, instance.Level) * deltaTime * GameManager.Instance.GameSpeed;

        foreach (MapObjectInstance affector in instance.Affecting)
            foreach (MOCRIncreaseIncome mocrIncome in affector.Parent.GetComponents<MOCRIncreaseIncome>())
                if (mocrIncome.IsApplicableForComponent(this))
                    amount *= mocrIncome.GetModifier(affector);

        return amount;
    }

    public override string GetDescription(MapObjectInstance instance)
    {
        return string.Format("Produces {0} {1} per second.", CalculateIncome(instance, 1), Tier.Instance.GetAsString(ResourceSysName));
    }
}
