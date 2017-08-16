using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    MapObjectInstance _currentlySelected = null;
    public MapObjectInstance CurrentlySelected
    {
        get
        {
            return _currentlySelected;
        }
    }

    static UIManager _instance = null;
    public static UIManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<UIManager>();
                if (_instance == null)
                    _instance = new UIManager();
            }
            return _instance;
        }
    }

    [SerializeField]
    CanvasGroup _mapView = null;
    public CanvasGroup MapView //TODO: remove
    {
        get
        {
            return _mapView;
        }
    }

    [SerializeField]
    CanvasGroup _researchView = null;
    public CanvasGroup ResearchView
    {
        get
        {
            return _researchView;
        }
    }

    [SerializeField]
    GameObject _resourceBar = null;
    public GameObject ResourceBar //TODO: remove
    {
        get
        {
            return _resourceBar;
        }
    }

    [SerializeField]
    Image descriptionPanelSprite = null;
    [SerializeField]
    Text descriptionPanelName = null;
    [SerializeField]
    Text descriptionPanelDescription = null;

    [SerializeField]
    GameObject actionPanel = null;

    [SerializeField]
    Image highlight = null;

    [SerializeField]
    GameObject tooltipPanel = null;
    GameObject currentTooltip = null;

    void OnEnable()
    {
        EventManager.Instance.AddListener<ActionPerformedEvent>(OnActionPerformed);
        EventManager.Instance.AddListener<NewSelectedInstanceEvent>(OnNewSelectedInstance);
        EventManager.Instance.AddListener<MapObjectInstanceClickedEvent>(OnMapObjectInstanceClicked);
        EventManager.Instance.AddListener<ObjectWithTooltipMousedOverEvent>(OnStartShowingTooltip);
        EventManager.Instance.AddListener<ObjectWithTooltipMousedAwayEvent>(OnStopShowingTooltip);
    }

    void OnDisable()
    {
        if (EventManager.Instance != null)
        {
            EventManager.Instance.RemoveListener<ActionPerformedEvent>(OnActionPerformed);
            EventManager.Instance.RemoveListener<NewSelectedInstanceEvent>(OnNewSelectedInstance);
            EventManager.Instance.RemoveListener<MapObjectInstanceClickedEvent>(OnMapObjectInstanceClicked);
            EventManager.Instance.RemoveListener<ObjectWithTooltipMousedOverEvent>(OnStartShowingTooltip);
            EventManager.Instance.RemoveListener<ObjectWithTooltipMousedAwayEvent>(OnStopShowingTooltip);
        }
    }

    void OnMapObjectInstanceClicked(MapObjectInstanceClickedEvent eventInst)
    {
        UpdateSelected(eventInst.instance);
    }
        
    void OnActionPerformed(ActionPerformedEvent eventInst)
    {
        UpdateDescriptionActionPanels();
        UpdateActionPanel();

        StopTooltip();
    }

    void OnNewSelectedInstance(NewSelectedInstanceEvent eventInst)
    {
        UpdateSelected(eventInst.instance);
    }

    void OnStartShowingTooltip(ObjectWithTooltipMousedOverEvent eventInst)
    {
        currentTooltip = eventInst.gameObject;

        StartTooltip(eventInst.text, eventInst.position);
    }

    void OnStopShowingTooltip(ObjectWithTooltipMousedAwayEvent eventInst)
    {
        if (currentTooltip == eventInst.gameObject)
            StopTooltip();
    }

    void Start()
    {
        SwitchTo(0);
    }

    void UpdateSelected(MapObjectInstance instance)
    {
        _currentlySelected = instance;

        UpdateDescriptionActionPanels();
        UpdateActionPanel();
        MoveSelectionHighlightOverCurrentMapObjectInstance();

        StopTooltip();
    }

    void UpdateDescriptionActionPanels()
    {
        if (CurrentlySelected == null)
            return;
        
        descriptionPanelSprite.enabled = true;
        descriptionPanelName.enabled = true;
        descriptionPanelDescription.enabled = true;

        descriptionPanelSprite.sprite = CurrentlySelected.Parent.MapSprite;
        descriptionPanelName.text = string.Format("{0} - Level {1}", CurrentlySelected.Parent.InGameName, CurrentlySelected.Level);
        descriptionPanelDescription.text = CurrentlySelected.MakeDescription();
    }

    void UpdateActionPanel() //TODO: make stuff and things. actions will probably need a link after all
    {
        List<GameObject> old = new List<GameObject>();
        foreach (Transform child in actionPanel.transform)
        {
            old.Add(child.gameObject);
        }
        old.ForEach(child => Destroy(child));

        foreach (Action action in CurrentlySelected.Parent.Actions)
        {
            if (action.CanBeShown(UIManager.Instance.CurrentlySelected))
            {
                ActionImageLink.MakeActionImageLink(action).transform.SetParent(actionPanel.transform);
            }
        }
    }

    void MoveSelectionHighlightOverCurrentMapObjectInstance()
    {
        highlight.transform.SetParent(CurrentlySelected.transform, false);
        highlight.enabled = true;
    }

    void StartTooltip(string text, Vector2 position)
    {
        tooltipPanel.gameObject.SetActive(true);

        tooltipPanel.GetComponentInChildren<Text>().text = text;
        tooltipPanel.transform.position = position;
    }

    void StopTooltip()
    {
        tooltipPanel.gameObject.SetActive(false);
    }

    public void SaveHighlighter()
    {
        highlight.transform.SetParent(null);
        highlight.enabled = false;
    }

    //TODO: less hardcoded
    public void SwitchTo(int index)
    {
        Hide(MapView);
        Hide(ResearchView);

        switch(index)
        {
        case 0:
            Show(MapView);
            break;
        case 1:
            Show(ResearchView);
            break;
        }
    }

    void Show(CanvasGroup canvasGroup)
    {
        SwitchStateForCanvasGroup(canvasGroup, true);
    }

    void Hide(CanvasGroup canvasGroup)
    {
        SwitchStateForCanvasGroup(canvasGroup, false);
    }

    void SwitchStateForCanvasGroup(CanvasGroup canvasGroup, bool state)
    {
        canvasGroup.alpha = state ? 1 : 0;

        canvasGroup.interactable = state;
        canvasGroup.blocksRaycasts = state;
    }
}
