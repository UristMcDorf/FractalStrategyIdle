using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ImprovementInstance : MapObjectInstance
{
    TileInstance _parentTile = null;
    public TileInstance ParentTile
    {
        get
        {
            return _parentTile;
        }
    }

    public ImprovementClass ParentAsImprovementClass
    {
        get
        {
            return (ImprovementClass)_parent;
        }
    }

    public void SetParentTile(TileInstance tile)
    {
        _parentTile = tile;
    }

    public void DestroyThis()
    {
        ParentTile.Improvement = null;
    }

    public override TileInstance GetTileInstance()
    {
        return ParentTile;
    }
}