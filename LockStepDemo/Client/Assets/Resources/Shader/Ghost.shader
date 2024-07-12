Shader "Unlit/Ghost"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Color("_Color", Color) = (1,1,1,1)
	}
	SubShader
	{
		LOD 100

		Pass
		{
			
			Tags{ "Queue" = "3000"}
			Blend SrcAlpha OneMinusSrcAlpha 

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			
			#include "UnityCG.cginc"


			struct v2f
			{
				fixed4 pos:POSITION;
				fixed4 vertex:COLOR0;
				fixed2 uv:TEXCOORD0;
				fixed3 n:NORMAL;//世界坐标中，法线方向

			};

			sampler2D _MainTex;
			fixed4 _MainTex_ST;
			fixed4 _Color;
			
			v2f vert (appdata_base v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);

				o.vertex = v.vertex;
				float3 N = normalize( v.normal);
                N = mul(float4(N,0),unity_WorldToObject);
				o.n = normalize(N);

				return o;
			}
			
			fixed4 frag (v2f i) : COLOR
			{
				float4 col = tex2D(_MainTex, i.uv);
				fixed gray = 0.299 * col.r + 0.587 * col.g + 0.114* col.b;
				col.rgb = float3(gray , gray,gray);
				col.rgb = col.rgb  + _Color.rgb*0.6;

                fixed3 viewDir = normalize( WorldSpaceViewDir(i.vertex)); //视角方向

				fixed viewAngle = dot(i.n,viewDir);

				col.a = _Color.a * pow( cos(viewAngle),4);
				return col;
			}
			ENDCG
		}
	}
}
