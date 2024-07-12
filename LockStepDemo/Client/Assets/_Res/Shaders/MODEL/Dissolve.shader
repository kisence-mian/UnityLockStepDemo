// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/Dissolve"{
Properties {//显示在面板上的可调参数
	  _Color ("Main Color", Color) = (1,1,1,1)
      _MainTex ("Base (RGB)", 2D) = "white"{}
	  _CutOff("Alpha Cutoff",Range(0,1)) = 0.44
	  _Pad("Pad",range(0,30)) = 30


	  //------消融使用参数
	  _DissolveTex ("DissolveTex (RGB)", 2D) = "white" {} //噪波图
	 // _Amount ("DissAmount", Range (-1, 1)) = 0          //溶解值，低于这个值，像素将被抛弃
	  _DissSize("DissSize", Range (0, 1)) = 0.1       //预溶解范围大小  
	  _DissColor ("DissColor", Color) = (0.47,0.21,0.07,1)    //预溶解范围渐变颜色，与_AddColor配合形成渐变色  
      _AddColor ("AddColor", Color) = (1,0.73,0.26,1)
   }

   SubShader {
		Pass {
			Tags{"queue" = "2050"}
			Blend SrcAlpha OneMinusSrcAlpha
			AlphaTest Greater [_CutOff]
			Cull Off

			CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Lighting.cginc"
            #include "UnityCG.cginc"


			struct appdata
			{
				float4 vertex : POSITION;
				float4 uv : TEXCOORD0;
				float4 uv2 : TEXCOORD1;
				float3 normal : NORMAL; 
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float _Pad;
			float4 _Color;


			//-------消融效果需要
			sampler2D _DissolveTex;
			float4 _DissolveTex_ST;
			float _Amount;
			float _DissSize;
			float4 _DissColor;
			float4 _AddColor;
			float _NowTime;

            struct v2f
            {
                float4 pos:POSITION;
				float4 col:COLOR0;
				float4 lightDir :COLOR1;
                float4 ver:COLOR2;
                float3 n:COLOR3;

				float2 uv : TEXCOORD0;

				////消融效果
				float2 uv2: TEXCOORD1;

            };


            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
				o.col = fixed4(0,0,0,1);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);

				//消融效果
				o.uv2 = TRANSFORM_TEX(float2(v.vertex.x,v.vertex.z),_DissolveTex);
				
				//_Amount += unity_DeltaTime.x * 10;

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
                float4 col = i.col;

                i.n = (mul(float4(i.n,0),unity_WorldToObject));
                i.n = normalize(i.n);

                //-------------镜面高光
                fixed angle = saturate(dot(i.n,i.lightDir));
                fixed3 outLightDir = 2*angle * i.n - i.lightDir;//反射光方向  推导原理

                fixed3 viewDir = normalize( WorldSpaceViewDir(i.ver)); //视角方向
                fixed viewAngle = dot(viewDir,outLightDir); //视角与反射光的夹角


				//------a-begion-----根据颜色到肉色的差距，调整高光强度和衰减
				fixed3 disToYellow = col.rgb - float3(0.8,0.6,0.34);
				fixed dis = abs(disToYellow.r)+ abs(disToYellow.g) + abs(disToYellow.b); 

				_Pad *= dis*0.1 ;
				_Pad += 3;
				
				viewAngle = saturate(viewAngle) ;
				//------a-end-------
				

                fixed strength = pow(viewAngle,_Pad);


				fixed3 highLightColor = UNITY_LIGHTMODEL_AMBIENT ;

                col.rgb += highLightColor * 4  * strength;
				

                //-------------溶解效果开始
				float clipNum = tex2D(_DissolveTex,i.uv2).r;
				_Amount = _Time.y - _NowTime;
				_Amount *= 0.8; // 消融速度调整
				float clipAmount = clipNum - _Amount;

				clip(clipAmount);

				clipAmount = clipAmount + 0.8;  //在0.2内消融
				clipAmount = saturate(clipAmount);

				float isNormal = saturate(1-clipAmount);

				float4 edgeColor = lerp(_DissColor,_AddColor,clipAmount * 5)*2 ; //对应于上方的0.2，进行融合的缩放
				
				//---------------溶解效果结束

				float4 tex = tex2D(_MainTex, i.uv);
				col.rgb  *= tex.rgb;
				col.rgb *= UNITY_LIGHTMODEL_AMBIENT * 7;
				col = (lerp(col,edgeColor,isNormal)); 

				//col.rgb *=  (1 +  2*saturate(1 - (_Time.y - _NowTime )*2));
				
                return col;
            }

            ENDCG


	}
   }


   //FallBack "Diffuse"
}