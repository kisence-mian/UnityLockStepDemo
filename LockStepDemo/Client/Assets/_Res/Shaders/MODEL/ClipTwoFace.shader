// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "MODEL/Cull/ClipTwoFace" { 
Properties {//显示在面板上的可调参数
	  _Color ("Main Color", Color) = (1,1,1,1)
      _MainTex ("Base (RGB)", 2D) = "white"{}
	  _CutOff("Alpha Cutoff",Range(0,1)) = 0.44
	  _Pad("Pad",range(0,30)) = 30
   }

   SubShader {
    Pass {
		Tags{"queue" = "2050"}
		//Blend SrcAlpha OneMinusSrcAlpha
		//AlphaTest Greater [_CutOff]
		Cull Off

		CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Lighting.cginc"
            #include "UnityCG.cginc"


			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float3 normal : NORMAL; 
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float _Pad;
			float4 _Color;

			float _NowTime;
			float _CutOff;

            struct v2f
            {
                float4 pos:POSITION;
				float4 col:COLOR0;
				float4 lightDir :COLOR1;
                float4 ver:COLOR2;
                float3 n:COLOR3;

				float2 uv : TEXCOORD0;

            };


            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
				o.col = fixed4(0,0,0,1);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);

				
				//------  漫反射
				float3 N = normalize( v.normal);
                N = mul(float4(N,0),unity_WorldToObject);
                N = normalize(N);

                float3 lightDir = normalize( _WorldSpaceLightPos0);

                float angle = saturate(dot(N,lightDir));

                o.col.rgb += _LightColor0 * angle;
				o.col.rgb = _Color.rgb;

				//-----漫反射结束

                o.lightDir = float4(lightDir,0);
                o.ver = v.vertex;
                o.n = v.normal;

                return o;
            }

			

            fixed4 frag (v2f i) : COLOR
            {
				float4 tex = tex2D(_MainTex, i.uv);
				clip(tex.a - _CutOff);

                float4 col = i.col;

                i.n = (mul(float4(i.n,0),unity_WorldToObject));
                i.n = normalize(i.n);

                //-------------镜面高光
                fixed angle = saturate(dot(i.n,i.lightDir));
                fixed3 outLightDir = 2*angle * i.n - i.lightDir;//反射光方向  推导原理

                fixed3 viewDir = normalize( WorldSpaceViewDir(i.ver)); //视角方向
                fixed viewAngle = dot(viewDir,outLightDir); //视角与反射光的夹角

                



				fixed3 disToYellow = col.rgb - float3(0.8,0.6,0.34);
				fixed dis = abs(disToYellow.r)+ abs(disToYellow.g) + abs(disToYellow.b); 

				_Pad *= dis*0.1 ;
				_Pad += 3;
				
				
				//viewAngle += (dis) * 0.02 - 0.15;
				viewAngle = saturate(viewAngle) ;

                fixed strength = pow(viewAngle,_Pad);

				fixed3 highLightColor = UNITY_LIGHTMODEL_AMBIENT ;

                col.rgb += highLightColor * 10 * strength;
				
                //-------------镜面高光结束

				

				col.rgb  *= tex.rgb;
				col.rgb  *= 1;
				col.rgb *= UNITY_LIGHTMODEL_AMBIENT * 7;

				col.rgb *=  (1 + 2 * saturate(1 - (_Time.y - _NowTime )*2));
				//col.a = tex.a;

                return col;
            }

            ENDCG


	}
   }


   FallBack "Diffuse"
}
