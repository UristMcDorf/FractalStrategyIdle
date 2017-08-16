using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResearchActionImageLink : ActionImageLink
{
    static protected string researchActionImageLinkPrefabPath = "CommonPrefabs/ResearchActionImageLink";
    static protected ResearchActionImageLink researchActionImageLinkPrefab = null;

    Research research;

    public override void OnPointerDown(UnityEngine.EventSystems.PointerEventData eventData)
    {
        parent.ActionClicked(research);
    }

    public override void OnPointerEnter(UnityEngine.EventSystems.PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
    }

    public void SetParentResearch(Research research)
    {
        this.research = research;
    }

    public static ResearchActionImageLink MakeResearchActionImageLink(Action action)
    {
        if (researchActionImageLinkPrefab == null)
        {
            researchActionImageLinkPrefab = Resources.Load<ResearchActionImageLink>(researchActionImageLinkPrefabPath);
        }

        ResearchActionImageLink rail = Instantiate(researchActionImageLinkPrefab);

        rail.SetParent(action);

        return rail;
    }
}
