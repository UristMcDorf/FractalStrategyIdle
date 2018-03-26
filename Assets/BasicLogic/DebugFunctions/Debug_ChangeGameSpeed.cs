using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Debug_ChangeGameSpeed : MonoBehaviour
{
    [SerializeField]
    InputField gameSpeedInput = null;

    public void Activate()
    {
        if (EventManager.Instance != null)
        {
            if (GameManager.Instance.SetGameSpeed(double.Parse(gameSpeedInput.text)))
                EventManager.Instance.QueueEvent(new DebugFunctionUsedEvent(string.Format("Game speed set to {0}.", double.Parse(gameSpeedInput.text))));
            else
                EventManager.Instance.QueueEvent(new DebugFunctionUsedEvent(string.Format("Unable to set game speed to {0} - invalid value. Try setting between {1} and {2}.", double.Parse(gameSpeedInput.text), GameManager.k_minGameSpeed, GameManager.k_maxGameSpeed)));
        }
    }
}
