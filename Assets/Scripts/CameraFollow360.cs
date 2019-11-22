using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow360 : MonoBehaviour {

    /**
     * This isn't a networked script and instead locally moves the camera to react to and rotate around the established Player.
     * 
     */

	static public Transform player;
	public float distance = 10;
	public float height = 5;
	public Vector3 lookOffset = new Vector3(0,1,0);
	public float cameraSpeed = 10;
	public float rotSpeed = 10;

	void FixedUpdate () 
	{
		if(player)
		{
            /**
             *Forces the camera to look at a position plus offeset then rotate around this relative position.
             */
			Vector3 lookPosition = player.position + lookOffset;
			Vector3 relativePos = lookPosition - transform.position;
        	Quaternion rot = Quaternion.LookRotation(relativePos);

            //https://tiborstanko.sk/lerp-vs-slerp.html 
            transform.rotation = Quaternion.Slerp(this.transform.rotation, rot, Time.deltaTime * rotSpeed * 0.1f);
			
			Vector3 targetPos = player.transform.position + player.transform.up * height - player.transform.forward * distance;
			
			this.transform.position = Vector3.Lerp(this.transform.position, targetPos, Time.deltaTime * cameraSpeed * 0.1f);
		}
	}
}
