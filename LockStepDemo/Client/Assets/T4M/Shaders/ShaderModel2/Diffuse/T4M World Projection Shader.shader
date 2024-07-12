// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "T4MShaders/ShaderModel2/Diffuse/T4M World Projection Shader" {
    Properties {
        _UpSide ("Up/Side Fighting", Float) = 2.5
        _Blend ("Blend Factor", Float) = 4
        _Tiling ("Up Texture Tiling (x/y) Side Texture Tiling (z/w)", Vector) = (0.5,0.5,0.5,0.5)
        _Splat0 ("Up Textue", 2D) = "white" {}
        _Splat1 ("Side Textue", 2D) = "white" {}
        _Control ("Never Used", 2D) = "white" {}
		_MainTex ("Never Used", 2D) = "white" {}
    }
	 Category {
	  SubShader { 
	        CGPROGRAM
	        #pragma surface surf Lambert vertex:vert
	        float _UpSide;
	        float _Blend;
	        float4 _Tiling;
	        sampler2D _Splat0;
	        sampler2D _Splat1;
	
	        struct Input {
	            float3 vertex;
	            float3 normal;
	            float3 worldPos;
	            float3 worldNormal; 
	        };
	
	        void vert (inout appdata_full v, out Input o) {
				 UNITY_INITIALIZE_OUTPUT(Input, o);
				 o.worldNormal = mul(unity_ObjectToWorld, float4(v.normal, 0.0f)).xyz;
	 	         o.worldPos = mul(unity_ObjectToWorld, v.vertex);
	        } 
	   
	        void surf (Input IN, inout SurfaceOutput o) {
	      	 	float2 SideTile = float2(_Tiling.z/10, _Tiling.w/10);
	        	fixed3 Up = tex2D (_Splat0, float2(_Tiling.x/10, _Tiling.y/10) *IN.worldPos.zx);
	        	fixed3 Side = tex2D (_Splat1, SideTile *IN.worldPos.xy);
	        	fixed3 Side2 = tex2D (_Splat1, SideTile *IN.worldPos.zy); 
	            fixed3 projnormal = saturate(pow(normalize(IN.worldNormal)*_UpSide, _Blend)); 
	            o.Albedo.rgb = lerp(lerp(Up, Side, projnormal.z), Side2, projnormal.x);
	            o.Alpha = 0.0;
	        }
	        ENDCG
	        }
	    }
    FallBack "Diffuse"
}