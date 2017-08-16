using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResearchPanel : MonoBehaviour
{
    static string researchPanelPrefabPath = "CommonPrefabs/ResearchPanel";
    static ResearchPanel researchPanelPrefab = null;

    [SerializeField]
    Text _nameField = null;
    public Text NameField
    {
        get
        {
            return _nameField;
        }
    }

    [SerializeField]
    Text _descriptionField = null;
    public Text DescriptionField
    {
        get
        {
            return _descriptionField;
        }
    }   

    [SerializeField]
    GameObject _actionAnchor = null;
    public GameObject ActionAnchor
    {
        get
        {
            return _actionAnchor;
        }   
    }

    public static ResearchPanel MakePanel(Research research)
    {
        if (researchPanelPrefab == null)
        {
            researchPanelPrefab = Resources.Load<ResearchPanel>(researchPanelPrefabPath);
        }

        ResearchPanel panel = Instantiate(researchPanelPrefab);

        panel.NameField.text = research.InGameName;
        panel.DescriptionField.text = research.Description;

        return panel;
    }
}
