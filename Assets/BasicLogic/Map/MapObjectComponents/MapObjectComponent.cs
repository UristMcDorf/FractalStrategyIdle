using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapObjectComponent : MonoBehaviour
{
    [SerializeField]
    protected bool _hidden = false;
    public bool Hidden
    {
        get
        {
            return _hidden;
        }
    }

    public virtual void UpdateOnInstance(MapObjectInstance instance)
    {
        
    }

    public virtual double GetModifier(MapObjectInstance instance)
    {
        return 1;
    }

    public virtual bool IsApplicableForComponent(MapObjectComponent component)
    {
        return true;
    }

    public virtual string GetDescription(MapObjectInstance instance)
    {
        return "";
    }
}
