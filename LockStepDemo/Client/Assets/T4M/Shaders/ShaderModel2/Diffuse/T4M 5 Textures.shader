Shader "T4MShaders/ShaderModel2/Diffuse/T4M 5 Textures" {
Properties {
				_Splat0 ("Layer 1", 2D) = "white" {}
                _Splat1 ("Layer 2", 2D) = "white" {}
                _Splat2 ("Layer 3", 2D) = "white" {}
                _Splat3 ("Layer 4", 2D) = "white" {}
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
#pragma surface surf Lambert 
#pragma exclude_renderers gles xbox360 ps3 flash


struct Input {
        float2 uv_Control : TEXCOORD0;
        float2 uv_Splat0 : TEXCOORD1;
        float2 uv_Splat1 : TEXCOORD2;
        float2 uv_Splat2 : TEXCOORD3;
        float2 uv_Splat3 : TEXCOORD4;
        float2 uv_Splat4 : TEXCOORD5;
};
 
sampler2D _Control,_Control2;
sampler2D _Splat0,_Splat1,_Splat2,_Splat3,_Splat4;
 
void surf (Input IN, inout SurfaceOutput o) {
                fixed4 splat_control = tex2D (_Control, IN.uv_Control).rgba;
                fixed2 splat_control2 = tex2D (_Control2, IN.uv_Control).rg;
                fixed3 col;
                col  = splat_control.r * tex2D (_Splat0, IN.uv_Splat0).rgb;
                col += splat_control.g * tex2D (_Splat1, IN.uv_Splat1).rgb;
                col += splat_control.b * tex2D (_Splat2, IN.uv_Splat2).rgb;
				fixed3 col2;
				col2 = splat_control2.r * tex2D (_Splat3, IN.uv_Splat3).rgb;
                col2 += splat_control2.g* tex2D (_Splat4, IN.uv_Splat4).rgb;
				
				col += splat_control.a * col2.rgb;
				
				
                o.Albedo = col.rgb;
                o.Alpha = 0.0;
}
ENDCG 
}
FallBack "Diffuse"
}
