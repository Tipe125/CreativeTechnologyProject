using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

    // Use this for initialization
    public Camera mainCamera;

    float actualSpeed = 3.5f;
	void Start () {
        
		
	}
	
	// Update is called once per frame
	void Update () {


        MovementCheck();
		
	}

    void MovementCheck()
    {

       float rotX = Input.GetAxis("Mouse X") * actualSpeed;
        float rotY = Input.GetAxis("Mouse Y") * actualSpeed;

        
        mainCamera.transform.Rotate(-rotY, 0, 0);

        transform.Rotate(0, rotX, 0);

        float forwardSpeed = Input.GetAxis("Vertical")*actualSpeed;
        float horizontalSpeed = Input.GetAxis("Horizontal") * actualSpeed;

        Vector3 speed = new Vector3(horizontalSpeed, 0, forwardSpeed);

        speed = transform.rotation * speed;

        CharacterController cc = GetComponent<CharacterController>();

        cc.SimpleMove(speed);

       /* if(Input.GetKey(KeyCode.W))
        {
            transform.Translate(0.0f, 0.0f, 1*Time.deltaTime);
        }
         if(Input.GetKey(KeyCode.D))
        {
            transform.Translate(1*Time.deltaTime, 0.0f, 0.0f);

        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(-1 * Time.deltaTime, 0.0f, 0.0f);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate (0.0f, 0.0f, -1*Time.deltaTime);
        }*/

    }
}
