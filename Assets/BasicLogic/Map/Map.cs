using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Map : FSIEntity
{
    protected Dictionary<Coordinates, TileInstance> tiles = new Dictionary<Coordinates, TileInstance>();

    static Map _instance = null;
    static public Map Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<Map>();
            }       

            return _instance;
        }
    }

    virtual public int GetDistanceBetween(MapObjectInstance a, MapObjectInstance b)
    {
        return 1;
    }

    virtual public int GetDistanceToNearest(MapObjectInstance distanceFrom, List<string> distanceToSysNames)
    {
        return 1;
    }

    protected virtual List<MapObjectInstance> GetAllInstancesInRange(MapObjectInstance a, int range)
    {
        return new List<MapObjectInstance>();
    }

    public List<T> GetAllInRange<T>(MapObjectInstance a, int range) where T:MapObjectInstance
    {
        List<T> toReturn = new List<T>();

        T toAdd = null;

        foreach (MapObjectInstance instance in GetAllInstancesInRange(a, range))
        {
            toAdd = instance as T;

            if (toAdd != null)
                toReturn.Add(toAdd);
        }

        return toReturn;
    }

    public List<MapObjectInstance> FilterByRange(MapObjectInstance a, List<MapObjectInstance> toFilter, int range)
    {
        List<MapObjectInstance> toReturn = new List<MapObjectInstance>();

        foreach (MapObjectInstance instance in toFilter)
        {
            if (GetDistanceBetween(a, instance) <= range)
                toReturn.Add(instance);
        }

        return toReturn;
    }

    virtual public void GenerateMap()
    {
    }

    virtual public TileInstance ReplaceTile(TileInstance sourceTile, TileClass newTile)
    {
        return null;
    }

    protected struct Coordinates
    {
        public int x;
        public int y;

        public Coordinates(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public bool Equals(Coordinates compare)
        {
            return (this.x == compare.x) && (this.y == compare.y);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            Coordinates objAsCoords = (Coordinates)obj;
            return this.Equals(objAsCoords);
        }

        public override int GetHashCode()
        {
            int calc = x + y;
            return calc.GetHashCode();
        }

        public static bool operator ==(Coordinates c1, Coordinates c2)
        {
            return c1.Equals(c2);
        }

        public static bool operator !=(Coordinates c1, Coordinates c2)
        {
            return !c1.Equals(c2);
        }
    }
}
