using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileClass : MapObjectClass
{
    public new TileInstance MakeInstance()
    {
        TileInstance instance = new GameObject().AddComponent<TileInstance>();
        instance.SetParent(this);
        instance.SetSprite(MapSprite);
        instance.name = this.name;

        return instance;
    }
}
