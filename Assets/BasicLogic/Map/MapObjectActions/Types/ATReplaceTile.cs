using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ATReplaceTile : ActionType
{
    [SerializeField]
    string tileSysName = null;

    public override void Perform(ILeveled instance)
    {
        TileInstance newInstance = null;

        if (instance.GetType() == typeof(TileInstance))
        {
            newInstance = Map.Instance.ReplaceTile((TileInstance)instance, Tier.Instance.GetSingularBySysName<TileClass>(tileSysName));
        }
        else if (instance.GetType() == typeof(ImprovementInstance))
        {
            newInstance = Map.Instance.ReplaceTile(((ImprovementInstance)instance).ParentTile, Tier.Instance.GetSingularBySysName<TileClass>(tileSysName));
        }

        EventManager.Instance.QueueEvent(new NewSelectedInstanceEvent(newInstance));

        base.Perform(instance);
    }

    public override string ToString()
    {
        return string.Format("Replace this space with a {0}.", Tier.Instance.GetAsString(tileSysName));
    }
}
