// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Upgrade NOTE: commented out 'sampler2D unity_Lightmap', a built-in variable
// Upgrade NOTE: replaced tex2D unity_Lightmap with UNITY_SAMPLE_TEX2D

Shader "T4MShaders/ShaderModel2/Unlit/T4M World Projection Shader + LM"{
    Properties{
    	_UpSide ("Up/Side Fighting", Float) = 2.5
        _Blend ("Blend Factor", Float) = 4
        _Tiling ("Up Texture Tiling (x/y) Side Texture Tiling (z/w)", Vector) = (0.5,0.5,0.5,0.5)
        _Splat0 ("Up Textue", 2D) = "white" {}
       	_Splat1 ("Side Textue", 2D) = "white" {}
        _Control ("Never Used", 2D) = "white" {}
		_MainTex ("Never Used", 2D) = "white" {}
    }
    SubShader{
        Pass {
            CGPROGRAM
// Upgrade NOTE: excluded shader from DX11 and Xbox360; has structs without semantics (struct v2f members worldnormal,worldpos)
#pragma exclude_renderers d3d11 xbox360
                #include "UnityCG.cginc"
				#pragma vertex vert
				#pragma fragment frag
				#pragma multi_compile LIGHTMAP_ON LIGHTMAP_OFF
				#pragma exclude_renderers xbox360 ps3
				
                uniform sampler2D _Splat0;
                uniform sampler2D _Splat1;
                uniform float _UpSide;
                uniform float _Blend;
                float4 _Tiling;
                #ifdef LIGHTMAP_ON
		            //fixed4 unity_LightmapST;
		            // sampler2D unity_Lightmap;
		        #endif
                struct v2f
                {
                	#ifdef LIGHTMAP_ON
					float2  uv[1] : TEXCOORD0;
					#endif
                	float4 pos : SV_POSITION;
                    float3 worldnormal;
                    float3 worldpos;
                }; 

                v2f vert(appdata_base v)
                {
                    v2f o;
                    o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
                    o.worldnormal = mul(unity_ObjectToWorld, float4(v.normal, 0.0f)).xyz;
                    o.worldpos = mul(unity_ObjectToWorld, v.vertex);
                    #ifdef LIGHTMAP_ON
		            	o.uv[0] = v.texcoord.xy * unity_LightmapST.xy + unity_LightmapST.zw;
		            #endif
                    return o;
                } 
               
                fixed4 frag(v2f i) : COLOR
                {
                    float2 SideTile = float2(_Tiling.z/10, _Tiling.w/10);
                    fixed3 Up = tex2D(_Splat0, float2(_Tiling.x/10, _Tiling.y/10) *i.worldpos.zx);
                    fixed3 Side = tex2D(_Splat1, SideTile*i.worldpos.xy);
                    fixed3 Side2 = tex2D(_Splat1, SideTile*i.worldpos.zy);
                    i.worldnormal = normalize(i.worldnormal);
                    fixed3 projnormal = saturate(pow(i.worldnormal*_UpSide, _Blend));
                    fixed4 c;
					c.xyz =  lerp(lerp(Up, Side, projnormal.z), Side2, projnormal.x);
                    c.w = 0;
                    #ifdef LIGHTMAP_ON
		           		 c.rgb *= DecodeLightmap(UNITY_SAMPLE_TEX2D(unity_Lightmap, i.uv[0]));
		            #endif
					return c;
                }
            ENDCG
        }
    }
    FallBack "Diffuse"
}