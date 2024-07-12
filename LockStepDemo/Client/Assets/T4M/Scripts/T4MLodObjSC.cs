//Update SC
using UnityEngine;
using System.Collections;

 public class T4MLodObjSC : MonoBehaviour {

	//[HideInInspector]
	public Renderer LOD1;
	//[HideInInspector]
	public Renderer LOD2;
	//[HideInInspector]
	public Renderer LOD3;
	
	//Lod 2.0
		[HideInInspector]
		public float Interval = 0.5f;
		[HideInInspector]
		public Transform PlayerCamera;
		[HideInInspector]
		public int Mode;
		private Vector3 OldPlayerPos;
		[HideInInspector]
		public int ObjLodStatus;
		[HideInInspector]
		public float MaxViewDistance= 60.0f;
		[HideInInspector]
		public float LOD2Start = 20.0f;
		[HideInInspector]
		public float LOD3Start = 40.0f;
		
		
		public void ActivateLODScrpt()
		{
			if (Mode != 2)
				return;
		
			if (PlayerCamera == null)
				PlayerCamera = Camera.main.transform;
			
			InvokeRepeating("AFLODScrpt", Random.Range(0,Interval), Interval);
		}
	
		public void ActivateLODLay()
		{
			if (Mode != 2)
				return;
		
			if (PlayerCamera == null)
				PlayerCamera = Camera.main.transform;
			
			InvokeRepeating("AFLODLay", Random.Range(0,Interval), Interval);
		}
		
		public void AFLODLay()
		{
			if (OldPlayerPos == PlayerCamera.position)
				return;
				
			OldPlayerPos = PlayerCamera.position;	
			
			float distanceFromCamera = Vector3.Distance(new Vector3(transform.position.x,PlayerCamera.position.y,transform.position.z), PlayerCamera.position);
		
			int Lay2 = gameObject.layer; 
		
			if(distanceFromCamera <= PlayerCamera.GetComponent<Camera>().layerCullDistances[Lay2]+5){
				 if(distanceFromCamera < LOD2Start && ObjLodStatus != 1){
					LOD3.enabled = LOD2.enabled = false;
					LOD1.enabled = true;
					ObjLodStatus=1;
				 }else if (distanceFromCamera >= LOD2Start && distanceFromCamera < LOD3Start && ObjLodStatus != 2){
					LOD1.enabled = LOD3.enabled = false;
					LOD2.enabled = true;
					ObjLodStatus=2;
				}else if (distanceFromCamera >= LOD3Start && ObjLodStatus != 3){
					LOD1.enabled = LOD2.enabled = false;
					LOD3.enabled = true;
					ObjLodStatus=3;
				}
			}
		}
	
		public void AFLODScrpt()
		{
		if (OldPlayerPos == PlayerCamera.position)
			return;
			
		OldPlayerPos = PlayerCamera.position;	
		
		float distanceFromCamera = Vector3.Distance(new Vector3(transform.position.x,PlayerCamera.position.y,transform.position.z), PlayerCamera.position);
		if(distanceFromCamera <= MaxViewDistance){
			 if(distanceFromCamera < LOD2Start && ObjLodStatus != 1){
				LOD3.enabled = LOD2.enabled = false;
				LOD1.enabled = true;
				ObjLodStatus=1;
			 }else if (distanceFromCamera >= LOD2Start && distanceFromCamera < LOD3Start && ObjLodStatus != 2){
				LOD1.enabled = LOD3.enabled = false;
				LOD2.enabled = true;
				ObjLodStatus=2;
			}else if (distanceFromCamera >= LOD3Start && ObjLodStatus != 3){
				LOD1.enabled = LOD2.enabled = false;
				LOD3.enabled = true;
				ObjLodStatus=3;
			}
		}else if(ObjLodStatus != 0){
			LOD1.enabled = LOD2.enabled = LOD3.enabled = false;
			ObjLodStatus=0;
			
		}
	}
}