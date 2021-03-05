using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DataHub
{
    //EQUIVALENT TO CLICKS PER SECOND
    public static int runningClicks = 0;

    public static int totalClicks = 0;
    public static float totalTime = float.MaxValue;

    public static void Clear()
    {
        runningClicks = 0;
        totalClicks = 0;
        totalTime = 0;
    }

    public static string ToString()
    {
        return $"Elapsed Time:{totalTime}, Total Clicks:{totalClicks}";
    }
}
