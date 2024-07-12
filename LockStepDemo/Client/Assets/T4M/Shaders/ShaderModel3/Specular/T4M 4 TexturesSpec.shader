Shader "T4MShaders/ShaderModel3/Specular/T4M 4 Textures Spec" {
Properties {
	_SpecColor ("Specular Color", Color) = (1, 1, 1, 1)
	_ShininessL0 ("Layer1Shininess", Range (0.03, 1)) = 0.078125
	_Splat0 ("Layer 1 (R)", 2D) = "white" {}
	_ShininessL1 ("Layer2Shininess", Range (0.03, 1)) = 0.078125
	_Splat1 ("Layer 2 (G)", 2D) = "white" {}
	_ShininessL2 ("Layer3Shininess", Range (0.03, 1)) = 0.078125
	_Splat2 ("Layer 3 (B)", 2D) = "white" {}
	_ShininessL3 ("Layer4Shininess", Range (0.03, 1)) = 0.078125
	_Splat3 ("Layer 4 (A)", 2D) = "white" {}
	_Control ("Control (RGBA)", 2D) = "white" {}
	_MainTex ("Never Used", 2D) = "white" {}
} 

SubShader {
	Tags {
		"SplatCount" = "4"
		"Queue" = "Geometry-100"
		"RenderType" = "Opaque"
	}
CGPROGRAM
#pragma surface surf BlinnPhong vertex:vert
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
sampler2D _Splat0,_Splat1,_Splat2,_Splat3;
fixed _ShininessL0;
fixed _ShininessL1;
fixed _ShininessL2;
fixed _ShininessL3;

void surf (Input IN, inout SurfaceOutput o) {

	half4 splat_control = tex2D (_Control, IN.uv_Control);
	half3 col;
	half4 splat0 = tex2D (_Splat0, IN.uv_Splat0);
	half4 splat1 = tex2D (_Splat1, IN.uv_Splat1);
	half4 splat2 = tex2D (_Splat2, IN.uv_Splat2);
	half4 splat3 = tex2D (_Splat3, IN.uv_Splat3);
	
	col  += splat_control.r * splat0.rgb;
	o.Gloss = splat0.a * splat_control.r ;
	o.Specular = _ShininessL0 * splat_control.r;

	col += splat_control.g * splat1.rgb;
	o.Gloss += splat1.a * splat_control.g;
	o.Specular += _ShininessL1 * splat_control.g;
	
	col += splat_control.b * splat2.rgb;
	o.Gloss += splat2.a * splat_control.b;
	o.Specular += _ShininessL2 * splat_control.b;
	
	col += splat_control.a * splat3.rgb;
	o.Gloss += splat3.a * splat_control.a;
	o.Specular += _ShininessL3 * splat_control.a;

	o.Albedo = col;
	o.Alpha = 0.0;
}
ENDCG  
}
FallBack "Specular"
}