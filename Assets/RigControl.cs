using System.Collections.Generic;
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
    GameState currentState;
    public float rotationSpeed = 3.5f;
    public float movementSpeed = 0.0001f;
    private float mouseX;
    private float mouseY;
    Transform camera;
    public BoxCollider avatarCollider;
    public Transform environment;
    Vector3 yConstraint = new Vector3(1.0f, 0.0f, 1.0f);

    UnityEngine.XR.InputDevice leftHandDevice;

    Transform stateStandingInShaft;

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



        if (currentState == GameState.FreeWalking)
        {
            if (leftHandDevice == null) return;


            List<UnityEngine.XR.InputFeatureUsage> featureUsages = new List<UnityEngine.XR.InputFeatureUsage>();
            featureUsages.Add((UnityEngine.XR.InputFeatureUsage)UnityEngine.XR.CommonUsages.primary2DAxis);
            bool dPadUsed = leftHandDevice.TryGetFeatureUsages(featureUsages);
            if (dPadUsed)
            {
                Vector2 move = new Vector2();
                leftHandDevice.TryGetFeatureValue(UnityEngine.XR.CommonUsages.primary2DAxis, out move);
                metersMade = Time.deltaTime * movementSpeed * yConstraint;
                environment.position -= Vector3.Scale((camera.forward * move.y + camera.right * move.x), metersMade);
            }
        }

    }

    public void SetState(GameState gs)
    {
        currentState = gs;

        switch (currentState)
        {
            case GameState.StandingInShaft:
                break;

            case GameState.CrouchingInShaft:
                break;

            case GameState.LyingInShaft:
                break;

            default:
                break;
        }
    }

}
