using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DatabaseManager : MonoBehaviour
{

    private static float _heartRate = 80.0f;

    public static float HeartRate { get => _heartRate; }

    public RigControl control;

    float baseHB = 80.0f;
    float maxHB = 110.0f;
    float minHB = 60.0f;
    float tolerance = 95.0f;
    float time = 0.0f;
    float timeAboveTolerance = 0.0f;

    string getString = "https://gigle-rest.azurewebsites.net/gethr";

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
            //    if (RigControl.CurrentState == GameState.FreeWalking)
            //    _hearBeatsPerSecond = Random.Range(minHB, baseHB);
            //else
            //    _hearBeatsPerSecond = Random.Range(baseHB, maxHB);
            StartCoroutine(GetHR());

            if (_heartRate > tolerance) timeAboveTolerance += Time.deltaTime;
            else timeAboveTolerance = 0.0f;

            if (timeAboveTolerance > 5.0f) control.SetPenalty();
            time = 0.0f;
        }
    }

    IEnumerator GetHR()
    {

        using (UnityWebRequest request = UnityWebRequest.Get(getString))
        {
            yield return request.SendWebRequest();

            if (request.isNetworkError || request.isHttpError)
            {
                Debug.Log(request.error);
            }
            else
            {
                _heartRate = float.Parse(request.downloadHandler.text.Split("[")[1].Split(",")[0]);
                Debug.Log("Received HR Data: " + _heartRate);
            }
        }
    }

}
