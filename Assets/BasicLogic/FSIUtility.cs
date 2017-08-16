using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static public class FSIUtility
{
    static public string NumberToString(double amount, int minDecimal = 0)
    {
        if (amount >= 1000000)
            return amount.ToString("0.000e0");

        string decimalModifier = "";

        if (minDecimal > 0)
        {
            decimalModifier = ".";

            for (int i = 0; i < minDecimal; i++)
                decimalModifier += "0";
        }

        return amount.ToString("0" + decimalModifier);
    }

    static public bool IsSameOrSubclass(System.Type parent, System.Type child)
    {
        return child == parent || child.IsSubclassOf(parent);
    }
}
