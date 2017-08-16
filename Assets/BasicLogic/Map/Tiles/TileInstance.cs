using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TileInstance : MapObjectInstance
{
    public TileClass ParentAsTileClass
    {
        get
        {
            return (TileClass)_parent;
        }
    }

    protected ImprovementInstance _improvement;
    public ImprovementInstance Improvement
    {
        get
        {
            return _improvement;
        }
        set
        {
            if (value == null)
            {
                if (_improvement != null)
                {
                    Destroy(_improvement.gameObject);
                }
            }
            else
            {
                value.transform.SetParent(this.transform, false);

                RectTransform rect = value.GetComponent<RectTransform>();
                rect.anchorMin = Vector2.zero;
                rect.anchorMax = Vector2.one;

                rect.sizeDelta = Vector2.zero;
            }

            _improvement = value;
        }
    }

    public override TileInstance GetTileInstance()
    {
        return this;
    }
}
