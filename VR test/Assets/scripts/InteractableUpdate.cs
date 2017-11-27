using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableUpdate : MonoBehaviour {

    public float zoomDistance_;
    public float zoomSpeed_;
    private Vector3 startPos_;
    private Vector3 gotoPos_;

    private Camera camera_;
    private bool active_;
    private AudioSource audio_;


	// Use this for initialization
	void Start () {

        camera_ = Camera.main;

        startPos_ = transform.position;

        audio_ = gameObject.GetComponent<AudioSource>();

        active_ = false;
	}
	
	// Update is called once per frame
	void Update () {

        if (active_) {

            if ((transform.position - startPos_).sqrMagnitude < zoomDistance_ * zoomDistance_) {

                Vector3 object2Cam = camera_.transform.position - transform.position;
                object2Cam.Normalize();

                transform.position += object2Cam * zoomSpeed_ * Time.deltaTime;

            }

        }
        else {

            if (transform.position != startPos_) {
                Vector3 pos2Start = startPos_ - transform.position;

                if (pos2Start.sqrMagnitude >= zoomSpeed_ * Time.deltaTime) {

                    pos2Start.Normalize();

                    transform.position += pos2Start * zoomSpeed_ * Time.deltaTime;
                }
                else {

                    transform.position = startPos_;
                }
            }
        }
	}


    public void EnterLookAt() {

        audio_.Play();

        active_ = true;
        
    }

    public void ExitLookAt() {


        audio_.Stop();

        active_ = false;
    }


}
