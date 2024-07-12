// Upgrade NOTE: replaced '_Projector' with 'unity_Projector'
// Upgrade NOTE: replaced '_ProjectorClip' with 'unity_ProjectorClip'

Shader "Hidden/BrushPreview"
{
	Properties {
		_Transp ("Transparency", Range(0,1)) = 1 
		_MainTex ("Texture", 2D) = "" { }
		_MaskTex ("Texture", 2D) = "" { }
	}

	SubShader{
		Tags{ "Queue" = "Transparent" }
		ColorMask RGB
		Blend SrcAlpha OneMinusSrcAlpha

		Pass{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata_t {
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
			};

			float4x4 unity_Projector;
			float4x4 unity_ProjectorClip;
			struct v2f {
				float4 vertex : SV_POSITION;
				float4 texcoord : TEXCOORD0;
			};

			sampler2D _MainTex;
			sampler2D _MaskTex;
			float4 _MainTex_ST;
			float _Transp;
			v2f vert(appdata_t v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.texcoord = mul(unity_Projector, v.vertex);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 colMain = tex2Dproj(_MainTex, UNITY_PROJ_COORD(i.texcoord));
				fixed4 colMask = tex2Dproj(_MaskTex, UNITY_PROJ_COORD(i.texcoord));
				fixed4 col = fixed4(colMain.x,colMain.y,colMain.z, colMask.a);
				return col;
			}
			ENDCG
		}
	}
}