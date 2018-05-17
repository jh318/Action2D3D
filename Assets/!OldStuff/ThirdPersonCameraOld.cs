using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour {

	public float cameraDistance = 10f;
	public float mouseSensitivity = 4f;
	public float scrollSensitivity = 2f;
	public float orbitDampening = 10f;
	public float scrollDampening = 6;
	public float minimumRotation = 0.0f;
	public float maximumRotation = 90.0f;
	public bool CameraDisabled = false;

	Transform pivotTransform;
	Vector3 rotation;

	void Start(){
		pivotTransform = transform.parent;
	}

	void Update(){
		if(Input.GetKeyDown(KeyCode.LeftShift)){
			CameraDisabled = !CameraDisabled;
		}
	}

	void LateUpdate(){
		if(!CameraDisabled){
			if(Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0){ //If mouse has input
				
				rotation.x += Input.GetAxis("Mouse X") * mouseSensitivity; //store mouse x in rotation vector
				rotation.y -= Input.GetAxis("Mouse Y") * mouseSensitivity; //store mouse y in rotation vector				

				rotation.y = Mathf.Clamp(rotation.y, minimumRotation, maximumRotation); //prevent rotation y from exceeding 90 degrees
			}

			if(Input.GetAxis("Mouse ScrollWheel") != 0f){ //if scrollwheel receieves input

				float ScrollAmount = Input.GetAxis("Mouse ScrollWheel") * scrollSensitivity; //store amount to scroll
			
				ScrollAmount *= (cameraDistance * 0.3f); //adjust scrollamount based on camera distance

				cameraDistance += ScrollAmount * -1f; //set camera's new distance, -1 to invert

				cameraDistance = Mathf.Clamp(cameraDistance, 1.5f, 100f); //prevent distance from leaving some range
			}

			Quaternion qt = Quaternion.Euler(rotation.y, rotation.x, 0); //Create new Quaternion (of eulers) with data from rotation vector3
			pivotTransform.rotation = Quaternion.Lerp(pivotTransform.rotation, qt, Time.deltaTime * orbitDampening); //set pivot's rotation to new Quaternion over time
			//pivotTransform.rotation
			//Set camera distance over time, *-1f to set position BEHIND pivot (instead of IN FRONT)
			transform.localPosition = new Vector3(0f, 0f, Mathf.Lerp(transform.localPosition.z, cameraDistance * -1f, Time.deltaTime * scrollDampening));

		}



	}
}
