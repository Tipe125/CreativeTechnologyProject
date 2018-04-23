using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomEvent : MonoBehaviour {



    //A public string to enter what will be put into the database

    public GameObject objectTracker;
    public bool collisionEnter;
    public string collisionEnterString;
    public bool collisionStay;
    public string collisionStayString;
    public bool collisionExit;
    public string collisionExitString;
    public bool triggerEnter;
    public string triggerEnterString;
    public bool triggerStay;
    public string triggerStayString;
    public bool triggerExit;
    public string triggerExitString;
    public bool destroyed;
    public string destroyedString;
   
    

    


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		
	}

    void OnCollisionEnter()
    {
        Debug.Log("collisionEntered");
        if(collisionEnter)
        objectTracker.GetComponent<ObjectTracking>().CustomEventTrigger(collisionEnterString);
    }

    void OnCollisionStay()
    {
        Debug.Log("collisionStayed");
        if (collisionStay)
        objectTracker.GetComponent<ObjectTracking>().CustomEventTrigger(collisionStayString);
    }

    void OnCollisionExit()
    {
        Debug.Log("collisionExited");
        if (collisionExit)
        objectTracker.GetComponent<ObjectTracking>().CustomEventTrigger(collisionExitString);
    }

    void OnTriggerEnter()
    {
        Debug.Log("triggerEntered");
        if (triggerEnter)
        objectTracker.GetComponent<ObjectTracking>().CustomEventTrigger(triggerEnterString);
    }
    
    void OnTriggerStay()
    {
        Debug.Log("triggerStayed");
        if (triggerStay)
        objectTracker.GetComponent<ObjectTracking>().CustomEventTrigger(triggerStayString);
    }

    void OnTriggerExit()
    {
        Debug.Log("triggerExited");
        if (triggerExit)
        objectTracker.GetComponent<ObjectTracking>().CustomEventTrigger(triggerExitString);
    }

    void OnDestroy()
    {
        Debug.Log("Destroyed");
        if (destroyed)
            objectTracker.GetComponent<ObjectTracking>().CustomEventTrigger(destroyedString);
    }



    


}
