Shader "T4MShaders/ShaderModel2/Bump/T4M 4 Textures Bumped" {
Properties {
	_Splat0 ("Layer 1", 2D) = "white" {}
	_Splat1 ("Layer 2", 2D) = "white" {}
	_Splat2 ("Layer 3", 2D) = "white" {}
	_Splat3 ("Layer 4", 2D) = "white" {}
	_BumpSplat0 ("Layer1Normalmap", 2D) = "bump" {}
	_BumpSplat1 ("Layer2Normalmap", 2D) = "bump" {}
	_BumpSplat2 ("Layer3Normalmap", 2D) = "bump" {}
	_BumpSplat3 ("Layer4Normalmap", 2D) = "bump" {}
	_Control ("Control (RGBA)", 2D) = "white" {}
	_MainTex ("Never Used", 2D) = "white" {}
}

SubShader {
	Tags {
		"SplatCount" = "4"
		"RenderType" = "Opaque"
	}
CGPROGRAM
#pragma surface surf T4M exclude_path:prepass noforwardadd  
#pragma exclude_renderers xbox360 ps3 flash gles
inline fixed4 LightingT4M (SurfaceOutput s, fixed3 lightDir, fixed atten)
{
	fixed diff = dot (s.Normal, lightDir);
	fixed4 c;
	c.rgb = s.Albedo * _LightColor0.rgb * (diff * atten * 2);
	c.a = 0.0;
	return c;
}

struct Input {
	float2 uv_Control : TEXCOORD0;
	float2 uv_Splat0: TEXCOORD1;
	float2 uv_Splat1 : TEXCOORD2;
	float2 uv_Splat2 : TEXCOORD3;
	float2 uv_Splat3 : TEXCOORD4;
};
sampler2D _Control;
sampler2D _Splat0, _Splat1,_Splat2,_Splat3;
sampler2D _BumpSplat0,_BumpSplat1,_BumpSplat2,_BumpSplat3;

void surf (Input IN, inout SurfaceOutput o) {
	fixed4 splat_control = tex2D (_Control, IN.uv_Control).rgba;
		
	fixed3 lay1 = tex2D (_Splat0, IN.uv_Splat0).rgb;
	fixed3 lay2 = tex2D (_Splat1, IN.uv_Splat1).rgb;
	fixed3 lay3 = tex2D (_Splat2, IN.uv_Splat2).rgb;
	fixed3 lay4 = tex2D (_Splat3, IN.uv_Splat3).rgb;
	o.Alpha = 0.0;
	o.Albedo.rgb = (lay1 * splat_control.r + lay2 * splat_control.g+ lay3 * splat_control.b + lay4 * splat_control.a);
	
	fixed3 lay1B = UnpackNormal (tex2D(_BumpSplat0, IN.uv_Splat0));
	fixed3 lay2B = UnpackNormal (tex2D(_BumpSplat1, IN.uv_Splat1));
	fixed3 lay3B = UnpackNormal (tex2D(_BumpSplat2, IN.uv_Splat2));
	fixed3 lay4B = UnpackNormal (tex2D(_BumpSplat3, IN.uv_Splat3));
	o.Normal = (lay1B * splat_control.r + lay2B * splat_control.g+ lay3B * splat_control.b+ lay4B * splat_control.a);
}
ENDCG  
}
// Fallback to Diffuse
Fallback "Diffuse"
}
