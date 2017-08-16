using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImprovementClass : MapObjectClass
{
	protected override void Start()
    {
        base.Start();
	}

    public new ImprovementInstance MakeInstance()
    {
        ImprovementInstance instance = new GameObject().AddComponent<ImprovementInstance>();
        instance.SetParent(this);
        instance.SetSprite(MapSprite);
        instance.name = this.name;

        return instance;
    }

    public MapObjectInstance BuildAt(MapObjectInstance instance)
    {
        ImprovementInstance newInstance = MakeInstance();
        TileInstance tile = null;

        if (instance.GetType() == typeof(TileInstance))
        {
            tile = (TileInstance)instance;

            if (tile.Improvement != null)
                tile.Improvement = null;
        }
        else if (instance.GetType() == typeof(ImprovementInstance))
        {
            tile = ((ImprovementInstance)instance).ParentTile;

            tile.Improvement = null;
        }

        tile.Improvement = newInstance;
        newInstance.SetParentTile(tile);

        return newInstance;
    }
}
