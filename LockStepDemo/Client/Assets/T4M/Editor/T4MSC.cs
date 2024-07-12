//Update SC
using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.IO;
using System.Text;
	
public class T4MSC : EditorWindow {
	
	static public Transform CurrentSelect ;
	public enum SM{
		ShaderModel1,
		ShaderModel2,
		ShaderModel3,
		CustomShader
	}
	public enum PlantMode{
		Classic,
		Follow_Normals
	}
	
	GUIContent[] MenuIcon = new GUIContent[7];
	
	static public int T4MMenuToolbar = 0;
	
	enum EnumShaderGLES1{	
		T4M_2_Textures_Auto_BeastLM_2DrawCall,
		T4M_2_Textures_ManualAdd_BeastLM_1DC,
		T4M_2_Textures_ManualAdd_CustoLM_1DC
	}
	
	enum CreaType{
		Classic_T4M,
		Custom	
	}
	CreaType CreationBB = CreaType.Classic_T4M;	
		
	enum LODMod{
		Mass_Control,
		Independent_Control
	}
	
	public enum ViewD{
		Close,
		Middle,
		Far,
		BackGround
	}
	 enum BillbAxe{
		Y_Axis,
		All_Axis
	}
	
	 enum OccludeBy{
		Max_View_Distance,
		Layer_Cull_Distances
	}
	
	
	OccludeBy LODocclusion = OccludeBy.Layer_Cull_Distances;
	OccludeBy BilBocclusion = OccludeBy.Layer_Cull_Distances;
	
	
	BillbAxe BillBoardAxis = BillbAxe.Y_Axis;
	
	
	static public ViewD[] ViewDistance= {ViewD.Middle, ViewD.Middle,ViewD.Middle,ViewD.Middle,ViewD.Middle,ViewD.Middle};
	
	enum EnumShaderGLES2{
		T4M_2_Textures_Unlit_Lightmap_Compatible,
		T4M_3_Textures_Unlit_Lightmap_Compatible,
		T4M_4_Textures_Unlit_Lightmap_Compatible,
		T4M_5_Textures_Unlit_Lightmap_Compatible,
		T4M_6_Textures_Unlit_Lightmap_Compatible,
		T4M_World_Projection_Unlit_Lightmap_Compatible,
		T4M_6_Textures_Unlit_No_Lightmap,
		T4M_2_Textures_HighSpec,
		T4M_3_Textures_HighSpec,
		T4M_4_Textures_HighSpec,
		T4M_5_Textures_HighSpec,
		T4M_6_Textures_HighSpec,
		T4M_World_Projection_HighSpec,
		T4M_2_Textures_Specular,
		T4M_3_Textures_Specular,
		T4M_4_Textures_Specular,
		T4M_2_Textures_4_Mobile,
		T4M_3_Textures_4_Mobile,
		T4M_4_Textures_4_Mobile,
		T4M_2_Textures_Toon,
		T4M_3_Textures_Toon,
		T4M_4_Textures_Toon,
		T4M_2_Textures_Bumped_Mobile,
		T4M_3_Textures_Bumped_Mobile,
		T4M_2_Textures_Bumped,
		T4M_3_Textures_Bumped,
		T4M_4_Textures_Bumped,
		T4M_2_Textures_Bumped_SPEC_Mobile,
		T4M_2_Textures_Bumped_SPEC,
		T4M_3_Textures_Bumped_SPEC,
		T4M_2_Textures_Bumped_DirectionalLM_Mobile,
		T4M_2_Textures_Bumped_DirectionalLM,
		T4M_3_Textures_Bumped_DirectionalLM
	}
	
	enum EnumShaderGLES3{
		T4M_2_Textures_Diffuse,
		T4M_3_Textures_Diffuse,
		T4M_4_Textures_Diffuse,
		T4M_5_Textures_Diffuse,
		T4M_6_Textures_Diffuse,
		T4M_2_Textures_Specular,
		T4M_3_Textures_Specular,
		T4M_4_Textures_Specular,
		T4M_2_Textures_Bumped,
		T4M_3_Textures_Bumped,
		T4M_4_Textures_Bumped,
		T4M_2_Textures_Bumped_SPEC,
		T4M_3_Textures_Bumped_SPEC,
		T4M_4_Textures_Bumped_SPEC
	}
	
	enum LODShaderStatus{
		New,
		AlreadyExist
	}
	
	string[] EnumMyT4M = {"T4M Settings", "ATS Mobile Foliage"};
	
	enum MaterialType{
		Classic,
		Substances
	}
	LODShaderStatus ShaderLOD1S = LODShaderStatus.New;
	LODShaderStatus ShaderLOD2S = LODShaderStatus.New;
	LODShaderStatus ShaderLOD3S = LODShaderStatus.New;
	LODShaderStatus OldShaderLOD1S;
	LODShaderStatus OldShaderLOD2S;
	LODShaderStatus OldShaderLOD3S;
	static public string T4MActived = "Activated";
	string terrainName = "";
	string[] MyT4MMen = {"Painter", "Materials"};
	string[] LODMenu = {"LOD Manager", "LOD Composer"};
	string[] BillMenu = {"Billboard Manager", "Billboard Creator"};
	string PrefabName = "Name";
	string CheckStatus;
		
	int tCount;
	int counter;
    int totalCount;
	int X;
	int Y;
	int T4MResolution = 90;
	float tRes =4.1f;
	float HeightmapWidth = 0;
	float HeightmapHeight = 0;
	TerrainData terrainDat;
	float progressUpdateInterval = 10000;
	int vertexInfo;
	int trisInfo;
	int LODM = 0;
	int BillM;
	int MyT4MV = 0;
	int EnumMyT4MV = 0;
	float MaximunView = 60.0f;
	float StartLOD2 = 20.0f;
	float StartLOD3 = 40.0f;
	float UpdateInterval = 1f;
	int selBrush =0;
	int selProcedural = 0;
	static public int brushSize  = 16;
	int oldSelBrush;
	int layerMask = 1 << 30;
	int oldBrushSizeInPourcent;
	int oldselTexture;	
	static public int T4MPlantSel =0;
	static public float T4MObjSize=1;
	 
	T4MObjSC[] T4MObjCounter;
	
	Texture Layer1;
	Texture Layer2;
	Texture Layer3;
	Texture Layer4;
	Texture Layer5;
	Texture Layer6;
	Texture LMMan;
	Texture Layer1Bump;
	Texture Layer2Bump;
	Texture Layer3Bump;
	Texture Layer4Bump;
	Texture LOD1T;
	Texture LOD1B;
	Texture LOD3T;
	Texture LOD3B;
	Texture LOD2T;
	Texture LOD2B ;
	Texture[] TexBrush;
	Texture[] TexTexture;
	Texture[] TexObject = new Texture[6];
	static public GameObject[] T4MObjectPlant = new GameObject[6];
	static public bool[] T4MBoolObj= new bool[6];
	Mesh LOD1;
	Mesh LOD2;
	Mesh LOD3;
	
	bool ActivatedBillboard=true;
	bool ActivatedLOD =true;
	bool CheckLOD1Collider;
	bool CheckLOD2Collider;
	bool CheckLOD3Collider;
	bool joinTiles = true;
	bool intialized=false;
	
	Vector2 Layer1Tile;
	Vector2 Layer2Tile;
	Vector2 Layer3Tile;
	Vector2 Layer4Tile;
	Vector2 Layer5Tile;
	Vector2 Layer6Tile;
	Vector2 scrollPos;
	
	GameObject  Child;
	GameObject UnityTerrain;
	GameObject AddObject;
	Transform PlayerCam;
	ProceduralMaterial PreceduralAdd;
	ProceduralMaterial Precedural;
	
	Shader LOD1S;
	Shader LOD2S;
	Shader LOD3S;
	
	Material LOD1Material;
	Material LOD2Material;
	Material LOD3Material;
	TerrainData terrain;
	
	EnumShaderGLES2 MenuTextureSM2 = EnumShaderGLES2.T4M_4_Textures_HighSpec;
	EnumShaderGLES1 MenuTextureSM1 = EnumShaderGLES1.T4M_2_Textures_Auto_BeastLM_2DrawCall;
	EnumShaderGLES3 MenuTextureSM3 = EnumShaderGLES3.T4M_4_Textures_Diffuse;
	SM ShaderModel = SM.ShaderModel2;
	MaterialType MaterialTyp = MaterialType.Classic;
	static public PlantMode T4MPlantMod = PlantMode.Classic;
	
	static public int T4MBrushSizeInPourcent;
	static public Color T4MtargetColor;
	static public Color T4MtargetColor2;
	static public Texture2D T4MMaskTex2;
	static public Texture2D T4MMaskTex;
	static public float[] T4MBrushAlpha;
	static public float T4MStronger = 0.5f;
	static public Projector T4MPreview;
	static int T4MSelectID;
	static public int T4MselTexture =0;
	static public Projector T4MProjectorPlt;
	
	string T4MEditorFolder = "Assets/T4M/Editor/";
	string T4MFolder = "Assets/T4M/";
	string T4MPrefabFolder = "Assets/T4MOBJ/";
	string FinalExpName;
	static public float T4MMaskTexUVCoord =1f;
	
	Shader CustomShader;
	float shiness0;
	float shiness1;
	float shiness2;
	float shiness3;
	Color ShinessColor;
	Texture MaterialAdd;
	static public float T4MDistanceMax = 15.0f;
	static public float T4MDistanceMin = 15.0f;
	static public float T4MrandX =0.0f;
	static public float T4MrandY =1.0f;
	static public float T4MrandZ =0.0f;
	static public bool T4MRandomRot = true;
	static public bool T4MRandomSpa;
	static public float T4MYOrigin =0.02f;
	static public float T4MSizeVar;
	static public string T4MGroupName = "Group1";
	static public bool T4MCreateColl;
	static public bool T4MStaticObj = true;
	static public int T4MselectObj;
	LODMod LODModeControler = LODMod.Mass_Control;
	bool NewPref=false;
	bool ActivatedLayerCul = true;
	float CloseDistMaxView = 30f;
	float NormalDistMaxView = 60f;	
	float FarDistMaxView = 200f;
	float BGDistMaxView = 10000f;
	float BillboardDist = 30f;
	float BillInterval = 0.1f;
	Mesh BillMesh;
	bool T4MMaster = true;
	static int nbrT4MObj;
	bool initMaster;
	int partofT4MObj=0;
	static public bool billActivate = false;
	static public bool LodActivate = false;
	bool keepTexture =false;
	static bool oldActivBillb;
	static bool oldActivLOD;
	public enum PaintHandle{
		Classic,
		Follow_Normal_Circle,
		Follow_Normal_WireCircle,
		Hide_preview
	}

    static public PaintHandle PaintPrev = PaintHandle.Classic;

    int OptimizeLevel =0;
	Vector4 UpSideTile = new Vector4(0.5f,0.5f,0.5f,0.5f);
	float UpSideF = 2.5f;
	float BlendFac = 4;
	
	Renderer[] NbrPartObj;
	
	
	void OnDestroy() 
	{
		T4MMenuToolbar = 0;
      	terrainDat = null;
		vertexInfo = 0;
		partofT4MObj=0;
		trisInfo =0;
		TexTexture = null;
		T4MSelectID = 0;
		Projector[] projectorObj = FindObjectsOfType(typeof(Projector)) as Projector[];
		foreach(Projector go in projectorObj)
			{
				if(go.hideFlags == HideFlags.HideInHierarchy || go.name == "previewT4M")
				{
					go.hideFlags=0;
					DestroyImmediate (go.gameObject);
				}
			}
    }
	
	[MenuItem ("Window/T4M Source Codes %t")]
	static void Initialize () 
	{
		T4MSC window = (T4MSC) EditorWindow.GetWindowWithRect(typeof (T4MSC),new Rect(0,0,386,582),false,"T4M SC");
		window.Show();
	}
	
	void OnInspectorUpdate() 
	{
        Repaint();
    }
	
	void OnGUI () 
	{
		CurrentSelect= Selection.activeTransform;
		nbrT4MObj = 0;
		T4MObjCounter = GameObject.FindObjectsOfType(typeof(T4MObjSC)) as T4MObjSC[];
		for (int i = 0; i < T4MObjCounter.Length; i++) {
			if (T4MObjCounter[i].Master ==1)
				nbrT4MObj =+1;
		}
		
		MenuIcon[0] = new GUIContent(AssetDatabase.LoadAssetAtPath(T4MEditorFolder+"Icons/conv.png", typeof(Texture2D))as Texture);
		MenuIcon[1] = new GUIContent(AssetDatabase.LoadAssetAtPath(T4MEditorFolder+"Icons/optimize.png", typeof(Texture2D))as Texture);
		MenuIcon[2] = new GUIContent(AssetDatabase.LoadAssetAtPath(T4MEditorFolder+"Icons/myt4m.png", typeof(Texture2D))as Texture);
		MenuIcon[3] = new GUIContent(AssetDatabase.LoadAssetAtPath(T4MEditorFolder+"Icons/paint.png", typeof(Texture2D))as Texture);
		MenuIcon[4] = new GUIContent(AssetDatabase.LoadAssetAtPath(T4MEditorFolder+"Icons/plant.png", typeof(Texture2D))as Texture);
		MenuIcon[5] = new GUIContent(AssetDatabase.LoadAssetAtPath(T4MEditorFolder+"Icons/lod.png", typeof(Texture2D))as Texture);
		MenuIcon[6] = new GUIContent(AssetDatabase.LoadAssetAtPath(T4MEditorFolder+"Icons/bill.png", typeof(Texture2D))as Texture);
		
		if (CurrentSelect && Selection.activeInstanceID != T4MSelectID || UnityTerrain && T4MMenuToolbar != 0 || T4MMenuToolbar != 3 && T4MPreview){
			IniNewSelect();
		}
			GUILayout.BeginHorizontal();
			GUILayout.BeginArea (new Rect (0,0,90,585));
					GUILayout.Label(AssetDatabase.LoadAssetAtPath(T4MEditorFolder+"Img/T4MBAN.jpg", typeof(Texture2D)) as Texture2D,GUILayout.Width(24),GUILayout.Height (582));
				GUILayout.EndArea ();
			GUILayout.BeginArea (new Rect (25,0,363,585));
				EditorGUILayout.Space();
				GUILayout.BeginHorizontal("box");
				T4MMenuToolbar = (int) GUILayout.Toolbar(T4MMenuToolbar, MenuIcon, "gridlist", GUILayout.Width(172), GUILayout.Height(18));
				GUILayout.FlexibleSpace();
				
				GUILayout.Label("Controls",GUILayout.Width(52));
				if(GUILayout.Button (T4MActived,GUILayout.Width(80))) {
					if (T4MActived == "Activated"){
						T4MActived ="Deactivated";
					}else{
						T4MActived = "Activated";
					}
			}
			GUILayout.EndHorizontal();
				GUILayout.Label(AssetDatabase.LoadAssetAtPath(T4MEditorFolder+"Img/separator.png", typeof(Texture))as Texture);
		
				if(CurrentSelect != null && T4MActived =="Activated"){
					if(CurrentSelect.GetComponent <T4MPartSC>()){
						Selection.activeTransform = CurrentSelect.parent;
					}
					
					Renderer[] rendererPart = CurrentSelect.GetComponentsInChildren<Renderer>();
			
					
					if (CurrentSelect.GetComponent <T4MObjSC>() && (!CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial || !CurrentSelect.gameObject.GetComponent<T4MObjSC>().T4MMesh)){
						
						if (rendererPart.Length==0){
							CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial = CurrentSelect.GetComponent<Renderer>().sharedMaterial;
							CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMesh = CurrentSelect.gameObject.GetComponent<MeshFilter>();
					
						}else {
							for (int i=0;i<rendererPart.Length;i++){
								CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial = rendererPart[0].sharedMaterial;
								CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMesh = rendererPart[0].gameObject.GetComponent<MeshFilter>();
							}
						}
					}else if (CurrentSelect.GetComponent <T4MObjSC>() && CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial){
						if(nbrT4MObj ==1  && CurrentSelect.GetComponent <T4MObjSC>().Master != 1)
							T4MMaster =false;
						else if (nbrT4MObj ==1 && CurrentSelect.GetComponent <T4MObjSC>().Master == 1 && T4MMaster ==false && initMaster==false){
							T4MMaster =true;
							initMaster = true; 
						}
						if (rendererPart.Length==0){
							if(CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial != CurrentSelect.GetComponent<Renderer>().sharedMaterial)
								CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial = CurrentSelect.GetComponent<Renderer>().sharedMaterial;
							EditorUtility.SetSelectedWireframeHidden(CurrentSelect.GetComponent<Renderer>(), true);
						}else {
							if(CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial != rendererPart[0].sharedMaterial){
								CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial = rendererPart[0].sharedMaterial;
							}
							for (int i=0;i<rendererPart.Length;i++){
								if(rendererPart[i].sharedMaterial != rendererPart[0].sharedMaterial){
									rendererPart[i].sharedMaterial = rendererPart[0].sharedMaterial;
								}
								EditorUtility.SetSelectedWireframeHidden(rendererPart[i], true);
							}
						}
					}
					if (CurrentSelect && !CurrentSelect.GetComponent <T4MObjSC>()){
						int countchild = CurrentSelect.transform.childCount;
						if (countchild>0){
							NbrPartObj = CurrentSelect.transform.GetComponentsInChildren<Renderer>();
						}
					}
					
					switch (T4MMenuToolbar)
					{
						case 0:
						ConverterMenu();
						break;
						
						case 1:
						Optimize();
						break;
				
						case 2:
						MyT4M();
						break;
						
						case 3:
						PainterMenu();
						break;
						
						case 4:
						Planting ();
						break;
						
						case 5:
						afLOD();						
						break;
				
						case 6:
						BillboardMenu();
						break;
				
					}
					if (oldActivBillb != billActivate){
							oldActivBillb = billActivate;
						 if (billActivate ==false)
							DeactivateBillBByScript();
					}
					if(oldActivLOD != LodActivate){
						oldActivLOD = LodActivate;
						if (LodActivate ==false)
							DeactivateLODByScript();	
					}
				}else{
					
						
					if (CurrentSelect && CurrentSelect.GetComponent <T4MObjSC>() && CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial){
						Renderer[] rendererPart = CurrentSelect.GetComponentsInChildren<Renderer>();
						if (rendererPart.Length==0){
							EditorUtility.SetSelectedWireframeHidden(CurrentSelect.GetComponent<Renderer>(), false);
						}else {
							for (int i=0;i<rendererPart.Length;i++){
								EditorUtility.SetSelectedWireframeHidden(rendererPart[i], false);
							}
						}
				}
				
					GUILayout.FlexibleSpace();
					GUILayout.BeginHorizontal();
					GUILayout.FlexibleSpace();
					GUILayout.Label(AssetDatabase.LoadAssetAtPath(T4MEditorFolder+"Img/waiting.png", typeof(Texture)) as Texture);
					GUILayout.FlexibleSpace();
					GUILayout.EndHorizontal();
					GUILayout.FlexibleSpace();
				
			}
		GUILayout.EndArea ();
		GUILayout.EndHorizontal();
		
		
	}
	
	void Optimize(){
		if (CurrentSelect.GetComponent <T4MObjSC>())
		{
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		GUILayout.Label("Optimization of Load Time", EditorStyles.boldLabel);
		OptimizeLevel = (int)EditorGUILayout.Slider("Level",OptimizeLevel,0,3);
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		GUILayout.Label("Load Time", EditorStyles.boldLabel);
		if (OptimizeLevel == 0)
			GUILayout.Label("Good");
		else if (OptimizeLevel == 1)
			GUILayout.Label("Very Good");
		else if (OptimizeLevel == 2)
			GUILayout.Label("Impressive");
		else if (OptimizeLevel == 3)
			GUILayout.Label("Amazing");
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		GUILayout.Label("Type", EditorStyles.boldLabel);
		if (OptimizeLevel == 0)
			GUILayout.Label("Optimize Mesh");
		else if (OptimizeLevel == 1)
			GUILayout.Label("Optimize Mesh + Low Compression");
		else if (OptimizeLevel == 2)
			GUILayout.Label("Optimize Mesh + Medium Compression");
		else if (OptimizeLevel == 3)
			GUILayout.Label("Optimize Mesh + High Compression");
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		GUILayout.Label("Mesh Impact", EditorStyles.boldLabel);
		if (OptimizeLevel == 0)
			GUILayout.Label("Nothing");
		else if (OptimizeLevel == 1)
			GUILayout.Label("Low Degradation");
		else if (OptimizeLevel == 2)
			GUILayout.Label("Medium Degradation");
		else if (OptimizeLevel == 3)
			GUILayout.Label("High Degradation");
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		
				GUILayout.BeginHorizontal();
				GUILayout.FlexibleSpace();
				GUILayout.Label("Can Take Some Time", EditorStyles.boldLabel);
				GUILayout.FlexibleSpace();
				GUILayout.EndHorizontal();	
		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
			
		if (GUILayout.Button("Process", GUILayout.Width(100), GUILayout.Height(30))) 
		{
			string AssetName="";
			int countchild = CurrentSelect.transform.childCount;
			if (countchild>0){
				MeshFilter[] T4MOBJPART = CurrentSelect.GetComponentsInChildren<MeshFilter>();
				AssetName= AssetDatabase.GetAssetPath(T4MOBJPART[0].sharedMesh) as string;
				Debug.Log(AssetName);
				
			}else{
				MeshFilter T4MOBJM = CurrentSelect.GetComponent<MeshFilter>();		
				AssetName= AssetDatabase.GetAssetPath(T4MOBJM.sharedMesh) as string;	
			}
			
			ModelImporter OBJI= ModelImporter.GetAtPath (AssetName) as ModelImporter;
			if (OptimizeLevel == 0){
				OBJI.optimizeMesh = true;
			}else if (OptimizeLevel == 1){
				OBJI.optimizeMesh = true;	
				OBJI.meshCompression = ModelImporterMeshCompression.Low;
			}else if (OptimizeLevel == 2){
				OBJI.optimizeMesh = true;	
				OBJI.meshCompression = ModelImporterMeshCompression.Medium;	
			}else if (OptimizeLevel == 3){
				OBJI.optimizeMesh = true;	
				OBJI.meshCompression = ModelImporterMeshCompression.High;	
			}
			AssetDatabase.ImportAsset(AssetName, ImportAssetOptions.ForceUpdate);	
			PrefabUtility.RevertPrefabInstance(CurrentSelect.gameObject);
			AssetName="";
		}
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		}
		else
		{
			GUILayout.BeginHorizontal();
					GUILayout.FlexibleSpace();
					GUILayout.Label("Please, select the T4M Object", EditorStyles.boldLabel);
					GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
		}
		
	}
	
	
	
	void Repair(){
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		GUILayout.Label("This T4M Object is Broken", EditorStyles.boldLabel);
		EditorGUILayout.Space();
		GUILayout.Label("Clean It and Reconvert it ?");
		GUILayout.FlexibleSpace();
		if (GUILayout.Button("CLEANING", GUILayout.Width(100), GUILayout.Height(30))) 
		{
			T4MObjSC ToSuppress = CurrentSelect.GetComponent <T4MObjSC>();
			DestroyImmediate (ToSuppress);
			T4MMenuToolbar = 0;
		}
		GUILayout.FlexibleSpace();
	}
	
	void BillboardMenu(){
		
		
		if (CurrentSelect.GetComponent <T4MObjSC>())
			{
			if (CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial && CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.HasProperty("_Splat0") && CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.HasProperty("_Splat1")&& CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.HasProperty("_Control")){
			
			GUILayout.BeginHorizontal();
					GUILayout.FlexibleSpace();	
					BillM = GUILayout.Toolbar(BillM, BillMenu, GUILayout.Width(290), GUILayout.Height(20));
					GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			switch (BillM)
			{
				case 0:
				if (T4MMaster){
					Billboard();
				}else{
					GUILayout.BeginHorizontal();
					GUILayout.FlexibleSpace();
					GUILayout.Label("Need to be an Master T4M", EditorStyles.boldLabel);
					GUILayout.FlexibleSpace();
					GUILayout.EndHorizontal();
				}
				break;
				case 1:
				BillboardCreator();
				break;
			}}
		}else
		{
			GUILayout.BeginHorizontal();
					GUILayout.FlexibleSpace();
					GUILayout.Label("Please, select the T4M Object", EditorStyles.boldLabel);
					GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
		}
		
	}
	
