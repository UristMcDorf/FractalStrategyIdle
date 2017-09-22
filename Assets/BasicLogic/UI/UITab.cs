using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Exists purely for registration purposes.

[RequireComponent(typeof(CanvasGroup))]
public class UITab : MonoBehaviour
{
    [SerializeField]
    private string id = "";

    public void Start()
    {
        UIManager.Instance.RegisterTab(id, GetComponent<CanvasGroup>());
    }
}
