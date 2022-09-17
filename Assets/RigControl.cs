using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigControl : MonoBehaviour
{

    public float rotationSpeed = 3.5f;
    public float movementSpeed = 0.0001f;
    private float mouseX;
    private float mouseY;
    Transform camera;
    Vector3 yConstraint = new Vector3(1.0f, 0.0f, 1.0f);

    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKey(KeyCode.Mouse1))
        {
            camera.Rotate(new Vector3(Input.GetAxis("Mouse Y") * rotationSpeed, Input.GetAxis("Mouse X") * rotationSpeed, 0));
            mouseX = camera.rotation.eulerAngles.x;
            mouseY = camera.rotation.eulerAngles.y;
            camera.rotation = Quaternion.Euler(mouseX, mouseY, 0);
        }

        Vector3 metersMade = Time.deltaTime * movementSpeed* yConstraint;
        if (Input.GetKey(KeyCode.W))
            camera.position += Vector3.Scale(camera.forward, metersMade);

        if(Input.GetKey(KeyCode.S))
            camera.position -= Vector3.Scale(camera.forward, metersMade);

        if (Input.GetKey(KeyCode.D))
            camera.position += Vector3.Scale(camera.right, metersMade);

        if (Input.GetKey(KeyCode.A))
            camera.position -= Vector3.Scale(camera.right, metersMade);


#endif
    }
}
