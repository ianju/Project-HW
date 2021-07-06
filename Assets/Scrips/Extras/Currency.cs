using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Currency
{
    public static string ToCurrency(this float amount)
    {
        int length = Math.Floor(amount).ToString().Split('.')[0].Length;
        if (length > 5)
        {
            return amount.ToString("0,,.##M");

        }
        else if (length > 3)
        {
            return amount.ToString("0,.##K");
        }
        return amount.ToString();
    }
}
