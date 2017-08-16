using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARResearched : ActionRequirement
{
    [SerializeField]
    string researchSysName = "default";
    [SerializeField]
    bool researchLevelMustMatch = false;
    [SerializeField]
    int researchLevel = 1;

    public override bool IsMet(ILeveled instance)
    {
        return ResearchManager.Instance.IsResearchedAndAtLeastLevel(researchSysName, (researchLevelMustMatch ? instance.Level : researchLevel));
    }

    public override string ToString()
    {
        return string.Format("Requires the \"{0}\" research.", ResearchManager.Instance.GetResearchName(researchSysName));
    }
}
