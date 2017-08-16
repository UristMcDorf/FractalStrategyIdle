using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ActionImageLink : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    static protected string actionImageLinkPrefabPath = "CommonPrefabs/ActionImageLink";
    static protected ActionImageLink actionImageLinkPrefab = null;

    protected Action parent;

    public void SetParent(Action parent)
    {
        this.parent = parent;
        GetComponent<Image>().sprite = parent.Icon;
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        parent.ActionClicked(UIManager.Instance.CurrentlySelected);
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        EventManager.Instance.QueueEvent(new ObjectWithTooltipMousedOverEvent(this.gameObject, parent.GetDescription(), eventData.position));
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        EventManager.Instance.QueueEvent(new ObjectWithTooltipMousedAwayEvent(this.gameObject));
    }

    public static ActionImageLink MakeActionImageLink(Action action)
    {
        if (actionImageLinkPrefab == null)
        {
            actionImageLinkPrefab = Resources.Load<ActionImageLink>(actionImageLinkPrefabPath);
        }

        ActionImageLink ail = Instantiate(actionImageLinkPrefab);

        ail.SetParent(action);

        return ail;
    }
}
