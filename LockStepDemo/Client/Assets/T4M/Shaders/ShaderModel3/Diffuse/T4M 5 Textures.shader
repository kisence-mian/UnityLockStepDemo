Shader "T4MShaders/ShaderModel3/Diffuse/T4M 5 Textures" {
Properties {
	_Splat0 ("Layer 1 (R)", 2D) = "white" {}
	_Splat1 ("Layer 2 (G)", 2D) = "white" {}
	_Splat2 ("Layer 3 (B)", 2D) = "white" {}
	_Splat3 ("Layer 4 (A)", 2D) = "white" {}
	_Splat4 ("Layer 5", 2D) = "white" {}
	_Control ("Control (RGBA)", 2D) = "white" {}
	_Control2 ("Control2 (RGBA)", 2D) = "white" {}
	_MainTex ("Never Used", 2D) = "white" {}
} 

SubShader {
	Tags {
		"SplatCount" = "5"
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
	float2 uv_Splat2 : TEXCOORD3;
	float2 uv_Splat3 : TEXCOORD4;
	float2 uv_Splat4 : TEXCOORD5;
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

sampler2D _Control,_Control2;
sampler2D _Splat0,_Splat1,_Splat2,_Splat3,_Splat4;

void surf (Input IN, inout SurfaceOutput o) {

	half4 splat_control = tex2D (_Control, IN.uv_Control);
	half2 splat_control2 = tex2D (_Control2, IN.uv_Control);
	half3 col;
	half3 col2;
	half3 splat0 = tex2D (_Splat0, IN.uv_Splat0).rgb;
	half3 splat1 = tex2D (_Splat1, IN.uv_Splat1).rgb;
	half3 splat2 = tex2D (_Splat2, IN.uv_Splat2).rgb;
	half3 splat3 = tex2D (_Splat3, IN.uv_Splat3).rgb;
	half3 splat4 = tex2D (_Splat4, IN.uv_Splat4).rgb;
	
	col  += splat_control.r * splat0.rgb;

	col += splat_control.g * splat1.rgb;
	
	col += splat_control.b * splat2.rgb;
	
	col2  += splat_control2.r * splat3.rgb;
	
	col2  += splat_control2.g * splat4.rgb;
	
	
	col += splat_control.a * col2.rgb;

	o.Albedo = col;
	o.Alpha = 0.0;
}
ENDCG  
}
FallBack "Diffuse"
}
