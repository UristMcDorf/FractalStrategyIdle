using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridMap : Map
{
    [SerializeField]
    protected int x;
    [SerializeField]
    protected int y;
    [SerializeField]
    TextAsset mapData = null;

    public override int GetDistanceBetween(MapObjectInstance a, MapObjectInstance b)
    {
        Coordinates coordsA = GetCoordinatesOf(a);
        Coordinates coordsB = GetCoordinatesOf(b);

        return Mathf.Abs(coordsA.x - coordsB.x) + Mathf.Abs(coordsA.y - coordsB.y);
    }

    public override int GetDistanceToNearest(MapObjectInstance distanceFrom, List<string> distanceToSysNames)
    {
        int currentRadius = 0;
        int maxRadius = Mathf.Max(x - 1, y - 1);
        Coordinates coords = GetCoordinatesOf(distanceFrom);
        List<MapObjectInstance> currentList = null;

        while (true)
        {
            currentRadius++;
            if (currentRadius > maxRadius)
                return -1;

            currentList = GetAllInRangeFromCoordinates(coords, currentRadius, true);

            foreach (MapObjectInstance instance in currentList)
            {
                if (distanceToSysNames.Contains(instance.Parent.SysName))
                    return currentRadius;
            }
        }
    }

    public override void GenerateMap()
    {
        //Rect rect = this.GetComponent<RectTransform>().rect;
        //this.GetComponent<GridLayoutGroup>().cellSize.Set(rect.width / x, rect.height / y);

        /*
        TileClass basicTile = Tier.Instance.GetTileClassBySysname("Plain");
        TileInstance newTile;

        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                newTile = MakeTile(basicTile, new Coordinates(i, j));
            }
        }

        Tier.Instance.GetImprovementClassBySysname("Hub").BuildAt(FindTile(new Coordinates(7, 7)));
        */

        MakeMapFromArray(MakeMapArrayFromString(mapData.text));
    }

    public override TileInstance ReplaceTile(TileInstance sourceTile, TileClass newTile)
    {
        if (tiles.ContainsValue(sourceTile))
        {
            Coordinates coords = GetCoordinatesOf(sourceTile);

            int index = sourceTile.transform.GetSiblingIndex();
            ImprovementInstance improvement = sourceTile.Improvement;

            Destroy(sourceTile.gameObject);
            tiles.Remove(coords);

            TileInstance instance = MakeTile(newTile, coords);
            instance.transform.SetSiblingIndex(index);
            instance.Improvement = improvement;

            return instance;
        }
        else
        {
            Debug.LogError("error means that some tiles exist outside of map - very bad");

            return null;
        }
    }

    TileInstance MakeTile(TileClass tileClass, Coordinates coords)
    {
        TileInstance newTile = tileClass.MakeInstance();

        newTile.gameObject.AddComponent<LayoutElement>();
        newTile.transform.SetParent(this.transform, false);

        tiles.Add(coords, newTile);

        return newTile;
    }

    public TileInstance FindTile(int x, int y)
    {
        return FindTile(new Coordinates(x, y));
    }

    TileInstance FindTile(Coordinates coords)
    {
        TileInstance tile;

        if (tiles.TryGetValue(coords, out tile))
            return tile;

        return null;
    }

    Coordinates GetCoordinatesOf(MapObjectInstance instance)
    {
        TileInstance tile = null;

        if (instance.GetType() == typeof(TileInstance))
            tile = (TileInstance)instance;
        else if (instance.GetType() == typeof(ImprovementInstance))
            tile = ((ImprovementInstance)instance).ParentTile;

        if (tiles.ContainsValue(tile))
        {
            foreach (Coordinates coords in tiles.Keys)
            {
                if (tiles[coords] == tile)
                    return coords;
            }
        }

        Debug.LogError("error means that some tiles exist outside of map - very bad");
        return new Coordinates(0, 0);
    }

    List<MapObjectInstance> GetAllInRangeFromCoordinates(Coordinates coords, int range, bool exclusive = false)
    {
        List<MapObjectInstance> returnList = new List<MapObjectInstance>();
        TileInstance currentTile;

        for (int i = range * -1; i <= range; i++)
        {
            for (int j = range * -1; j <= range; j++)
            {
                if (coords.x + i < 0 || coords.x + i >= x || coords.y + j < 0 || coords.y + j >= y)
                    continue;
                if ((Mathf.Abs(i) + Mathf.Abs(j)) > range)
                    continue;
                if (exclusive && (Mathf.Abs(i) + Mathf.Abs(j)) != range)
                    continue;

                currentTile = FindTile(new Coordinates(coords.x + i, coords.y + j));

                returnList.Add(currentTile);

                if (currentTile.Improvement != null)
                    returnList.Add(currentTile.Improvement);
            }
        }

        return returnList;
    }

    protected override List<MapObjectInstance> GetAllInstancesInRange(MapObjectInstance a, int range)
    {
        return GetAllInRangeFromCoordinates(GetCoordinatesOf(a), range);
    }

    void MakeMapFromArray(string [,] mapData)
    {
        Dictionary<string, MapObjectClass> inUse = new Dictionary<string, MapObjectClass>();

        string[] inThisCell = null;
        MapObjectClass current = null;

        for (int i = 0; i < y; i++)
        {
            for (int j = 0; j < x; j++)
            {
                inThisCell = mapData[j, i].Split(':');

                if (!inUse.TryGetValue(inThisCell[0], out current))
                {
                    current = Tier.Instance.GetSingularBySysName<TileClass>(inThisCell[0]);
                    inUse.Add(inThisCell[0], current);
                }

                MakeTile((TileClass)current, new Coordinates(j, i));

                //TODO: less magic numbers?
                if (inThisCell.Length == 2)
                {
                    if (!inUse.TryGetValue(inThisCell[1], out current))
                    {
                        current = Tier.Instance.GetSingularBySysName<ImprovementClass>(inThisCell[1]);
                        inUse.Add(inThisCell[1], current);
                    }

                    ((ImprovementClass)current).BuildAt(FindTile(new Coordinates(j, i)));
                }
            }
        }
    }

    //make sure the x/y match EXACTLY with the rows and columns
    string[,] MakeMapArrayFromString(string mapData)
    {
        string[,] returnArray = new string[x, y];
        string[] rows = mapData.Split('\n');
        string[] cells = null;

        for (int i = 0; i < y; i++)
        {
            cells = rows[i].TrimEnd('\r', '\n').Split(';');

            for (int j = 0; j < x; j++)
            {
                returnArray[j, i] = cells[j];
            }
        }

        return returnArray;
    }
}
