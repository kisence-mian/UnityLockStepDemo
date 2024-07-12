Shader "T4MShaders/ShaderModel3/Diffuse/T4M 2 Textures" {
Properties {
	_Splat0 ("Layer 1 (R)", 2D) = "white" {}
	_Splat1 ("Layer 2 (G)", 2D) = "white" {}
	_Control ("Control (RGBA)", 2D) = "white" {}
	_MainTex ("Never Used", 2D) = "white" {}
} 

SubShader {
	Tags {
		"SplatCount" = "2"
		"Queue" = "Geometry-100"
		"RenderType" = "Opaque"
	}
CGPROGRAM
#pragma surface surf Lambert vertex:vert
#pragma target 3.0
#pragma exclude_renderers gles xbox360 ps3
#include "UnityCG.cginc"

struct Input {
	float3 worldPos;
	float2 uv_Control : TEXCOORD0;
	float2 uv_Splat0 : TEXCOORD1;
	float2 uv_Splat1 : TEXCOORD2;
};

void vert (inout appdata_full v) {

	float3 T1 = float3(1, 0, 1);
	float3 Bi = cross(T1, v.normal);
	float3 newTangent = cross(v.normal, Bi);
	
	normalize(newTangent);

	v.tangent.xyz = newTangent.xyz;
	
	if (dot(cross(v.normal,newTangent),Bi) < 0)
		v.tangent.w = -1.0f;
	else
		v.tangent.w = 1.0f;
}

sampler2D _Control;
sampler2D _Splat0,_Splat1;

void surf (Input IN, inout SurfaceOutput o) {

	half2 splat_control = tex2D (_Control, IN.uv_Control);
	half3 col;
	half4 splat0 = tex2D (_Splat0, IN.uv_Splat0);
	half4 splat1 = tex2D (_Splat1, IN.uv_Splat1);
	
	col  += splat_control.r * splat0.rgb;

	col += splat_control.g * splat1.rgb;

	o.Albedo = col;
	o.Alpha = 0.0;
}
ENDCG  
}
FallBack "Diffuse"
}
