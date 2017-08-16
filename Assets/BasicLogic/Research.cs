using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Research : FSIEntity, ILeveled
{
    [SerializeField]
    Action _researchAction = null;
    public Action ResearchAction
    {
        get
        {
            return _researchAction;
        }
    }

    ResearchActionImageLink _completeActionImageLink = null;
    public ResearchActionImageLink CompleteActionImageLink
    {
        get
        {
            return _completeActionImageLink;
        }
    }

    [SerializeField]
    Action _upgradeAction = null;
    public Action UpgradeAction
    {
        get
        {
            return _upgradeAction;
        }
    }

    ResearchActionImageLink _upgradeActionImageLink = null;
    public ResearchActionImageLink UpgradeActionImageLink
    {
        get
        {
            return _upgradeActionImageLink;
        }
    }

    bool _researched = false;
    public bool Researched
    {
        get
        {
            return _researched;
        }
    }

    [SerializeField]
    int _level = 1;
    public int Level
    {
        get
        {
            return _level;
        }
    }

    [SerializeField]
    int _maxLevel = 0;
    public int MaxLevel
    {
        get
        {
            return _maxLevel;
        }
    }

    public bool SetLevel(int level, bool clamp = true)
    {
        int oldLevel = Level;
        bool returnValue = false;

        if (level > MaxLevel)
        {
            if (clamp)
                _level = MaxLevel;
            returnValue = false;
        }
        else if (level < 1)
        {
            if (clamp)
                _level = 1;
            returnValue = false;
        }
        else
        {
            _level = level;
            returnValue = true;
        }

        if (Level != oldLevel)
            EventManager.Instance.QueueEvent(new ResearchUpgradedEvent(this, Level));

        if (Level >= MaxLevel)
        {
            UpgradeActionImageLink.gameObject.SetActive(false);
        }

        return returnValue;
    }

    public void IncrementLevel()
    {
        int oldLevel = Level;

        if (_maxLevel > 0)
            _level = System.Math.Min(MaxLevel, Level + 1);
        else
            _level++;

        if (Level != oldLevel)
            EventManager.Instance.QueueEvent(new ResearchUpgradedEvent(this, Level));

        if (Level >= MaxLevel)
        {
            UpgradeActionImageLink.gameObject.SetActive(false);
        }
    }

    public void DecrementLevel()
    {
        int oldLevel = Level;

        _level = System.Math.Max(1, Level - 1);

        if (Level != oldLevel)
            EventManager.Instance.QueueEvent(new ResearchUpgradedEvent(this, Level));
    }

    public void Complete()
    {
        _researched = true;

        EventManager.Instance.QueueEvent(new ResearchCompletedEvent(this));

        CompleteActionImageLink.gameObject.SetActive(false);

        if (UpgradeAction != null)
            UpgradeActionImageLink.gameObject.SetActive(true);
    }

    public void Setup()
    {
        _completeActionImageLink = ResearchActionImageLink.MakeResearchActionImageLink(ResearchAction);
        CompleteActionImageLink.SetParentResearch(this);
        CompleteActionImageLink.gameObject.SetActive(true);

        if (_upgradeAction != null)
        {
            _upgradeActionImageLink = ResearchActionImageLink.MakeResearchActionImageLink(UpgradeAction);
            UpgradeActionImageLink.SetParentResearch(this);
            UpgradeActionImageLink.gameObject.SetActive(false);
        }
    }

    public bool CanBeShown()
    {
        if (!Researched)
        {
            return ResearchAction.CanBeShown(this);
        }   
        else
        {
            if (UpgradeAction == null)
                return true;
            else
                return UpgradeAction.CanBeShown(this);
        }
    }

    public override string ToString()
    {
        return InGameName;
    }
}