	void BillboardCreator(){
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		
		GUILayout.BeginHorizontal("box");
			GUILayout.Label("BillBoard Prefab Name", EditorStyles.boldLabel);
				PrefabName=GUILayout.TextField (PrefabName, 25, GUILayout.Width(155));
		GUILayout.EndHorizontal();
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		CreationBB =(CreaType) EditorGUILayout.EnumPopup ("New Billboard Type", CreationBB, GUILayout.Width(340));
		
		if (CreationBB == CreaType.Custom){
			GUILayout.Label("Billboard Meshes", EditorStyles.boldLabel);
			BillMesh= (Mesh)EditorGUILayout.ObjectField("Mesh",  BillMesh, typeof(Mesh),false);
		}
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		GUILayout.BeginHorizontal();
		
			GUILayout.Label("BillBoard Setup", EditorStyles.boldLabel, GUILayout.Width(223));
			GUILayout.Label("MainTex", EditorStyles.boldLabel, GUILayout.Width(68));
			GUILayout.Label("Bump", EditorStyles.boldLabel, GUILayout.Width(60));
		GUILayout.EndHorizontal();
		EditorGUILayout.Space();
		GUILayout.BeginHorizontal();
			GUILayout.BeginVertical();
			GUILayout.BeginHorizontal();
				GUILayout.Label("BillBoard Shader", GUILayout.Width(103));
				ShaderLOD1S =(LODShaderStatus) EditorGUILayout.EnumPopup (ShaderLOD1S, GUILayout.Width(95));
			GUILayout.EndHorizontal();
		EditorGUILayout.Space();
				if (ShaderLOD1S == LODShaderStatus.New)
					LOD1S= (Shader)EditorGUILayout.ObjectField(LOD1S, typeof(Shader),true, GUILayout.MaxWidth(220));
				else LOD1Material= (Material)EditorGUILayout.ObjectField(LOD1Material, typeof(Material),false, GUILayout.MaxWidth(220));
			GUILayout.EndVertical();
				if (LOD1S)
					LOD1Material =  new Material (Shader.Find (LOD1S.name));
				
					if (LOD1Material && LOD1Material.HasProperty("_MainTex")){
						if (LOD1Material.GetTexture("_MainTex"))
							LOD1T = LOD1Material.GetTexture("_MainTex");
						LOD1T=EditorGUILayout.ObjectField(LOD1T, typeof(Texture),false, GUILayout.Width(60), GUILayout.Height(60)) as Texture;
						if (LOD1Material && LOD1Material.HasProperty("_BumpMap")){
							if (LOD1Material.GetTexture("_BumpMap"))
								LOD1B = LOD1Material.GetTexture("_BumpMap");
							LOD1B=EditorGUILayout.ObjectField(LOD1B, typeof(Texture),false, GUILayout.Width(60), GUILayout.Height(60)) as Texture;
						}
					} 
				
				GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		GUILayout.FlexibleSpace();
			GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
					if (GUILayout.Button("CONSTRUCT", GUILayout.Width(100), GUILayout.Height(30))) {
						if (PrefabName != "" &&  LOD1Material)
							CreatePrefabBB();
						else  EditorUtility.DisplayDialog("T4M Message", "You must complete the formulary before make the construct", "OK");
					}
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
				GUILayout.FlexibleSpace();
		
	}
	void CreatePrefabBB(){
			if (!System.IO.Directory.Exists(T4MPrefabFolder+"BillBObjects/"))
            {
                System.IO.Directory.CreateDirectory(T4MPrefabFolder+"BillBObjects/");
            } 
			 if (!System.IO.Directory.Exists(T4MPrefabFolder+"BillBObjects/Material/"))
            {
                System.IO.Directory.CreateDirectory(T4MPrefabFolder+"BillBObjects/Material/");
            } 
			AssetDatabase.Refresh(); 	 
			GameObject LOD1Temp;
			LOD1Temp = new GameObject (PrefabName);
			LOD1Temp.AddComponent<MeshFilter> ();
			LOD1Temp.AddComponent<MeshRenderer> ();
			
			if (CreationBB == CreaType.Custom){
				LOD1Temp.GetComponent<MeshFilter>().mesh = BillMesh;
			}else{
				GameObject Temp = (GameObject)AssetDatabase.LoadAssetAtPath(T4MEditorFolder+"MeshBillb/Billboard.fbx", typeof(GameObject));
				Mesh Test2 = Temp.GetComponent<MeshFilter>().sharedMesh;
				LOD1Temp.GetComponent<MeshFilter>().mesh = Test2;
			}
			LOD1Temp.AddComponent<T4MBillBObjSC>();
			LOD1Temp.GetComponent<Renderer>().sharedMaterial = LOD1Material;
			LOD1Temp.GetComponent<Renderer>().sharedMaterial.SetTexture ("_MainTex", LOD1T);
			LOD1Temp.GetComponent <T4MBillBObjSC>().Render = LOD1Temp.GetComponent<Renderer>();
			LOD1Temp.GetComponent <T4MBillBObjSC>().Transf = LOD1Temp.transform;
		
			bool ExportSuccess = false;
			int num =1;
			UnityEngine.Object BasePrefab;
			string Next;
			do { 
				Next=PrefabName+num;
				
				if (!System.IO.File.Exists(T4MPrefabFolder+"BillBObjects/"+PrefabName+".prefab"))
				{
					if (ShaderLOD1S == LODShaderStatus.New)
						 AssetDatabase.CreateAsset(LOD1Material, T4MPrefabFolder+"BillBObjects/Material/"+PrefabName+".mat");
					
					BasePrefab = PrefabUtility.CreateEmptyPrefab(T4MPrefabFolder+"BillBObjects/"+PrefabName+".prefab");
					PrefabUtility.ReplacePrefab(LOD1Temp, BasePrefab);
					ExportSuccess = true;
				}
				else if (!System.IO.File.Exists(T4MPrefabFolder+"BillBObjects/"+Next+".prefab"))
				{
					if (ShaderLOD1S == LODShaderStatus.New)
						AssetDatabase.CreateAsset(LOD1Material, T4MPrefabFolder+"BillBObjects/Material/"+Next+".mat");
					BasePrefab= PrefabUtility.CreateEmptyPrefab(T4MPrefabFolder+"BillBObjects/"+Next+".prefab");
					PrefabUtility.ReplacePrefab(LOD1Temp, BasePrefab);
					ExportSuccess = true;
				}
				num++;
			}while (!ExportSuccess);
				AssetDatabase.SaveAssets();
				AssetDatabase.Refresh();
				
		
			DestroyImmediate(LOD1Temp);
			EditorUtility.DisplayDialog("T4M Message", "Construction Completed", "OK");
	}
	
	void Billboard(){
		
			EditorGUILayout.Space();
			EditorGUILayout.Space();
			GUILayout.Label("Culling BillBoard Mode", EditorStyles.boldLabel);
			BilBocclusion = (OccludeBy) EditorGUILayout.EnumPopup ("Mode", BilBocclusion, GUILayout.Width(340));
			
			EditorGUILayout.Space();
			EditorGUILayout.Space();
		
		
			GUILayout.Label("BillBoard Rotation Axis", EditorStyles.boldLabel);
			BillBoardAxis =(BillbAxe) EditorGUILayout.EnumPopup ("Axis", BillBoardAxis, GUILayout.Width(340));
			EditorGUILayout.Space();
			EditorGUILayout.Space();
			
			GUILayout.Label("BillBoard Update Interval in Seconde", EditorStyles.boldLabel, GUILayout.Width(400));
				GUILayout.BeginHorizontal();
				GUILayout.Label("(less value = less performance)", GUILayout.Width(300));
				BillInterval = EditorGUILayout.FloatField(BillInterval, GUILayout.Width(50));
			GUILayout.EndHorizontal();
			EditorGUILayout.Space();
			EditorGUILayout.Space();
			GUILayout.BeginHorizontal();
					GUILayout.Label("Maximum View Distance", EditorStyles.boldLabel, GUILayout.Width(298));
					BillboardDist= EditorGUILayout.FloatField(BillboardDist, GUILayout.Width(50));
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
				GUILayout.FlexibleSpace();
				BillboardDist=GUILayout.HorizontalScrollbar (BillboardDist,0.0f,0f,200f,GUILayout.Width(350));
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			GUILayout.FlexibleSpace();
			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
		
			Texture Swap;
			string buttonBill;
			if (billActivate == true){
				buttonBill = "DEACTIVATE";	
				Swap= (Texture)AssetDatabase.LoadAssetAtPath(T4MEditorFolder+"Img/on.png", typeof(Texture));	
			}else{ 
				buttonBill = "ACTIVATE";
				Swap= (Texture)AssetDatabase.LoadAssetAtPath(T4MEditorFolder+"Img/off.png", typeof(Texture));
			}
	
			
			if (GUILayout.Button(buttonBill, GUILayout.Width(100), GUILayout.Height(30))) 
					{
						ActivateDeactivateBillBoard();
					}
				GUILayout.Label(Swap, GUILayout.Width(75), GUILayout.Height(30));
					GUILayout.FlexibleSpace();
				GUILayout.EndHorizontal();
			GUILayout.FlexibleSpace();
	
	}
	
	void DeactivateBillBByScript(){
		T4MBillBObjSC[] T4MBillObjGet =  GameObject.FindObjectsOfType(typeof(T4MBillBObjSC)) as T4MBillBObjSC[];
						
		for(var i=0;i<T4MBillObjGet.Length;i++){
			T4MBillObjGet[i].Render.enabled = true;
		T4MBillObjGet[i].Transf.LookAt(Vector3.zero , Vector3.up);
		}	
		
		CurrentSelect.gameObject.GetComponent <T4MObjSC>().BillboardPosition= new Vector3[0];
		CurrentSelect.gameObject.GetComponent <T4MObjSC>().BillStatus=new int[0];
		CurrentSelect.gameObject.GetComponent <T4MObjSC>().BillScript= new T4MBillBObjSC[0];
		PrefabUtility.RecordPrefabInstancePropertyModifications(CurrentSelect.gameObject.GetComponent<T4MObjSC>());
		Debug.LogWarning ("The Number of Activated Billboard Objects has changed, reactivate the billboards in the 'Billboard' Tab.");
	}
	
	void ActivateDeactivateBillBoard(){
			if (billActivate == true){ //si le billboard est actif
					T4MBillBObjSC[] T4MBillObjGet =  GameObject.FindObjectsOfType(typeof(T4MBillBObjSC)) as T4MBillBObjSC[];
									
					for(var i=0;i<T4MBillObjGet.Length;i++){
						T4MBillObjGet[i].Render.enabled = true;
					T4MBillObjGet[i].Transf.LookAt(Vector3.zero , Vector3.up);
					}	
					
					CurrentSelect.gameObject.GetComponent <T4MObjSC>().BillboardPosition= new Vector3[0];
					CurrentSelect.gameObject.GetComponent <T4MObjSC>().BillStatus=new int[0];
					CurrentSelect.gameObject.GetComponent <T4MObjSC>().BillScript= new T4MBillBObjSC[0];
					PrefabUtility.RecordPrefabInstancePropertyModifications(CurrentSelect.gameObject.GetComponent<T4MObjSC>());
					billActivate = false;
					Debug.LogWarning ("Billboard deactivated !");
			}else{
					if (!PlayerCam)
						PlayerCam=Camera.main.transform;
			
					T4MBillBObjSC[] T4MBillObjGet =  GameObject.FindObjectsOfType(typeof(T4MBillBObjSC)) as T4MBillBObjSC[];
					Vector3[] T4MBillVectGetR = new Vector3[T4MBillObjGet.Length]; 
					int[] T4MBillValueGetR = new int[T4MBillObjGet.Length]; 
					
						for (var j =0; j <T4MBillObjGet.Length;j++){
							T4MBillVectGetR[j]=T4MBillObjGet[j].transform.position;
							if(Vector3.Distance(T4MBillObjGet[j].transform.position, PlayerCam.transform.position) <= BillboardDist){
								T4MBillObjGet[j].Render.enabled = true;
								T4MBillValueGetR[j] = 1;
					
							}else {
								if (BilBocclusion == OccludeBy.Max_View_Distance){								
									T4MBillObjGet[j].Render.enabled = false;
									T4MBillValueGetR[j] = 0;
								}else{
									T4MBillObjGet[j].Render.enabled = true;
									T4MBillValueGetR[j] = 1;
								}
							}
							if (BillBoardAxis == BillbAxe.Y_Axis)
								T4MBillObjGet[j].Transf.LookAt(new Vector3 (PlayerCam.transform.position.x,T4MBillObjGet[j].Transf.position.y,PlayerCam.transform.position.z) , Vector3.up);	
							else	
								T4MBillObjGet[j].Transf.LookAt(PlayerCam.transform.position, Vector3.up);
							
						}
					if (BilBocclusion == OccludeBy.Max_View_Distance)
						CurrentSelect.GetComponent <T4MObjSC>().BilBbasedOnScript= true;
					else CurrentSelect.GetComponent <T4MObjSC>().BilBbasedOnScript= false;
		
					CurrentSelect.gameObject.GetComponent <T4MObjSC>().BillboardPosition= T4MBillVectGetR;
					CurrentSelect.gameObject.GetComponent <T4MObjSC>().BillStatus = T4MBillValueGetR;
					CurrentSelect.gameObject.GetComponent <T4MObjSC>().BillScript= T4MBillObjGet;
					CurrentSelect.gameObject.GetComponent <T4MObjSC>().BillMaxViewDistance= BillboardDist;
					CurrentSelect.gameObject.GetComponent <T4MObjSC>().BillInterval= BillInterval;
					if (BillBoardAxis == BillbAxe.Y_Axis)
						CurrentSelect.gameObject.GetComponent <T4MObjSC>().Axis= 0;
					else CurrentSelect.gameObject.GetComponent <T4MObjSC>().Axis= 1;
					PrefabUtility.RecordPrefabInstancePropertyModifications(CurrentSelect.gameObject.GetComponent<T4MObjSC>());
					
					billActivate = true;
					Debug.LogWarning ("Billboard (re)activated !");
			}	
	}
	
	void Planting (){
		if (CurrentSelect.GetComponent <T4MObjSC>())
			{
			if (CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial && CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.HasProperty("_Splat0") && CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.HasProperty("_Splat1")&& CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.HasProperty("_Control")){	
		if(T4MRandomRot && T4MPlantMod == PlantMode.Follow_Normals)
			T4MRandomRot=false;
		
		GUILayout.BeginVertical("box");
		GUILayout.BeginHorizontal();
			GUILayout.Label("Add to List", GUILayout.Width(105));
			GUILayout.FlexibleSpace();
			AddObject = (GameObject)EditorGUILayout.ObjectField("",AddObject, typeof(GameObject),true,GUILayout.Width(190));
		GUILayout.EndHorizontal();
		EditorGUILayout.BeginHorizontal();
		
		GUILayout.Label("",GUILayout.Width(1));
		if(GUILayout.Button ((Texture)AssetDatabase.LoadAssetAtPath(T4MEditorFolder+"Img/down.png", typeof(Texture)),GUILayout.Width(53))) {
			if (AddObject)	
				T4MObjectPlant[0] = AddObject;
			else {
				T4MObjectPlant[0] = null;
			}
			AddObject = null;
		}
		
		if(GUILayout.Button ((Texture)AssetDatabase.LoadAssetAtPath(T4MEditorFolder+"Img/down.png", typeof(Texture)),GUILayout.Width(53))) {
			if (AddObject)
				T4MObjectPlant[1] = AddObject;
			else {
				T4MObjectPlant[1] = null;
			}
			AddObject = null;
		}
		
		if(GUILayout.Button ((Texture)AssetDatabase.LoadAssetAtPath(T4MEditorFolder+"Img/down.png", typeof(Texture)),GUILayout.Width(53))) {
			if (AddObject)
				T4MObjectPlant[2] = AddObject;
			else {
				T4MObjectPlant[2] = null;
			}
			AddObject = null;
		}
	
		if(GUILayout.Button ((Texture)AssetDatabase.LoadAssetAtPath(T4MEditorFolder+"Img/down.png", typeof(Texture)),GUILayout.Width(53))) {
			if (AddObject)
				T4MObjectPlant[3] = AddObject;
			else {
				T4MObjectPlant[3] = null;
			}
			AddObject = null;
		}
	
		if(GUILayout.Button ((Texture)AssetDatabase.LoadAssetAtPath(T4MEditorFolder+"Img/down.png", typeof(Texture)),GUILayout.Width(53))) {
			if (AddObject)
				T4MObjectPlant[4] = AddObject;
			else {
				T4MObjectPlant[4] = null;
			}
			AddObject = null;
		}
	
		if(GUILayout.Button ((Texture)AssetDatabase.LoadAssetAtPath(T4MEditorFolder+"Img/down.png", typeof(Texture)),GUILayout.Width(53))) {
			if (AddObject)
				T4MObjectPlant[5] = AddObject;
			else {
				T4MObjectPlant[5] = null;
			}
			AddObject = null;
		}
				
		EditorGUILayout.EndHorizontal();
		
		GUILayout.EndVertical();
		if(T4MObjectPlant[0] != null)
			TexObject[0]=AssetPreview.GetAssetPreview(T4MObjectPlant[0])as Texture;
		else TexObject[0]=null;
		if(T4MObjectPlant[1] != null)	
			TexObject[1]=AssetPreview.GetAssetPreview(T4MObjectPlant[1])as Texture;
		else TexObject[1]=null;
		if(T4MObjectPlant[2] != null)
			TexObject[2]=AssetPreview.GetAssetPreview(T4MObjectPlant[2])as Texture;
		else TexObject[2]=null;
		if(T4MObjectPlant[3] != null)	
			TexObject[3]=AssetPreview.GetAssetPreview(T4MObjectPlant[3])as Texture;
		else TexObject[3]=null;
		if(T4MObjectPlant[4] != null)
			TexObject[4]=AssetPreview.GetAssetPreview(T4MObjectPlant[4])as Texture;
		else TexObject[4]=null;
		if(T4MObjectPlant[5] != null)
			TexObject[5]=AssetPreview.GetAssetPreview(T4MObjectPlant[5])as Texture;
		else TexObject[5]=null;
		 
		GUILayout.BeginHorizontal("box");
			GUILayout.FlexibleSpace();
				T4MPlantSel = GUILayout.SelectionGrid (T4MPlantSel, TexObject, 6 ,"gridlist",GUILayout.Width(340), GUILayout.Height(58));
			GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
			
		//LayerMask	
		GUILayout.BeginVertical("box");	
			GUILayout.BeginHorizontal();
				GUILayout.Label("",GUILayout.Width(22));
				T4MselectObj = 0;
				for (int i=0;i<T4MBoolObj.Length;i++){
					if (T4MObjectPlant[i]){	
						T4MBoolObj[i] = EditorGUILayout.Toggle(T4MBoolObj[i],GUILayout.Width(53));
						if (T4MBoolObj[i] == true)
							T4MselectObj +=1;
					}else GUILayout.Label("",GUILayout.Width(53));
				} 
			GUILayout.EndHorizontal();
				
			GUILayout.BeginHorizontal();	
				GUILayout.Label("",GUILayout.Width(2));
				for (int j=0;j<T4MObjectPlant.Length;j++){
					if (T4MObjectPlant[j]){	
						ViewDistance[j] =(ViewD) EditorGUILayout.EnumPopup (ViewDistance[j], GUILayout.Width(53));
					}else GUILayout.Label("",GUILayout.Width(53));
				}
			GUILayout.EndHorizontal();	 
		GUILayout.EndVertical();
			
			
		T4MPlantMod =(PlantMode) EditorGUILayout.EnumPopup ("Plant Mode", T4MPlantMod, GUILayout.Width(340));
		
		GUILayout.BeginVertical("box");
		GUILayout.Label("General", EditorStyles.boldLabel);
		EditorGUILayout.Space();
		T4MObjSize = EditorGUILayout.Slider("LocalSize",T4MObjSize,0.1f,4);	
		T4MSizeVar = EditorGUILayout.Slider("Size Var.(Random +/-)",T4MSizeVar,0,0.5f);
			
		T4MYOrigin = EditorGUILayout.Slider("Y Origin Corrector ",T4MYOrigin,-10,10);
		EditorGUILayout.Space();
		GUILayout.BeginHorizontal();
			GUILayout.Label("Create Collider", GUILayout.Width(120));
			T4MCreateColl = EditorGUILayout.Toggle(T4MCreateColl, GUILayout.Width(15));
		GUILayout.FlexibleSpace();
			GUILayout.Label("Static Object", GUILayout.Width(120));
			T4MStaticObj = EditorGUILayout.Toggle(T4MStaticObj, GUILayout.Width(30));
		GUILayout.EndHorizontal();
		EditorGUILayout.Space();
		GUILayout.BeginHorizontal();
			GUILayout.Label("Group Name", GUILayout.Width(150));
			T4MGroupName = GUILayout.TextField (T4MGroupName, 20, GUILayout.Width(120));
		GUILayout.EndHorizontal();
		EditorGUILayout.Space();
		GUILayout.EndVertical();
		EditorGUILayout.Space();
		GUILayout.BeginVertical("box");
			GUILayout.Label("Spacing Distances", EditorStyles.boldLabel, GUILayout.Width(150));
		EditorGUILayout.Space();
			GUILayout.BeginHorizontal();
				GUILayout.Label("Random Spacing", GUILayout.Width(150));
				T4MRandomSpa = EditorGUILayout.Toggle(T4MRandomSpa, GUILayout.Width(15));
			GUILayout.EndHorizontal();
			T4MDistanceMin = EditorGUILayout.Slider("Safe Zone",T4MDistanceMin,0.1f,50.0f);
			if(T4MRandomSpa)
			T4MDistanceMax = EditorGUILayout.Slider("Random Instance Zone",T4MDistanceMax,0.1f,50.0f);
		GUILayout.EndVertical();
		
		if (T4MDistanceMin > T4MDistanceMax)
			T4MDistanceMax= T4MDistanceMin;
		EditorGUILayout.Space();
		
		GUILayout.BeginVertical("box");
			GUILayout.BeginHorizontal();
				GUILayout.Label("Random Rotation(s)", EditorStyles.boldLabel, GUILayout.Width(150));
		
				T4MRandomRot = EditorGUILayout.Toggle(T4MRandomRot, GUILayout.Width(15));
			GUILayout.EndHorizontal();
			EditorGUILayout.Space();
			if(T4MRandomRot){
				T4MrandX = EditorGUILayout.Slider("Random X:",T4MrandX,0,1);
				T4MrandY = EditorGUILayout.Slider("Random Y:",T4MrandY,0,1);
				T4MrandZ = EditorGUILayout.Slider("Random Z:",T4MrandZ,0,1);			
			}
		GUILayout.EndVertical();
		}
		}else
			{
			GUILayout.BeginHorizontal();
					GUILayout.FlexibleSpace();
					GUILayout.Label("Please, select the T4M Object", EditorStyles.boldLabel);
					GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			
		}
		
	}
	

