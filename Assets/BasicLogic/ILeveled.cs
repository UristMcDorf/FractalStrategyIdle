using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILeveled
{
    int Level
    {
        get;
    }
    int MaxLevel
    {
        get;
    }

    bool SetLevel(int level, bool clamp);
    void IncrementLevel();
    void DecrementLevel();
}
