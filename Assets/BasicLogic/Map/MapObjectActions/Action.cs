using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Action : MonoBehaviour
{

    [SerializeField]
    protected Sprite _icon = null;
    public Sprite Icon
    {
        get
        {
            return _icon;
        }
    }

    protected List<ActionRequirement> requirements = null;
    protected List<ActionType> types = null;

    protected void Start()
    {
        if (requirements == null)
            requirements = new List<ActionRequirement>(GetComponents<ActionRequirement>());
        if (types == null)
            types = new List<ActionType>(GetComponents<ActionType>());
    }

    public bool CanBeShown(ILeveled instance)
    {
        foreach (ActionRequirement requirement in requirements)
        {
            if (requirement.preventsShowing && !requirement.IsMet(instance))
                return false;
        }

        return true;
    }

    public void ActionClicked(ILeveled instance)
    {
        if (requirements.Count > 0)
        {
            int current = 0;

            for (int i = 0; i < requirements.Count; i++)
            {
                if (current > requirements[i].requirementIndex)
                    continue;

                if (requirements[i].IsMet(instance))
                {
                    current++;
                    continue;
                }

                if (current < requirements[i].requirementIndex || i == requirements.Count - 1)
                {
                    return;
                }
            }
        }

        TryPerform(instance);
    }

    protected void TryPerform(ILeveled instance)
    {
        if (requirements.Count > 0)
        {
            int current = 0;

            for (int i = 0; i < requirements.Count; i++)
            {
                if (current > requirements[i].requirementIndex)
                    continue;

                if (requirements[i].TryDo(instance))
                {
                    current++;
                    continue;
                }

                if (current < requirements[i].requirementIndex || i == requirements.Count - 1)
                {
                    return;
                }
            }
        }

        foreach (ActionType type in types)
            type.Perform(instance);
    }

    public virtual string GetDescription()
    {
        string description = "";

        for (int i = 0; i < types.Count; i++)
        {
            description += types[i].ToString();
            if (i != types.Count - 1)
            {
                description += '\n';
            }
        }

        if (requirements.Count > 0)
            description += MakeRequirementsString();

        return description;
    }

    string MakeRequirementsString()
    {
        if (requirements.Count == 0)
        {
            return "";
        }

        List<string> toReturn = new List<string>();
        List<string> alternates = new List<string>();
        int current = 0;

        for (int i = 0; i < requirements.Count; i++)
        {
            if (requirements[i].isShown)
            {
                if (current == requirements[i].requirementIndex)
                {
                    alternates.Add(requirements[i].ToString());
                }
                else
                {
                    toReturn.Add(string.Join(" OR ", alternates.ToArray()));
                    alternates = new List<string>();
                    current++;
                }
            }
        }

        toReturn.Add(string.Join(" OR ", alternates.ToArray()));

        return "\n\n" + string.Join("\n", toReturn.ToArray());
    }
}
