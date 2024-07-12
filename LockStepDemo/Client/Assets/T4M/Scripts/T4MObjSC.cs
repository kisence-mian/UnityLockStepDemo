//Update SC
using UnityEngine;
using System.Collections;
[ExecuteInEditMode]
public class T4MObjSC : MonoBehaviour {
	[HideInInspector] 
	public string ConvertType = "";
	 [HideInInspector]
	public bool EnabledLODSystem=true;
	[HideInInspector] 
	public Vector3[] ObjPosition;
	[HideInInspector] 
	public T4MLodObjSC[] ObjLodScript ;
	[HideInInspector] 
	public int[] ObjLodStatus; //0=Occlude // 1=LOD1 // 2=LOD=2 // 3=LOD3
	[HideInInspector] 
	public float MaxViewDistance= 60.0f;
	[HideInInspector] 
	public float LOD2Start = 20.0f;
	[HideInInspector] 
	public float LOD3Start = 40.0f;
	[HideInInspector] 
	public float Interval = 0.5f;
	[HideInInspector] 
	public Transform PlayerCamera;
	private Vector3 OldPlayerPos;
	[HideInInspector] 
	public int Mode =1;
	[HideInInspector] 
	public int Master; 
	[HideInInspector] 
	public bool enabledBillboard=true;
	[HideInInspector] 
	public Vector3[] BillboardPosition;
	[HideInInspector] 
	public float BillInterval = 0.05f;
	[HideInInspector] 
	public int[] BillStatus; //0=Occlude // 1=Active
	[HideInInspector] 
	public float BillMaxViewDistance= 30.0f;
	[HideInInspector] 
	public T4MBillBObjSC[] BillScript ;
	[HideInInspector] 
	public bool enabledLayerCul=true;
	[HideInInspector] 
	public float BackGroundView  = 1000f;
	[HideInInspector] 
	public float FarView  = 200.0f;
	[HideInInspector] 
	public float NormalView = 60.0f;
	[HideInInspector] 
	public float CloseView = 30.0f;
	float[] distances = new float[32];
	[HideInInspector] 
	public int Axis =0;
	[HideInInspector] 
	public bool LODbasedOnScript = true;
	[HideInInspector] 
	public bool BilBbasedOnScript = true;

	public Material T4MMaterial;
	public MeshFilter T4MMesh;
	
	//ATS
	[HideInInspector] 
	public Color TranslucencyColor= new Color(0.73f,0.85f,0.4f,1f);
	[HideInInspector] 
	public Vector4 Wind = new Vector4(0.85f,0.075f,0.4f,0.5f);
	[HideInInspector] 
	public float  WindFrequency = 0.75f;
	[HideInInspector] 
	public float GrassWindFrequency = 1.5f;
	[HideInInspector] 
	public bool ActiveWind = false;
	
	public bool LayerCullPreview = false;
	public bool LODPreview = false;
	public bool BillboardPreview = false;
	
