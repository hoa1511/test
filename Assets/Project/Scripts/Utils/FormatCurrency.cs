using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FormatCurrency
{
    public static string Format(long number)
    {
        if (number >= 1000000000000000)
        {
            return (number / 1000000000000000f).ToString("0.#") + "Q";
        }
        else if (number >= 1000000000000)
        {
            return (number / 1000000000000f).ToString("0.#") + "T";
        }
        else if (number >= 1000000000)
        {
            return (number / 1000000000f).ToString("0.#") + "B";
        }
        else if (number >= 1000000)
        {
            return (number / 1000000f).ToString("0.#") + "M";
        }
        else if (number >= 1000)
        {
            return (number / 1000f).ToString("0.#") + "K";
        }
        else
        {
            return number.ToString();
        }
    }
}
