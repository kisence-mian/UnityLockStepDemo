using UnityEngine;
using System.Collections;

public class FlyCam : MonoBehaviour {
	private int currentWaypoint  = 0;
	public float rotateSpeed = 1.0f;
	public float moveSpeed = 10.0f;
	public float magnitudeMax = 10.0f;
	
	
	void Update () {
		if (WaypointCam.waypoints.Length>0){
			Vector3 RelativeWaypointPosition  = transform.InverseTransformPoint(new Vector3(WaypointCam.waypoints[currentWaypoint].position.x, WaypointCam.waypoints[currentWaypoint].position.y,WaypointCam.waypoints[currentWaypoint].position.z ) );
			Vector3 targetPoint =new Vector3(WaypointCam.waypoints[currentWaypoint].position.x,WaypointCam.waypoints[currentWaypoint].position.y,WaypointCam.waypoints[currentWaypoint].position.z );
			Quaternion targetrot = Quaternion.LookRotation ( targetPoint - transform.position);
			transform.rotation = Quaternion.Slerp(transform.rotation, targetrot, Time.deltaTime * rotateSpeed);
			var forward = transform.TransformDirection(Vector3.forward);
			transform.position += forward * moveSpeed*Time.deltaTime;
			if ( RelativeWaypointPosition.magnitude < magnitudeMax ) {
				currentWaypoint ++;
				if ( currentWaypoint >= WaypointCam.waypoints.Length ) {
					currentWaypoint = 0;
				}
			}
		}
	}
}