	void PixelPainterMenu()
	{
		if (CurrentSelect.GetComponent <T4MObjSC>())
			{
			if (CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial && CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.HasProperty("_Splat0") && CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.HasProperty("_Splat1")&& CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.HasProperty("_Control")){	
				IniBrush();
				InitPincil();
			if (!T4MPreview)
				InitPreview();
			if (intialized){	
				GUILayout.BeginHorizontal();
					GUILayout.FlexibleSpace();
						GUILayout.Label(AssetDatabase.LoadAssetAtPath(T4MEditorFolder+"Img/brushes.jpg", typeof(Texture)) as Texture,"label");
						GUILayout.BeginHorizontal("box", GUILayout.Width(318));
						GUILayout.FlexibleSpace();
						selBrush= GUILayout.SelectionGrid (selBrush, TexBrush, 9,"gridlist", GUILayout.Width(290), GUILayout.Height(70));
						GUILayout.FlexibleSpace();
						GUILayout.EndHorizontal();
					GUILayout.FlexibleSpace();
				GUILayout.EndHorizontal();
				
				GUILayout.BeginHorizontal();
					GUILayout.FlexibleSpace();
						GUILayout.BeginHorizontal("box", GUILayout.Width(340));
						GUILayout.FlexibleSpace();
						if (TexTexture.Length >4)
						T4MselTexture= GUILayout.SelectionGrid (T4MselTexture, TexTexture, 6 ,"gridlist",GUILayout.Width(340), GUILayout.Height(58));
						else
						T4MselTexture= GUILayout.SelectionGrid (T4MselTexture, TexTexture, 4 ,"gridlist",GUILayout.Width(340), GUILayout.Height(86));
						GUILayout.FlexibleSpace();
						GUILayout.EndHorizontal();
					GUILayout.FlexibleSpace();
				GUILayout.EndHorizontal();
				
				 EditorGUILayout.Space();
				 
					
				GUILayout.BeginHorizontal();
					GUILayout.FlexibleSpace();
					GUILayout.BeginVertical("box", GUILayout.Width(347));
						GUILayout.BeginHorizontal();
						GUILayout.Label("Preview Type", GUILayout.Width(145));
						PaintPrev = (PaintHandle) EditorGUILayout.EnumPopup (PaintPrev, GUILayout.Width(160));
						GUILayout.EndHorizontal();
						brushSize = (int)EditorGUILayout.Slider("Brush Size",brushSize,1,36);
						T4MStronger = EditorGUILayout.Slider("Brush Stronger",T4MStronger,0.05f,1f);
					GUILayout.EndVertical();
					GUILayout.FlexibleSpace();
				GUILayout.EndHorizontal();
				
				
				if (CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.HasProperty("_SpecColor")){
					EditorGUILayout.Space();
						GUILayout.BeginHorizontal();
						GUILayout.FlexibleSpace();
						GUILayout.BeginVertical("box", GUILayout.Width(347), GUILayout.Height(96));
							ShinessColor = EditorGUILayout.ColorField("Shininess Color", ShinessColor);
							CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.SetColor("_SpecColor",ShinessColor);
							EditorGUILayout.Space();
							if (CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.HasProperty("_ShininessL0")){
								 shiness0 = EditorGUILayout.Slider("Shininess Layer 1",shiness0,0.00f,1.0f);
								CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.SetFloat ("_ShininessL0",shiness0);
							}
							if (CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.HasProperty("_ShininessL1")){
								 shiness1 = EditorGUILayout.Slider("Shininess Layer 2",shiness1,0.00f,1.0f);
								CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.SetFloat ("_ShininessL1",shiness1);
							}
							if (CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.HasProperty("_ShininessL2")){
								 shiness2 = EditorGUILayout.Slider("Shininess Layer 3",shiness2,0.00f,1.0f);
								CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.SetFloat ("_ShininessL2",shiness2);
							}
							if (CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.HasProperty("_ShininessL3")){
								 shiness3 = EditorGUILayout.Slider("Shininess Layer 4",shiness3,0.00f,1.0f);
								CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.SetFloat ("_ShininessL3",shiness3);
							}
					
						GUILayout.EndVertical();
						GUILayout.FlexibleSpace();
					GUILayout.EndHorizontal();
					
				}
				EditorGUILayout.Space();
				
				
							
							
				GUILayout.BeginHorizontal();
					GUILayout.FlexibleSpace();
					if (CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.HasProperty("_SpecColor")){
						scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width (350), GUILayout.Height (140));
						GUILayout.BeginVertical("box", GUILayout.Width(320));	
					}else {
						GUILayout.BeginVertical("box", GUILayout.Width(320));
						if (TexTexture.Length >4)
							scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width (340), GUILayout.Height (275));
						else scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width (340), GUILayout.Height (240));
					}
					
					joinTiles = EditorGUILayout.Toggle ("Tiling : Join X/Y", joinTiles);
					EditorGUILayout.Space();
					if (joinTiles){
						Layer1Tile.x =Layer1Tile.y= EditorGUILayout.Slider("Layer1 Tiling :",Layer1Tile.x,1,500*T4MMaskTexUVCoord);
						CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.SetTextureScale ("_Splat0",new  Vector2(Layer1Tile.x,Layer1Tile.x));
						EditorGUILayout.Space();
						Layer2Tile.x = Layer2Tile.y =EditorGUILayout.Slider("Layer2 Tiling :",Layer2Tile.x,1,500*T4MMaskTexUVCoord);
						CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.SetTextureScale ("_Splat1",new  Vector2(Layer2Tile.x,Layer2Tile.x));
						EditorGUILayout.Space();
						if (CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.HasProperty("_Splat2")){
						Layer3Tile.x = Layer3Tile.y =EditorGUILayout.Slider("Layer3 Tiling :",Layer3Tile.x,1,500*T4MMaskTexUVCoord);
						CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.SetTextureScale ("_Splat2",new  Vector2(Layer3Tile.x,Layer3Tile.x));
						}
						EditorGUILayout.Space();
						if (CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.HasProperty("_Splat3")){
						Layer4Tile.x = Layer4Tile.y =EditorGUILayout.Slider("Layer4 Tiling :",Layer4Tile.x,1,500*T4MMaskTexUVCoord);
						CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.SetTextureScale ("_Splat3",new  Vector2(Layer4Tile.x,Layer4Tile.x));
						}
						EditorGUILayout.Space();
						if (CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.HasProperty("_Splat4")){
						Layer5Tile.x = Layer5Tile.y =EditorGUILayout.Slider("Layer5 Tiling :",Layer5Tile.x,1,500*T4MMaskTexUVCoord);
						CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.SetTextureScale ("_Splat4",new  Vector2(Layer5Tile.x,Layer5Tile.x));
						}
						EditorGUILayout.Space();
						if (CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.HasProperty("_Splat5")){
						Layer6Tile.x = Layer6Tile.y =EditorGUILayout.Slider("Layer6 Tiling :",Layer6Tile.x,1,500*T4MMaskTexUVCoord);
						CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.SetTextureScale ("_Splat5",new  Vector2(Layer6Tile.x,Layer6Tile.x));
						}
					}else{
						Layer1Tile.x  = EditorGUILayout.Slider("Layer1 TilingX :",Layer1Tile.x,1,500*T4MMaskTexUVCoord);
						Layer1Tile.y = EditorGUILayout.Slider("Layer1 TilingZ :",Layer1Tile.y,1,500*T4MMaskTexUVCoord);
						CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.SetTextureScale ("_Splat0",new  Vector2(Layer1Tile.x,Layer1Tile.y));
						EditorGUILayout.Space();
						Layer2Tile.x = EditorGUILayout.Slider("Layer2 TilingX :",Layer2Tile.x,1,500*T4MMaskTexUVCoord);
						Layer2Tile.y = EditorGUILayout.Slider("Layer2 TilingZ :",Layer2Tile.y,1,500*T4MMaskTexUVCoord);
						CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.SetTextureScale ("_Splat1",new  Vector2(Layer2Tile.x,Layer2Tile.y));
						EditorGUILayout.Space();
						if (CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.HasProperty("_Splat2")){
							Layer3Tile.x = EditorGUILayout.Slider("Layer3 TilingX :",Layer3Tile.x,1,500*T4MMaskTexUVCoord);
							Layer3Tile.y = EditorGUILayout.Slider("Layer3 TilingZ :",Layer3Tile.y,1,500*T4MMaskTexUVCoord);
							CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.SetTextureScale ("_Splat2",new  Vector2(Layer3Tile.x,Layer3Tile.y));
						}
						EditorGUILayout.Space();
						if (CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.HasProperty("_Splat3")){
							Layer4Tile.x = EditorGUILayout.Slider("Layer4 TilingX :",Layer4Tile.x,1,500*T4MMaskTexUVCoord);
							Layer4Tile.y = EditorGUILayout.Slider("Layer4 TilingZ :",Layer4Tile.y,1,500*T4MMaskTexUVCoord);
							CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.SetTextureScale ("_Splat3",new  Vector2(Layer4Tile.x,Layer4Tile.y));
						}
						EditorGUILayout.Space();
						if (CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.HasProperty("_Splat4")){
							Layer5Tile.x = EditorGUILayout.Slider("Layer5 TilingX :",Layer5Tile.x,1,500*T4MMaskTexUVCoord);
							Layer5Tile.y = EditorGUILayout.Slider("Layer5 TilingZ :",Layer5Tile.y,1,500*T4MMaskTexUVCoord);
							CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.SetTextureScale ("_Splat4",new  Vector2(Layer5Tile.x,Layer5Tile.y));
						}
						EditorGUILayout.Space();
						if (CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.HasProperty("_Splat5")){
							Layer6Tile.x = EditorGUILayout.Slider("Layer6 TilingX :",Layer6Tile.x,1,500*T4MMaskTexUVCoord);
							Layer6Tile.y = EditorGUILayout.Slider("Layer6 TilingZ :",Layer6Tile.y,1,500*T4MMaskTexUVCoord);
							CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.SetTextureScale ("_Splat5",new  Vector2(Layer6Tile.x,Layer6Tile.y));
						}
					}
				EditorGUILayout.EndScrollView();
				GUILayout.EndVertical();
				GUILayout.FlexibleSpace();
				GUILayout.EndHorizontal();
				
				if (TexBrush.Length>0){
					T4MPreview.material.SetTexture("_MaskTex", TexBrush[selBrush]);
					MeshFilter temp = CurrentSelect.GetComponent<MeshFilter>();
					if (temp == null)
						temp = CurrentSelect.GetComponent<T4MObjSC>().T4MMesh;
					T4MPreview.orthographicSize = (brushSize* CurrentSelect.localScale.x)*(temp.sharedMesh.bounds.size.x/200);
				}
					
					float test = T4MStronger*200/100;
					T4MPreview.material.SetFloat ("_Transp", Mathf.Clamp(test,0.4f,1));
				
				T4MBrushSizeInPourcent = (int)Mathf.Round ((brushSize*T4MMaskTex.width)/100);
				
				 
					
				if (T4MselTexture == 0)
					T4MPreview.material.SetTextureScale ("_MainTex", Layer1Tile);	
				else if (T4MselTexture == 1)	
					T4MPreview.material.SetTextureScale ("_MainTex", Layer2Tile);
				else if (T4MselTexture == 2)	
					T4MPreview.material.SetTextureScale ("_MainTex", Layer3Tile);
				else if (T4MselTexture == 3 )
					T4MPreview.material.SetTextureScale ("_MainTex", Layer4Tile);
				else if (T4MselTexture == 4 )
					T4MPreview.material.SetTextureScale ("_MainTex", Layer5Tile);
				else if (T4MselTexture == 5 )
					T4MPreview.material.SetTextureScale ("_MainTex", Layer6Tile);
				
				if (selBrush != oldSelBrush || T4MBrushSizeInPourcent != oldBrushSizeInPourcent || T4MBrushAlpha == null || T4MselTexture != oldselTexture){
					if (T4MselTexture == 0){
					T4MPreview.material.SetTexture("_MainTex", CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.GetTexture("_Splat0") as Texture);
					T4MtargetColor = new Color(1f, 0f, 0f, 0f);
					if (T4MMaskTex2)
							T4MtargetColor2 = new Color(0, 0, 0, 0);
					}
					else if (T4MselTexture == 1){
						T4MPreview.material.SetTexture("_MainTex", CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.GetTexture("_Splat1") as Texture);
						if (CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.shader == Shader.Find ("T4MShaders/ShaderModel1/T4M 2 Textures Auto BeastLM 2DrawCall") || 
								CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.shader == Shader.Find ("T4MShaders/ShaderModel1/T4M 2 Textures ManualAdd BeastLM_1DC")||
								CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.shader == Shader.Find ("T4MShaders/ShaderModel1/T4M 2 Textures ManualAdd CustoLM 1DC"))
							T4MtargetColor = new Color(0, 0, 0, 1);
						else {
						T4MtargetColor = new Color(0, 1, 0, 0);
						if (T4MMaskTex2)
							T4MtargetColor2 = new Color(0, 0, 0, 0);
						}							
					}
					else if (T4MselTexture == 2){
						T4MPreview.material.SetTexture("_MainTex", CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.GetTexture("_Splat2") as Texture);
						T4MtargetColor = new Color(0, 0, 1, 0);
						if (T4MMaskTex2)
							T4MtargetColor2 = new Color(0, 0, 0, 0);
					}
					else if (T4MselTexture == 3 ){
						T4MPreview.material.SetTexture("_MainTex", CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.GetTexture("_Splat3") as Texture);
						T4MtargetColor = new Color(0, 0, 0, 1);
						if (T4MMaskTex2)
							T4MtargetColor2 = new Color(1, 0, 0, 0);
					}
					else if (T4MselTexture == 4 ){
						T4MPreview.material.SetTexture("_MainTex", CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.GetTexture("_Splat4") as Texture);
						T4MtargetColor = new Color(0, 0, 0, 1);
						if (T4MMaskTex2)
							T4MtargetColor2 = new Color(0, 1, 0, 0);
					}
					else if (T4MselTexture == 5 ){
						T4MPreview.material.SetTexture("_MainTex", CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.GetTexture("_Splat5") as Texture);
						T4MtargetColor = new Color(0, 0, 0, 1);
						if (T4MMaskTex2)
							T4MtargetColor2 = new Color(0, 0, 1, 0);
					}
					Texture2D TBrush = TexBrush[selBrush] as Texture2D;
					T4MBrushAlpha = new float[T4MBrushSizeInPourcent * T4MBrushSizeInPourcent];
					for( int  i = 0; i < T4MBrushSizeInPourcent; i++ ) {
						for( int  j = 0;j < T4MBrushSizeInPourcent; j++ ) {
							T4MBrushAlpha[j * T4MBrushSizeInPourcent + i] =  TBrush.GetPixelBilinear(((float)i) / T4MBrushSizeInPourcent, ((float)j) /T4MBrushSizeInPourcent).a;
						}
					}
					oldselTexture =T4MselTexture;
					oldSelBrush =selBrush;
					oldBrushSizeInPourcent =T4MBrushSizeInPourcent;
				}
			}}
			}else
			{
			GUILayout.BeginHorizontal();
					GUILayout.FlexibleSpace();
					GUILayout.Label("Please, select the T4M Object", EditorStyles.boldLabel);
					GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			
		}
		
	}
	

	
	void InitPreview()
	{
		var ProjectorB = new GameObject ("PreviewT4M");
		ProjectorB.AddComponent(typeof(Projector));
		ProjectorB.hideFlags = HideFlags.HideInHierarchy;
		T4MPreview= ProjectorB.GetComponent(typeof(Projector)) as Projector;
		MeshFilter SizeOfGeo = CurrentSelect.GetComponent<MeshFilter>();
		if (SizeOfGeo == null)
			SizeOfGeo = CurrentSelect.GetComponent<T4MObjSC>().T4MMesh;
		Vector2 MeshSize = new Vector2(SizeOfGeo.sharedMesh.bounds.size.x,SizeOfGeo.sharedMesh.bounds.size.z);
		T4MPreview.nearClipPlane = -20;
        T4MPreview.farClipPlane = 20;
        T4MPreview.orthographic = true;
		T4MPreview.orthographicSize = (brushSize* CurrentSelect.localScale.x)*(MeshSize.x/100);
		T4MPreview.ignoreLayers =  ~layerMask;
        T4MPreview.transform.Rotate(90, -90, 0);
		Shader myShader = Shader.Find("Hidden/BrushPreview");
        Material NewPMat = new Material(myShader);
        T4MPreview.material = NewPMat;
		T4MPreview.material.SetTexture("_MainTex", TexTexture[T4MselTexture]);
		T4MPreview.material.SetTexture("_MaskTex", TexBrush[selBrush]);
		if (T4MselTexture == 0){
			T4MPreview.material.SetTextureScale ("_MainTex", Layer1Tile);
			T4MPreview.material.SetTexture("_MainTex", CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.GetTexture("_Splat0") as Texture);
		}
		else if (T4MselTexture == 1){
			T4MPreview.material.SetTextureScale ("_MainTex", Layer2Tile);
			T4MPreview.material.SetTexture("_MainTex", CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.GetTexture("_Splat1") as Texture);
		}
		else if (T4MselTexture == 2){
			T4MPreview.material.SetTextureScale ("_MainTex", Layer3Tile);
			T4MPreview.material.SetTexture("_MainTex", CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.GetTexture("_Splat2") as Texture);
		}
		else if (T4MselTexture == 3){
			T4MPreview.material.SetTextureScale ("_MainTex", Layer4Tile);
			T4MPreview.material.SetTexture("_MainTex", CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.GetTexture("_Splat3") as Texture);
		}
		else if (T4MselTexture == 4 ){
			T4MPreview.material.SetTextureScale ("_MainTex", Layer5Tile);
			T4MPreview.material.SetTexture("_MainTex", CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.GetTexture("_Splat4") as Texture);
		}
		else if (T4MselTexture == 5 ){
			T4MPreview.material.SetTextureScale ("_MainTex", Layer6Tile);
			T4MPreview.material.SetTexture("_MainTex", CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.GetTexture("_Splat5") as Texture);
		}
	}
	
	static public void SaveTexture()
	{
		var path = AssetDatabase.GetAssetPath (T4MMaskTex);
		var bytes   = T4MMaskTex.EncodeToPNG ();
		File.WriteAllBytes (path, bytes);
		if (T4MMaskTex2){
			var path2 = AssetDatabase.GetAssetPath (T4MMaskTex2);
			var bytes2   = T4MMaskTex2.EncodeToPNG ();
			File.WriteAllBytes (path2, bytes2);
		}
		//AssetDatabase.Refresh ();
	}
	
	void InitPincil()
	{
		
		if (CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.HasProperty("_Splat5")){
			TexTexture = new Texture[6];
			TexTexture[0]=AssetPreview.GetAssetPreview(CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.GetTexture("_Splat0"))as Texture;
			TexTexture[1]=AssetPreview.GetAssetPreview(CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.GetTexture("_Splat1"))as Texture;
			TexTexture[2]=AssetPreview.GetAssetPreview(CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.GetTexture("_Splat2"))as Texture;
			TexTexture[3]=AssetPreview.GetAssetPreview(CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.GetTexture("_Splat3"))as Texture;
			TexTexture[4]=AssetPreview.GetAssetPreview(CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.GetTexture("_Splat4"))as Texture;
			TexTexture[5]=AssetPreview.GetAssetPreview(CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.GetTexture("_Splat5"))as Texture;
		}else if (CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.HasProperty("_Splat4")){
			TexTexture = new Texture[5];
			TexTexture[0]=AssetPreview.GetAssetPreview(CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.GetTexture("_Splat0"))as Texture;
			TexTexture[1]=AssetPreview.GetAssetPreview(CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.GetTexture("_Splat1"))as Texture;
			TexTexture[2]=AssetPreview.GetAssetPreview(CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.GetTexture("_Splat2"))as Texture;
			TexTexture[3]=AssetPreview.GetAssetPreview(CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.GetTexture("_Splat3"))as Texture;
			TexTexture[4]=AssetPreview.GetAssetPreview(CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.GetTexture("_Splat4"))as Texture;
		}else if (CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.HasProperty("_Splat3")){
			TexTexture = new Texture[4];
			TexTexture[0]=AssetPreview.GetAssetPreview(CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.GetTexture("_Splat0"))as Texture;
			TexTexture[1]=AssetPreview.GetAssetPreview(CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.GetTexture("_Splat1"))as Texture;
			TexTexture[2]=AssetPreview.GetAssetPreview(CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.GetTexture("_Splat2"))as Texture;
			TexTexture[3]=AssetPreview.GetAssetPreview(CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.GetTexture("_Splat3"))as Texture;
		}else if (CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.HasProperty("_Splat2")){
			TexTexture = new Texture[3];
			TexTexture[0]=AssetPreview.GetAssetPreview(CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.GetTexture("_Splat0"))as Texture;
			TexTexture[1]=AssetPreview.GetAssetPreview(CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.GetTexture("_Splat1"))as Texture;
			TexTexture[2]=AssetPreview.GetAssetPreview(CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.GetTexture("_Splat2"))as Texture;
		}else{
			TexTexture = new Texture[2];
			TexTexture[0]=AssetPreview.GetAssetPreview(CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.GetTexture("_Splat0"))as Texture;
			TexTexture[1]=AssetPreview.GetAssetPreview(CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.GetTexture("_Splat1"))as Texture;
			
		}
	}
	
	void IniBrush()
	{
		ArrayList BrushList = new ArrayList ();
		Texture  BrushesTL;
		int BrushNum = 0;
		do {
			BrushesTL = (Texture) AssetDatabase.LoadAssetAtPath (T4MEditorFolder+"Brushes/Brush"+BrushNum+".png", typeof(Texture));
			if (BrushesTL){
				BrushList.Add (BrushesTL);
			}
			BrushNum++;
		}while (BrushesTL);
		TexBrush = BrushList.ToArray(typeof(Texture)) as Texture[];
	}
	
	void afLOD()
	{
		if (CurrentSelect.GetComponent <T4MObjSC>())
			{
			if (CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial && CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.HasProperty("_Splat0") && CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.HasProperty("_Splat1")&& CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.HasProperty("_Control")){	
			GUILayout.BeginHorizontal();
					GUILayout.FlexibleSpace();	
					LODM = GUILayout.Toolbar(LODM, LODMenu, GUILayout.Width(290), GUILayout.Height(20));
					GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			switch (LODM)
			{
				case 0:
				LODManager();
				break;
				case 1:
				LODObjectC();
				break;
			}
		}
		}else
		{
			GUILayout.BeginHorizontal();
					GUILayout.FlexibleSpace();
					GUILayout.Label("Please, select the T4M Object", EditorStyles.boldLabel);
					GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
		}
	}
	
	void LODObjectC()
	{
		
		
		EditorGUILayout.Space();
	
		GUILayout.BeginHorizontal("box");
			GUILayout.Label("LOD Prefab Name", EditorStyles.boldLabel);
				PrefabName=GUILayout.TextField (PrefabName, 25, GUILayout.Width(155));
		GUILayout.EndHorizontal();
		
		EditorGUILayout.Space();
		
		GUILayout.Label("LOD Meshes", EditorStyles.boldLabel);
		LOD1= (Mesh)EditorGUILayout.ObjectField("LOD1 : Close",  LOD1, typeof(Mesh),false);
		LOD2= (Mesh)EditorGUILayout.ObjectField("LOD2 : Medium",LOD2, typeof(Mesh),false);
		LOD3= (Mesh)EditorGUILayout.ObjectField("LOD3 : Far",LOD3, typeof(Mesh),false);
		
		EditorGUILayout.Space();
		
		GUILayout.BeginHorizontal();
			GUILayout.Label("LOD1 Setup", EditorStyles.boldLabel, GUILayout.Width(223));
			GUILayout.Label("MainTex", EditorStyles.boldLabel, GUILayout.Width(68));
			GUILayout.Label("Bump", EditorStyles.boldLabel, GUILayout.Width(60));
		GUILayout.EndHorizontal();
		
		GUILayout.BeginHorizontal();
			GUILayout.BeginVertical();
				GUILayout.BeginHorizontal();
					GUILayout.Label("Place the MeshCollider on this LOD", GUILayout.Width(205));
					CheckLOD1Collider = EditorGUILayout.Toggle(CheckLOD1Collider, GUILayout.Width(15));
				GUILayout.EndHorizontal();
		
			GUILayout.BeginHorizontal();
				GUILayout.Label("LOD1 Shader", GUILayout.Width(103));
				ShaderLOD1S =(LODShaderStatus) EditorGUILayout.EnumPopup (ShaderLOD1S, GUILayout.Width(95));
			GUILayout.EndHorizontal();	
				if (ShaderLOD1S == LODShaderStatus.New)
					LOD1S= (Shader)EditorGUILayout.ObjectField(LOD1S, typeof(Shader),true, GUILayout.MaxWidth(220));
				else LOD1Material= (Material)EditorGUILayout.ObjectField(LOD1Material, typeof(Material),false, GUILayout.MaxWidth(220));
			GUILayout.EndVertical();
				if (LOD1S)
					LOD1Material =  new Material (Shader.Find (LOD1S.name));
				
					if (LOD1Material && LOD1Material.HasProperty("_MainTex")){
						if (LOD1Material.GetTexture("_MainTex"))
							LOD1T = LOD1Material.GetTexture("_MainTex");
						LOD1T=EditorGUILayout.ObjectField(LOD1T, typeof(Texture),false, GUILayout.Width(60), GUILayout.Height(60)) as Texture;
						if (LOD1Material && LOD1Material.HasProperty("_BumpMap")){
							if (LOD1Material.GetTexture("_BumpMap"))
								LOD1B = LOD1Material.GetTexture("_BumpMap");
							LOD1B=EditorGUILayout.ObjectField(LOD1B, typeof(Texture),false, GUILayout.Width(60), GUILayout.Height(60)) as Texture;
						}
					}
				
				GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		
		EditorGUILayout.Space();
		
		GUILayout.BeginHorizontal();
			GUILayout.Label("LOD2 Setup", EditorStyles.boldLabel, GUILayout.Width(223));
			GUILayout.Label("MainTex", EditorStyles.boldLabel, GUILayout.Width(68));
			GUILayout.Label("Bump", EditorStyles.boldLabel, GUILayout.Width(60));
		GUILayout.EndHorizontal();
		
		GUILayout.BeginHorizontal();
			GUILayout.BeginVertical();
				GUILayout.BeginHorizontal();
					GUILayout.Label("Place the MeshCollider on this LOD", GUILayout.Width(205));
					CheckLOD2Collider = EditorGUILayout.Toggle(CheckLOD2Collider, GUILayout.Width(15));
				GUILayout.EndHorizontal();
		
				GUILayout.BeginHorizontal();
				GUILayout.Label("LOD2 Shader", GUILayout.Width(103));
				ShaderLOD2S =(LODShaderStatus) EditorGUILayout.EnumPopup (ShaderLOD2S, GUILayout.Width(95));
				GUILayout.EndHorizontal();	
			if (ShaderLOD2S == LODShaderStatus.New)
				LOD2S= (Shader)EditorGUILayout.ObjectField(LOD2S, typeof(Shader),false, GUILayout.MaxWidth(220));
			else LOD2Material= (Material)EditorGUILayout.ObjectField(LOD2Material, typeof(Material),false, GUILayout.MaxWidth(220));
			GUILayout.EndVertical();
				if (LOD2S)
					LOD2Material =  new Material (Shader.Find (LOD2S.name)); 
					if (LOD2Material && LOD2Material.HasProperty("_MainTex")){
						if (LOD2Material.GetTexture("_MainTex"))
							LOD2T = LOD2Material.GetTexture("_MainTex");
						LOD2T=EditorGUILayout.ObjectField(LOD2T, typeof(Texture),false, GUILayout.Width(60), GUILayout.Height(60)) as Texture;
						if (LOD2Material && LOD2Material.HasProperty("_BumpMap")){
						if (LOD2Material.GetTexture("_BumpMap"))
							LOD2B = LOD2Material.GetTexture("_BumpMap");
							LOD2B=EditorGUILayout.ObjectField(LOD2B, typeof(Texture),false, GUILayout.Width(60), GUILayout.Height(60)) as Texture;
						}
					}
				
				GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		
		EditorGUILayout.Space();
		
		GUILayout.BeginHorizontal();
			GUILayout.Label("LOD3 Setup", EditorStyles.boldLabel, GUILayout.Width(223));
			GUILayout.Label("MainTex", EditorStyles.boldLabel, GUILayout.Width(68));
			GUILayout.Label("Bump", EditorStyles.boldLabel, GUILayout.Width(60));
		GUILayout.EndHorizontal();
		
		GUILayout.BeginHorizontal();
			GUILayout.BeginVertical();
				GUILayout.BeginHorizontal();
					GUILayout.Label("Place the MeshCollider on this LOD", GUILayout.Width(205));
				CheckLOD3Collider = EditorGUILayout.Toggle(CheckLOD3Collider, GUILayout.Width(15));
				GUILayout.EndHorizontal();
		
				GUILayout.BeginHorizontal();
				GUILayout.Label("LOD3 Shader", GUILayout.Width(103));
				ShaderLOD3S =(LODShaderStatus) EditorGUILayout.EnumPopup (ShaderLOD3S, GUILayout.Width(95));
				GUILayout.EndHorizontal();
			if (ShaderLOD3S == LODShaderStatus.New)
				LOD3S= (Shader)EditorGUILayout.ObjectField(LOD3S, typeof(Shader),false, GUILayout.MaxWidth(220));
			else LOD3Material= (Material)EditorGUILayout.ObjectField(LOD3Material, typeof(Material),false, GUILayout.MaxWidth(220));
			GUILayout.EndVertical();
				if (LOD3S)
					LOD3Material =  new Material (Shader.Find (LOD3S.name)); 
					if (LOD3Material && LOD3Material.HasProperty("_MainTex")){
						if (LOD3Material.GetTexture("_MainTex"))
							LOD3T = LOD3Material.GetTexture("_MainTex");
						LOD3T=EditorGUILayout.ObjectField(LOD3T, typeof(Texture),false, GUILayout.Width(60), GUILayout.Height(60)) as Texture;
						if (LOD3Material && LOD3Material.HasProperty("_BumpMap")){
						if (LOD3Material.GetTexture("_BumpMap"))
							LOD3B = LOD3Material.GetTexture("_BumpMap");
							LOD3B=EditorGUILayout.ObjectField(LOD3B, typeof(Texture),false, GUILayout.Width(60), GUILayout.Height(60)) as Texture;
						}
					}
				
				GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		GUILayout.FlexibleSpace();
			GUILayout.BeginHorizontal();
				GUILayout.FlexibleSpace();
					if (GUILayout.Button("CONSTRUCT", GUILayout.Width(100), GUILayout.Height(30))) {
						if (PrefabName != "" && LOD1  && LOD2 && LOD3 && LOD1Material && LOD2Material && LOD3Material)
							CreatePrefab();
						else  EditorUtility.DisplayDialog("T4M Message", "You must complete the formulary before make the construct", "OK");
					}
				GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
		GUILayout.FlexibleSpace();
		
		if (CheckLOD1Collider == true && CheckStatus !="LOD1"){
			CheckStatus ="LOD1";
			CheckLOD2Collider = false;
			CheckLOD3Collider = false;
		}
		if (CheckLOD2Collider == true && CheckStatus !="LOD2")
		{
			CheckStatus ="LOD2";
			CheckLOD1Collider = false;
			CheckLOD3Collider = false;
		}
		 if (CheckLOD3Collider == true && CheckStatus !="LOD3")
		{
			CheckStatus ="LOD3";
			CheckLOD1Collider = false;
			CheckLOD2Collider = false;
		}
		if (OldShaderLOD1S != ShaderLOD1S){
			LOD1B =null;
			LOD1T = null;
			LOD1Material =null;
			LOD1S = null;
			OldShaderLOD1S = ShaderLOD1S;
		}
		if (OldShaderLOD2S != ShaderLOD2S){
			LOD2B =null;
			LOD2T = null;
			LOD2Material =null;
			LOD2S = null;
			OldShaderLOD2S = ShaderLOD2S;
		}
		if (OldShaderLOD3S != ShaderLOD3S){
			LOD3B =null;
			LOD3T = null;
			LOD3Material =null;
			LOD3S = null;
			OldShaderLOD3S = ShaderLOD3S;
		}
	}
	
	void CreatePrefab()
	{
			if (!System.IO.Directory.Exists(T4MPrefabFolder+"LODObjects/"))
            {
                System.IO.Directory.CreateDirectory(T4MPrefabFolder+"LODObjects/");
            } 
			 if (!System.IO.Directory.Exists(T4MPrefabFolder+"LODObjects/Material/"))
            {
                System.IO.Directory.CreateDirectory(T4MPrefabFolder+"LODObjects/Material/");
            } 
			AssetDatabase.Refresh(); 	 
			
			GameObject LOD1Temp;
			LOD1Temp = new GameObject (PrefabName+"LOD1");
			LOD1Temp.AddComponent<MeshFilter> ();
			LOD1Temp.AddComponent<MeshRenderer> ();
			LOD1Temp.AddComponent<T4MLodObjSC> ();
			LOD1Temp.GetComponent<MeshFilter>().mesh = LOD1;
			LOD1Temp.GetComponent<Renderer>().sharedMaterial = LOD1Material;
			LOD1Temp.GetComponent<Renderer>().sharedMaterial.SetTexture ("_MainTex", LOD1T);
			LOD1Temp.GetComponent <T4MLodObjSC>().LOD1= LOD1Temp.GetComponent<Renderer>();
			if (CheckLOD1Collider){
				LOD1Temp.AddComponent <MeshCollider>();
				LOD1Temp.GetComponent<MeshCollider>().sharedMesh = LOD1;
			}
						
			GameObject LOD2Temp;
			LOD2Temp = new GameObject (PrefabName+"LOD2");
			LOD2Temp.AddComponent<MeshFilter>();
			LOD2Temp.AddComponent<MeshRenderer>();
			LOD2Temp.GetComponent<MeshFilter>().mesh = LOD2;
			LOD2Temp.GetComponent<Renderer>().sharedMaterial = LOD2Material;
			LOD2Temp.GetComponent<Renderer>().sharedMaterial.SetTexture ("_MainTex", LOD2T);
			LOD1Temp.GetComponent<T4MLodObjSC>().LOD2= LOD2Temp.GetComponent<Renderer>();
			LOD2Temp.GetComponent<MeshRenderer>().enabled= false;
			LOD2Temp.transform.parent = LOD1Temp.transform; 
			if (CheckLOD2Collider){
				LOD2Temp.AddComponent<MeshCollider> ();
				LOD2Temp.GetComponent<MeshCollider>().sharedMesh = LOD2;
			}
						
			GameObject LOD3Temp;
			LOD3Temp = new GameObject (PrefabName+"LOD3");
			LOD3Temp.AddComponent <MeshFilter>();
			LOD3Temp.AddComponent <MeshRenderer>();
			LOD3Temp.GetComponent<MeshFilter>().mesh = LOD3;
			LOD3Temp.GetComponent<Renderer>().sharedMaterial = LOD3Material;
			LOD3Temp.GetComponent<Renderer>().sharedMaterial.SetTexture ("_MainTex", LOD3T);
			LOD1Temp.GetComponent <T4MLodObjSC>().LOD3= LOD3Temp.GetComponent<Renderer>();
			LOD3Temp.GetComponent <MeshRenderer>().enabled= false;
			LOD3Temp.transform.parent = LOD1Temp.transform; 
			if (CheckLOD3Collider){
				LOD3Temp.AddComponent <MeshCollider>();
				LOD3Temp.GetComponent<MeshCollider>().sharedMesh = LOD3;
			}
			
			bool ExportSuccess = false;
			int num =1;
			UnityEngine.Object BasePrefab;
			string Next;
			do { 
				Next=PrefabName+num;
				
				if (!System.IO.File.Exists(T4MPrefabFolder+"LODObjects/"+PrefabName+"_LOD.prefab"))
				{
					
					if (ShaderLOD1S == LODShaderStatus.New)
						 AssetDatabase.CreateAsset(LOD1Material, T4MPrefabFolder+"LODObjects/Material/"+PrefabName+"LOD1.mat");
					if (ShaderLOD2S == LODShaderStatus.New)
						AssetDatabase.CreateAsset(LOD2Material, T4MPrefabFolder+"LODObjects/Material/"+PrefabName+"LOD2.mat");	
					if (ShaderLOD3S == LODShaderStatus.New)
						AssetDatabase.CreateAsset(LOD3Material, T4MPrefabFolder+"LODObjects/Material/"+PrefabName+"LOD3.mat");
				BasePrefab = PrefabUtility.CreateEmptyPrefab(T4MPrefabFolder+"LODObjects/"+PrefabName+".prefab");
					PrefabUtility.ReplacePrefab(LOD1Temp, BasePrefab);
					ExportSuccess = true;
				}
				else if (!System.IO.File.Exists(T4MPrefabFolder+"LODObjects/"+Next+"_LOD.prefab"))
				{
					
					if (ShaderLOD1S == LODShaderStatus.New)
						AssetDatabase.CreateAsset(LOD1Material, T4MPrefabFolder+"LODObjects/Material/"+Next+"LOD1.mat");
					if (ShaderLOD2S == LODShaderStatus.New)
						AssetDatabase.CreateAsset(LOD2Material, T4MPrefabFolder+"LODObjects/Material/"+Next+"LOD2.mat");
					if (ShaderLOD3S == LODShaderStatus.New)
						AssetDatabase.CreateAsset(LOD3Material, T4MPrefabFolder+"LODObjects/Material/"+Next+"LOD3.mat");
					BasePrefab= PrefabUtility.CreateEmptyPrefab(T4MPrefabFolder+"LODObjects/"+Next+".prefab");
					PrefabUtility.ReplacePrefab(LOD1Temp, BasePrefab);
					ExportSuccess = true;
				}
				num++;
			}while (!ExportSuccess);
				AssetDatabase.SaveAssets();
				AssetDatabase.Refresh();
				
		
			
			DestroyImmediate(LOD1Temp);
			EditorUtility.DisplayDialog("T4M Message", "Construction Completed", "OK");
	}
	
	void LODManager()
	{
		if (T4MMaster){
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		GUILayout.Label("LOD Mode", EditorStyles.boldLabel);
		LODModeControler =(LODMod) EditorGUILayout.EnumPopup ("controller", LODModeControler, GUILayout.Width(340));
		EditorGUILayout.Space();
		GUILayout.Label("Culling LOD Object Mode", EditorStyles.boldLabel);
		LODocclusion = (OccludeBy) EditorGUILayout.EnumPopup ("Mode", LODocclusion, GUILayout.Width(340));
		
		EditorGUILayout.Space();
			EditorGUILayout.Space();
		GUILayout.BeginHorizontal();
				GUILayout.Label("Maximum View Distance", EditorStyles.boldLabel, GUILayout.Width(300));
				MaximunView = EditorGUILayout.FloatField(MaximunView, GUILayout.Width(50));
		GUILayout.EndHorizontal();	
				
			
			EditorGUILayout.Space();
			EditorGUILayout.Space();
			
			GUILayout.Label("LOD Update Interval in Seconde", EditorStyles.boldLabel, GUILayout.Width(400));
				GUILayout.BeginHorizontal();
				GUILayout.Label("(less value = less performance)", GUILayout.Width(300));
				UpdateInterval = EditorGUILayout.FloatField(UpdateInterval, GUILayout.Width(50));
			GUILayout.EndHorizontal();
			
			EditorGUILayout.Space();
			EditorGUILayout.Space();
			
			EditorGUILayout.Space();
			EditorGUILayout.Space();
			
			GUILayout.BeginHorizontal();
				GUILayout.Label("LOD2 Start", EditorStyles.boldLabel, GUILayout.Width(170));
				GUILayout.FlexibleSpace();
				StartLOD2= EditorGUILayout.FloatField(StartLOD2, GUILayout.Width(50));
			GUILayout.EndHorizontal();
			
			GUILayout.BeginHorizontal();
				GUILayout.FlexibleSpace();
					StartLOD2= GUILayout.HorizontalScrollbar (StartLOD2,0.0f,10f,MaximunView,GUILayout.Width(350));
				GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			
			EditorGUILayout.Space();
			EditorGUILayout.Space();
			
			if (StartLOD2 > StartLOD3-5)
				StartLOD3= StartLOD2+5;
			if(StartLOD2 > MaximunView-10)
				StartLOD2 = MaximunView-10;
			if(StartLOD3 > MaximunView-5)
				StartLOD3 = MaximunView-5;
				
			EditorGUILayout.Space();
			EditorGUILayout.Space();
			
			GUILayout.BeginHorizontal();
				GUILayout.Label("LOD3 Start", EditorStyles.boldLabel, GUILayout.Width(170));
				GUILayout.FlexibleSpace();
				StartLOD3= EditorGUILayout.FloatField(StartLOD3, GUILayout.Width(50));
			GUILayout.EndHorizontal();
			
			GUILayout.BeginHorizontal();
				GUILayout.FlexibleSpace();
					StartLOD3= GUILayout.HorizontalScrollbar (StartLOD3,0.0f,10f,MaximunView,GUILayout.Width(350));
				GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			GUILayout.FlexibleSpace();
			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			
			string buttonLod;
			Texture Swap;
			if (LodActivate == true){
				buttonLod = "DEACTIVATE";	
				Swap= (Texture)AssetDatabase.LoadAssetAtPath(T4MEditorFolder+"Img/on.png", typeof(Texture));	
			}else{
				buttonLod = "ACTIVATE";
				Swap= (Texture)AssetDatabase.LoadAssetAtPath(T4MEditorFolder+"Img/off.png", typeof(Texture));
			}
			
				if (GUILayout.Button(buttonLod, GUILayout.Width(100), GUILayout.Height(30))) 
				{
						ActivateDeactivateLOD ();
				}
				GUILayout.Label(Swap, GUILayout.Width(75), GUILayout.Height(30));
				
				GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			GUILayout.FlexibleSpace();
			}else{
				GUILayout.BeginHorizontal();
				GUILayout.FlexibleSpace();
				GUILayout.Label("Need to be an Master T4M", EditorStyles.boldLabel);
				GUILayout.FlexibleSpace();
				GUILayout.EndHorizontal();
			}
	}
	
	void DeactivateLODByScript(){
		T4MLodObjSC[] T4MLodObjGet =  GameObject.FindObjectsOfType(typeof(T4MLodObjSC)) as T4MLodObjSC[];
						
		for(var i=0;i<T4MLodObjGet.Length;i++){
			T4MLodObjGet[i].LOD2.enabled= T4MLodObjGet[i].LOD3.enabled = false;
			T4MLodObjGet[i].LOD1.enabled= true;
			if(LODModeControler == LODMod.Mass_Control)
				T4MLodObjGet[i].Mode =0;
			else if(LODModeControler ==LODMod.Independent_Control)
				T4MLodObjGet[i].Mode =0;
			PrefabUtility.RecordPrefabInstancePropertyModifications(T4MLodObjGet[i].GetComponent<T4MLodObjSC>());
		}	
		
		CurrentSelect.gameObject.GetComponent <T4MObjSC>().ObjLodScript= new T4MLodObjSC[0];
		CurrentSelect.gameObject.GetComponent <T4MObjSC>().ObjPosition= new Vector3[0];
		CurrentSelect.gameObject.GetComponent <T4MObjSC>().ObjLodStatus=new int[0];
		CurrentSelect.gameObject.GetComponent <T4MObjSC>().Mode =0;
		PrefabUtility.RecordPrefabInstancePropertyModifications(CurrentSelect.gameObject.GetComponent<T4MObjSC>());
		Debug.Log ("The Number of Activated LOD Objects has changed, reactivate the billboards in the 'LOD' Tab.");
		CurrentSelect.gameObject.GetComponent <T4MObjSC>().Awake();
	}
	
	void ActivateDeactivateLOD (){
		if (LodActivate == true){ //Lod actif
					T4MLodObjSC[] T4MLodObjGet =  GameObject.FindObjectsOfType(typeof(T4MLodObjSC)) as T4MLodObjSC[];
									
					for(var i=0;i<T4MLodObjGet.Length;i++){
						T4MLodObjGet[i].LOD2.enabled= T4MLodObjGet[i].LOD3.enabled = false;
						T4MLodObjGet[i].LOD1.enabled= true;
						if(LODModeControler == LODMod.Mass_Control)
							T4MLodObjGet[i].Mode =0;
						else if(LODModeControler ==LODMod.Independent_Control)
							T4MLodObjGet[i].Mode =0;
						PrefabUtility.RecordPrefabInstancePropertyModifications(T4MLodObjGet[i].GetComponent<T4MLodObjSC>());
					}	
					
					CurrentSelect.gameObject.GetComponent <T4MObjSC>().ObjLodScript= new T4MLodObjSC[0];
					CurrentSelect.gameObject.GetComponent <T4MObjSC>().ObjPosition= new Vector3[0];
					CurrentSelect.gameObject.GetComponent <T4MObjSC>().ObjLodStatus=new int[0];
					CurrentSelect.gameObject.GetComponent <T4MObjSC>().Mode =0;
					PrefabUtility.RecordPrefabInstancePropertyModifications(CurrentSelect.gameObject.GetComponent<T4MObjSC>());
					LodActivate = false;
					Debug.LogWarning ("LOD deactivated !");
				}else{
					if (!PlayerCam)
					PlayerCam=Camera.main.transform;
		
				T4MLodObjSC[] T4MLodObjGetR  = GameObject.FindObjectsOfType(typeof(T4MLodObjSC)) as T4MLodObjSC[];
				Vector3[] T4MLodVectGetR = new Vector3[T4MLodObjGetR.Length]; 
				int[] T4MLodValueGetR = new int[T4MLodObjGetR.Length]; 
				
				
				for(int i=0;i<T4MLodObjGetR.Length;i++){
					T4MLodVectGetR[i] = T4MLodObjGetR[i].transform.position;
					
					
					float distanceFromCameraR = Vector3.Distance(new Vector3(T4MLodObjGetR[i].transform.position.x,PlayerCam.position.y,T4MLodObjGetR[i].transform.position.z),PlayerCam.transform.position);	
					
					
					if(distanceFromCameraR <= MaximunView){
						 if(distanceFromCameraR < StartLOD2)
						 {
							T4MLodObjGetR[i].LOD2.enabled= T4MLodObjGetR[i].LOD3.enabled = false;
							T4MLodObjGetR[i].LOD1.enabled= true;
							T4MLodObjGetR[i].ObjLodStatus=T4MLodValueGetR[i] = 1;
						}
						else if (distanceFromCameraR >= StartLOD2 && distanceFromCameraR < StartLOD3 )
						{
							T4MLodObjGetR[i].LOD1.enabled= T4MLodObjGetR[i].LOD3.enabled = false;
							T4MLodObjGetR[i].LOD2.enabled= true;
							T4MLodObjGetR[i].ObjLodStatus=T4MLodValueGetR[i] = 2;
						}
						else if (distanceFromCameraR >= StartLOD3)
						{
							T4MLodObjGetR[i].LOD1.enabled= T4MLodObjGetR[i].LOD2.enabled = false;
							T4MLodObjGetR[i].LOD3.enabled= true;
							T4MLodObjGetR[i].ObjLodStatus=T4MLodValueGetR[i] = 3;
						}
					}	
					else
					{ 
						if (LODocclusion == OccludeBy.Max_View_Distance){
							T4MLodObjGetR[i].LOD3.enabled= T4MLodObjGetR[i].LOD1.enabled= T4MLodObjGetR[i].LOD2.enabled = false;
							T4MLodObjGetR[i].ObjLodStatus= T4MLodValueGetR[i] = 0;
						}else{
							T4MLodObjGetR[i].LOD1.enabled= T4MLodObjGetR[i].LOD2.enabled = false;
							T4MLodObjGetR[i].LOD3.enabled= true;
							T4MLodObjGetR[i].ObjLodStatus=T4MLodValueGetR[i] = 3;
						}
					}
					//To each LOD
					T4MLodObjGetR[i].Interval = UpdateInterval;
					T4MLodObjGetR[i].MaxViewDistance = MaximunView;
					T4MLodObjGetR[i].PlayerCamera = PlayerCam;
					T4MLodObjGetR[i].LOD2Start =StartLOD2;
					T4MLodObjGetR[i].LOD3Start =StartLOD3;
					if(LODModeControler == LODMod.Mass_Control)
						T4MLodObjGetR[i].Mode =1;
					else if(LODModeControler ==LODMod.Independent_Control)
						T4MLodObjGetR[i].Mode =2;
					PrefabUtility.RecordPrefabInstancePropertyModifications(T4MLodObjGetR[i].GetComponent<T4MLodObjSC>());
				}
		
				if(LODModeControler ==LODMod.Mass_Control)
					CurrentSelect.gameObject.GetComponent <T4MObjSC>().Mode =1;
				else if(LODModeControler ==LODMod.Independent_Control)
					CurrentSelect.gameObject.GetComponent <T4MObjSC>().Mode =2;
		
		
				if (LODocclusion == OccludeBy.Max_View_Distance)
					CurrentSelect.GetComponent <T4MObjSC>().LODbasedOnScript= true;
				else CurrentSelect.GetComponent <T4MObjSC>().LODbasedOnScript= false;
		
				CurrentSelect.GetComponent <T4MObjSC>().ObjLodScript= T4MLodObjGetR;
				CurrentSelect.GetComponent <T4MObjSC>().ObjPosition= T4MLodVectGetR;
				CurrentSelect.GetComponent <T4MObjSC>().ObjLodStatus=T4MLodValueGetR;
				CurrentSelect.GetComponent <T4MObjSC>().Interval =UpdateInterval;
				CurrentSelect.GetComponent <T4MObjSC>().MaxViewDistance =MaximunView;
				
				CurrentSelect.GetComponent <T4MObjSC>().LOD2Start =StartLOD2;
				CurrentSelect.GetComponent <T4MObjSC>().LOD3Start =StartLOD3;
				CurrentSelect.GetComponent <T4MObjSC>().PlayerCamera = PlayerCam;
				
				PrefabUtility.RecordPrefabInstancePropertyModifications(CurrentSelect.gameObject.GetComponent<T4MObjSC>());
				
				LodActivate = true;
				Debug.LogWarning ("LOD (re)activated !");
			}
		CurrentSelect.gameObject.GetComponent <T4MObjSC>().Awake();
	}
	
	
	void ConverterMenu()
	{	
		if (vertexInfo ==0 && trisInfo ==0 && partofT4MObj ==0){
			
			if ((CurrentSelect.GetComponent<Renderer>() || CurrentSelect.GetComponent<Terrain>() || NbrPartObj !=null && NbrPartObj.Length != 0) && !CurrentSelect.GetComponent <T4MObjSC>() && !CurrentSelect.GetComponent <T4MPartSC>() )
			{
				GUILayout.BeginHorizontal();
					GUILayout.FlexibleSpace();
					if (CurrentSelect.GetComponent<Renderer>() && !CurrentSelect.GetComponent<Terrain>() || NbrPartObj !=null && NbrPartObj.Length != 0 && !CurrentSelect.GetComponent<Terrain>())
					{
						if (terrainDat)
							terrainDat = null;
						GUILayout.Label(">>>>>>>> Object to T4M Terrain <<<<<<<<", EditorStyles.boldLabel);
					}
					else {
						if (!terrainDat && CurrentSelect.GetComponent<Terrain>())
							GetHeightmap();
						GUILayout.Label(">> UnityTerrain to T4M Terrain (Experimental) <<", EditorStyles.boldLabel);
					} 
					GUILayout.FlexibleSpace();
				GUILayout.EndHorizontal();
				EditorGUILayout.Space();
					GUILayout.Label("Name", EditorStyles.boldLabel);
				GUILayout.BeginHorizontal("box");
					
					 GUILayout.Label("(empty = Object Name)");
					terrainName = GUILayout.TextField (terrainName, 25, GUILayout.Width(155));
				GUILayout.EndHorizontal();
				
					if (CurrentSelect.GetComponent<Renderer>() && !CurrentSelect.GetComponent<Terrain>() || NbrPartObj !=null && NbrPartObj.Length != 0 && !CurrentSelect.GetComponent<Terrain>()){
						GUILayout.BeginHorizontal();
						GUILayout.Label("New Prefab", EditorStyles.boldLabel,GUILayout.Width(90));
						NewPref = EditorGUILayout.Toggle(NewPref,GUILayout.Width(53));
						GUILayout.EndHorizontal();
				
					}else{
						GUILayout.BeginHorizontal();
						GUILayout.Label("Keep the textures", EditorStyles.boldLabel,GUILayout.Width(150));
						keepTexture = EditorGUILayout.Toggle(keepTexture,GUILayout.Width(53));
						GUILayout.EndHorizontal();
						 GUILayout.Label("(Can keep the first 4 splats and first Blend)",GUILayout.Width(300));
					}
					
				if(CurrentSelect.GetComponent<Terrain>()){
					EditorGUILayout.Space();
					EditorGUILayout.Space();
					GUILayout.Label("T4M Quality", EditorStyles.boldLabel);
					
					
					EditorGUILayout.Space();
					EditorGUILayout.Space();
					GUILayout.BeginHorizontal();
					GUILayout.Label(" <" );
					GUILayout.FlexibleSpace();
					T4MResolution = EditorGUILayout.IntField(T4MResolution, GUILayout.Width(30));
					GUILayout.Label("x "+T4MResolution+ " : "+(X*Y).ToString()+ " Verts");
					GUILayout.FlexibleSpace();
					GUILayout.Label(" >" );
					GUILayout.EndHorizontal();
					GUILayout.BeginHorizontal();
						GUILayout.FlexibleSpace();
							T4MResolution= (int)GUILayout.HorizontalScrollbar (T4MResolution,0,32,350,GUILayout.Width(350));
						GUILayout.FlexibleSpace();
					GUILayout.EndHorizontal();	
					EditorGUILayout.Space();
					EditorGUILayout.Space();
					tRes = (HeightmapWidth)/T4MResolution;
					X = (int)((HeightmapWidth-1) / tRes + 1);
					Y = (int)((HeightmapHeight-1) / tRes + 1);
					EditorGUILayout.Space();
					EditorGUILayout.Space();
					GUILayout.Label("Vertex Performances (Approximate Indications)", EditorStyles.boldLabel);
					EditorGUILayout.Space();
					EditorGUILayout.Space();
					GUILayout.BeginHorizontal();
						GUILayout.Label("iPhone 3GS",GUILayout.Width(300));
						if(X*Y <= 15000)
							GUILayout.Label(AssetDatabase.LoadAssetAtPath(T4MEditorFolder+"Img/ok.png", typeof(Texture)) as Texture);
						else if(X*Y > 15000 && X*Y < 30000)
							GUILayout.Label(AssetDatabase.LoadAssetAtPath(T4MEditorFolder+"Img/avoid.png", typeof(Texture)) as Texture);
						else if(X*Y >= 30000)
							GUILayout.Label(AssetDatabase.LoadAssetAtPath(T4MEditorFolder+"Img/ko.png", typeof(Texture)) as Texture);
					GUILayout.EndHorizontal();
					GUILayout.BeginHorizontal();
						GUILayout.Label("iPad 1",GUILayout.Width(300));
						if(X*Y <= 15000)
							GUILayout.Label(AssetDatabase.LoadAssetAtPath(T4MEditorFolder+"Img/ok.png", typeof(Texture)) as Texture);
						else if(X*Y > 15000 && X*Y < 30000)
							GUILayout.Label(AssetDatabase.LoadAssetAtPath(T4MEditorFolder+"Img/avoid.png", typeof(Texture)) as Texture);
						else if(X*Y >= 30000)
							GUILayout.Label(AssetDatabase.LoadAssetAtPath(T4MEditorFolder+"Img/ko.png", typeof(Texture)) as Texture);
					GUILayout.EndHorizontal();
					GUILayout.BeginHorizontal();
						GUILayout.Label("iPhone 4",GUILayout.Width(300));
						if(X*Y <= 20000)
							GUILayout.Label(AssetDatabase.LoadAssetAtPath(T4MEditorFolder+"Img/ok.png", typeof(Texture)) as Texture);
						else if(X*Y > 20000 && X*Y < 40000)
							GUILayout.Label(AssetDatabase.LoadAssetAtPath(T4MEditorFolder+"Img/avoid.png", typeof(Texture)) as Texture);
						else if(X*Y >= 40000)
							GUILayout.Label(AssetDatabase.LoadAssetAtPath(T4MEditorFolder+"Img/ko.png", typeof(Texture)) as Texture);
					GUILayout.EndHorizontal();
					GUILayout.BeginHorizontal();
						GUILayout.Label("Tegra 2",GUILayout.Width(300));
						if(X*Y <= 20000)
							GUILayout.Label(AssetDatabase.LoadAssetAtPath(T4MEditorFolder+"Img/ok.png", typeof(Texture)) as Texture);
						else if(X*Y > 20000 && X*Y < 40000)
							GUILayout.Label(AssetDatabase.LoadAssetAtPath(T4MEditorFolder+"Img/avoid.png", typeof(Texture)) as Texture);
						else if(X*Y >= 40000)
							GUILayout.Label(AssetDatabase.LoadAssetAtPath(T4MEditorFolder+"Img/ko.png", typeof(Texture)) as Texture);
					GUILayout.EndHorizontal();
					GUILayout.BeginHorizontal();
						GUILayout.Label("iPad 2",GUILayout.Width(300));
						if(X*Y <= 25000)
							GUILayout.Label(AssetDatabase.LoadAssetAtPath(T4MEditorFolder+"Img/ok.png", typeof(Texture)) as Texture);
						else if(X*Y > 25000 && X*Y < 45000)
							GUILayout.Label(AssetDatabase.LoadAssetAtPath(T4MEditorFolder+"Img/avoid.png", typeof(Texture)) as Texture);
						else if(X*Y >= 45000)
							GUILayout.Label(AssetDatabase.LoadAssetAtPath(T4MEditorFolder+"Img/ko.png", typeof(Texture)) as Texture);
					GUILayout.EndHorizontal();
					GUILayout.BeginHorizontal();
						GUILayout.Label("iPhone 4S",GUILayout.Width(300));
						if(X*Y <= 25000)
							GUILayout.Label(AssetDatabase.LoadAssetAtPath(T4MEditorFolder+"Img/ok.png", typeof(Texture)) as Texture);
						else if(X*Y > 25000 && X*Y < 45000)
							GUILayout.Label(AssetDatabase.LoadAssetAtPath(T4MEditorFolder+"Img/avoid.png", typeof(Texture)) as Texture);
						else if(X*Y >= 45000)
							GUILayout.Label(AssetDatabase.LoadAssetAtPath(T4MEditorFolder+"Img/ko.png", typeof(Texture)) as Texture);
					GUILayout.EndHorizontal();
					
					GUILayout.BeginHorizontal();
						GUILayout.Label("Flash",GUILayout.Width(300));
						if(X*Y <= 45000)
							GUILayout.Label(AssetDatabase.LoadAssetAtPath(T4MEditorFolder+"Img/ok.png", typeof(Texture)) as Texture);
						else if(X*Y > 45000 && X*Y < 60000)
							GUILayout.Label(AssetDatabase.LoadAssetAtPath(T4MEditorFolder+"Img/avoid.png", typeof(Texture)) as Texture);
						else if(X*Y >= 60000)
							GUILayout.Label(AssetDatabase.LoadAssetAtPath(T4MEditorFolder+"Img/ko.png", typeof(Texture)) as Texture);
					GUILayout.EndHorizontal();
					GUILayout.BeginHorizontal();
						GUILayout.Label("Web",GUILayout.Width(300));
						GUILayout.Label(AssetDatabase.LoadAssetAtPath(T4MEditorFolder+"Img/ok.png", typeof(Texture)) as Texture);
					GUILayout.EndHorizontal();
					GUILayout.BeginHorizontal();
						GUILayout.Label("Desktop",GUILayout.Width(300));
						
							GUILayout.Label(AssetDatabase.LoadAssetAtPath(T4MEditorFolder+"Img/ok.png", typeof(Texture)) as Texture);
						
					GUILayout.EndHorizontal();
				
				}
				GUILayout.FlexibleSpace();
				GUILayout.BeginHorizontal();
				GUILayout.FlexibleSpace();
				GUILayout.Label("Can Take Some Time", EditorStyles.boldLabel);
				GUILayout.FlexibleSpace();
				GUILayout.EndHorizontal();
				
				GUILayout.BeginHorizontal();
					GUILayout.FlexibleSpace();
						if (CurrentSelect.GetComponent<Renderer>() && !CurrentSelect.GetComponent<Terrain>() || NbrPartObj !=null && NbrPartObj.Length != 0 && !CurrentSelect.GetComponent<Terrain>())
						{
							if (GUILayout.Button("PROCESS", GUILayout.Width(100), GUILayout.Height(30))) {
								Obj2T4M();
							}
						}
						else
						{
							if (GUILayout.Button("PROCESS", GUILayout.Width(100), GUILayout.Height(30))) {
								ConvertUTerrain();
							}
						
						}
					GUILayout.FlexibleSpace();
				GUILayout.EndHorizontal();
				GUILayout.FlexibleSpace();
			}
			else
			{
				terrainDat = null;
				GUILayout.BeginHorizontal();
					GUILayout.FlexibleSpace();
					if(CurrentSelect.GetComponent <T4MObjSC>())
						GUILayout.Label("Already T4M Object", EditorStyles.boldLabel);
					else GUILayout.Label("Can't convert that to T4M Object", EditorStyles.boldLabel);
					GUILayout.FlexibleSpace();
				GUILayout.EndHorizontal();
			}
		}
		else
		{
			GUILayout.Label("T4M Final Resolution : " , EditorStyles.boldLabel);
			if (partofT4MObj>1)
				GUILayout.Label("Vertex : ~"+vertexInfo+" in "+ partofT4MObj+" Parts" );
			else GUILayout.Label("Vertex : "+vertexInfo+" in "+ partofT4MObj+" Part" );
			GUILayout.Label("Triangle : "+trisInfo);
			EditorGUILayout.Space();
			EditorGUILayout.Space();
			GUILayout.BeginVertical("Box");
			GUILayout.Label("Since Unity 3.5, some converted objects can be ", EditorStyles.boldLabel);
			GUILayout.Label("no smooth : ", EditorStyles.boldLabel);
			EditorGUILayout.Space();
			GUILayout.Label("Select the New Mesh in the Project window :");
			GUILayout.Label("in T4MOBJ/Meshes/\"yourobject\"");
			EditorGUILayout.Space();
			GUILayout.Label("In Inspector window :");
			GUILayout.Label("Descrease \"Smoothing Angle\", Increase again to 180");
			GUILayout.Label("And \"Apply\"");
			EditorGUILayout.Space();
			GUILayout.Label("Now Select your Object on the scene :");
			GUILayout.Label("Uncheck/check the box the \"Mesh Collider\" in ");
			GUILayout.Label("Inspector window");
			GUILayout.EndVertical();
			EditorGUILayout.Space();
			EditorGUILayout.Space();
			if (GUILayout.Button("Keep my Conversion and Destroy Original")) {
				DestroyImmediate(CurrentSelect.gameObject);
				Selection.activeTransform = Child.transform;
				vertexInfo = 0;
				trisInfo = 0;
				partofT4MObj = 0;
				T4MMenuToolbar = 1;
				
				if (nbrT4MObj == 0){
					Child.gameObject.GetComponent <T4MObjSC>().EnabledLODSystem = ActivatedLOD;
					Child.gameObject.GetComponent <T4MObjSC>().enabledBillboard = ActivatedBillboard;
					Child.gameObject.GetComponent <T4MObjSC>().enabledLayerCul = ActivatedLayerCul;
					Child.gameObject.GetComponent <T4MObjSC>().CloseView = CloseDistMaxView;
					Child.gameObject.GetComponent <T4MObjSC>().NormalView = NormalDistMaxView;
					Child.gameObject.GetComponent <T4MObjSC>().FarView = FarDistMaxView;
					Child.gameObject.GetComponent <T4MObjSC>().BackGroundView = BGDistMaxView;
					Child.gameObject.GetComponent <T4MObjSC>().Master = 1;
				}
			}
			if (GUILayout.Button("Modify Options and Start a New Conversion")) {
				DestroyImmediate(Child);
				AssetDatabase.DeleteAsset (T4MPrefabFolder+"Terrains/Meshes/"+FinalExpName+".obj");
				AssetDatabase.DeleteAsset (T4MPrefabFolder+"Terrains/"+FinalExpName+".prefab");
				AssetDatabase.DeleteAsset (T4MPrefabFolder+"Terrains/Texture/"+FinalExpName+".png");
				AssetDatabase.DeleteAsset (T4MPrefabFolder+"Terrains/Material/"+FinalExpName+".mat");
				CurrentSelect.GetComponent <Terrain>().enabled = true;
				vertexInfo=0;
				trisInfo = 0;
				partofT4MObj=0;
				UnityTerrain = null;
				terrainDat = null;
			}
			if (GUILayout.Button("Keep Both and Continue")) {
				UnityTerrain.SetActive(false);
				UnityTerrain =null;
				Selection.activeTransform = Child.transform;
				vertexInfo = 0;
				trisInfo = 0;
				partofT4MObj = 0;
				T4MMenuToolbar = 1;
				
				if (nbrT4MObj == 0){
					Child.gameObject.GetComponent <T4MObjSC>().EnabledLODSystem = ActivatedLOD;
					Child.gameObject.GetComponent <T4MObjSC>().enabledBillboard = ActivatedBillboard;
					Child.gameObject.GetComponent <T4MObjSC>().enabledLayerCul = ActivatedLayerCul;
					Child.gameObject.GetComponent <T4MObjSC>().CloseView = CloseDistMaxView;
					Child.gameObject.GetComponent <T4MObjSC>().NormalView = NormalDistMaxView;
					Child.gameObject.GetComponent <T4MObjSC>().FarView = FarDistMaxView;
					Child.gameObject.GetComponent <T4MObjSC>().BackGroundView = BGDistMaxView;
					Child.gameObject.GetComponent <T4MObjSC>().Master = 1;
				}
			}
		}
	}
	
	void PainterMenu()
	{
		if (CurrentSelect.GetComponent <T4MObjSC>())
			{
			
			GUILayout.BeginHorizontal();
				GUILayout.FlexibleSpace();	
				MyT4MV = GUILayout.Toolbar(MyT4MV, MyT4MMen, GUILayout.Width(290), GUILayout.Height(20));
				GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			switch (MyT4MV)
			{
				case 0:
				if (CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.shader != Shader.Find("T4MShaders/ShaderModel2/Unlit/T4M World Projection Shader + LM") && 
					CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.shader != Shader.Find("T4MShaders/ShaderModel2/Diffuse/T4M World Projection Shader") &&
					CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.shader != Shader.Find("T4MShaders/ShaderModel2/MobileLM/T4M World Projection Shader_Mobile") && !CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.HasProperty("_Tiling")){
						PixelPainterMenu();
				}else ProjectionWorldConfig();
				break;
				case 1:
				if (CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial && CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.HasProperty("_Splat0") && CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.HasProperty("_Splat1")&& CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.HasProperty("_Control")){
			
				EditorGUILayout.Space();
				InitPincil();
				GUILayout.BeginHorizontal();
					GUILayout.FlexibleSpace();
						GUILayout.BeginHorizontal("box", GUILayout.Width(310));
						GUILayout.FlexibleSpace();
						
							selProcedural = GUILayout.SelectionGrid (selProcedural, TexTexture, 6 ,"gridlist",GUILayout.Width(340), GUILayout.Height(58));
						
						
						GUILayout.FlexibleSpace();
						GUILayout.EndHorizontal();
					GUILayout.FlexibleSpace();
				GUILayout.EndHorizontal();
				
				GUILayout.Label("Add / Replace / Substances Update" , EditorStyles.boldLabel);
					EditorGUILayout.BeginVertical("box");
					EditorGUILayout.BeginHorizontal();
						GUILayout.Label("",GUILayout.Width(3));
						 if(GUILayout.Button ((Texture)AssetDatabase.LoadAssetAtPath(T4MEditorFolder+"Img/up.png", typeof(Texture)),GUILayout.Width(53))) {
							if (!PreceduralAdd && !MaterialAdd && Precedural)
								PreceduralAdd = Precedural;
					
								if (PreceduralAdd){
								CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.SetTexture("_Splat0", PreceduralAdd.GetTexture ("_MainTex"));
								if (CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.HasProperty("_BumpSplat0"))
									CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.SetTexture("_BumpSplat0", PreceduralAdd.GetTexture ("_BumpMap"));
								}else if (MaterialAdd){
									CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.SetTexture("_Splat0", MaterialAdd);
								}
								selProcedural = 0;
								PreceduralAdd = null;
								MaterialAdd=null;
								IniNewSelect();
						}
						if(GUILayout.Button ((Texture)AssetDatabase.LoadAssetAtPath(T4MEditorFolder+"Img/up.png", typeof(Texture)),GUILayout.Width(53))) {
							if (!PreceduralAdd && !MaterialAdd && Precedural)
								PreceduralAdd = Precedural;
							if (PreceduralAdd){
							CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.SetTexture("_Splat1", PreceduralAdd.GetTexture ("_MainTex"));
							if (CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.HasProperty("_BumpSplat1"))
								CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.SetTexture("_BumpSplat1", PreceduralAdd.GetTexture ("_BumpMap"));
							}else if (MaterialAdd){
									CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.SetTexture("_Splat1", MaterialAdd);
								}
								selProcedural = 1;
								PreceduralAdd = null;
								MaterialAdd=null;
								IniNewSelect();
						}
						if (CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.HasProperty("_Splat2"))
							if(GUILayout.Button ((Texture)AssetDatabase.LoadAssetAtPath(T4MEditorFolder+"Img/up.png", typeof(Texture)),GUILayout.Width(53))) {
								if (!PreceduralAdd && !MaterialAdd && Precedural)
								PreceduralAdd = Precedural;
								if (PreceduralAdd){
									CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.SetTexture("_Splat2", PreceduralAdd.GetTexture ("_MainTex"));
									if (CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.HasProperty("_BumpSplat2"))
										CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.SetTexture("_BumpSplat2", PreceduralAdd.GetTexture ("_BumpMap"));
								}else if (MaterialAdd){
									CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.SetTexture("_Splat2", MaterialAdd);
								}
								selProcedural = 2;
								PreceduralAdd = null;
								MaterialAdd=null;
								IniNewSelect();
							}
						if (CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.HasProperty("_Splat3"))
							if(GUILayout.Button ((Texture)AssetDatabase.LoadAssetAtPath(T4MEditorFolder+"Img/up.png", typeof(Texture)),GUILayout.Width(53))) {
								if (!PreceduralAdd && !MaterialAdd && Precedural)
								PreceduralAdd = Precedural;
								if (PreceduralAdd){
									CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.SetTexture("_Splat3", PreceduralAdd.GetTexture ("_MainTex"));
									if (CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.HasProperty("_BumpSplat3"))
										CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.SetTexture("_BumpSplat3", PreceduralAdd.GetTexture ("_BumpMap"));
								}else if (MaterialAdd){
									CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.SetTexture("_Splat3", MaterialAdd);
								}
								selProcedural = 3;
								PreceduralAdd = null;
								MaterialAdd=null;
								IniNewSelect();
							}
						if (CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.HasProperty("_Splat4"))
							if(GUILayout.Button ((Texture)AssetDatabase.LoadAssetAtPath(T4MEditorFolder+"Img/up.png", typeof(Texture)),GUILayout.Width(53))) {
								if (!PreceduralAdd && !MaterialAdd && Precedural)
								PreceduralAdd = Precedural;
					
								if (PreceduralAdd){
									CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.SetTexture("_Splat4", PreceduralAdd.GetTexture ("_MainTex"));
								}else if (MaterialAdd){
									CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.SetTexture("_Splat4", MaterialAdd);
								}
								selProcedural = 4;
								PreceduralAdd = null;
								MaterialAdd=null;
								IniNewSelect();
							}
						if (CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.HasProperty("_Splat5"))
							if(GUILayout.Button ((Texture)AssetDatabase.LoadAssetAtPath(T4MEditorFolder+"Img/up.png", typeof(Texture)),GUILayout.Width(53))) {
								if (!PreceduralAdd && !MaterialAdd && Precedural)
								PreceduralAdd = Precedural;
					
								if (PreceduralAdd){
									CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.SetTexture("_Splat5", PreceduralAdd.GetTexture ("_MainTex"));
							}else if (MaterialAdd){
									CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.SetTexture("_Splat5", MaterialAdd);
							}
							selProcedural = 5;
							PreceduralAdd = null;
							MaterialAdd=null;
							IniNewSelect();
						}
						
				EditorGUILayout.EndHorizontal();
				
				
				string AssetName= AssetDatabase.GetAssetPath(CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.GetTexture("_Splat"+selProcedural)) as string;
				
				
				SubstanceImporter SubstanceI= AssetImporter.GetAtPath (AssetName) as SubstanceImporter;
				
				if (SubstanceI){
					
				 	ProceduralMaterial[] ProcMat = SubstanceI.GetMaterials() as  ProceduralMaterial[];
					
					for (int i = 0; i<ProcMat.Length;i++){
						if (ProcMat[i].name+"_Diffuse" == CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.GetTexture("_Splat"+selProcedural).name){
							Precedural = ProcMat[i];
							//SubstanceI.SetTextureAlphaSource(Precedural, Precedural.name+"_Diffuse", ProceduralOutputType.Diffuse);
						}
					}
				}else Precedural = null;
				
				EditorGUILayout.Space();
				EditorGUILayout.Space();
				
				
					MaterialTyp =(MaterialType) EditorGUILayout.EnumPopup ("Material Type", MaterialTyp, GUILayout.Width(340));
					EditorGUILayout.BeginHorizontal();
					
					if (MaterialTyp != MaterialType.Classic){
						GUILayout.Label("Substances To Add : ");
						MaterialAdd = null;
						PreceduralAdd = EditorGUILayout.ObjectField(PreceduralAdd, typeof(ProceduralMaterial),true, GUILayout.Width(220)) as ProceduralMaterial;
					}
					else{ 
						GUILayout.Label("Texture To Add : ");
						PreceduralAdd = null;
						MaterialAdd = EditorGUILayout.ObjectField(MaterialAdd, typeof(Texture2D),true, GUILayout.Width(220)) as Texture;
				}
				
				
				GUILayout.FlexibleSpace();
				
				EditorGUILayout.EndVertical();
				EditorGUILayout.Space();
				EditorGUILayout.EndHorizontal();
				
				EditorGUILayout.Space();
				
				if (Precedural){
					GUILayout.Label("Modify" , EditorStyles.boldLabel);
					EditorGUILayout.BeginHorizontal("box");
						GUILayout.FlexibleSpace();
							scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width (350), GUILayout.Height (296));
							Substance();
							EditorGUILayout.EndScrollView();
						GUILayout.FlexibleSpace();
					 EditorGUILayout.EndHorizontal();
				}else{
					
					ClassicMat();	
					
				}
				}
				break;
				
			}
				
		}else
		{
			GUILayout.BeginHorizontal();
					GUILayout.FlexibleSpace();
					GUILayout.Label("Please, select the T4M Object", EditorStyles.boldLabel);
					GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			
		}
	}
	void ClassicMat(){
			
			if (selProcedural == 0){
				if(Layer1){
					GUILayout.Label("Modify" , EditorStyles.boldLabel);
					GUILayout.BeginHorizontal("Box");
					GUILayout.Label((Texture)AssetDatabase.LoadAssetAtPath(T4MEditorFolder+"Img/TDiff.jpg", typeof(Texture)));
					Layer1=EditorGUILayout.ObjectField(Layer1, typeof(Texture2D),true, GUILayout.Width(75), GUILayout.Height(75)) as Texture;
					CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.SetTexture("_Splat0",Layer1);
					if (CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.HasProperty("_BumpSplat0")){
						GUILayout.Label((Texture)AssetDatabase.LoadAssetAtPath(T4MEditorFolder+"Img/TBump.jpg", typeof(Texture)));
						Layer1Bump=EditorGUILayout.ObjectField(Layer1Bump, typeof(Texture2D),true, GUILayout.Width(75), GUILayout.Height(75)) as Texture;
						CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.SetTexture("_BumpSplat0",Layer1Bump);
					}
				GUILayout.FlexibleSpace();
				GUILayout.EndHorizontal();
				}
				
			}else if (selProcedural == 1){
				if(Layer2){
					GUILayout.Label("Modify" , EditorStyles.boldLabel);
					GUILayout.BeginHorizontal("Box");
					GUILayout.Label((Texture)AssetDatabase.LoadAssetAtPath(T4MEditorFolder+"Img/TDiff.jpg", typeof(Texture)));
					Layer2=EditorGUILayout.ObjectField(Layer2, typeof(Texture2D),true, GUILayout.Width(75), GUILayout.Height(75)) as Texture;
					CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.SetTexture("_Splat1",Layer2);
					if (CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.HasProperty("_BumpSplat1")){
						GUILayout.Label((Texture)AssetDatabase.LoadAssetAtPath(T4MEditorFolder+"Img/TBump.jpg", typeof(Texture)));
						Layer2Bump=EditorGUILayout.ObjectField(Layer2Bump, typeof(Texture2D),true, GUILayout.Width(75), GUILayout.Height(75)) as Texture;
						CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.SetTexture("_BumpSplat1",Layer2Bump);
					}
				GUILayout.FlexibleSpace();
				GUILayout.EndHorizontal();
				}
				
			}else if (selProcedural == 2){
				if(Layer3){
					GUILayout.Label("Modify" , EditorStyles.boldLabel);
					GUILayout.BeginHorizontal("Box");
					GUILayout.Label((Texture)AssetDatabase.LoadAssetAtPath(T4MEditorFolder+"Img/TDiff.jpg", typeof(Texture)));
					Layer3=EditorGUILayout.ObjectField(Layer3, typeof(Texture2D),true, GUILayout.Width(75), GUILayout.Height(75)) as Texture;
					CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.SetTexture("_Splat2",Layer3);
					if (CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.HasProperty("_BumpSplat2")){
						GUILayout.Label((Texture)AssetDatabase.LoadAssetAtPath(T4MEditorFolder+"Img/TBump.jpg", typeof(Texture)));
						Layer3Bump=EditorGUILayout.ObjectField(Layer3Bump, typeof(Texture2D),true, GUILayout.Width(75), GUILayout.Height(75)) as Texture;
						CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.SetTexture("_BumpSplat2",Layer3Bump);
					}
					GUILayout.FlexibleSpace();
					GUILayout.EndHorizontal();
				}
				
			}else if (selProcedural == 3){
				if(Layer4){
					GUILayout.Label("Modify" , EditorStyles.boldLabel);
					GUILayout.BeginHorizontal("Box");
					GUILayout.Label((Texture)AssetDatabase.LoadAssetAtPath(T4MEditorFolder+"Img/TDiff.jpg", typeof(Texture)));
					Layer4=EditorGUILayout.ObjectField(Layer4, typeof(Texture2D),true, GUILayout.Width(75), GUILayout.Height(75)) as Texture;
					CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.SetTexture("_Splat3",Layer4);
					if (CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.HasProperty("_BumpSplat3")){
						GUILayout.Label((Texture)AssetDatabase.LoadAssetAtPath(T4MEditorFolder+"Img/TBump.jpg", typeof(Texture)));
						Layer4Bump=EditorGUILayout.ObjectField(Layer4Bump, typeof(Texture2D),true, GUILayout.Width(75), GUILayout.Height(75)) as Texture;
						CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.SetTexture("_BumpSplat3",Layer4Bump);
						
					}
						GUILayout.FlexibleSpace();
						GUILayout.EndHorizontal();
				}
				
			}else if (selProcedural == 4){
				if(Layer5){	
					GUILayout.Label("Modify" , EditorStyles.boldLabel);
					GUILayout.BeginHorizontal("Box");
					GUILayout.Label((Texture)AssetDatabase.LoadAssetAtPath(T4MEditorFolder+"Img/TDiff.jpg", typeof(Texture)));
					Layer5=EditorGUILayout.ObjectField(Layer5, typeof(Texture2D),true, GUILayout.Width(75), GUILayout.Height(75)) as Texture;
					CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.SetTexture("_Splat4",Layer5);
					GUILayout.FlexibleSpace();
					GUILayout.EndHorizontal();
				}
			}else if (selProcedural == 5){
				if(Layer6){
					GUILayout.Label("Modify" , EditorStyles.boldLabel);
					GUILayout.BeginHorizontal("Box");
					GUILayout.Label((Texture)AssetDatabase.LoadAssetAtPath(T4MEditorFolder+"Img/TDiff.jpg", typeof(Texture)));
					Layer6=EditorGUILayout.ObjectField(Layer6, typeof(Texture2D),true, GUILayout.Width(75), GUILayout.Height(75)) as Texture;
					CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.SetTexture("_Splat5",Layer6);
				}
			}
		
			if(CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.shader == Shader.Find("T4MShaders/ShaderModel1/T4M 2 Textures ManualAdd BeastLM_1DC") || CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.shader == Shader.Find("T4MShaders/ShaderModel1/T4M 2 Textures ManualAdd CustoLM 1DC")){
				GUILayout.Label("Manual Lightmap Add" , EditorStyles.boldLabel);
				GUILayout.BeginHorizontal("Box");
				GUILayout.Label((Texture)AssetDatabase.LoadAssetAtPath(T4MEditorFolder+"Img/TLM.jpg", typeof(Texture)));
				LMMan=EditorGUILayout.ObjectField(LMMan, typeof(Texture2D),true, GUILayout.Width(75), GUILayout.Height(75)) as Texture;
				CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.SetTexture("_Lightmap",LMMan);
				GUILayout.FlexibleSpace();
				GUILayout.EndHorizontal();
			}
	}
	
	void ProjectionWorldConfig(){
					if (UpSideTile.x != UpSideTile.y && joinTiles==true || UpSideTile.z != UpSideTile.w && joinTiles==true){
						joinTiles = false;
					}	
					EditorGUILayout.Space();
					GUILayout.Label("Painting Menu is not available for this shader" , EditorStyles.boldLabel);
					EditorGUILayout.Space();
					EditorGUILayout.Space();
					GUILayout.Label("World Projection Shaders Options", EditorStyles.boldLabel);
					EditorGUILayout.Space();
					UpSideF = EditorGUILayout.Slider("UP/SIDES Fighting :",UpSideF,0,10);
					BlendFac= EditorGUILayout.Slider("Blend Factor :",BlendFac,0,20);
					EditorGUILayout.Space();
					EditorGUILayout.Space();
					joinTiles = EditorGUILayout.Toggle ("Tiling : Join X/Y", joinTiles);
					EditorGUILayout.Space();
					if (joinTiles){
						UpSideTile.x =UpSideTile.y= EditorGUILayout.Slider("Up Texture Tiling :",UpSideTile.x,0.01f,10);
						UpSideTile.z =UpSideTile.w =EditorGUILayout.Slider("Side Tecture Tiling :",UpSideTile.z,0.01f,10);
					}else{
						UpSideTile.x= EditorGUILayout.Slider("Up Texture Tiling X:",UpSideTile.x,0.01f,2);
						UpSideTile.y= EditorGUILayout.Slider("Up Texture Tiling Y:",UpSideTile.y,0.01f,2);
						UpSideTile.z =EditorGUILayout.Slider("Side Tecture Tiling X:",UpSideTile.z,0.01f,2);
						UpSideTile.w =EditorGUILayout.Slider("Side Tecture Tiling Y:",UpSideTile.w,0.01f,2);
					}
					CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.SetVector ("_Tiling",new  Vector4(UpSideTile.x,UpSideTile.y, UpSideTile.z, UpSideTile.w));
					CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.SetFloat ("_UpSide",UpSideF);
					CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.SetFloat ("_Blend",BlendFac);
	}
	
	void Substance()
	{
		
			var inputs = Precedural.GetProceduralPropertyDescriptions ();
			
			for (int i   = 0; i < inputs.Length; i++) {
				var input = inputs[i];
				var type = input.type;
								
				if (type == ProceduralPropertyType.Boolean) {
					var inputBool = Precedural.GetProceduralBoolean (input.name);
					var oldInputBool = inputBool;
					inputBool = EditorGUILayout.Toggle ( input.name,inputBool);
					if (inputBool != oldInputBool)
						Precedural.SetProceduralBoolean (input.name, inputBool);
				}
				
				else if (type == ProceduralPropertyType.Float) {
					if (input.hasRange) {
					
						GUILayout.Label (input.name, EditorStyles.boldLabel);
						
						var inputFloat = Precedural.GetProceduralFloat (input.name);
						var oldInputFloat = inputFloat;
						
						inputFloat = EditorGUILayout.Slider (inputFloat, input.minimum, input.maximum);
						if (inputFloat != oldInputFloat)
							Precedural.SetProceduralFloat (input.name, inputFloat);
					}
				}
				
				else if (type == ProceduralPropertyType.Vector2 ||
					type == ProceduralPropertyType.Vector3 ||
					type == ProceduralPropertyType.Vector4
				) {
					
					if (input.hasRange) {
						GUILayout.Label (input.name, EditorStyles.boldLabel);
						
						
						var vectorComponentAmount = 4;
						if (type == ProceduralPropertyType.Vector2) vectorComponentAmount = 2;
						if (type == ProceduralPropertyType.Vector3) vectorComponentAmount = 3;
						
						var inputVector = Precedural.GetProceduralVector (input.name);
						var oldInputVector = inputVector;
						
						
						for (int c  = 0; c < vectorComponentAmount; c++)
							inputVector[c] = EditorGUILayout.Slider (
								inputVector[c], input.minimum, input.maximum);
						
						if (inputVector != oldInputVector)
							Precedural.SetProceduralVector (input.name, inputVector);
					}
				}
				
				else if (type == ProceduralPropertyType.Color3 || type == ProceduralPropertyType.Color4) {
					GUILayout.Label (input.name, EditorStyles.boldLabel);
					
					
					
					
					var colorInput = Precedural.GetProceduralColor (input.name);
					var oldColorInput = colorInput;
					
					colorInput =EditorGUILayout.ColorField("Shader Color", colorInput);
					
					if (colorInput != oldColorInput)
						Precedural.SetProceduralColor (input.name, colorInput);
				}
				
				
				else if (type == ProceduralPropertyType.Enum) {
					GUILayout.Label (input.name, EditorStyles.boldLabel);
					
					var enumInput = Precedural.GetProceduralEnum (input.name);
					var oldEnumInput = enumInput;
					var enumOptions = input.enumOptions;
					
					enumInput = GUILayout.SelectionGrid (enumInput, enumOptions, 1);
					if (enumInput != oldEnumInput)
						Precedural.SetProceduralEnum (input.name, enumInput);
				}
			}
			Precedural.RebuildTexturesImmediately();

		
	
	}
	
	void MyT4M()
	{
		if (CurrentSelect.GetComponent (typeof(T4MObjSC)))
		{
			GUILayout.BeginHorizontal();
				GUILayout.FlexibleSpace();	
				EnumMyT4MV = GUILayout.Toolbar(EnumMyT4MV, EnumMyT4M, GUILayout.Width(290), GUILayout.Height(20));
				GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
					
			
			GUILayout.Label("Cleaning Scene", EditorStyles.boldLabel);
			GUILayout.BeginHorizontal();
				GUILayout.FlexibleSpace();
				if (GUILayout.Button("Cleaning Now", GUILayout.Width(200), GUILayout.Height(20))) {
					MeshRenderer[] prev = GameObject.FindObjectsOfType(typeof(MeshRenderer)) as MeshRenderer[];
					foreach(MeshRenderer go in prev)
					{
						if(go.hideFlags == HideFlags.HideInHierarchy)
						{
							go.hideFlags=0;
							DestroyImmediate (go.gameObject);
						}
					}
					EditorUtility.DisplayDialog("Scene Cleaned", "", "OK");
				}
				GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
				EditorGUILayout.Space();
			switch (EnumMyT4MV)
			{
				case 0:
				GUILayout.Label("Shader Model", EditorStyles.boldLabel);
				
				ShaderModel =(SM) EditorGUILayout.EnumPopup ("Shader Model", ShaderModel, GUILayout.Width(340));
				
				
				if (ShaderModel == SM.ShaderModel1){
					MenuTextureSM1 =(EnumShaderGLES1) EditorGUILayout.EnumPopup ("Shader", MenuTextureSM1, GUILayout.Width(340));
				}else if (ShaderModel == SM.ShaderModel2){	
					MenuTextureSM2 =(EnumShaderGLES2) EditorGUILayout.EnumPopup ("Shader", MenuTextureSM2, GUILayout.Width(340));
				}else if (ShaderModel == SM.ShaderModel3)
					MenuTextureSM3 =(EnumShaderGLES3) EditorGUILayout.EnumPopup ("Shader", MenuTextureSM3, GUILayout.Width(340));
				else CustomShader=EditorGUILayout.ObjectField("Select your Shader",CustomShader, typeof(Shader),true, GUILayout.Width(350)) as Shader;
				EditorGUILayout.Space();
				
		
				if (ShaderModel != SM.CustomShader){
				GUILayout.Label("Shader Compatibility", EditorStyles.boldLabel);
						GUILayout.BeginHorizontal();
							GUILayout.Label("GLES 1.1",GUILayout.Width(300));
							if(ShaderModel != SM.ShaderModel3 && ShaderModel != SM.ShaderModel2)
								GUILayout.Label(AssetDatabase.LoadAssetAtPath(T4MEditorFolder+"Img/ok.png", typeof(Texture)) as Texture);
							else
								GUILayout.Label(AssetDatabase.LoadAssetAtPath(T4MEditorFolder+"Img/ko.png", typeof(Texture)) as Texture);
						GUILayout.EndHorizontal();
						GUILayout.BeginHorizontal();
							GUILayout.Label("GLES 2",GUILayout.Width(300));
							if((ShaderModel == SM.ShaderModel1)|| (ShaderModel != SM.ShaderModel3) && (ShaderModel == SM.ShaderModel2 && MenuTextureSM2 != EnumShaderGLES2.T4M_6_Textures_HighSpec)&& (ShaderModel == SM.ShaderModel2 && MenuTextureSM2 != EnumShaderGLES2.T4M_4_Textures_Bumped)
									&& (ShaderModel == SM.ShaderModel2 && MenuTextureSM2 != EnumShaderGLES2.T4M_5_Textures_HighSpec )&& (ShaderModel == SM.ShaderModel2 && MenuTextureSM2 != EnumShaderGLES2.T4M_3_Textures_Bumped_DirectionalLM)&& ( ShaderModel == SM.ShaderModel2 && MenuTextureSM2 != EnumShaderGLES2.T4M_6_Textures_Unlit_Lightmap_Compatible)
									&& (ShaderModel == SM.ShaderModel2 && MenuTextureSM2 != EnumShaderGLES2.T4M_World_Projection_HighSpec)
									&& (ShaderModel == SM.ShaderModel2 &&MenuTextureSM2 != EnumShaderGLES2.T4M_World_Projection_Unlit_Lightmap_Compatible)){
						
								GUILayout.Label(AssetDatabase.LoadAssetAtPath(T4MEditorFolder+"Img/ok.png", typeof(Texture)) as Texture);
							}else{
								GUILayout.Label(AssetDatabase.LoadAssetAtPath(T4MEditorFolder+"Img/ko.png", typeof(Texture)) as Texture);
							}
						GUILayout.EndHorizontal();
						GUILayout.BeginHorizontal();
							GUILayout.Label("Desktop",GUILayout.Width(300));
								GUILayout.Label(AssetDatabase.LoadAssetAtPath(T4MEditorFolder+"Img/ok.png", typeof(Texture)) as Texture);
						GUILayout.EndHorizontal();
						GUILayout.BeginHorizontal();
							GUILayout.Label("Unity WebPlayer",GUILayout.Width(300));
							GUILayout.Label(AssetDatabase.LoadAssetAtPath(T4MEditorFolder+"Img/ok.png", typeof(Texture)) as Texture);
							
						GUILayout.EndHorizontal();
						GUILayout.BeginHorizontal();
							GUILayout.Label("Flash",GUILayout.Width(300));
							if((ShaderModel == SM.ShaderModel1)|| (ShaderModel != SM.ShaderModel3) && (ShaderModel == SM.ShaderModel2 && MenuTextureSM2 != EnumShaderGLES2.T4M_6_Textures_HighSpec)
									&& (ShaderModel == SM.ShaderModel2 && MenuTextureSM2 != EnumShaderGLES2.T4M_5_Textures_HighSpec) && (ShaderModel == SM.ShaderModel2 && MenuTextureSM2 != EnumShaderGLES2.T4M_4_Textures_Bumped)
									&& (ShaderModel == SM.ShaderModel2 && MenuTextureSM2 != EnumShaderGLES2.T4M_3_Textures_Bumped)
									&& (ShaderModel == SM.ShaderModel2 && MenuTextureSM2 != EnumShaderGLES2.T4M_3_Textures_Bumped_SPEC)
									&& (ShaderModel == SM.ShaderModel2 && MenuTextureSM2 != EnumShaderGLES2.T4M_6_Textures_Unlit_Lightmap_Compatible)
									&& (ShaderModel == SM.ShaderModel2 && MenuTextureSM2 != EnumShaderGLES2.T4M_World_Projection_HighSpec)
									&& (ShaderModel == SM.ShaderModel2 &&MenuTextureSM2 != EnumShaderGLES2.T4M_World_Projection_Unlit_Lightmap_Compatible)
									)
								GUILayout.Label(AssetDatabase.LoadAssetAtPath(T4MEditorFolder+"Img/ok.png", typeof(Texture)) as Texture);
							else
								GUILayout.Label(AssetDatabase.LoadAssetAtPath(T4MEditorFolder+"Img/ko.png", typeof(Texture)) as Texture);
						GUILayout.EndHorizontal();
						GUILayout.BeginHorizontal();
							GUILayout.Label("NaCI",GUILayout.Width(300));
							if((ShaderModel == SM.ShaderModel1)|| (ShaderModel != SM.ShaderModel3) && (ShaderModel == SM.ShaderModel2 && MenuTextureSM2 != EnumShaderGLES2.T4M_6_Textures_HighSpec)
									&& (ShaderModel == SM.ShaderModel2 && MenuTextureSM2 != EnumShaderGLES2.T4M_5_Textures_HighSpec) && (ShaderModel == SM.ShaderModel2 && MenuTextureSM2 != EnumShaderGLES2.T4M_4_Textures_Bumped)&& ( ShaderModel == SM.ShaderModel2 && MenuTextureSM2 != EnumShaderGLES2.T4M_6_Textures_Unlit_Lightmap_Compatible))
								GUILayout.Label(AssetDatabase.LoadAssetAtPath(T4MEditorFolder+"Img/ok.png", typeof(Texture)) as Texture);
							else
								GUILayout.Label(AssetDatabase.LoadAssetAtPath(T4MEditorFolder+"Img/ko.png", typeof(Texture)) as Texture);
						GUILayout.EndHorizontal();
						
					}
				
				EditorGUILayout.Space();
				
				
				GUILayout.BeginHorizontal();
				GUILayout.Label("Master T4M Object", EditorStyles.boldLabel, GUILayout.Width(150));
				
				
				T4MMaster = EditorGUILayout.Toggle(T4MMaster);
				
				GUILayout.EndHorizontal();
				
					if(T4MMaster){
						GUILayout.BeginVertical("box");	
						
							GUILayout.BeginHorizontal();
							GUILayout.Label("Scene Camera", EditorStyles.boldLabel, GUILayout.Width(190));
							PlayerCam= EditorGUILayout.ObjectField(PlayerCam, typeof(Transform),true) as Transform;
						GUILayout.EndHorizontal();
						GUILayout.BeginHorizontal();
							GUILayout.Label("Activate LOD System  ", EditorStyles.boldLabel, GUILayout.Width(190));
							ActivatedLOD = EditorGUILayout.Toggle(ActivatedLOD);
							GUILayout.Label("   Editor Preview", EditorStyles.boldLabel, GUILayout.Width(120));
							CurrentSelect.GetComponent<T4MObjSC>().LODPreview = EditorGUILayout.Toggle(CurrentSelect.GetComponent<T4MObjSC>().LODPreview);
							
						GUILayout.EndHorizontal();
						
						GUILayout.BeginHorizontal();
							GUILayout.Label("Activate Billboard System  ", EditorStyles.boldLabel, GUILayout.Width(190));
							ActivatedBillboard = EditorGUILayout.Toggle(ActivatedBillboard);
							GUILayout.Label("   Editor Preview", EditorStyles.boldLabel, GUILayout.Width(120));
							CurrentSelect.GetComponent<T4MObjSC>().BillboardPreview = EditorGUILayout.Toggle(CurrentSelect.GetComponent<T4MObjSC>().BillboardPreview);
						GUILayout.EndHorizontal();
						
						GUILayout.BeginHorizontal();
							GUILayout.Label("Activate LayerCullDistance  ", EditorStyles.boldLabel, GUILayout.Width(190));
							ActivatedLayerCul = EditorGUILayout.Toggle(ActivatedLayerCul);
							GUILayout.Label("   Editor Preview", EditorStyles.boldLabel, GUILayout.Width(120));
							CurrentSelect.GetComponent<T4MObjSC>().LayerCullPreview = EditorGUILayout.Toggle(CurrentSelect.GetComponent<T4MObjSC>().LayerCullPreview);
						GUILayout.EndHorizontal();
						EditorGUILayout.Space();
						if (ActivatedLayerCul){
							GUILayout.BeginVertical("box");
						
								GUILayout.Label("Maximum distances of view", EditorStyles.boldLabel, GUILayout.Width(220));
								CloseDistMaxView = EditorGUILayout.Slider("Close Distance",CloseDistMaxView,0,500);	
								NormalDistMaxView = EditorGUILayout.Slider("Middle Distance",NormalDistMaxView,0,500);	
								FarDistMaxView = EditorGUILayout.Slider("Far Distance",FarDistMaxView,0,500);
								BGDistMaxView = EditorGUILayout.Slider("BackGround Distance",BGDistMaxView,0,10000);
							GUILayout.EndVertical();
						} 
						
						if (BGDistMaxView<FarDistMaxView)
							BGDistMaxView = FarDistMaxView;
						else if (FarDistMaxView<NormalDistMaxView)
							FarDistMaxView = NormalDistMaxView;
						else if (NormalDistMaxView<CloseDistMaxView)
							NormalDistMaxView = CloseDistMaxView;
					GUILayout.EndVertical();
					}
						GUILayout.FlexibleSpace();
							GUILayout.BeginHorizontal();
								GUILayout.FlexibleSpace();
										if (GUILayout.Button("UPDATE", GUILayout.Width(100), GUILayout.Height(25))) {
											MyT4MApplyChange();
							
										}
								GUILayout.FlexibleSpace();
							GUILayout.EndHorizontal();
						GUILayout.FlexibleSpace();
				break;	
				case 1:
				EditorGUILayout.Space();
					GUILayout.BeginHorizontal();
						GUILayout.Label("ATS Foliage Wind Activation", EditorStyles.boldLabel, GUILayout.Width(220));
						CurrentSelect.gameObject.GetComponent <T4MObjSC>().ActiveWind= EditorGUILayout.Toggle(CurrentSelect.gameObject.GetComponent <T4MObjSC>().ActiveWind);
					GUILayout.EndHorizontal();
				EditorGUILayout.Space();
				GUILayout.BeginVertical("box");	
					EditorGUILayout.Space();
					GUILayout.BeginHorizontal();
						GUILayout.FlexibleSpace();
							if (GUILayout.Button("Download The Package",  GUILayout.Width(160), GUILayout.Height(15))) {
									Application.OpenURL ("http://u3d.as/content/forst/ats-mobile-foliage/2XM");
							}
						GUILayout.FlexibleSpace();
					GUILayout.EndHorizontal();
					EditorGUILayout.Space();
					EditorGUILayout.Space();
					EditorGUILayout.Space();
					CurrentSelect.gameObject.GetComponent <T4MObjSC>().TranslucencyColor= EditorGUILayout.ColorField ("Translucency Color ",CurrentSelect.gameObject.GetComponent <T4MObjSC>().TranslucencyColor);
					EditorGUILayout.Space();
					CurrentSelect.gameObject.GetComponent <T4MObjSC>().Wind = EditorGUILayout.Vector4Field("Wind Vector",CurrentSelect.gameObject.GetComponent <T4MObjSC>().Wind);
					EditorGUILayout.Space();
					CurrentSelect.gameObject.GetComponent <T4MObjSC>().WindFrequency = EditorGUILayout.Slider("Wind Frequency",CurrentSelect.gameObject.GetComponent <T4MObjSC>().WindFrequency,0,5);
					EditorGUILayout.Space();
					CurrentSelect.gameObject.GetComponent <T4MObjSC>().GrassWindFrequency = EditorGUILayout.Slider("Grass Wind Frequency",CurrentSelect.gameObject.GetComponent <T4MObjSC>().GrassWindFrequency,0,5);
					EditorGUILayout.Space();
					GUILayout.BeginHorizontal();
						GUILayout.FlexibleSpace();
							if (GUILayout.Button("Reset", GUILayout.Width(100), GUILayout.Height(15))) {
								CurrentSelect.gameObject.GetComponent <T4MObjSC>().TranslucencyColor= new Color(0.73f,0.85f,0.4f,1f);
								CurrentSelect.gameObject.GetComponent <T4MObjSC>().Wind = new Vector4(0.85f,0.075f,0.4f,0.5f);
								CurrentSelect.gameObject.GetComponent <T4MObjSC>().WindFrequency = 0.75f;
								CurrentSelect.gameObject.GetComponent <T4MObjSC>().GrassWindFrequency = 1.5f;
							}
					GUILayout.EndHorizontal();
				GUILayout.EndVertical();	
				GUILayout.FlexibleSpace();
							GUILayout.BeginHorizontal();
								GUILayout.FlexibleSpace();
										if (GUILayout.Button("UPDATE", GUILayout.Width(100), GUILayout.Height(30))) {
											MyT4MApplyChange();
							
										}
								GUILayout.FlexibleSpace();
							GUILayout.EndHorizontal();
				GUILayout.FlexibleSpace();	
					GUILayout.BeginHorizontal();
						GUILayout.FlexibleSpace();
							GUILayout.Label("By Forst (Lars)", EditorStyles.boldLabel, GUILayout.Width(105), GUILayout.Height(15));
					GUILayout.EndHorizontal();
					GUILayout.BeginHorizontal();
						GUILayout.FlexibleSpace();
							if (GUILayout.Button("Others Assets by Forst", "textarea", GUILayout.Width(140), GUILayout.Height(15))) {
								Application.OpenURL ("http://u3d.as/publisher/forst/1Lf");
							}
				GUILayout.EndHorizontal();
				EditorGUILayout.Space();
				break;
			}
		}else{
			GUILayout.BeginHorizontal();
					GUILayout.FlexibleSpace();
					GUILayout.Label("Please, select the T4M Object", EditorStyles.boldLabel);
					GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
		}
	}
	
	void MyT4MApplyChange()
	{
		if (ShaderModel == SM.ShaderModel1){
			//Diffuse SM1 
			if (MenuTextureSM1 == EnumShaderGLES1.T4M_2_Textures_Auto_BeastLM_2DrawCall){	
					CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.shader =  Shader.Find("T4MShaders/ShaderModel1/T4M 2 Textures Auto BeastLM 2DrawCall");
			}
			else 
			if (MenuTextureSM1 == EnumShaderGLES1.T4M_2_Textures_ManualAdd_BeastLM_1DC){	
					CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.shader =  Shader.Find("T4MShaders/ShaderModel1/T4M 2 Textures ManualAdd BeastLM_1DC");
			}
			else 
			if (MenuTextureSM1 == EnumShaderGLES1.T4M_2_Textures_ManualAdd_CustoLM_1DC){	
					CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.shader =  Shader.Find("T4MShaders/ShaderModel1/T4M 2 Textures ManualAdd CustoLM 1DC");
			}
		}else if (ShaderModel == SM.ShaderModel2){
				//Unlit SM2
				if (MenuTextureSM2 == EnumShaderGLES2.T4M_2_Textures_Unlit_Lightmap_Compatible){	
					CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.shader =  Shader.Find("T4MShaders/ShaderModel2/Unlit/T4M 2 Textures Unlit LM");
				}else if (MenuTextureSM2 == EnumShaderGLES2.T4M_3_Textures_Unlit_Lightmap_Compatible){			
					CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.shader =  Shader.Find("T4MShaders/ShaderModel2/Unlit/T4M 3 Textures Unlit LM");
				}else if (MenuTextureSM2 == EnumShaderGLES2.T4M_4_Textures_Unlit_Lightmap_Compatible){			
					CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.shader =  Shader.Find("T4MShaders/ShaderModel2/Unlit/T4M 4 Textures Unlit LM");		
				}else if (MenuTextureSM2 == EnumShaderGLES2.T4M_5_Textures_Unlit_Lightmap_Compatible){			
					CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.shader =  Shader.Find("T4MShaders/ShaderModel2/Unlit/T4M 5 Textures Unlit LM");
				}else if (MenuTextureSM2 == EnumShaderGLES2.T4M_6_Textures_Unlit_Lightmap_Compatible){			
					CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.shader =  Shader.Find("T4MShaders/ShaderModel2/Unlit/T4M 6 Textures Unlit LM");
				}else if (MenuTextureSM2 == EnumShaderGLES2.T4M_6_Textures_Unlit_No_Lightmap){			
					CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.shader =  Shader.Find("T4MShaders/ShaderModel2/Unlit/T4M 6 Textures Unlit NoLM");
				}else if (MenuTextureSM2 == EnumShaderGLES2.T4M_World_Projection_Unlit_Lightmap_Compatible){			
					CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.shader =  Shader.Find("T4MShaders/ShaderModel2/Unlit/T4M World Projection Shader + LM");
				}
				
				//Diffuse SM2
				else if (MenuTextureSM2 == EnumShaderGLES2.T4M_2_Textures_HighSpec){	
					CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.shader =  Shader.Find("T4MShaders/ShaderModel2/Diffuse/T4M 2 Textures");
				}else if (MenuTextureSM2 == EnumShaderGLES2.T4M_3_Textures_HighSpec){							
					CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.shader =  Shader.Find("T4MShaders/ShaderModel2/Diffuse/T4M 3 Textures");						
				}else if (MenuTextureSM2 == EnumShaderGLES2.T4M_4_Textures_HighSpec){							
					CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.shader =  Shader.Find("T4MShaders/ShaderModel2/Diffuse/T4M 4 Textures");						
				}else if (MenuTextureSM2 == EnumShaderGLES2.T4M_5_Textures_HighSpec){	
					CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.shader =  Shader.Find("T4MShaders/ShaderModel2/Diffuse/T4M 5 Textures");
				}else if (MenuTextureSM2 == EnumShaderGLES2.T4M_6_Textures_HighSpec){	
					CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.shader =  Shader.Find("T4MShaders/ShaderModel2/Diffuse/T4M 6 Textures");
				}else if (MenuTextureSM2 == EnumShaderGLES2.T4M_World_Projection_HighSpec){			
					CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.shader =  Shader.Find("T4MShaders/ShaderModel2/Diffuse/T4M World Projection Shader");
				}
				
				//Specular SM2
				else if (MenuTextureSM2 == EnumShaderGLES2.T4M_2_Textures_Specular){	
					CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.shader =  Shader.Find("T4MShaders/ShaderModel2/Specular/T4M 2 Textures Spec");
				}else if (MenuTextureSM2 == EnumShaderGLES2.T4M_3_Textures_Specular){							
					CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.shader =  Shader.Find("T4MShaders/ShaderModel2/Specular/T4M 3 Textures Spec");						
				}else if (MenuTextureSM2 == EnumShaderGLES2.T4M_4_Textures_Specular){							
					CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.shader =  Shader.Find("T4MShaders/ShaderModel2/Specular/T4M 4 Textures Spec");						
				}
				
				
				//4 mobile lightmap SM2
				else if (MenuTextureSM2 == EnumShaderGLES2.T4M_2_Textures_4_Mobile){	
					CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.shader =  Shader.Find("T4MShaders/ShaderModel2/MobileLM/T4M 2 Textures for Mobile");
				}else if (MenuTextureSM2 == EnumShaderGLES2.T4M_3_Textures_4_Mobile){							
					CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.shader =  Shader.Find("T4MShaders/ShaderModel2/MobileLM/T4M 3 Textures for Mobile");						
				}else if (MenuTextureSM2 == EnumShaderGLES2.T4M_4_Textures_4_Mobile){			
					CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.shader =  Shader.Find("T4MShaders/ShaderModel2/MobileLM/T4M 4 Textures for Mobile");
				}//else if (MenuTextureSM2 == EnumShaderGLES2.T4M_World_Projection_Mobile){			
				//	CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.shader =  Shader.Find("T4MShaders/ShaderModel2/MobileLM/T4M World Projection Shader_Mobile");
				//}
				
				//Toon SM2
				else if (MenuTextureSM2 == EnumShaderGLES2.T4M_2_Textures_Toon){
					CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.shader =  Shader.Find("T4MShaders/ShaderModel2/Toon/T4M 2 Textures Toon");
					Cubemap ToonShade =(Cubemap) AssetDatabase.LoadAssetAtPath(T4MFolder+"Shaders/Sources/toony lighting.psd",typeof (Cubemap));
					CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.SetTexture("_ToonShade",ToonShade );
				}else if (MenuTextureSM2 == EnumShaderGLES2.T4M_3_Textures_Toon){	
					CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.shader =  Shader.Find("T4MShaders/ShaderModel2/Toon/T4M 3 Textures Toon");
					Cubemap ToonShade =(Cubemap) AssetDatabase.LoadAssetAtPath(T4MFolder+"Shaders/Sources/toony lighting.psd",typeof (Cubemap));
					CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.SetTexture("_ToonShade",ToonShade );
				}else if (MenuTextureSM2 == EnumShaderGLES2.T4M_4_Textures_Toon){	
					CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.shader =  Shader.Find("T4MShaders/ShaderModel2/Toon/T4M 4 Textures Toon");
					Cubemap ToonShade =(Cubemap) AssetDatabase.LoadAssetAtPath(T4MFolder+"Shaders/Sources/toony lighting.psd",typeof (Cubemap));
					CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.SetTexture("_ToonShade",ToonShade );
				}
				
				//Bumped SM2
				else if (MenuTextureSM2 == EnumShaderGLES2.T4M_2_Textures_Bumped){			
					CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.shader =  Shader.Find("T4MShaders/ShaderModel2/Bump/T4M 2 Textures Bumped");		
				}else if (MenuTextureSM2 == EnumShaderGLES2.T4M_3_Textures_Bumped){	
					CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.shader =  Shader.Find("T4MShaders/ShaderModel2/Bump/T4M 3 Textures Bumped");
				}else if (MenuTextureSM2 == EnumShaderGLES2.T4M_4_Textures_Bumped){	
					CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.shader =  Shader.Find("T4MShaders/ShaderModel2/Bump/T4M 4 Textures Bumped");
				}
				
				//Mobile Bumped
				else if (MenuTextureSM2 == EnumShaderGLES2.T4M_2_Textures_Bumped_Mobile){			
					CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.shader =  Shader.Find("T4MShaders/ShaderModel2/Bump/T4M 2 Textures Bumped Mobile");		
				}else if (MenuTextureSM2 == EnumShaderGLES2.T4M_3_Textures_Bumped_Mobile){	
					CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.shader =  Shader.Find("T4MShaders/ShaderModel2/Bump/T4M 3 Textures Bumped Mobile");
				}
			
				//Mobile Bumped Spec
				else if (MenuTextureSM2 == EnumShaderGLES2.T4M_2_Textures_Bumped_SPEC_Mobile){			
					CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.shader =  Shader.Find("T4MShaders/ShaderModel2/Bump/T4M 2 Textures Bump Specular Mobile");
				}
			
				//Mobile Bump LM
				else if (MenuTextureSM2 == EnumShaderGLES2.T4M_2_Textures_Bumped_DirectionalLM_Mobile){			
					CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.shader =  Shader.Find("T4MShaders/ShaderModel2/BumpDLM/T4M 2 Textures Bumped DLM Mobile");		
				}
			
			
			
				//Bump Spec SM2
				else if (MenuTextureSM2 == EnumShaderGLES2.T4M_2_Textures_Bumped_SPEC){			
					CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.shader =  Shader.Find("T4MShaders/ShaderModel2/Bump/T4M 2 Textures Bump Specular");		
				}else if (MenuTextureSM2 == EnumShaderGLES2.T4M_3_Textures_Bumped_SPEC){	
					CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.shader =  Shader.Find("T4MShaders/ShaderModel2/Bump/T4M 3 Textures Bump Specular");
				}
				//Bump LM SM2
				else if (MenuTextureSM2 == EnumShaderGLES2.T4M_2_Textures_Bumped_DirectionalLM){			
					CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.shader =  Shader.Find("T4MShaders/ShaderModel2/BumpDLM/T4M 2 Textures Bumped DLM");		
				}else if (MenuTextureSM2 == EnumShaderGLES2.T4M_3_Textures_Bumped_DirectionalLM){	
					CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.shader =  Shader.Find("T4MShaders/ShaderModel2/BumpDLM/T4M 3 Textures Bumped DLM");
				}
				
		}
		else if (ShaderModel == SM.ShaderModel3){
				//Diffuse SM3
				if (MenuTextureSM3 == EnumShaderGLES3.T4M_2_Textures_Diffuse){	
					CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.shader =  Shader.Find("T4MShaders/ShaderModel3/Diffuse/T4M 2 Textures");
				}else if (MenuTextureSM3 == EnumShaderGLES3.T4M_3_Textures_Diffuse){							
					CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.shader =  Shader.Find("T4MShaders/ShaderModel3/Diffuse/T4M 3 Textures");						
				}else if (MenuTextureSM3 == EnumShaderGLES3.T4M_4_Textures_Diffuse){							
					CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.shader =  Shader.Find("T4MShaders/ShaderModel3/Diffuse/T4M 4 Textures");						
				}else if (MenuTextureSM3 == EnumShaderGLES3.T4M_5_Textures_Diffuse){	
					CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.shader =  Shader.Find("T4MShaders/ShaderModel3/Diffuse/T4M 5 Textures");
				}else if (MenuTextureSM3 == EnumShaderGLES3.T4M_6_Textures_Diffuse){	
					CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.shader =  Shader.Find("T4MShaders/ShaderModel3/Diffuse/T4M 6 Textures");
				}
			
				//Specular
				else if (MenuTextureSM3 == EnumShaderGLES3.T4M_2_Textures_Specular){	
					CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.shader =  Shader.Find("T4MShaders/ShaderModel3/Specular/T4M 2 Textures Spec");
				}else if (MenuTextureSM3 == EnumShaderGLES3.T4M_3_Textures_Specular){							
					CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.shader =  Shader.Find("T4MShaders/ShaderModel3/Specular/T4M 3 Textures Spec");						
				}else if (MenuTextureSM3 == EnumShaderGLES3.T4M_4_Textures_Specular){							
					CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.shader =  Shader.Find("T4MShaders/ShaderModel3/Specular/T4M 4 Textures Spec");						
				}
				
				//Bumped SM3
				else if (MenuTextureSM3 == EnumShaderGLES3.T4M_2_Textures_Bumped){			
					CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.shader =  Shader.Find("T4MShaders/ShaderModel3/Bump/T4M 2 Textures Bump");		
				}else if (MenuTextureSM3 == EnumShaderGLES3.T4M_3_Textures_Bumped){	
					CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.shader =  Shader.Find("T4MShaders/ShaderModel3/Bump/T4M 3 Textures Bump");
				}else if (MenuTextureSM3 == EnumShaderGLES3.T4M_4_Textures_Bumped){	
					CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.shader =  Shader.Find("T4MShaders/ShaderModel3/Bump/T4M 4 Textures Bump");
				}
				
				//Bump Spec SM3
				else if (MenuTextureSM3 == EnumShaderGLES3.T4M_2_Textures_Bumped_SPEC){			
					CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.shader =  Shader.Find("T4MShaders/ShaderModel3/BumpSpec/T4M 2 Textures Bump Spec");		
				}else if (MenuTextureSM3 == EnumShaderGLES3.T4M_3_Textures_Bumped_SPEC){	
					CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.shader =  Shader.Find("T4MShaders/ShaderModel3/BumpSpec/T4M 3 Textures Bump Spec");
				}else if (MenuTextureSM3 == EnumShaderGLES3.T4M_4_Textures_Bumped_SPEC){	
					CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.shader =  Shader.Find("T4MShaders/ShaderModel3/BumpSpec/T4M 4 Textures Bump Spec");
				}
			
		}else{
			Material temp  = new Material( CustomShader );
			if (CustomShader !=null && temp.HasProperty("_Control") && temp.HasProperty("_Splat0") && temp.HasProperty("_Splat1")){
				CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.shader = CustomShader;
				
			}else EditorUtility.DisplayDialog("T4M Message", "This Shader is not compatible", "OK");
		}
		
		
		
		if (CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.HasProperty("_Control2")){
			Texture Control2;
			if (CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.GetTexture("_Control") != null){
				Control2 =(Texture) AssetDatabase.LoadAssetAtPath(T4MPrefabFolder+"Terrains/Texture/"+CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.GetTexture("_Control").name+"C2.png",typeof (Texture));
			}else Control2 =(Texture) AssetDatabase.LoadAssetAtPath(T4MPrefabFolder+"Terrains/Texture/"+CurrentSelect.gameObject.name+"C2.png",typeof (Texture));
		
				if (Control2 == null)
					CreateControl2Text();
				else CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.SetTexture("_Control2", Control2);
		}
		
		
		
		if (CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.HasProperty("_Splat0"))
			CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.SetTexture("_Splat0", Layer1);

		if (CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.HasProperty("_Splat1"))
			CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.SetTexture("_Splat1", Layer2);	
		
		if (CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.HasProperty("_Splat2")){
			CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.SetTexture("_Splat2", Layer3);
		}
		if (CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.HasProperty("_Splat3")){
			CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.SetTexture("_Splat3", Layer4);
		}
		if (CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.HasProperty("_Splat4")){
			CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.SetTexture("_Splat4", Layer5);
		}
		if (CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.HasProperty("_Splat5")){
			CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.SetTexture("_Splat5", Layer6);
		}
		if (CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.HasProperty("_BumpSplat0")){
			CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.SetTexture("_BumpSplat0", Layer1Bump);
		}
		if (CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.HasProperty("_BumpSplat1")){
			CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.SetTexture("_BumpSplat1", Layer2Bump);
		}
		if (CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.HasProperty("_BumpSplat2")){
			CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.SetTexture("_BumpSplat2", Layer3Bump);
		}
		if (CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.HasProperty("_BumpSplat3")){
			CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.SetTexture("_BumpSplat3", Layer4Bump);
		}
		if(T4MMaster){
			CurrentSelect.GetComponent <T4MObjSC>().EnabledLODSystem = ActivatedLOD;
			CurrentSelect.GetComponent <T4MObjSC>().enabledBillboard = ActivatedBillboard;
			CurrentSelect.GetComponent <T4MObjSC>().enabledLayerCul = ActivatedLayerCul;
			CurrentSelect.GetComponent <T4MObjSC>().CloseView = CloseDistMaxView;
			CurrentSelect.GetComponent <T4MObjSC>().NormalView = NormalDistMaxView;
			CurrentSelect.GetComponent <T4MObjSC>().FarView = FarDistMaxView;
			CurrentSelect.GetComponent <T4MObjSC>().BackGroundView = BGDistMaxView;
			CurrentSelect.GetComponent <T4MObjSC>().Master = 1;
			CurrentSelect.GetComponent <T4MObjSC>().PlayerCamera = PlayerCam;
			PrefabUtility.RecordPrefabInstancePropertyModifications(CurrentSelect.gameObject.GetComponent<T4MObjSC>());
		}else{
			CurrentSelect.GetComponent <T4MObjSC>().EnabledLODSystem = false;
			CurrentSelect.GetComponent <T4MObjSC>().enabledBillboard = false;
			CurrentSelect.GetComponent <T4MObjSC>().enabledLayerCul = false;
			CurrentSelect.GetComponent <T4MObjSC>().Master = 0;
			
			T4MLodObjSC[] T4MLodObjGet =  GameObject.FindObjectsOfType(typeof(T4MLodObjSC)) as T4MLodObjSC[];				
			for(var i=0;i<T4MLodObjGet.Length;i++){
				T4MLodObjGet[i].LOD2.enabled= T4MLodObjGet[i].LOD3.enabled = false;
				T4MLodObjGet[i].LOD1.enabled= true;
				if(LODModeControler == LODMod.Mass_Control)
					T4MLodObjGet[i].Mode =0;
				else if(LODModeControler ==LODMod.Independent_Control)
					T4MLodObjGet[i].Mode =0;
				PrefabUtility.RecordPrefabInstancePropertyModifications(T4MLodObjGet[i].GetComponent<T4MLodObjSC>());
			}	
			CurrentSelect.gameObject.GetComponent <T4MObjSC>().ObjLodScript= new T4MLodObjSC[0];
			CurrentSelect.gameObject.GetComponent <T4MObjSC>().ObjPosition= new Vector3[0];
			CurrentSelect.gameObject.GetComponent <T4MObjSC>().ObjLodStatus=new int[0];
			CurrentSelect.gameObject.GetComponent <T4MObjSC>().Mode =0;
			
			T4MBillBObjSC[] T4MBillObjGet =  GameObject.FindObjectsOfType(typeof(T4MBillBObjSC)) as T4MBillBObjSC[];
			for(var i=0;i<T4MBillObjGet.Length;i++){
				T4MBillObjGet[i].Render.enabled = true;
				T4MBillObjGet[i].Transf.LookAt(Vector3.zero , Vector3.up);
			}	
			CurrentSelect.gameObject.GetComponent <T4MObjSC>().BillboardPosition= new Vector3[0];
			CurrentSelect.gameObject.GetComponent <T4MObjSC>().BillStatus=new int[0];
			CurrentSelect.gameObject.GetComponent <T4MObjSC>().BillScript= new T4MBillBObjSC[0];
			PrefabUtility.RecordPrefabInstancePropertyModifications(CurrentSelect.gameObject.GetComponent<T4MObjSC>());
		}
		TexTexture = null;
		IniNewSelect();
	}
	
	void CreateControl2Text()
	{
		Texture2D Control2 = new Texture2D (512, 512,  TextureFormat.ARGB32, true);
		Color[] ColorBase = new Color[512 * 512];
		for (var t = 0; t < ColorBase.Length; t++) {
				ColorBase[t] = new Color (1, 0, 0, 0);
		}
		
		Control2.SetPixels (ColorBase);
		string path;
		if (CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.GetTexture("_Control") != null){
		 	path = T4MPrefabFolder+"Terrains/Texture/"+CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.GetTexture("_Control").name+"C2.png";
			
		}else path = T4MPrefabFolder+"Terrains/Texture/"+CurrentSelect.gameObject.name+"C2.png";	
		byte[] data = Control2.EncodeToPNG ();
		File.WriteAllBytes (path, data);
		AssetDatabase.ImportAsset (path, ImportAssetOptions.ForceUpdate);
		
		TextureImporter TextureI= AssetImporter.GetAtPath (path) as TextureImporter;
		TextureI.textureFormat = TextureImporterFormat.ARGB32;
		TextureI.isReadable = true;
		TextureI.anisoLevel = 9;
		TextureI.mipmapEnabled = false;
		TextureI.wrapMode = TextureWrapMode.Clamp;
		AssetDatabase.ImportAsset (path, ImportAssetOptions.ForceUpdate);
		
		Texture Contr2 =(Texture) AssetDatabase.LoadAssetAtPath(path, typeof(Texture));
		CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.SetTexture("_Control2", Contr2);
		IniNewSelect();
	}
	
	
	 void ConvertUTerrain()
	{
		if (terrainName=="")
			terrainName =CurrentSelect.name; 
				
		if (!System.IO.Directory.Exists(T4MPrefabFolder+"Terrains/"))
		{
			System.IO.Directory.CreateDirectory(T4MPrefabFolder+"Terrains/");
		} 
		if (!System.IO.Directory.Exists(T4MPrefabFolder+"Terrains/Material/"))
		{
			System.IO.Directory.CreateDirectory(T4MPrefabFolder+"Terrains/Material/");
		} 
		if (!System.IO.Directory.Exists(T4MPrefabFolder+"Terrains/Texture/"))
		{
			System.IO.Directory.CreateDirectory(T4MPrefabFolder+"Terrains/Texture/");
		} 
		if (!System.IO.Directory.Exists(T4MPrefabFolder+"Terrains/Meshes/"))
		{
			System.IO.Directory.CreateDirectory(T4MPrefabFolder+"Terrains/Meshes/");
		} 
		AssetDatabase.Refresh(); 
		
		terrain = CurrentSelect.GetComponent <Terrain>().terrainData;
        int w = terrain.heightmapWidth;
        int h = terrain.heightmapHeight;
        Vector3 meshScale = terrain.size;
        meshScale = new Vector3(meshScale.x/(h-1)*tRes, meshScale.y, meshScale.z/(w-1)*tRes);
	    Vector2 uvScale =  new Vector2((float)(1.0/(w-1)),(float) (1.0/(h-1)));
		
        float[,] tData = terrain.GetHeights(0, 0, w, h);
        w = (int)((w-1) / tRes + 1);
        h = (int)((h-1) / tRes + 1);
        Vector3[] tVertices = new Vector3[w * h];
        Vector2[] tUV = new Vector2[w * h];
        int[] tPolys = new int[(w-1) * (h-1) * 6];
		int y=0;
		int x  =0;
        for (y = 0; y < h; y++) {
			for ( x = 0; x < w; x++) {
                //tVertices[y*w + x] = Vector3.Scale(meshScale, new Vector3(x, tData[(int)(x*tRes),(int)(y*tRes)], y));
                tVertices[y*w + x] = Vector3.Scale(meshScale, new Vector3(-y, tData[(int)(x*tRes),(int)(y*tRes)], x)); //Thank Cid Newman
				tUV[y*w + x] = Vector2.Scale(new Vector2(y*tRes, x*tRes), uvScale);
            }
        }
		
		y  =0;
		x =0;
        int  index = 0;
        for ( y = 0; y < h-1; y++) {
                for ( x = 0; x < w-1; x++) {
                    tPolys[index++] = (y*w) + x;
                    tPolys[index++] = ((y+1) * w) + x;
                    tPolys[index++] = (y*w) + x + 1;
        
                    tPolys[index++] = ((y+1) * w) + x;
                    tPolys[index++] = ((y+1) * w) + x + 1;
                    tPolys[index++] = (y*w) + x + 1;
                }
            }
		
			bool ExportNameSuccess = false;
			int num =1;
			string Next;
			do { 
				Next=terrainName+num;
				
				if (!System.IO.File.Exists(T4MPrefabFolder+"Terrains/"+terrainName+".prefab"))
				{
					FinalExpName = terrainName;
					ExportNameSuccess = true;
				}
				else if (!System.IO.File.Exists(T4MPrefabFolder+"Terrains/"+Next+".prefab"))
				{
					FinalExpName=Next;
					ExportNameSuccess = true;
				}
				num++;
			}while (!ExportNameSuccess);
	
		 //StreamWriter  sw = new StreamWriter(T4MPrefabFolder+"Terrains/Meshes/"+FinalExpName+".obj");
		StreamWriter  sw = new StreamWriter(FinalExpName+".obj");
        try {
           
            sw.WriteLine("# T4M File");
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            counter = tCount = 0;
            totalCount = (int) ((tVertices.Length*2 + (tPolys.Length/3)) / progressUpdateInterval);
            for (int i = 0; i < tVertices.Length; i++) {
                UpdateProgress();
                 StringBuilder sb = new StringBuilder("v ", 20);
                sb.Append(tVertices[i].x.ToString()).Append(" ").
                   Append(tVertices[i].y.ToString()).Append(" ").
                   Append(tVertices[i].z.ToString());
                sw.WriteLine(sb);
            }
			
            for (int i = 0; i < tUV.Length; i++) {
                UpdateProgress();
                StringBuilder sb = new StringBuilder("vt ", 22);
                sb.Append(tUV[i].x.ToString()).Append(" ").
                   Append(tUV[i].y.ToString());
                sw.WriteLine(sb);
            }
                for (int i = 0; i < tPolys.Length; i += 3) {
                    UpdateProgress();
                    StringBuilder sb = new StringBuilder("f ", 43);
                    sb.Append(tPolys[i]+1).Append("/").Append(tPolys[i]+1).Append(" ").
                       Append(tPolys[i+1]+1).Append("/").Append(tPolys[i+1]+1).Append(" ").
                       Append(tPolys[i+2]+1).Append("/").Append(tPolys[i+2]+1);
                    sw.WriteLine(sb);
                }
        }
        catch (Exception err) {
            Debug.Log("Error saving file: " + err.Message);
        }
        sw.Close();
		AssetDatabase.SaveAssets(); 
	
		string path = T4MPrefabFolder+"Terrains/Texture/"+FinalExpName+".png";
		
		//Control Texture Creation or Recuperation
		string AssetName = AssetDatabase.GetAssetPath(CurrentSelect.GetComponent <Terrain>().terrainData) as string;
		UnityEngine.Object[] AssetName2 = AssetDatabase.LoadAllAssetsAtPath (AssetName);
		if (AssetName2 !=null && AssetName2.Length>1 && keepTexture){
			for (var b = 0; b < AssetName2.Length; b++) {
				if(AssetName2[b].name == "SplatAlpha 0"){
					Texture2D texture  = AssetName2[b] as Texture2D;
					byte[] bytes = texture.EncodeToPNG();
			  		File.WriteAllBytes(path, bytes);
					AssetDatabase.ImportAsset (path, ImportAssetOptions.ForceUpdate);
				}
			}
		}else{ 
			Texture2D NewMaskText = new Texture2D (512, 512,  TextureFormat.RGBA32, true);
			Color[] ColorBase = new Color[512 * 512];
			for (var t = 0; t < ColorBase.Length; t++) {
					ColorBase[t] = new Color (1, 0, 0, 0);
			}
			NewMaskText.SetPixels (ColorBase);
			byte[] data = NewMaskText.EncodeToPNG ();
			File.WriteAllBytes (path, data);
			AssetDatabase.ImportAsset (path, ImportAssetOptions.ForceUpdate);
		}
		AssetDatabase.ImportAsset (path, ImportAssetOptions.ForceUpdate);
		
		UpdateProgress();
		
		//Modification de la Texture 
		TextureImporter TextureI= AssetImporter.GetAtPath (path) as TextureImporter;
		TextureI.textureFormat = TextureImporterFormat.ARGB32;
		TextureI.isReadable = true;
		TextureI.anisoLevel = 9;
		TextureI.mipmapEnabled = false;
		TextureI.wrapMode = TextureWrapMode.Clamp;
		TextureI.textureCompression = TextureImporterCompression.Uncompressed;
		AssetDatabase.Refresh();
		
		AssetDatabase.ImportAsset (path, ImportAssetOptions.ForceUpdate);
		
		UpdateProgress();
		
		//Creation du Materiel
		Material Tmaterial;
		Tmaterial = new Material (Shader.Find("T4MShaders/ShaderModel2/Diffuse/T4M 4 Textures"));
		AssetDatabase.CreateAsset(Tmaterial, T4MPrefabFolder+"Terrains/Material/"+FinalExpName+".mat");
		AssetDatabase.ImportAsset (T4MPrefabFolder+"Terrains/Material/"+FinalExpName+".mat", ImportAssetOptions.ForceUpdate);
		AssetDatabase.Refresh();
		
		//Recuperation des Texture du terrain
		if (keepTexture){
			SplatPrototype[] texts = CurrentSelect.GetComponent <Terrain>().terrainData.splatPrototypes;
			for (int e = 0 ; e < texts.Length ; e++){
				if (e<4){
					Tmaterial.SetTexture("_Splat"+e, texts[e].texture);
					Tmaterial.SetTextureScale ("_Splat"+e, texts[e].tileSize*8.9f);
				}
			}
		}
		
		//Attribution de la Texture Control sur le materiau
		Texture test =(Texture) AssetDatabase.LoadAssetAtPath(path, typeof(Texture));
		Tmaterial.SetTexture ("_Control", test);
		
		
		UpdateProgress();
		
		
		//Deplacement de l'obj dans les repertoire mesh
		FileUtil.CopyFileOrDirectory (FinalExpName+".obj",T4MPrefabFolder+"Terrains/Meshes/"+FinalExpName+".obj");
		FileUtil.DeleteFileOrDirectory(FinalExpName+".obj");
		
		
		
		//Force Update
		AssetDatabase.ImportAsset (T4MPrefabFolder+"Terrains/Meshes/"+FinalExpName+".obj", ImportAssetOptions.ForceUpdate);
		
		UpdateProgress();
		
		//Instance du T4M
		GameObject  prefab = (GameObject)AssetDatabase.LoadAssetAtPath(T4MPrefabFolder+"Terrains/Meshes/"+FinalExpName+".obj", typeof(GameObject));
		
		AssetDatabase.Refresh();
		
		
		GameObject forRotate = (GameObject) Instantiate (prefab, CurrentSelect.transform.position, Quaternion.identity) as GameObject;
		Transform childCheck = forRotate.transform.Find("default");
		Child = childCheck.gameObject;
		forRotate.transform.DetachChildren();
		DestroyImmediate(forRotate);
		Child.name = FinalExpName;
		Child.AddComponent<T4MObjSC>();
		//Child.transform.rotation= Quaternion.Euler(0, 90, 0);
		
		UpdateProgress();
				
		//Application des Parametres sur le Script
		Child.GetComponent <T4MObjSC>().T4MMaterial = Tmaterial;
		Child.GetComponent<T4MObjSC>().ConvertType= "UT";
		
		//Regalges Divers
		vertexInfo = 0;
		partofT4MObj = 0;
		trisInfo = 0; 
		int countchild = Child.transform.childCount;
		if (countchild>0){
			Renderer[] T4MOBJPART = Child.GetComponentsInChildren<Renderer>();
			for (int i = 0; i < T4MOBJPART.Length; i++) 
			{
				if (!T4MOBJPART[i].gameObject.AddComponent<MeshCollider>())
					T4MOBJPART[i].gameObject.AddComponent<MeshCollider>();
				T4MOBJPART[i].gameObject.isStatic = true;
				T4MOBJPART[i].material = Tmaterial;
				T4MOBJPART[i].gameObject.layer = 30; 
				T4MOBJPART[i].gameObject.AddComponent<T4MPartSC>();
				Child.GetComponent<T4MObjSC>().T4MMesh = T4MOBJPART[0].GetComponent<MeshFilter>();
				partofT4MObj +=1;
				vertexInfo += T4MOBJPART[i].gameObject.GetComponent<MeshFilter>().sharedMesh.vertexCount;
				trisInfo += T4MOBJPART[i].gameObject.GetComponent<MeshFilter>().sharedMesh.triangles.Length / 3;
			}
		}else {
			Child.AddComponent<MeshCollider>();
			Child.isStatic = true;
			Child.GetComponent<Renderer>().material = Tmaterial;
			Child.layer = 30; 
			vertexInfo += Child.GetComponent<MeshFilter>().sharedMesh.vertexCount;
			trisInfo += Child.GetComponent<MeshFilter>().sharedMesh.triangles.Length / 3;
			partofT4MObj +=1;
		}
		
		UpdateProgress();
		
		
		GameObject BasePrefab2 = PrefabUtility.CreatePrefab(T4MPrefabFolder+"Terrains/"+FinalExpName+".prefab", Child);
		AssetDatabase.ImportAsset (T4MPrefabFolder+"Terrains/"+FinalExpName+".prefab", ImportAssetOptions.ForceUpdate);
		GameObject forRotate2 = (GameObject) PrefabUtility.InstantiatePrefab (BasePrefab2) as GameObject;
		
		DestroyImmediate (Child.gameObject);
		
		Child = forRotate2.gameObject;
		
		CurrentSelect.GetComponent<Terrain>().enabled = false;
		
		EditorUtility.SetSelectedWireframeHidden(Child.GetComponent<Renderer>(), true);
		
		UnityTerrain = CurrentSelect.gameObject;
		
		EditorUtility.ClearProgressBar();
		
		AssetDatabase.DeleteAsset(T4MPrefabFolder+"Terrains/Meshes/Materials");
		terrainName="";
		AssetDatabase.StartAssetEditing();
			//Modification des attribut du mesh avant de le pr茅fabriquer
			ModelImporter OBJI= ModelImporter.GetAtPath(T4MPrefabFolder+"Terrains/Meshes/"+FinalExpName+".obj") as ModelImporter;
			OBJI.globalScale = 1;
			OBJI.splitTangentsAcrossSeams = true;
			OBJI.normalImportMode =ModelImporterTangentSpaceMode.Calculate;
			OBJI.tangentImportMode=ModelImporterTangentSpaceMode.Calculate;
			OBJI.generateAnimations = ModelImporterGenerateAnimations.None;
			OBJI.meshCompression=ModelImporterMeshCompression.Off;
			OBJI.normalSmoothingAngle = 180f;
			//AssetDatabase.ImportAsset (T4MPrefabFolder+"Terrains/Meshes/"+FinalExpName+".obj", ImportAssetOptions.TryFastReimportFromMetaData);
			AssetDatabase.ImportAsset (T4MPrefabFolder+"Terrains/Meshes/"+FinalExpName+".obj", ImportAssetOptions.ForceSynchronousImport);
		AssetDatabase.StopAssetEditing();
		PrefabUtility.ResetToPrefabState (Child);
	}
	
	
	void Obj2T4M()
	{
		if (terrainName=="")
			terrainName =CurrentSelect.name; 
		
		if (!System.IO.Directory.Exists(T4MPrefabFolder+"Terrains/"))
		{
			System.IO.Directory.CreateDirectory(T4MPrefabFolder+"Terrains/");
		} 
		if (!System.IO.Directory.Exists(T4MPrefabFolder+"Terrains/Material/"))
		{
			System.IO.Directory.CreateDirectory(T4MPrefabFolder+"Terrains/Material/");
		} 
		if (!System.IO.Directory.Exists(T4MPrefabFolder+"Terrains/Texture/"))
		{
			System.IO.Directory.CreateDirectory(T4MPrefabFolder+"Terrains/Texture/");
		} 
		AssetDatabase.Refresh(); 
		
		Texture2D NewMaskText = new Texture2D (512, 512,  TextureFormat.RGBA32, true);
		Color[] ColorBase  = new Color[512 * 512];
		for (var t = 0; t < ColorBase.Length; t++) {
				ColorBase[t] = new Color (1, 0, 0, 0);
		}
		
		NewMaskText.SetPixels (ColorBase);
		
		
		bool ExportNameSuccess = false;
		int num =1;
		string Next;
		do { 
			Next=terrainName+num;
			
			if (!System.IO.File.Exists(T4MPrefabFolder+"Terrains/Material/"+terrainName+".mat"))
			{
				FinalExpName = terrainName;
				ExportNameSuccess = true;
			}
			else if (!System.IO.File.Exists(T4MPrefabFolder+"Terrains/Material/"+Next+".mat"))
			{
				FinalExpName=Next;
				ExportNameSuccess = true;
			}
			num++;
		}while (!ExportNameSuccess);
		
		
		var path = T4MPrefabFolder+"Terrains/Texture/"+ FinalExpName +".png";
		var data = NewMaskText.EncodeToPNG ();
		File.WriteAllBytes (path, data);
		AssetDatabase.ImportAsset (path, ImportAssetOptions.ForceUpdate);
		var TextureIm= AssetImporter.GetAtPath (path) as TextureImporter;
		TextureIm.textureFormat = TextureImporterFormat.ARGB32;
		TextureIm.isReadable = true;
		TextureIm.anisoLevel = 9;
		TextureIm.mipmapEnabled = false;
		TextureIm.wrapMode = TextureWrapMode.Clamp;
		TextureIm.textureCompression = TextureImporterCompression.Uncompressed;
		AssetDatabase.ImportAsset (path, ImportAssetOptions.ForceUpdate);
		Material Tmaterial;
		Tmaterial = new Material (Shader.Find("T4MShaders/ShaderModel2/Diffuse/T4M 4 Textures"));
		AssetDatabase.CreateAsset(Tmaterial, T4MPrefabFolder+"Terrains/Material/"+FinalExpName+".mat");
		AssetDatabase.ImportAsset (T4MPrefabFolder+"Terrains/Material/"+FinalExpName+".mat", ImportAssetOptions.ForceUpdate);
		
		
		Texture FinalMaterial = (Texture)AssetDatabase.LoadAssetAtPath(path, typeof(Texture));
		
		
		CurrentSelect.gameObject.AddComponent<T4MObjSC>();
		
		
		int countchild = CurrentSelect.transform.childCount;
		if (countchild>0){
			Renderer[] T4MOBJPART = CurrentSelect.GetComponentsInChildren<Renderer>();
			for (int i = 0; i < T4MOBJPART.Length; i++) 
			{
				if (T4MOBJPART[i].gameObject.GetComponent<Collider>())
					DestroyImmediate(T4MOBJPART[i].gameObject.GetComponent<Collider>());
					
				T4MOBJPART[i].gameObject.AddComponent<MeshCollider>();
				
				T4MOBJPART[i].gameObject.isStatic = true;
				
				T4MOBJPART[i].material = Tmaterial;
				T4MOBJPART[i].gameObject.layer = 30; 
				T4MOBJPART[i].gameObject.AddComponent<T4MPartSC>();
				CurrentSelect.GetComponent<T4MObjSC>().T4MMesh = T4MOBJPART[0].GetComponent<MeshFilter>();
				
			}
		}else {
			if (CurrentSelect.GetComponent<Collider>())
				DestroyImmediate(CurrentSelect.GetComponent<Collider>());
			
			CurrentSelect.gameObject.AddComponent<MeshCollider>();
			CurrentSelect.gameObject.GetComponent<Renderer>().material = CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial = Tmaterial;
			CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMesh = CurrentSelect.gameObject.GetComponent<MeshFilter>();
			
		}
		CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial = Tmaterial;
		CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.SetTexture ("_Control", FinalMaterial);
		CurrentSelect.gameObject.isStatic = true;
		CurrentSelect.gameObject.layer = 30;
		
		if (nbrT4MObj ==0){
				CurrentSelect.gameObject.GetComponent <T4MObjSC>().EnabledLODSystem = ActivatedLOD;
				CurrentSelect.gameObject.GetComponent <T4MObjSC>().enabledBillboard = ActivatedBillboard;
				CurrentSelect.gameObject.GetComponent <T4MObjSC>().enabledLayerCul = ActivatedLayerCul;
				CurrentSelect.gameObject.GetComponent <T4MObjSC>().CloseView = CloseDistMaxView;
				CurrentSelect.gameObject.GetComponent <T4MObjSC>().NormalView = NormalDistMaxView;
				CurrentSelect.gameObject.GetComponent <T4MObjSC>().FarView = FarDistMaxView;
				CurrentSelect.gameObject.GetComponent <T4MObjSC>().BackGroundView = BGDistMaxView;
				CurrentSelect.gameObject.GetComponent <T4MObjSC>().Master = 1;
		}
		
		
		if (NewPref){
			UnityEngine.Object BasePrefab = PrefabUtility.CreateEmptyPrefab(T4MPrefabFolder+"Terrains/"+FinalExpName+".prefab");
			PrefabUtility.ReplacePrefab(CurrentSelect.gameObject, BasePrefab);
			
			GameObject prefab = (GameObject)AssetDatabase.LoadAssetAtPath(T4MPrefabFolder+"Terrains/"+FinalExpName+".prefab", typeof(GameObject));
			GameObject forRotate =(GameObject) PrefabUtility.InstantiatePrefab (prefab) as GameObject;
			
			DestroyImmediate(CurrentSelect.gameObject);
			Selection.activeTransform = forRotate.transform;
			EditorUtility.SetSelectedWireframeHidden(forRotate.GetComponent<Renderer>(), true);
		}else{
			Selection.activeTransform = CurrentSelect.transform;
			EditorUtility.SetSelectedWireframeHidden(CurrentSelect.GetComponent<Renderer>(), true);
		}
		
		CurrentSelect.gameObject.name = FinalExpName;
		
		EditorUtility.DisplayDialog("T4M Message", "Conversion Completed !", "OK");
		
		
		
		T4MMenuToolbar = 1;
		terrainName="";
		AssetDatabase.SaveAssets();
	}
	
	
	void GetHeightmap()
	{
		terrainDat = CurrentSelect.GetComponent <Terrain>().terrainData ;
		HeightmapWidth = terrainDat.heightmapWidth;
		HeightmapHeight = terrainDat.heightmapHeight;
	}
	
	 void UpdateProgress () 
	 {
        if (counter++ == progressUpdateInterval) {
            counter = 0;
            EditorUtility.DisplayProgressBar("Generate...", "", Mathf.InverseLerp(0, totalCount, ++tCount));
        }
    }
	
	void IniNewSelect()
	{
		
			
		if (UnityTerrain){
			DestroyImmediate(UnityTerrain);
			if(Child){
				Selection.activeTransform = Child.transform;
				vertexInfo = 0;
				trisInfo = 0;
				partofT4MObj = 0;
				if (nbrT4MObj == 0){
					Child.gameObject.GetComponent <T4MObjSC>().EnabledLODSystem = ActivatedLOD;
					Child.gameObject.GetComponent <T4MObjSC>().enabledBillboard = ActivatedBillboard;
					Child.gameObject.GetComponent <T4MObjSC>().enabledLayerCul = ActivatedLayerCul;
					Child.gameObject.GetComponent <T4MObjSC>().CloseView = CloseDistMaxView;
					Child.gameObject.GetComponent <T4MObjSC>().NormalView = NormalDistMaxView;
					Child.gameObject.GetComponent <T4MObjSC>().FarView = FarDistMaxView;
					Child.gameObject.GetComponent <T4MObjSC>().BackGroundView = BGDistMaxView;
					Child.gameObject.GetComponent <T4MObjSC>().Master = 1;
				}
			}
		}
		if (CurrentSelect && CurrentSelect.GetComponent <T4MObjSC>()  && CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial){
				EditorUtility.SetSelectedWireframeHidden(CurrentSelect.GetComponent<Renderer>(), true);
				initMaster = false;
			
				if (CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.HasProperty("_Splat0")){
					Layer1 =CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.GetTexture("_Splat0");
					Layer1Tile = CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.GetTextureScale("_Splat0");
				}else Layer1 =null;
				if (CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.HasProperty("_Splat1")){
					Layer2 =CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.GetTexture("_Splat1");
					Layer2Tile = CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.GetTextureScale("_Splat1");
				}else Layer2 =null;
				if (CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.HasProperty("_Splat2")){
					Layer3 =CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.GetTexture("_Splat2");
					Layer3Tile = CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.GetTextureScale("_Splat2");
				}else Layer3 =null;
				if (CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.HasProperty("_Splat3")){
					Layer4 =CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.GetTexture("_Splat3");
					Layer4Tile = CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.GetTextureScale("_Splat3");
				}else Layer4 =null;
				if (CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.HasProperty("_Splat4")){
					Layer5 =CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.GetTexture("_Splat4");
					Layer5Tile = CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.GetTextureScale("_Splat4");
				}else Layer5 =null;
				if (CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.HasProperty("_Splat5")){
					Layer6 =CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.GetTexture("_Splat5");
					Layer6Tile = CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.GetTextureScale("_Splat5");
				}else Layer6 =null;
				
				if (CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.HasProperty("_BumpSplat0")){
					Layer1Bump =CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.GetTexture("_BumpSplat0");
					Layer2Bump =CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.GetTexture("_BumpSplat1");
					if (CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.HasProperty("_BumpSplat2"))
						Layer3Bump =CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.GetTexture("_BumpSplat2");
					if (CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.HasProperty("_BumpSplat3"))
						Layer4Bump =CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.GetTexture("_BumpSplat3");
					
				} 
					if(CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.shader == Shader.Find("T4MShaders/ShaderModel1/T4M 2 Textures ManualAdd BeastLM_1DC") || CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.shader == Shader.Find("T4MShaders/ShaderModel1/T4M 2 Textures ManualAdd CustoLM 1DC")){
						LMMan = CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.GetTexture("_Lightmap");
					}
					CheckShader();
					ActivatedLOD =CurrentSelect.GetComponent <T4MObjSC>().EnabledLODSystem ;
					ActivatedBillboard =	CurrentSelect.GetComponent <T4MObjSC>().enabledBillboard ;
					MaximunView = CurrentSelect.GetComponent <T4MObjSC>().MaxViewDistance;
					StartLOD2= CurrentSelect.GetComponent <T4MObjSC>().LOD2Start;
					StartLOD3 = CurrentSelect.GetComponent <T4MObjSC>().LOD3Start;
					UpdateInterval = CurrentSelect.GetComponent <T4MObjSC>().Interval;
					PlayerCam = CurrentSelect.GetComponent <T4MObjSC>().PlayerCamera;
					BillInterval = CurrentSelect.GetComponent <T4MObjSC>().BillInterval;
					BillboardDist = CurrentSelect.GetComponent <T4MObjSC>().BillMaxViewDistance;
					ActivatedLayerCul = CurrentSelect.GetComponent <T4MObjSC>().enabledLayerCul;
					BGDistMaxView = CurrentSelect.GetComponent <T4MObjSC>().BackGroundView;
					FarDistMaxView=CurrentSelect.GetComponent <T4MObjSC>().FarView;
					NormalDistMaxView=CurrentSelect.GetComponent <T4MObjSC>().NormalView;
					CloseDistMaxView=CurrentSelect.GetComponent <T4MObjSC>().CloseView;
					if (CurrentSelect.GetComponent <T4MObjSC>().Mode == 1)
						LODModeControler = LODMod.Mass_Control;
					else LODModeControler = LODMod.Independent_Control;
			
					if(CurrentSelect.GetComponent <T4MObjSC>().Axis == 0)
						BillBoardAxis = BillbAxe.Y_Axis;
					else BillBoardAxis = BillbAxe.All_Axis;
			
					if(CurrentSelect.GetComponent <T4MObjSC>().LODbasedOnScript == true)
						LODocclusion = OccludeBy.Max_View_Distance;
					else LODocclusion = OccludeBy.Layer_Cull_Distances;
			 
			
					if(CurrentSelect.GetComponent <T4MObjSC>().BilBbasedOnScript == true)
						BilBocclusion = OccludeBy.Max_View_Distance;
					else BilBocclusion = OccludeBy.Layer_Cull_Distances;
					
					
					if (CurrentSelect.gameObject.GetComponent <T4MObjSC>().BillboardPosition != null && CurrentSelect.gameObject.GetComponent <T4MObjSC>().BillboardPosition.Length>0)
						billActivate = true;
					else billActivate = false;
					
					if (CurrentSelect.gameObject.GetComponent <T4MObjSC>().ObjPosition != null && CurrentSelect.gameObject.GetComponent <T4MObjSC>().ObjPosition.Length>0)
						LodActivate = true;
					else LodActivate = false;
			
			
					if (PlayerCam == null && Camera.main)
						PlayerCam = Camera.main.transform;
					else if (PlayerCam == null && !Camera.main){
						Camera[] Cam =  GameObject.FindObjectsOfType(typeof(Camera)) as Camera[];
						for (var b =0; b <Cam.Length;b++){
							if (Cam[b].GetComponent<AudioListener>()){
								PlayerCam = Cam[b].transform; 
							}
						}
					}
			
			
				if (CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.HasProperty("_SpecColor")){
					
					ShinessColor = CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.GetColor("_SpecColor");
					
					if (CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.HasProperty("_ShininessL0")){
						shiness0 = CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.GetFloat ("_ShininessL0");
					}
					if (CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.HasProperty("_ShininessL1")){
						shiness1 = CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.GetFloat ("_ShininessL1");
					}
					if (CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.HasProperty("_ShininessL2")){
						shiness2 = CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.GetFloat ("_ShininessL2");
					}
					if (CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.HasProperty("_ShininessL3")){
						shiness3 =CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.GetFloat ("_ShininessL3");
					}
				}
				if (CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.HasProperty("_Control2") && CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.GetTexture("_Control2"))
					T4MMaskTex2 = (Texture2D)CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.GetTexture("_Control2");
				else T4MMaskTex2 = null;
				if (CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.HasProperty("_Control"))
				{
					T4MMaskTexUVCoord = CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.GetTextureScale("_Control").x;
					T4MMaskTex = (Texture2D)CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.GetTexture("_Control");
					intialized=true;
					
				}
			
				
			}
			Projector[] projectorObj = FindObjectsOfType(typeof(Projector)) as Projector[];
			if(projectorObj.Length>0)
			for (var i = 0; i < projectorObj.Length; i++)
			{
				if (projectorObj[i].gameObject.name == "PreviewT4M")
				DestroyImmediate (projectorObj[i].gameObject);
			}
			terrainDat = null;
			vertexInfo = 0;
			trisInfo= 0;
			partofT4MObj = 0;
			TexTexture = null;
			
		T4MSelectID = Selection.activeInstanceID;
		
	}
	
	void CheckShader(){
		
		if (CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.shader == Shader.Find("T4MShaders/ShaderModel1/T4M 2 Textures Auto BeastLM 2DrawCall")){
			MenuTextureSM1 = EnumShaderGLES1.T4M_2_Textures_Auto_BeastLM_2DrawCall ;
			ShaderModel = SM.ShaderModel1;
		}else
		if (CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.shader == Shader.Find("T4MShaders/ShaderModel1/T4M 2 Textures ManualAdd BeastLM_1DC")){
			MenuTextureSM1 = EnumShaderGLES1.T4M_2_Textures_ManualAdd_BeastLM_1DC ;
			ShaderModel = SM.ShaderModel1;
		}else	
		if (CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.shader == Shader.Find("T4MShaders/ShaderModel1/T4M 2 Textures ManualAdd CustoLM 1DC")){
			MenuTextureSM1 = EnumShaderGLES1.T4M_2_Textures_ManualAdd_CustoLM_1DC ;
			ShaderModel = SM.ShaderModel1;
		}else	
		if (CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.shader == Shader.Find("T4MShaders/ShaderModel2/Unlit/T4M 2 Textures Unlit LM")){
			MenuTextureSM2 = EnumShaderGLES2.T4M_2_Textures_Unlit_Lightmap_Compatible;
			ShaderModel = SM.ShaderModel2;
		}else
		if (CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.shader == Shader.Find("T4MShaders/ShaderModel2/Unlit/T4M 3 Textures Unlit LM")){
			MenuTextureSM2 = EnumShaderGLES2.T4M_3_Textures_Unlit_Lightmap_Compatible;
			ShaderModel = SM.ShaderModel2;
		}else
		if (CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.shader == Shader.Find("T4MShaders/ShaderModel2/Unlit/T4M 4 Textures Unlit LM")){
			MenuTextureSM2 = EnumShaderGLES2.T4M_4_Textures_Unlit_Lightmap_Compatible;
			ShaderModel = SM.ShaderModel2;
		}else
		if (CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.shader == Shader.Find("T4MShaders/ShaderModel2/Unlit/T4M 5 Textures Unlit LM")){
			MenuTextureSM2 = EnumShaderGLES2.T4M_5_Textures_Unlit_Lightmap_Compatible;
			ShaderModel = SM.ShaderModel2;
		}else
		if (CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.shader == Shader.Find("T4MShaders/ShaderModel2/Unlit/T4M 6 Textures Unlit LM")){
			MenuTextureSM2 = EnumShaderGLES2.T4M_6_Textures_Unlit_Lightmap_Compatible;
			ShaderModel = SM.ShaderModel2;
		}else
		if (CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.shader == Shader.Find("T4MShaders/ShaderModel2/Unlit/T4M 6 Textures Unlit NoL")){
			MenuTextureSM2 = EnumShaderGLES2.T4M_6_Textures_Unlit_No_Lightmap;
			ShaderModel = SM.ShaderModel2;
		}else
		if (CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.shader == Shader.Find("T4MShaders/ShaderModel2/Unlit/T4M World Projection Shader + LM")){
			MenuTextureSM2 = EnumShaderGLES2.T4M_World_Projection_Unlit_Lightmap_Compatible;
			ShaderModel = SM.ShaderModel2;
			UpSideTile = CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.GetVector ("_Tiling");
			UpSideF = CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.GetFloat ("_UpSide");
			BlendFac= CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.GetFloat ("_Blend");
			
		}else
		if (CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.shader == Shader.Find("T4MShaders/ShaderModel2/Diffuse/T4M 2 Textures")){
			MenuTextureSM2 = EnumShaderGLES2.T4M_2_Textures_HighSpec;
			ShaderModel = SM.ShaderModel2;
		}else
		if (CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.shader == Shader.Find("T4MShaders/ShaderModel2/Diffuse/T4M 3 Textures")){
			MenuTextureSM2 = EnumShaderGLES2.T4M_3_Textures_HighSpec;
			ShaderModel = SM.ShaderModel2;
		}else
		if (CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.shader == Shader.Find("T4MShaders/ShaderModel2/Diffuse/T4M 4 Textures")){
			MenuTextureSM2 = EnumShaderGLES2.T4M_4_Textures_HighSpec;
			ShaderModel = SM.ShaderModel2;
		}else
		if (CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.shader == Shader.Find("T4MShaders/ShaderModel2/Diffuse/T4M 5 Textures")){
			MenuTextureSM2 = EnumShaderGLES2.T4M_5_Textures_HighSpec;
			ShaderModel = SM.ShaderModel2;
		}else
		if (CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.shader == Shader.Find("T4MShaders/ShaderModel2/Diffuse/T4M 6 Textures")){
			MenuTextureSM2 = EnumShaderGLES2.T4M_6_Textures_HighSpec;
			ShaderModel = SM.ShaderModel2;
		}else
		if (CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.shader == Shader.Find("T4MShaders/ShaderModel2/Diffuse/T4M World Projection Shader")){
			MenuTextureSM2 = EnumShaderGLES2.T4M_World_Projection_HighSpec;
			ShaderModel = SM.ShaderModel2;
			UpSideTile = CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.GetVector ("_Tiling");
			UpSideF = CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.GetFloat ("_UpSide");
			BlendFac= CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.GetFloat ("_Blend");
		}else
		if (CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.shader == Shader.Find("T4MShaders/ShaderModel2/Specular/T4M 2 Textures Spec")){
			MenuTextureSM2 = EnumShaderGLES2.T4M_2_Textures_Specular;
			ShaderModel = SM.ShaderModel2;
		}else
		if (CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.shader == Shader.Find("T4MShaders/ShaderModel2/Specular/T4M 3 Textures Spec")){
			MenuTextureSM2 = EnumShaderGLES2.T4M_3_Textures_Specular;
			ShaderModel = SM.ShaderModel2;
		}else
		if (CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.shader == Shader.Find("T4MShaders/ShaderModel2/Specular/T4M 4 Textures Spec")){
			MenuTextureSM2 = EnumShaderGLES2.T4M_4_Textures_Specular;
			ShaderModel = SM.ShaderModel2;
		}else
		if (CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.shader == Shader.Find("T4MShaders/ShaderModel2/MobileLM/T4M 2 Textures for Mobile")){
			MenuTextureSM2 = EnumShaderGLES2.T4M_2_Textures_4_Mobile;
			ShaderModel = SM.ShaderModel2;
		}else
		if (CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.shader == Shader.Find("T4MShaders/ShaderModel2/MobileLM/T4M 3 Textures for Mobile")){
			MenuTextureSM2 = EnumShaderGLES2.T4M_3_Textures_4_Mobile;
			ShaderModel = SM.ShaderModel2;
		}else
		if (CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.shader == Shader.Find("T4MShaders/ShaderModel2/MobileLM/T4M 4 Textures for Mobile")){
			MenuTextureSM2 = EnumShaderGLES2.T4M_4_Textures_4_Mobile;
			ShaderModel = SM.ShaderModel2;
		}//else
		//if (CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.shader == Shader.Find("T4MShaders/ShaderModel2/MobileLM/T4M World Projection Shader_Mobile")){
		//	MenuTextureSM2 = EnumShaderGLES2.T4M_World_Projection_Mobile;
		//	ShaderModel = SM.ShaderModel2;
		//	UpSideTile = CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.GetVector ("_Tiling");
		//	UpSideF = CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.GetFloat ("_UpSide");
		//	BlendFac= CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.GetFloat ("_Blend");
		//}
		else
		if (CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.shader == Shader.Find("T4MShaders/ShaderModel2/Toon/T4M 2 Textures Toon")){
			MenuTextureSM2 = EnumShaderGLES2.T4M_2_Textures_Toon;
			ShaderModel = SM.ShaderModel2;
		}else
		if (CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.shader == Shader.Find("T4MShaders/ShaderModel2/Toon/T4M 3 Textures Toon")){
			MenuTextureSM2 = EnumShaderGLES2.T4M_3_Textures_Toon;
			ShaderModel = SM.ShaderModel2;
		}else
		if (CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.shader == Shader.Find("T4MShaders/ShaderModel2/Toon/T4M 4 Textures Toon")){
			MenuTextureSM2 = EnumShaderGLES2.T4M_4_Textures_Toon;
			ShaderModel = SM.ShaderModel2;
		}else
		if (CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.shader == Shader.Find("T4MShaders/ShaderModel2/Bump/T4M 2 Textures Bumped")){
			MenuTextureSM2 = EnumShaderGLES2.T4M_2_Textures_Bumped;
			ShaderModel = SM.ShaderModel2;
		}else
		if (CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.shader == Shader.Find("T4MShaders/ShaderModel2/Bump/T4M 3 Textures Bumped")){
			MenuTextureSM2 = EnumShaderGLES2.T4M_3_Textures_Bumped;
			ShaderModel = SM.ShaderModel2;
		}else
		if (CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.shader == Shader.Find("T4MShaders/ShaderModel2/Bump/T4M 4 Textures Bumped")){
			MenuTextureSM2 = EnumShaderGLES2.T4M_4_Textures_Bumped;
			ShaderModel = SM.ShaderModel2;
		}else
		if (CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.shader == Shader.Find("T4MShaders/ShaderModel2/Bump/T4M 2 Textures Bumped Mobile")){
			MenuTextureSM2 = EnumShaderGLES2.T4M_2_Textures_Bumped_Mobile;
			ShaderModel = SM.ShaderModel2;
		}else
		if (CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.shader == Shader.Find("T4MShaders/ShaderModel2/Bump/T4M 3 Textures Bumped Mobile")){
			MenuTextureSM2 = EnumShaderGLES2.T4M_3_Textures_Bumped_Mobile;
			ShaderModel = SM.ShaderModel2;
		}else
		if (CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.shader == Shader.Find("T4MShaders/ShaderModel2/Bump/T4M 2 Textures Bump Specular Mobile")){
			MenuTextureSM2 = EnumShaderGLES2.T4M_2_Textures_Bumped_SPEC_Mobile;
			ShaderModel = SM.ShaderModel2;
		}else
		if (CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.shader == Shader.Find("T4MShaders/ShaderModel2/BumpDLM/T4M 2 Textures Bumped DLM Mobile")){
			MenuTextureSM2 = EnumShaderGLES2.T4M_2_Textures_Bumped_DirectionalLM_Mobile;
			ShaderModel = SM.ShaderModel2;
		}else
		if (CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.shader == Shader.Find("T4MShaders/ShaderModel2/Bump/T4M 2 Textures Bump Specular")){
			MenuTextureSM2 = EnumShaderGLES2.T4M_2_Textures_Bumped_SPEC;
			ShaderModel = SM.ShaderModel2;
		}else
		if (CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.shader == Shader.Find("T4MShaders/ShaderModel2/Bump/T4M 3 Textures Bump Specular")){
			MenuTextureSM2 = EnumShaderGLES2.T4M_3_Textures_Bumped_SPEC;
			ShaderModel = SM.ShaderModel2;
		}else
		if (CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.shader == Shader.Find("T4MShaders/ShaderModel2/BumpDLM/T4M 2 Textures Bumped DLM")){
			MenuTextureSM2 = EnumShaderGLES2.T4M_2_Textures_Bumped_DirectionalLM;
			ShaderModel = SM.ShaderModel2;
		}else
		if (CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.shader == Shader.Find("T4MShaders/ShaderModel2/BumpDLM/T4M 3 Textures Bumped DLM")){
			MenuTextureSM2 = EnumShaderGLES2.T4M_3_Textures_Bumped_DirectionalLM;
			ShaderModel = SM.ShaderModel2;
		}else
		if (CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.shader == Shader.Find("T4MShaders/ShaderModel3/Diffuse/T4M 2 Textures")){
			MenuTextureSM3 = EnumShaderGLES3.T4M_2_Textures_Diffuse;
			ShaderModel = SM.ShaderModel3;
		}else
		if (CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.shader == Shader.Find("T4MShaders/ShaderModel3/Diffuse/T4M 3 Textures")){
			MenuTextureSM3 = EnumShaderGLES3.T4M_3_Textures_Diffuse;
			ShaderModel = SM.ShaderModel3;
		}else
		if (CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.shader == Shader.Find("T4MShaders/ShaderModel3/Diffuse/T4M 3 Textures")){
			MenuTextureSM3 = EnumShaderGLES3.T4M_3_Textures_Diffuse;
			ShaderModel = SM.ShaderModel3;
		}else
		if (CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.shader == Shader.Find("T4MShaders/ShaderModel3/Diffuse/T4M 4 Textures")){
			MenuTextureSM3 = EnumShaderGLES3.T4M_4_Textures_Diffuse;
			ShaderModel = SM.ShaderModel3;
		}else
		if (CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.shader == Shader.Find("T4MShaders/ShaderModel3/Diffuse/T4M 5 Textures")){
			MenuTextureSM3 = EnumShaderGLES3.T4M_5_Textures_Diffuse;
			ShaderModel = SM.ShaderModel3;
		}else
		if (CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.shader == Shader.Find("T4MShaders/ShaderModel3/Diffuse/T4M 6 Textures")){
			MenuTextureSM3 = EnumShaderGLES3.T4M_6_Textures_Diffuse;
			ShaderModel = SM.ShaderModel3;
		}else
		if (CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.shader == Shader.Find("T4MShaders/ShaderModel3/Specular/T4M 2 Textures Spec")){
			MenuTextureSM3 = EnumShaderGLES3.T4M_2_Textures_Specular;
			ShaderModel = SM.ShaderModel3;
		}else
		if (CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.shader == Shader.Find("T4MShaders/ShaderModel3/Specular/T4M 3 Textures Spec")){
			MenuTextureSM3 = EnumShaderGLES3.T4M_3_Textures_Specular;
			ShaderModel = SM.ShaderModel3;
		}else
		if (CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.shader == Shader.Find("T4MShaders/ShaderModel3/Specular/T4M 4 Textures Spec")){
			MenuTextureSM3 = EnumShaderGLES3.T4M_4_Textures_Specular;
			ShaderModel = SM.ShaderModel3;
		}else
		if (CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.shader == Shader.Find("T4MShaders/ShaderModel3/Bump/T4M 2 Textures Bump")){
			MenuTextureSM3 = EnumShaderGLES3.T4M_2_Textures_Bumped;
			ShaderModel = SM.ShaderModel3;
		}else
		if (CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.shader == Shader.Find("T4MShaders/ShaderModel3/Bump/T4M 3 Textures Bump")){
			MenuTextureSM3 = EnumShaderGLES3.T4M_3_Textures_Bumped;
			ShaderModel = SM.ShaderModel3;
		}else
		if (CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.shader == Shader.Find("T4MShaders/ShaderModel3/Bump/T4M 4 Textures Bump")){
			MenuTextureSM3 = EnumShaderGLES3.T4M_4_Textures_Bumped;
			ShaderModel = SM.ShaderModel3;
		}
		else
		if (CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.shader == Shader.Find("T4MShaders/ShaderModel3/BumpSpec/T4M 2 Textures Bump Spec")){
			MenuTextureSM3 = EnumShaderGLES3.T4M_2_Textures_Bumped_SPEC;
			ShaderModel = SM.ShaderModel3;
		}else
		if (CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.shader == Shader.Find("T4MShaders/ShaderModel3/BumpSpec/T4M 3 Textures Bump Spec")){
			MenuTextureSM3 = EnumShaderGLES3.T4M_3_Textures_Bumped_SPEC;
			ShaderModel = SM.ShaderModel3;
		}else
		if (CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.shader == Shader.Find("T4MShaders/ShaderModel3/BumpSpec/T4M 4 Textures Bump Spec")){
			MenuTextureSM3 = EnumShaderGLES3.T4M_4_Textures_Bumped_SPEC;
			ShaderModel = SM.ShaderModel3;
		}else{
			ShaderModel = SM.CustomShader;
			CustomShader=CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.shader;
			if (CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.HasProperty("_Tiling")){
				UpSideTile = CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.GetVector ("_Tiling");
				UpSideF = CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.GetFloat ("_UpSide");
				BlendFac= CurrentSelect.gameObject.GetComponent <T4MObjSC>().T4MMaterial.GetFloat ("_Blend");
				
			}
		}		
					
	}
	
}