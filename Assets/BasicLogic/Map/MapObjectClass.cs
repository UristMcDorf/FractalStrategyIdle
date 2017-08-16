using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapObjectClass : FSIEntity
{
    protected List<MapObjectComponent> components = null;

    [SerializeField]
    protected Sprite _mapSprite;
    public Sprite MapSprite
    {
        get
        {
            return _mapSprite;
        }
    }

    protected List<Action> _actions;
    public List<Action> Actions
    {
        get
        {
            return _actions;
        }
    }

    protected override void Start()
    {
        base.Start();

        components = new List<MapObjectComponent>(GetComponents<MapObjectComponent>());
        _actions = new List<Action>(GetComponentsInChildren<Action>());
    }

    public MapObjectInstance MakeInstance()
    {
        MapObjectInstance instance = new GameObject().AddComponent<MapObjectInstance>();

        return instance;
    }

    protected void SetupInstance(MapObjectInstance instance)
    {
        instance.SetParent(this);
        instance.SetSprite(MapSprite);
        instance.name = this.name;
    }

    public void AskForUpdateByChild(MapObjectInstance instance)
    {
        foreach (MapObjectComponent component in components)
        {
            component.UpdateOnInstance(instance);
        }
    }
}
