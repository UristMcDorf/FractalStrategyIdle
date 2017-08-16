using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResearchManager : MonoBehaviour
{
    Dictionary<string, Research> researches = null;
    Dictionary<string, ResearchPanel> researchPanels = null;

    static ResearchManager _instance = null;
    static public ResearchManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<ResearchManager>();
            }
            return _instance;
        }
    }


	void Start()
    {
        SetupResearch();
	}

    void Update()
    {
        UpdateShownResearch();
    }

    void SetupResearch()
    {
        List<Research> researchPrefabs = new List<Research>();

        if (researches == null)
        {
            researchPrefabs = new List<Research>(Resources.LoadAll<Research>(Tier.tierBundlePath + '/' + Tier.Instance.SysName + "/Objects/Research"));
            researches = new Dictionary<string, Research>();
            researchPanels = new Dictionary<string, ResearchPanel>();
        }

        foreach (Research research in researchPrefabs)
        {
            Research instance = Instantiate<Research>(research);

            instance.Setup();

            instance.transform.SetParent(this.transform);
            researches.Add(instance.SysName, instance);

            ResearchPanel newPanel = ResearchPanel.MakePanel(research);

            researchPanels.Add(instance.SysName, newPanel);
            newPanel.transform.SetParent(UIManager.Instance.ResearchView.transform, false);

            instance.CompleteActionImageLink.transform.SetParent(newPanel.ActionAnchor.transform, false);
            if (instance.UpgradeActionImageLink != null)
                instance.UpgradeActionImageLink.transform.SetParent(newPanel.ActionAnchor.transform, false);
        }
    }

    public bool IsResearchedAndAtLeastLevel(string researchSysName, int level = 1)
    {
        Research research = null;

        if (researches.TryGetValue(researchSysName, out research))
            return research.Researched;

        return false;   
    }

    public string GetResearchName(string researchSysName)
    {
        Research research = null;

        if (researches.TryGetValue(researchSysName, out research))
            return research.ToString();

        return "N/A";
    }

    void UpdateShownResearch()
    {
        foreach (Research research in researches.Values)
        {
            if (research.CanBeShown())
                Show(research);
            else
                Hide(research);
        }
    }

    void Show(Research research)
    {
        ResearchPanel panel = researchPanels[research.SysName];
        panel.gameObject.SetActive(true);
    }

    void Hide(Research research)
    {
        researchPanels[research.SysName].gameObject.SetActive(false);
    }
}