	public void Awake()
	{
		if (Master ==1){
			if (PlayerCamera == null && Camera.main)
				PlayerCamera = Camera.main.transform;
			else if (PlayerCamera == null && !Camera.main){
				Camera[] Cam =  GameObject.FindObjectsOfType(typeof(Camera)) as Camera[];
				for (var b =0; b <Cam.Length;b++){
					if (Cam[b].GetComponent<AudioListener>()){
						PlayerCamera = Cam[b].transform; 
					}
				}
			}
			if (enabledLayerCul){
				distances[26] = CloseView;
				distances[27] = NormalView;
				distances[28] = FarView;
				distances[29] = BackGroundView;
			    PlayerCamera.GetComponent<Camera>().layerCullDistances = distances;
			}
			
			if (EnabledLODSystem && ObjPosition.Length>0 && Mode ==1){
				if(ObjLodScript[0].gameObject != null){
					if (LODbasedOnScript)
						InvokeRepeating("LODScript", Random.Range(0,Interval), Interval);	
					else InvokeRepeating("LODLay", Random.Range(0,Interval), Interval);
				}
			}else if (EnabledLODSystem && ObjPosition.Length>0 && Mode ==2){
				if(ObjLodScript[0] != null){
					for (var i =0; i <ObjPosition.Length;i++){
						if (ObjLodScript[i] !=null){
							if (LODbasedOnScript)
								ObjLodScript[i].ActivateLODScrpt();
							else ObjLodScript[i].ActivateLODLay();
						}	
					}
				}
			}
			
			if (enabledBillboard && BillboardPosition.Length>0){
				if(BillScript[0] != null){
					if (BilBbasedOnScript)
						InvokeRepeating("BillScrpt", Random.Range(0,BillInterval), BillInterval);
					else InvokeRepeating("BillLay", Random.Range(0,BillInterval), BillInterval);
				}
			}
		}
	} public Texture2D T4MMaskTex2d;
	 public Texture2D T4MMaskTexd;
	void OnGUI(){
		if(Application.isPlaying == false && Master ==1){
			if (LayerCullPreview && enabledLayerCul){
				GUI.color = Color.green;
				GUI.Label(new Rect(0,0,200,200), "LayerCull Preview ON");
			}else{
				GUI.color = Color.red;
				GUI.Label(new Rect(0,0,200,200), "LayerCull Preview OFF");
			}
			if (LODPreview && ObjPosition.Length>0){
				GUI.color = Color.green;
				GUI.Label(new Rect(0,20,200,200), "LOD Preview ON");
			}else if (LODPreview && ObjPosition.Length==0){
				GUI.color = Color.red;
				GUI.Label(new Rect(0,20,200,200), "Activate the LOD First");
			}else{
				GUI.color = Color.red;
				GUI.Label(new Rect(0,20,200,200), "LOD Preview OFF");
			}
			 if(BillboardPreview  && BillboardPosition.Length>0){
			    GUI.color = Color.green;
				GUI.Label(new Rect(0,40,200,200), "Billboard Preview ON");
			}else if (BillboardPreview && BillboardPosition.Length==0){
				GUI.color = Color.red;
				GUI.Label(new Rect(0,40,200,200), "Activate the Billboard First");
			}else{
				GUI.color = Color.red;
				GUI.Label(new Rect(0,40,200,200), "Billboard Preview OFF");
			}
		}
	}
	void LateUpdate () {
		if(ActiveWind){
			Color WindRGBA = Wind *  ( (Mathf.Sin(Time.realtimeSinceStartup * WindFrequency)));
			WindRGBA.a = Wind.w;
			Color GrassWindRGBA  = Wind *  ( (Mathf.Sin(Time.realtimeSinceStartup * GrassWindFrequency)));
			GrassWindRGBA.a = Wind.w;
			Shader.SetGlobalColor("_Wind", WindRGBA);
			Shader.SetGlobalColor("_GrassWind", GrassWindRGBA);
			Shader.SetGlobalColor("_TranslucencyColor", TranslucencyColor);
			Shader.SetGlobalFloat("_TranslucencyViewDependency;", 0.65f);
		}
		
		if (PlayerCamera && Application.isPlaying == false  && Master ==1){
			if (LayerCullPreview && enabledLayerCul){
				distances[26] = CloseView;
				distances[27] = NormalView;
				distances[28] = FarView;
				distances[29] = BackGroundView;
			    PlayerCamera.GetComponent<Camera>().layerCullDistances = distances;
			}else{
				distances[26] = PlayerCamera.GetComponent<Camera>().farClipPlane;
				distances[27] = PlayerCamera.GetComponent<Camera>().farClipPlane;
				distances[28] = PlayerCamera.GetComponent<Camera>().farClipPlane;
				distances[29] = PlayerCamera.GetComponent<Camera>().farClipPlane;
			    PlayerCamera.GetComponent<Camera>().layerCullDistances = distances;	
			}
			
			if (LODPreview){
				if (EnabledLODSystem && ObjPosition.Length>0 && Mode ==1){
				if(ObjLodScript[0].gameObject != null){
					if (LODbasedOnScript)
						LODScript();	
					else LODLay();
				}
				}else if (EnabledLODSystem && ObjPosition.Length>0 && Mode ==2){
					if(ObjLodScript[0] != null){
						for (var i =0; i <ObjPosition.Length;i++){
							if (ObjLodScript[i] !=null){
								if (LODbasedOnScript)
									ObjLodScript[i].AFLODScrpt();
								else ObjLodScript[i].AFLODLay();
							}	
						}
					}
				}	
			}
			
			if (BillboardPreview){
				if (enabledBillboard && BillboardPosition.Length>0){
					if(BillScript[0] != null){
						if (BilBbasedOnScript)
							BillScrpt();
						else BillLay();
					}
				}
			}
		}		
	}
	
