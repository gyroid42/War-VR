using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableUpdate : MonoBehaviour {

    public Shader shaderOutline_;
    public Shader shaderDefault_;
    public float zoomDistance_;
    public float zoomSpeed_;
    public Vector3 startPos_;
    private Vector3 gotoPos_;

    private Camera camera_;
    private bool active_;
    private AudioSource audio_;
    private Material material_;


	// Use this for initialization
	void Start () {

        camera_ = Camera.main;

        startPos_ = transform.position;

        audio_ = gameObject.GetComponent<AudioSource>();
        material_ = gameObject.GetComponent<MeshRenderer>().material;

        material_.shader = shaderDefault_;

        active_ = false;
	}
	
	// Update is called once per frame
	void Update () {

        if (active_) {

            Vector3 object2Cam = camera_.transform.position - transform.position;

            if ((object2Cam).sqrMagnitude > zoomDistance_ * zoomDistance_) {

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

    public void Selecting() {

        material_.shader = shaderOutline_;


    }

    public void UnSelecting() {

        material_.shader = shaderDefault_;

    }


    public void EnterLookAt() {


        audio_.Play();

        active_ = true;
        
    }

    public void ExitLookAt() {


        audio_.Stop();

        active_ = false;

        UnSelecting();
    }


}
