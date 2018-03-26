using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public const double k_minGameSpeed = 0.1d;
    public const double k_maxGameSpeed = 100d;

    [SerializeField]
    double _gameSpeed = 1d;
    public double GameSpeed
    {
        get
        {
            return _gameSpeed;
        }
    }

    static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();
            }
            return _instance;
        }
    }

    public bool SetGameSpeed(double newSpeed)
    {
        if (newSpeed < k_minGameSpeed)
            return false;
        if (newSpeed > k_maxGameSpeed)
            return false;

        _gameSpeed = newSpeed;
        return true;
    }
}
