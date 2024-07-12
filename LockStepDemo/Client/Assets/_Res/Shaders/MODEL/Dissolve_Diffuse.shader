Shader "MODEL/Dissolve/Dissolve_Diffuse" {
Properties {
	_MainTex ("Base (RGB)", 2D) = "white" {}
	_DissolveTex ("Dissolve (R)", 2D) = "white" {}
	_Amount("Amount",Range(0,1)) = 1
	_CutOff("Alpha Cutoff",Range(0,1)) = .5
}

SubShader {
	
	Tags {"IgnoreProjector"="True" "RenderType"="TransparentCutout"}
	LOD 300

	//Pass{
		//AlphaTest Greater [_CutOff]
		//SetTexture[_DissolveTex]
		//SetTexture[_MainTex]
	//}

	Cull Back
	ZWrite On

	CGPROGRAM
	sampler2D _MainTex;
	sampler2D _DissolveTex;
	
	half _Amount;
	
	#pragma surface surf Lambert alphatest:Zero

	struct Input {
		float2 uv_MainTex;
		float2 uv_DissolveTex;
	};

	void surf (Input IN, inout SurfaceOutput o) {	
		half4 tex = tex2D(_MainTex, IN.uv_MainTex);
		half4 texd = tex2D(_DissolveTex, IN.uv_DissolveTex);
		o.Albedo = tex.rgb;
		o.Alpha =  _Amount - texd.r;
	}

	ENDCG
}

Fallback "MODEL/Cull/Alpha Cull Back"
}