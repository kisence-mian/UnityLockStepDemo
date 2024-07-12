Shader "T4MShaders/ShaderModel2/Bump/T4M 3 Textures Bump Specular" {
Properties {
	_SpecColor ("Specular Color", Color) = (1, 1, 1, 1)
	_ShininessL0 ("Layer1Shininess", Range (0.03, 1)) = 0.078125
	_Splat0 ("Layer 1", 2D) = "white" {}
	_ShininessL1 ("Layer2Shininess", Range (0.03, 1)) = 0.078125
	_Splat1 ("Layer 2", 2D) = "white" {}
	_ShininessL2 ("Layer3Shininess", Range (0.03, 1)) = 0.078125
	_Splat2 ("Layer 3", 2D) = "white" {}
	_BumpSplat0 ("Layer1Normalmap", 2D) = "bump" {}
	_BumpSplat1 ("Layer2Normalmap", 2D) = "bump" {}
	_BumpSplat2 ("Layer3Normalmap", 2D) = "bump" {}
	_Control ("Control (RGBA)", 2D) = "white" {}
	_MainTex ("Never Used", 2D) = "white" {}
}

SubShader {
	Tags {
		"SplatCount" = "3"
		"RenderType" = "Opaque"
	}
CGPROGRAM
#pragma surface surf T4MBlinnPhong exclude_path:prepass noforwardadd 
#pragma exclude_renderers xbox360 ps3 flash
inline fixed4 LightingT4MBlinnPhong (SurfaceOutput s, fixed3 lightDir, fixed3 halfDir, fixed atten)
{
	fixed diff = max (0, dot (s.Normal, lightDir));
	fixed nh = max (0, dot (s.Normal, halfDir));
	fixed spec = pow (nh, s.Specular*128) * s.Gloss;
	
	fixed4 c;
	c.rgb = (s.Albedo * _LightColor0.rgb * diff + _SpecColor.rgb * spec) * (atten*2);
	c.a = 0.0;
	return c;
}
struct Input {

	float2 uv_Control : TEXCOORD0;
	float2 uv_Splat0: TEXCOORD1;
	float2 uv_Splat1 : TEXCOORD2;
	float2 uv_Splat2 : TEXCOORD3;
};
sampler2D _Control;
sampler2D _Splat0, _Splat1,_Splat2;
sampler2D _BumpSplat0,_BumpSplat1,_BumpSplat2;
fixed _ShininessL0;
fixed _ShininessL1;
fixed _ShininessL2;

void surf (Input IN, inout SurfaceOutput o) {
	fixed3 splat_control = tex2D (_Control, IN.uv_Control).rgb;
		
	fixed4 lay1 = tex2D (_Splat0, IN.uv_Splat0).rgba;
	fixed4 lay2 = tex2D (_Splat1, IN.uv_Splat1).rgba;
	fixed4 lay3 = tex2D (_Splat2, IN.uv_Splat2).rgba;
	o.Alpha = 0.0;
	o.Albedo.rgb = (lay1.rgb * splat_control.r + lay2.rgb * splat_control.g+ lay3.rgb * splat_control.b);
	
	fixed3 lay1B = UnpackNormal (tex2D(_BumpSplat0, IN.uv_Splat0)).rgb;
	fixed3 lay2B = UnpackNormal (tex2D(_BumpSplat1, IN.uv_Splat1)).rgb;
	fixed3 lay3B = UnpackNormal (tex2D(_BumpSplat2, IN.uv_Splat2)).rgb;
	o.Normal = (lay1B.rgb * splat_control.r + lay2B.rgb * splat_control.g+ lay3B.rgb * splat_control.b);
	o.Gloss = (lay1.a * splat_control.r + lay2.a * splat_control.g + lay3.a * splat_control.b);
	o.Specular = (_ShininessL0 * splat_control.r+_ShininessL1 * splat_control.g+_ShininessL2 * splat_control.b) ;
}
ENDCG  
}
FallBack "Specular"
}