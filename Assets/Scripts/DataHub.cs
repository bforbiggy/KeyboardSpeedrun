using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DataHub
{
    public static float averageCPS = 0;
    public static int runningClicks = 0;
    public static readonly float timeFrame = 0.5f;

    public static int totalClicks = 0;
    public static float totalTime = float.MaxValue;

    public static void Clear()
    {
        averageCPS = 0;
        runningClicks = 0;
        totalClicks = 0;
        totalTime = 0;
    }

    public static string ToString()
    {
        return $"Elapsed Time:{totalTime}, Total Clicks:{totalClicks}";
    }
}
