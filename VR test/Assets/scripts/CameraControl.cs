using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;


public class CameraControl : MonoBehaviour {

    public float XSensitivity = 2f;
    public float YSensitivity = 2f;

    private float yaw = 0.0f;
    private float pitch = 0.0f;


    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {

        LookRotation();

	}


    public void LookRotation() {
        yaw += CrossPlatformInputManager.GetAxis("Mouse X") * XSensitivity;
        pitch -= CrossPlatformInputManager.GetAxis("Mouse Y") * YSensitivity;

        transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
    }
}
