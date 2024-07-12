using UnityEngine;
using System.Collections;

public class WaypointCam : MonoBehaviour {
	public Color WaypointsColor = new Color(1,0,0,1);
	public bool draw = true;
	static public Transform[] waypoints;
	
	void Awake(){
		waypoints = gameObject.GetComponentsInChildren<Transform>();
		
	}
	void OnDrawGizmos () {	
		if (draw == true){
			waypoints = gameObject.GetComponentsInChildren<Transform>();
        
		foreach (Transform waypoint in waypoints){
			Gizmos.color = WaypointsColor;
			Gizmos.DrawSphere( waypoint.position, 1.0f );
				Gizmos.color = WaypointsColor;
			Gizmos.DrawWireSphere ( waypoint.position, 6.0f );
			}
		}
	}
}
