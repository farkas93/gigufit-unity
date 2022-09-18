using System.Collections;
using System.Collections.Generic;
using UnityEditor.XR.LegacyInputHelpers;
using UnityEngine;
using UnityEngine.InputSystem;


public enum GameState
{
    FreeWalking = 0,
    StandingInShaft = 1,
    CrouchingInShaft,
    LyingInShaft,
    Penalty
}

public class RigControl : MonoBehaviour
{
    private static GameState currentState;
    public float rotationSpeed = 3.5f;
    public float movementSpeed = 0.0001f;
    private float mouseX;
    private float mouseY;
    Transform camera;
    public BoxCollider avatarCollider;
    public Transform environment;
    Vector3 yConstraint = new Vector3(1.0f, 0.0f, 1.0f);

    float camYOffsetLying = 0.8f;
    float camYOffsetCrouch = 1.5f;
    float camYOffsetUpright = 2.0f;

    UnityEngine.XR.InputDevice leftHandDevice;

    private static float metersMadeTotal = 0.0f;

    public static float MetersMadeTotal { get => metersMadeTotal; }
    public static GameState CurrentState { get => currentState; }

    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main.transform;
        currentState = GameState.FreeWalking;

        var devices = new List<UnityEngine.XR.InputDevice>();
        var desiredCharacteristics = UnityEngine.XR.InputDeviceCharacteristics.HeldInHand | UnityEngine.XR.InputDeviceCharacteristics.Left | UnityEngine.XR.InputDeviceCharacteristics.Controller;
        UnityEngine.XR.InputDevices.GetDevicesWithCharacteristics(desiredCharacteristics, devices);

        if (devices.Count >= 1)
        {
            leftHandDevice = devices[0];
            Debug.Log(string.Format("Device name '{0}' with role '{1}'", leftHandDevice.name, leftHandDevice.role.ToString()));
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 metersMade;
#if UNITY_EDITOR
        if (Input.GetKey(KeyCode.Mouse1))
        {
            camera.Rotate(new Vector3(-Input.GetAxis("Mouse Y") * rotationSpeed, Input.GetAxis("Mouse X") * rotationSpeed, 0));
            mouseX = camera.rotation.eulerAngles.x;
            mouseY = camera.rotation.eulerAngles.y;
            camera.rotation = Quaternion.Euler(mouseX, mouseY, 0);
        }

        metersMade = Time.deltaTime * movementSpeed* yConstraint;
        if (Input.GetKey(KeyCode.W))
            environment.position -= Vector3.Scale(camera.forward, metersMade);

        if(Input.GetKey(KeyCode.S))
            environment.position += Vector3.Scale(camera.forward, metersMade);

        if (Input.GetKey(KeyCode.D))
            environment.position -= Vector3.Scale(camera.right, metersMade);

        if (Input.GetKey(KeyCode.A))
            environment.position += Vector3.Scale(camera.right, metersMade);


#endif

        if (leftHandDevice == null) return;

        List<UnityEngine.XR.InputFeatureUsage> featureUsages = new List<UnityEngine.XR.InputFeatureUsage>();
        featureUsages.Add((UnityEngine.XR.InputFeatureUsage)UnityEngine.XR.CommonUsages.primary2DAxis);
        bool dPadUsed = leftHandDevice.TryGetFeatureUsages(featureUsages);
        if (dPadUsed || currentState == GameState.Penalty)
        {
            Vector2 move = new Vector2();
            leftHandDevice.TryGetFeatureValue(UnityEngine.XR.CommonUsages.primary2DAxis, out move);
            metersMade = Time.deltaTime * movementSpeed * yConstraint;
            if (currentState == GameState.FreeWalking)
                    environment.position -= Vector3.Scale((camera.forward * move.y + camera.right * move.x), metersMade);
            else if (currentState == GameState.Penalty)
            {
                environment.position += Vector3.Scale(camera.forward, metersMade);

            }
            else if ( currentState == GameState.StandingInShaft ||
                 currentState == GameState.CrouchingInShaft ||
                 currentState == GameState.LyingInShaft)
            {

                float sign = (move.y > 0.0f) ? 1.0f : 0.0f;
                metersMadeTotal += metersMade.x * sign;
                environment.position -= Vector3.Scale((camera.forward * move.y), metersMade);
            }
        }

    }

    public void SetState(GameObject target)
    {
        CameraOffset offset = this.GetComponent<CameraOffset>();
        Vector3 direction = (target.transform.position - camera.position);
        GameState nextState = GameState.StandingInShaft;
        switch (currentState)
        {
            case GameState.FreeWalking:
                /*if (target.tag == "StandingInShaft")*/ nextState = GameState.StandingInShaft;
                metersMadeTotal = 0.0f;
                offset.cameraYOffset = camYOffsetUpright;
                break;
            case GameState.StandingInShaft:
                if (target.tag == "StandingInShaft") nextState = GameState.FreeWalking;
                else /*if (target.tag == "CrouchingInShaft")*/
                {
                    nextState = GameState.CrouchingInShaft;
                    offset.cameraYOffset = camYOffsetCrouch;
                }
                break;

            case GameState.CrouchingInShaft:
                if (target.tag == "CrouchingInShaft")
                {
                    nextState = GameState.StandingInShaft;
                    offset.cameraYOffset = camYOffsetUpright;
                }
                else
                {/*if (target.tag == "LyingInShaft")*/
                    nextState = GameState.LyingInShaft;
                    offset.cameraYOffset = camYOffsetLying;
                }
                break;

            case GameState.LyingInShaft:
                if (target.tag == "LyingInShaft")
                {
                    nextState = GameState.CrouchingInShaft;
                    offset.cameraYOffset = camYOffsetCrouch;
                }
                break;

            default:
                break;        
        }
        currentState = nextState;

        environment.position -= Vector3.Scale(direction, new Vector3(1.0f, 0.0f, 1.0f));
    }


    public void SetPenalty() {
        currentState = GameState.Penalty;
    }


}
