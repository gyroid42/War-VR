using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Timer {

    private float timeRemaining_;
    private float resetTime_ = 5.0f;
    private bool isActive_ = false;


    public void init(float time) {

        resetTime_ = time;

        timeRemaining_ = resetTime_;

    }


    public bool update() {

        if (isActive_) {
            timeRemaining_ -= Time.deltaTime; // * 144.0f;
        }

        Debug.Log("timer delta");
        Debug.Log(timeRemaining_);
        return isFinished();

    }

    public void Reset() {

        timeRemaining_ = resetTime_;
    }

    public bool isFinished() {

        if (timeRemaining_ <= 0) {
            return true;
        }

        return false;
    }

    public void setActive(bool active) {
        isActive_ = active;
    }

    public bool IsActive() {

        return isActive_;
    }


}

enum LOOK_AT_STATE {

    FINDING_OBJECT,
    FOUND_OBJECT,
    LOOKING_AT_OBJECT
};


public class LookAtObject : MonoBehaviour {

    public List<GameObject> interactables_;
    private LOOK_AT_STATE state_;
    private LOOK_AT_STATE nextState_;
    private bool isChangingState_;
    private GameObject currentObject_;
    private Timer timer_;
    public float lookAccuracy = 180.0f;
    public float timeToExamine;

    private InteractableUpdate objectUpdate_;

	// Use this for initialization
	void Start () {

        timer_ = new Timer();
        timer_.init(timeToExamine);
        timer_.setActive(false);
        state_ = LOOK_AT_STATE.FINDING_OBJECT;
        isChangingState_ = false;
	}

    void InitFinding() {



    }
    void InitFound() {


        timer_.Reset();
        timer_.setActive(true);


    }
    void InitLookingAt() {

        //audio = currentObject_.GetComponent<AudioSource>();
        //audio.Play();

        objectUpdate_ = currentObject_.GetComponent<InteractableUpdate>();

        objectUpdate_.EnterLookAt();

    }

    void CleanupFinding() {



    }


    void CleanupFound() {

        timer_.Reset();
        timer_.setActive(false);
    }
    void CleanupLookingAt() {

        objectUpdate_.ExitLookAt();

        objectUpdate_ = null;

    }


    void ChangeState() {

        switch (state_) {

            case LOOK_AT_STATE.FINDING_OBJECT:
                CleanupFinding();
                break;

            case LOOK_AT_STATE.FOUND_OBJECT:
                CleanupFound();
                break;

            case LOOK_AT_STATE.LOOKING_AT_OBJECT:
                CleanupLookingAt();
                break;
        }

        state_ = nextState_;


        switch (state_) {

            case LOOK_AT_STATE.FINDING_OBJECT:
                InitFinding();
                break;

            case LOOK_AT_STATE.FOUND_OBJECT:
                InitFound();
                break;

            case LOOK_AT_STATE.LOOKING_AT_OBJECT:
                InitLookingAt();
                break;
        }

        isChangingState_ = false;

    }
	
	// Update is called once per frame
	void Update () {


        switch (state_) {

            case LOOK_AT_STATE.FINDING_OBJECT:
                FindingUpdate();
                break;


            case LOOK_AT_STATE.FOUND_OBJECT:
                FoundUpdate();
                break;

            case LOOK_AT_STATE.LOOKING_AT_OBJECT:
                LookingAtUpdate();
                break;

            default:
                break;

        }

        if (isChangingState_) {
            ChangeState();
        }
	}


    void FindingUpdate() {

        bool found = false;

        for (int i = 0; i < interactables_.Count; i++) {

            if (checkIfLooking(interactables_[i])) {

                //ChangeState(FOUND);

                nextState_ = LOOK_AT_STATE.FOUND_OBJECT;
                isChangingState_ = true;


                currentObject_ = interactables_[i];

                found = true;
                Debug.Log("is looking at object");

                break;
            }
        }

        if (!found) {
            //Debug.Log("nope");
        }
    }
    void FoundUpdate() {


        // if not looking at current object
        if (!checkIfLooking(currentObject_)) {

            Debug.Log("is no longer looking");
            nextState_ = LOOK_AT_STATE.FINDING_OBJECT;
            isChangingState_ = true;
        }
        else if (timer_.update()) {

            Debug.Log("i'm going deep");
            nextState_ = LOOK_AT_STATE.LOOKING_AT_OBJECT;
            isChangingState_ = true;

        }

    }
    void LookingAtUpdate() {


        if (!checkIfLooking(currentObject_)) {

            Debug.Log("is no longer looking");
            nextState_ = LOOK_AT_STATE.FINDING_OBJECT;
            isChangingState_ = true;
        }

    }


    bool checkIfLooking(GameObject Object) {


        Vector3 Cam2Object = Object.transform.position - Camera.main.transform.position;

        float angle = Vector3.Angle(Camera.main.transform.forward, Cam2Object);

        if (angle <= lookAccuracy) {

            return true;
        }

        return false;
    }

}
