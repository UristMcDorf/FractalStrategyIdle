using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvent
{

}


public class ProduceIncomeEvent : GameEvent
{
    public string resourceSysname;
    public double amount;

    public ProduceIncomeEvent(string resourceSysname, double amount)
    {
        this.resourceSysname = resourceSysname;
        this.amount = amount;
    }
}

public class ProduceImmediateIncomeEvent : GameEvent
{
    public string resourceSysname;
    public double amount;

    public ProduceImmediateIncomeEvent(string resourceSysname, double amount)
    {
        this.resourceSysname = resourceSysname;
        this.amount = amount;
    }
}

public class MapObjectInstanceClickedEvent : GameEvent
{
    public MapObjectInstance instance;

    public MapObjectInstanceClickedEvent(MapObjectInstance instance)
    {
        this.instance = instance;
    }
}

public class MapObjectCreatedEvent : GameEvent
{
    public MapObjectInstance instance;

    public MapObjectCreatedEvent(MapObjectInstance instance)
    {
        this.instance = instance;
    }
}

public class MapObjectDestroyedEvent : GameEvent
{
    public MapObjectInstance instance;

    public MapObjectDestroyedEvent(MapObjectInstance instance)
    {
        this.instance = instance;
    }
}

public class NewSelectedInstanceEvent : GameEvent
{
    public MapObjectInstance instance;

    public NewSelectedInstanceEvent(MapObjectInstance instance)
    {
        this.instance = instance;
    }
}

public class ActionPerformedEvent : GameEvent
{
    public Action action;

    public ActionPerformedEvent(Action action)
    {
        this.action = action;
    }
}

public class ObjectWithTooltipMousedOverEvent : GameEvent
{
    public GameObject gameObject;
    public string text;
    public Vector2 position;

    public ObjectWithTooltipMousedOverEvent(GameObject gameObject, string text, Vector2 position)
    {
        this.gameObject = gameObject;
        this.text = text;
        this.position = position;
    }
}

public class ObjectWithTooltipMousedAwayEvent : GameEvent
{
    public GameObject gameObject;

    public ObjectWithTooltipMousedAwayEvent(GameObject gameObject)
    {
        this.gameObject = gameObject;
    }
}

public class ResearchCompletedEvent : GameEvent
{
    public Research research;

    public ResearchCompletedEvent(Research research)
    {
        this.research = research;
    }
}

public class ResearchUpgradedEvent : GameEvent
{
    public Research research;
    public int newLevel;

    public ResearchUpgradedEvent(Research research, int newLevel)
    {
        this.research = research;
        this.newLevel = newLevel;
    }
}

public class DebugFunctionUsedEvent : GameEvent
{
    public string text;

    public DebugFunctionUsedEvent(string text)
    {
        this.text = text;
    }
}