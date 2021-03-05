using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DataUpdater : MonoBehaviour
{
    public TextMeshProUGUI timeElapsed;
    public TextMeshProUGUI avgCPS;

    private void FixedUpdate()
    {
        timeElapsed.text = $"Time Elapsed: {DataHub.totalTime:N3}";
        avgCPS.text = $"Average CPS: {DataHub.averageCPS:N1}";
    }
}
