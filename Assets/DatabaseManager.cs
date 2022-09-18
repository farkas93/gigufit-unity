using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatabaseManager : MonoBehaviour
{

    private static float _hearBeatsPerSecond = 80.0f;

    public static float HearBeatsPerSecond { get => _hearBeatsPerSecond; }

    public RigControl control;

    float baseHB = 80.0f;
    float maxHB = 110.0f;
    float minHB = 60.0f;
    float tolerance = 95.0f;
    float time = 0.0f;
    float timeAboveTolerance = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time > 2.0f)
        {
            if (RigControl.CurrentState == GameState.FreeWalking)
                _hearBeatsPerSecond = Random.Range(minHB, baseHB);
            else
                _hearBeatsPerSecond = Random.Range(baseHB, maxHB);

            if (_hearBeatsPerSecond > tolerance) timeAboveTolerance += Time.deltaTime;
            else timeAboveTolerance = 0.0f;

            if (timeAboveTolerance > 5.0f) control.SetPenalty();
            time = 0.0f;
        }
    }
}
