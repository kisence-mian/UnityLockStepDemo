// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "T4MShaders/ShaderModel2/Toon/T4M 4 Textures Toon" {
	Properties {
		_Splat0 ("Layer 1", 2D) = "white" {}
		_Splat1 ("Layer 2", 2D) = "white" {}
		_Splat2 ("Layer 3", 2D) = "white" {}
		_Splat3 ("Layer 4", 2D) = "white" {}
		_Control ("Control (RGBA)", 2D) = "white" {}
		_ToonShade ("ToonShader Cubemap(RGB)", CUBE) = "" { Texgen CubeNormal }
		_MainTex ("Never Used", 2D) = "white" {}
		
	}
		CGINCLUDE
#pragma exclude_renderers xbox360 ps3
		#include "UnityCG.cginc"
 
		fixed3 _Color;
		sampler2D _Splat0 ;
		sampler2D _Splat1 ;
		sampler2D _Splat2 ;
		sampler2D _Splat3 ;
		sampler2D _Control;
		samplerCUBE _ToonShade;

		struct appdata {
			float4 vertex : POSITION;
			float2 texcoord : TEXCOORD0;
			float3 normal : NORMAL;
			};
		struct v2f {
			float4  pos : SV_POSITION;
			float2  uv[5] : TEXCOORD0;
			float3 cubenormal : TEXCOORD5;
		}; 
 
		float4 _Splat0_ST;
		float4 _Splat1_ST;
		float4 _Splat2_ST;
		float4 _Splat3_ST;
		float4 _Control_ST;
 
		v2f vert (appdata v)
		{
			v2f o;
			o.pos = UnityObjectToClipPos (v.vertex);
			o.uv[0] = TRANSFORM_TEX (v.texcoord, _Splat0);
			o.uv[1] = TRANSFORM_TEX (v.texcoord, _Splat1);
			o.uv[2] = TRANSFORM_TEX (v.texcoord, _Splat2);
			o.uv[3] = TRANSFORM_TEX (v.texcoord, _Splat3);
			o.uv[4] = TRANSFORM_TEX (v.texcoord, _Control);
			o.cubenormal = mul (UNITY_MATRIX_MV, float4(v.normal,0));
			return o;
		}
 
		fixed4 frag (v2f i) : COLOR
		{
			fixed4 Mask = tex2D( _Control, i.uv[4].xy );
			fixed4 c;
			
			fixed3 cube = texCUBE(_ToonShade, i.cubenormal);
			
			c.xyz = (tex2D( _Splat0, i.uv[0].xy ).xyz * Mask.r + tex2D( _Splat1, i.uv[1].xy ).xyz * Mask.g + tex2D( _Splat2, i.uv[2].xy ).xyz * Mask.b+ tex2D( _Splat3, i.uv[3].xy ).xyz * Mask.a)*cube.rgb * 2 ;
			c.w = 0;
			return c;
		}
		ENDCG
		SubShader {
			Tags { "RenderType"="Opaque" }
			Pass {
			Cull Back
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			ENDCG
		}
	}	
}