using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MapObjectInstance : MonoBehaviour, IPointerDownHandler, ILeveled
{
    protected int _level = 1;
    public int Level
    {
        get
        {
            return _level;
        }
    }

    int _maxLevel = 0;
    public int MaxLevel
    {
        get
        {
            return _maxLevel;
        }
    }

    protected MapObjectClass _parent;
    public MapObjectClass Parent
    {
        get
        {
            return _parent;
        }
    }

    protected List<MapObjectInstance> _affecting = new List<MapObjectInstance>();
    public List<MapObjectInstance> Affecting
    {
        get
        {
            return _affecting;
        }
    }

    public void Start()
    {
        EventManager.Instance.QueueEvent(new MapObjectCreatedEvent(this));
    }

    public void OnDestroy()
    {
        if (UIManager.Instance.CurrentlySelected == this)
            UIManager.Instance.SaveHighlighter(); //TODO: less hacky

        if (EventManager.Instance != null)
            EventManager.Instance.QueueEvent(new MapObjectDestroyedEvent(this));
    }

    public void OnEnable()
    {
        EventManager.Instance.AddListener<MapObjectDestroyedEvent>(OnMapObjectPossiblyAffectingDestroyed);
    }

    public void OnDisable()
    {
        if (EventManager.Instance != null)
            EventManager.Instance.RemoveListener<MapObjectDestroyedEvent>(OnMapObjectPossiblyAffectingDestroyed);
    }

    public void OnMapObjectPossiblyAffectingDestroyed(MapObjectDestroyedEvent eventInst)
    {
        _affecting.Remove(eventInst.instance);
    }

    public void Update()
    {
        Parent.AskForUpdateByChild(this);
    }

    public virtual void SetParent(MapObjectClass parent)
    {
        this._parent = parent;
    }

    public virtual void SetSprite(Sprite sprite)
    {
        this.gameObject.AddComponent<Image>().sprite = sprite;
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        EventManager.Instance.QueueEvent(new MapObjectInstanceClickedEvent(this));
    }

    public virtual TileInstance GetTileInstance()
    {
        return null;
    }

    public void AddAffectedBy(MapObjectInstance instance)
    {
        if (!Affecting.Contains(instance))
            _affecting.Add(instance);

        //Debug.Log(this + " is now affected by " + instance);
    }

    public bool SetLevel(int level, bool clamp = true)
    {
        if (level > MaxLevel)
        {
            if (clamp)
                _level = MaxLevel;
            return false;
        }
        else if (level < 1)
        {
            if (clamp)
                _level = 1;
            return false;
        }
        else
        {
            _level = level;
            return true;
        }
    }

    public void IncrementLevel()
    {
        if (_maxLevel > 0)
            _level = System.Math.Min(MaxLevel, Level + 1);
        else
            _level++;

        foreach (MOCInRadius component in Parent.GetComponents<MOCInRadius>())
        {
            component.UpdateOnInstance(this);
        }
    }

    public void DecrementLevel()
    {
        _level = System.Math.Max(0, Level - 1);

        foreach (MOCInRadius component in Parent.GetComponents<MOCInRadius>())
        {
            component.UpdateOnInstance(this);
        }
    }

    public string MakeDescription()
    {
        string description = Parent.Description;

        foreach (MapObjectComponent moComponent in Parent.GetComponents<MapObjectComponent>())
        {
            if (!moComponent.Hidden)
            {
                description += '\n' + moComponent.GetDescription(this);
            }
        }

        return description;
    }
}
