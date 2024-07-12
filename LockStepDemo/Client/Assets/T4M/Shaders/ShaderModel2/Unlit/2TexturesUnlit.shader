// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: commented out 'sampler2D unity_Lightmap', a built-in variable
// Upgrade NOTE: replaced tex2D unity_Lightmap with UNITY_SAMPLE_TEX2D

Shader "T4MShaders/ShaderModel2/Unlit/T4M 2 Textures Unlit LM" {
Properties {
    _Splat0 ("Layer1 (RGB)", 2D) = "white" {}
	_Splat1 ("Layer2 (RGB)", 2D) = "white" {}
	_Control ("Control (RGBA)", 2D) = "white" {}
	_MainTex ("Never Used", 2D) = "white" {}
}
SubShader {
    Pass {

		CGPROGRAM
		#include "UnityCG.cginc"
		#pragma vertex vert
		#pragma fragment frag
		#pragma multi_compile LIGHTMAP_ON LIGHTMAP_OFF
		#pragma exclude_renderers xbox360 ps3
		sampler2D _Splat0 ;
		sampler2D _Splat1 ;
		sampler2D _Control;

		struct v2f {
			float4  pos : SV_POSITION;
			#ifdef LIGHTMAP_ON
			float2  uv[4] : TEXCOORD0;
			#endif
			#ifdef LIGHTMAP_OFF
			float2  uv[3] : TEXCOORD0;
			#endif
		};

		float4 _Splat0_ST;
		float4 _Splat1_ST;;
		float4 _Control_ST;
		#ifdef LIGHTMAP_ON
            //fixed4 unity_LightmapST;
            // sampler2D unity_Lightmap;
        #endif
		v2f vert (appdata_full v)
		{
			v2f o;
			o.pos = UnityObjectToClipPos (v.vertex);
			o.uv[0] = TRANSFORM_TEX (v.texcoord, _Splat0);
			o.uv[1] = TRANSFORM_TEX (v.texcoord, _Splat1);
			o.uv[2] = TRANSFORM_TEX (v.texcoord, _Control);
			#ifdef LIGHTMAP_ON
            	o.uv[3] = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
            #endif
			return o;
		}

		fixed4 frag (v2f i) : COLOR
		{
			fixed2 Mask = tex2D( _Control, i.uv[2].xy ).rg;
			fixed3 lay1 = tex2D( _Splat0, i.uv[0].xy );
			fixed3 lay2 = tex2D( _Splat1, i.uv[1].xy );
   				
    		fixed4 c;
			c.xyz = (lay1.xyz * Mask.r + lay2.xyz * Mask.g);
			#ifdef LIGHTMAP_ON
           		 c.rgb *= DecodeLightmap(UNITY_SAMPLE_TEX2D(unity_Lightmap, i.uv[3]));
            #endif
			c.w = 0;
			return c;
		}
		ENDCG
    }
}
} 