	void BillScrpt()
	{  
		for (var j =0; j <BillboardPosition.Length;j++){
		
			if(Vector3.Distance(BillboardPosition[j], PlayerCamera.position) <= BillMaxViewDistance){
				if (BillStatus[j] != 1){
					BillScript[j].Render.enabled = true;
					BillStatus[j] = 1;
				}
				if (Axis == 0)
					BillScript[j].Transf.LookAt(new Vector3 (PlayerCamera.position.x,BillScript[j].Transf.position.y,PlayerCamera.position.z) , Vector3.up);	
				else	
					BillScript[j].Transf.LookAt(PlayerCamera.position, Vector3.up);
				
			}else if (BillStatus[j] != 0 && !BillScript[j].Render.enabled){
					BillScript[j].Render.enabled = false;
					BillStatus[j] = 0;
			}
		}
	}
	
    void BillLay()
	{  
		for (var j =0; j <BillboardPosition.Length;j++){
			int Lay = BillScript[j].gameObject.layer; 
			if(Vector3.Distance(BillboardPosition[j], PlayerCamera.position) <= distances[Lay]){
				if (Axis == 0)
					BillScript[j].Transf.LookAt(new Vector3 (PlayerCamera.position.x,BillScript[j].Transf.position.y,PlayerCamera.position.z) , Vector3.up);	
				else	
					BillScript[j].Transf.LookAt(PlayerCamera.position, Vector3.up);
			}
		}
	}
	
	
	void LODScript()
	{
		if (OldPlayerPos == PlayerCamera.position)
			return;
			
		OldPlayerPos = PlayerCamera.position;	
		for (var i =0; i <ObjPosition.Length;i++){
		
			float distanceFromCamera = Vector3.Distance(new Vector3(ObjPosition[i].x,PlayerCamera.position.y,ObjPosition[i].z), PlayerCamera.position);
			if(distanceFromCamera <= MaxViewDistance ){
				 if(distanceFromCamera < LOD2Start && ObjLodStatus[i] != 1){
					ObjLodScript[i].LOD2.enabled = ObjLodScript[i].LOD3.enabled = false;
					ObjLodScript[i].LOD1.enabled = true;
					ObjLodStatus[i]=1;
				 }else if (distanceFromCamera >= LOD2Start && distanceFromCamera < LOD3Start && ObjLodStatus[i] != 2){
					ObjLodScript[i].LOD1.enabled = ObjLodScript[i].LOD3.enabled = false;
					ObjLodScript[i].LOD2.enabled = true;
					ObjLodStatus[i]=2;
				}else if (distanceFromCamera >= LOD3Start && ObjLodStatus[i] != 3){
					ObjLodScript[i].LOD2.enabled = ObjLodScript[i].LOD1.enabled = false;
					ObjLodScript[i].LOD3.enabled = true;
					ObjLodStatus[i]=3;
				}
			}else if(ObjLodStatus[i] != 0){
				ObjLodScript[i].LOD1.enabled = ObjLodScript[i].LOD2.enabled = ObjLodScript[i].LOD3.enabled = false;
				ObjLodStatus[i]=0;
			}
		}		
	}
	
	void LODLay()
	{
		if (OldPlayerPos == PlayerCamera.position)
			return;
			
		OldPlayerPos = PlayerCamera.position;
		
		for (var i =0; i <ObjPosition.Length;i++){
			float distanceFromCamera =  Vector3.Distance(new Vector3(ObjPosition[i].x,PlayerCamera.position.y,ObjPosition[i].z), PlayerCamera.position);
			
			int Lay2 = ObjLodScript[i].gameObject.layer; 
			if(distanceFromCamera <= distances[Lay2]+5){
				 if(distanceFromCamera < LOD2Start && ObjLodStatus[i] != 1){
					ObjLodScript[i].LOD2.enabled = ObjLodScript[i].LOD3.enabled = false;
					ObjLodScript[i].LOD1.enabled = true;
					ObjLodStatus[i]=1;
				 }else if (distanceFromCamera >= LOD2Start && distanceFromCamera < LOD3Start && ObjLodStatus[i] != 2){
					ObjLodScript[i].LOD1.enabled = ObjLodScript[i].LOD3.enabled = false;
					ObjLodScript[i].LOD2.enabled = true;
					ObjLodStatus[i]=2;
				}else if (distanceFromCamera >= LOD3Start && ObjLodStatus[i] != 3){
					ObjLodScript[i].LOD2.enabled = ObjLodScript[i].LOD1.enabled = false;
					ObjLodScript[i].LOD3.enabled = true;
					ObjLodStatus[i]=3;
				}
			}		
		}
	}
}
