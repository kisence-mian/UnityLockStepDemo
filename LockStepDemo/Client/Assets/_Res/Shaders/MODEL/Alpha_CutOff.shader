Shader "MODEL/Cull/Alpha Cull Back" {
Properties {//显示在面板上的可调参数
	  _Color ("Main Color", Color) = (1,1,1,1)
      _MainTex ("Base (RGB)", 2D) = "white"{}
	  _CutOff("Alpha Cutoff",Range(0,1)) = .5
   }
   SubShader {
      Pass {

			Cull Back // Back | Front | Off
			//ZWrite On //On | Off
			Blend SrcAlpha OneMinusSrcAlpha
			AlphaTest Greater [_CutOff]

			//ZTest LEqual //Less | Greater | LEqual | GEqual | Equal | NotEqual | Always

			//SeparateSpecular On
			//Blend SrcAlpha OneMinusSrcAlpha
			//Blend One One

			//ColorMask 0
			//ZTest NotEqual
			
			//Lighting On
			SetTexture[_MainTex]{ 
                constantColor [_Color]
                combine texture * constant
			}
			
			//{
			//	Combine Primary * Texture
			//}

			//UsePass "Transparent/Diffuse/FORWARD"
      }



	  
   } 
}
