Shader "T4MShaders/ShaderModel2/Specular/T4M 4 Textures Spec" {
Properties {
	_SpecColor ("Specular Color", Color) = (1, 1, 1, 1)
	_ShininessL0 ("Layer1Shininess", Range (0.03, 1)) = 0.078125
	_Splat0 ("Layer 1", 2D) = "white" {}
	_ShininessL1 ("Layer2Shininess", Range (0.03, 1)) = 0.078125
	_Splat1 ("Layer 2", 2D) = "white" {}
	_ShininessL2 ("Layer3Shininess", Range (0.03, 1)) = 0.078125
	_Splat2 ("Layer 3", 2D) = "white" {}
	_ShininessL3 ("Layer4Shininess", Range (0.03, 1)) = 0.078125
	_Splat3 ("Layer 4", 2D) = "white" {}
	_Control ("Control (RGBA)", 2D) = "white" {}
	_MainTex ("Never Used", 2D) = "white" {}
}
                
SubShader {
	Tags {
	   "SplatCount" = "4"
	   "RenderType" = "Opaque"
	}
CGPROGRAM
#pragma surface surf T4MBlinnPhong
#pragma exclude_renderers xbox360 ps3

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
	float2 uv_Splat0 : TEXCOORD1;
	float2 uv_Splat1 : TEXCOORD2;
	float2 uv_Splat2 : TEXCOORD3;
	float2 uv_Splat3 : TEXCOORD4;
};
 
sampler2D _Control;
sampler2D _Splat0,_Splat1,_Splat2,_Splat3;
fixed _ShininessL0;
fixed _ShininessL1;
fixed _ShininessL2;
fixed _ShininessL3;

void surf (Input IN, inout SurfaceOutput o) {
	fixed4 splat_control = tex2D (_Control, IN.uv_Control).rgba;
		
	fixed4 lay1 = tex2D (_Splat0, IN.uv_Splat0);
	fixed4 lay2 = tex2D (_Splat1, IN.uv_Splat1);
	fixed4 lay3 = tex2D (_Splat2, IN.uv_Splat2);
	fixed4 lay4 = tex2D (_Splat3, IN.uv_Splat3);
	o.Alpha = 0.0;
	o.Albedo.rgb = (lay1 * splat_control.r + lay2 * splat_control.g + lay3 * splat_control.b + lay4 * splat_control.a);
	o.Gloss = (lay1.a * splat_control.r + lay2.a * splat_control.g + lay3.a * splat_control.b + lay4.a * splat_control.a);
	o.Specular = (_ShininessL0 * splat_control.r + _ShininessL1 * splat_control.g + _ShininessL2 * splat_control.b + _ShininessL3 * splat_control.a);
}
ENDCG 
}
FallBack "Specular"
}