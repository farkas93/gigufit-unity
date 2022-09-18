using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUDUpdater : MonoBehaviour
{

    TextMeshPro mesh;
    // Start is called before the first frame update
    void Start()
    {
        mesh = GetComponent<TextMeshPro>();
    }

    // Update is called once per frame
    void Update()
    {
        mesh.text = "Heart Rate: " + DatabaseManager.HeartRate.ToString("F") + "/s\nMeters Made: " + RigControl.MetersMadeTotal.ToString("F");
    }
}
