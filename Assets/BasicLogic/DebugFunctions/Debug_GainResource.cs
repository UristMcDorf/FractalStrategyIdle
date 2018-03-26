using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Debug_GainResource : MonoBehaviour
{
    [SerializeField]
    Dropdown resourceTypeSelector = null;
    [SerializeField]
    InputField resourceAmountInput = null;

    public void Activate()
    {
        if (EventManager.Instance != null)
        {
            EventManager.Instance.QueueEvent(new ProduceImmediateIncomeEvent(resourceTypeSelector.captionText.text, double.Parse(resourceAmountInput.text)));
            EventManager.Instance.QueueEvent(new DebugFunctionUsedEvent(string.Format("Added {0} of {1}.", double.Parse(resourceAmountInput.text), resourceTypeSelector.captionText.text)));
        }
    }
}
