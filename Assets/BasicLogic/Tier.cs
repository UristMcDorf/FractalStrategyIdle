using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tier : FSIEntity
{
    public static string tierBundlePath = "TierBundles";
    public static string defaultTier = "T0";

    static protected Tier _instance;
    static public Tier Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<Tier>();
                if (_instance == null)
                    _instance = new Tier(defaultTier);
            }
            return _instance;
        }
        private set
        {
            Instance = value;
        }
    }

    [SerializeField]
    Map map;

    Dictionary<string, ClassInfo> all = null;

    Tier() : this(defaultTier)
    {
    }

    Tier(string tier)
    {
        //sysname = tier; //TODO: add Tier constructor based on tier type
    }

    protected override void Start()
    {
        base.Start();
        Setup();
    }

    protected override void Update()
    {
        
    }

    void Setup()
    {
        all = new Dictionary<string, ClassInfo>();

        SetupResources();
        SetupTiles();
        SetupImprovements();
        SetupMap();
    }

    void SetupTiles()
    {
        List<TileClass> tileClassPrefabs = new List<TileClass>(Resources.LoadAll<TileClass>(tierBundlePath + '/' + SysName + "/Objects/Tiles"));

        foreach (TileClass tile in tileClassPrefabs)
        {
            TileClass instance = Instantiate<TileClass>(tile);

            instance.transform.SetParent(this.transform);
            all.Add(instance.SysName, new ClassInfo(instance));
        }
    }

    void SetupMap()
    {
        if (map == null)
        {
            map = Resources.Load<Map>(tierBundlePath + '/' + SysName + "/Objects/Map");
        }

        map = Instantiate<Map>(map);

        map.transform.SetParent(UIManager.Instance.MapView.transform, false);
        map.GenerateMap();
    }

    void SetupResources()
    { 
        List<Resource> resourcePrefabs = new List<Resource>(Resources.LoadAll<Resource>(tierBundlePath + '/' + SysName + "/Objects/Resources"));

        foreach (Resource resource in resourcePrefabs)
        {
            Resource instance = Instantiate<Resource>(resource);

            instance.transform.SetParent(this.transform);
            all.Add(instance.SysName, new ClassInfo(instance));
        }
    }

    void SetupImprovements()
    {
        List<ImprovementClass> improvementClassPrefabs = new List<ImprovementClass>(Resources.LoadAll<ImprovementClass>(tierBundlePath + '/' + SysName + "/Objects/Improvements"));

        foreach (ImprovementClass improvement in improvementClassPrefabs)
        {
            ImprovementClass instance = Instantiate<ImprovementClass>(improvement);

            instance.transform.SetParent(this.transform);
            all.Add(instance.SysName, new ClassInfo(instance));
        }
    }

    public T GetSingularBySysName<T>(string sysName) where T:FSIEntity
    {
        List<string> toGet = new List<string>();
        toGet.Add(sysName);

        return GetBySysNames<T>(toGet)[0];
    }

    public List<T> GetBySysNames<T>(List<string> sysNames) where T:FSIEntity
    {
        List<ClassInfo> data = GetData(sysNames);

        List<T> culledEntities = new List<T>();

        foreach (ClassInfo datum in data)
        {
            if (datum.Entity.GetType() == typeof(T))
                culledEntities.Add((T)datum.Entity);
        }

        return culledEntities;
    }

    public string GetAsString(List<string> sysNames)
    {
        if (sysNames.Count == 0)
            return "All";

        List<ClassInfo> data = GetData(sysNames);

        List<string> toReturn = new List<string>();

        foreach (ClassInfo datum in data)
        {
            toReturn.Add(datum.Entity.InGameName);
        }

        return string.Join(", ", toReturn.ToArray());
    }

    public string GetAsString(string sysName)
    {
        List<string> a = new List<string>();
        a.Add(sysName);

        return GetAsString(a);
    }

    public int GetTotalCountOf(List<string> sysNames, int requiredLevel = 0)
    {
        List<ClassInfo> data = GetData(sysNames);
            
        int count = 0;

        foreach (ClassInfo datum in data)
            foreach (MapObjectInstance instance in datum.Instances)
                if (instance.Level >= requiredLevel)
                    count++;

        return count;
    }

    public int GetCountInRange(MapObjectInstance a, List<string> sysNames, int range)
    {
        List<ClassInfo> data = GetData(sysNames);
        List<MapObjectInstance> toCount = new List<MapObjectInstance>();

        foreach (ClassInfo datum in data)
            toCount.AddRange(Map.Instance.FilterByRange(a, datum.Instances, range));

        return toCount.Count;
    }

    List<ClassInfo> GetData(List<string> sysNames)
    {
        List<ClassInfo> data = new List<ClassInfo>();

        if (sysNames.Count == 0)
            return data;
        else if (sysNames[0] == "All")
        {
            foreach (KeyValuePair<string, ClassInfo> pair in all)
            {
                data.Add(pair.Value);
            }
        }
        else
        {
            ClassInfo current = null;

            foreach (string sysName in sysNames)
            {
                if (all.TryGetValue(sysName, out current))
                    data.Add(current);
            }
        }

        return data;
    }

    //For holding resources and tile/instance classes
    class ClassInfo
    {
        public FSIEntity Entity
        {
            get;
            private set;
        }

        public System.Type Type
        {
            get;
            private set;
        }

        //not used for everything, just some
        public List<MapObjectInstance> Instances
        {
            get;
            private set;
        }

        public ClassInfo(FSIEntity entity)
        {
            Entity = entity;
            Type = entity.GetType();
            Instances = new List<MapObjectInstance>();

            EventManager.Instance.AddListener<MapObjectCreatedEvent>(OnMapObjectCreated);
            EventManager.Instance.AddListener<MapObjectDestroyedEvent>(OnMapObjectDestroyed);
        }

        void OnMapObjectCreated(MapObjectCreatedEvent eventInst)
        {
            if (eventInst.instance.Parent == Entity)
                Instances.Add(eventInst.instance);
        }

        void OnMapObjectDestroyed(MapObjectDestroyedEvent eventInst)
        {
            if (eventInst.instance.Parent == Entity)
                Instances.Remove(eventInst.instance);
        }
    }
}
