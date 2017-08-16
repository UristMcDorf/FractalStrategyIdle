using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MOCInRadius : MapObjectComponent
{
    [SerializeField]
    int radiusPerLevel = 1;

    protected Dictionary<MapObjectInstance, List<MapObjectInstance>> affected = new Dictionary<MapObjectInstance, List<MapObjectInstance>>();

    protected void OnEnable()
    {
        EventManager.Instance.AddListener<MapObjectCreatedEvent>(OnMapObjectCreated);
        EventManager.Instance.AddListener<MapObjectDestroyedEvent>(OnMapObjectDestroyed);
    }

    protected void OnDisable()
    {
        if (EventManager.Instance != null)
        {
            EventManager.Instance.RemoveListener<MapObjectCreatedEvent>(OnMapObjectCreated);
            EventManager.Instance.RemoveListener<MapObjectDestroyedEvent>(OnMapObjectDestroyed);
        }
    }

    protected void OnMapObjectCreated(MapObjectCreatedEvent eventInst)
    {
        if (eventInst.instance.Parent.gameObject == this.gameObject)
        {
            AddAndRecalculateForInstance(eventInst.instance);
            return;
        }

        List<MapObjectInstance> toRecalc = new List<MapObjectInstance>();

        foreach (MapObjectInstance affector in affected.Keys)
        {
            foreach (MapObjectInstance moInstance in affected[affector])
            {
                if (moInstance == eventInst.instance)
                {
                    toRecalc.Add(affector);
                }

                ImprovementInstance iInstance = eventInst.instance as ImprovementInstance;
                TileInstance tInstance = moInstance as TileInstance;

                if (iInstance != null && tInstance != null && iInstance.ParentTile == tInstance)
                    toRecalc.Add(affector);
            }
        }

        foreach (MapObjectInstance affector in toRecalc)
            AddAndRecalculateForInstance(affector);
    }

    protected void OnMapObjectDestroyed(MapObjectDestroyedEvent eventInst)
    {
        affected.Remove(eventInst.instance);
    }

    public override void UpdateOnInstance(MapObjectInstance instance)
    {
        if (!affected.ContainsKey(instance))
            AddAndRecalculateForInstance(instance);
    }

    protected void AddAndRecalculateForInstance(MapObjectInstance instance)
    {
        affected.Remove(instance);

        affected.Add(instance, Map.Instance.GetAllInRange<MapObjectInstance>(instance, CalculateRadius(instance)));
        foreach (MapObjectInstance moInstance in affected[instance])
        {
            moInstance.AddAffectedBy(instance);
        }
    }

    protected int CalculateRadius(MapObjectInstance instance)
    {
        return radiusPerLevel * instance.Level;
    }
}
