using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogManager : MonoBehaviour
{
    [SerializeField]
    Text logHeader = null;
    [SerializeField]
    Text logBody = null;

    [SerializeField]
    int maxEntries = 50;

    Queue<string> entries = new Queue<string>();

    void Start()
    {
        logHeader.text = "Fractal Strategy Idle";
    }

    void OnEnable()
    {
        EventManager.Instance.AddListener<MapObjectCreatedEvent>(OnImprovementCreated);
        EventManager.Instance.AddListener<MapObjectDestroyedEvent>(OnImprovementDestroyed);
        EventManager.Instance.AddListener<ResearchCompletedEvent>(OnResearched);
        EventManager.Instance.AddListener<ResearchUpgradedEvent>(OnResearchUpgraded);
        EventManager.Instance.AddListener<DebugFunctionUsedEvent>(OnDebugFunctionUsed);
    }

    void OnDisable()
    {
        if (EventManager.Instance != null)
        {
            EventManager.Instance.RemoveListener<MapObjectCreatedEvent>(OnImprovementCreated);
            EventManager.Instance.RemoveListener<MapObjectDestroyedEvent>(OnImprovementDestroyed);
            EventManager.Instance.RemoveListener<ResearchCompletedEvent>(OnResearched);
            EventManager.Instance.RemoveListener<ResearchUpgradedEvent>(OnResearchUpgraded);
            EventManager.Instance.RemoveListener<DebugFunctionUsedEvent>(OnDebugFunctionUsed);
        }
    }

    void OnImprovementCreated(MapObjectCreatedEvent eventInst)
    {
        if (eventInst.instance.GetType() == typeof(ImprovementInstance)) //TODO: less hacky?
            WriteMessage(string.Format("Built {0}.", eventInst.instance.Parent.InGameName));
    }

    void OnImprovementDestroyed(MapObjectDestroyedEvent eventInst)
    {
        if (eventInst.instance.GetType() == typeof(ImprovementInstance)) //TODO: less hacky?
            WriteMessage(string.Format("Destroyed {0}.", eventInst.instance.Parent.InGameName));
    }

    void OnResearched(ResearchCompletedEvent eventInst)
    {
        WriteMessage(string.Format("Researched \"{0}\".", eventInst.research.ToString()));
    }

    void OnResearchUpgraded(ResearchUpgradedEvent eventInst)
    {
        WriteMessage(string.Format("Research \"{0}\" upgraded to level {1}.", eventInst.research.ToString(), eventInst.newLevel));
    }

    void OnDebugFunctionUsed(DebugFunctionUsedEvent eventInst)
    {
        WriteMessage(string.Format("DEBUG: {0}", eventInst.text));
    }

    void WriteMessage(string text)
    {
        if (entries.Count >= maxEntries)
            entries.Dequeue();
        
        entries.Enqueue(System.DateTime.Now + ": " + text);

        UpdateLog();
    }

    void UpdateLog()
    {
        logBody.text = "";

        foreach (string entry in entries)
        {
            logBody.text += '\n' + entry;
        }
    }
}
