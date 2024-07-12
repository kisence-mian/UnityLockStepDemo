Shader "T4MShaders/ShaderModel1/T4M 2 Textures ManualAdd CustoLM 1DC" {
Properties {
	_Splat0 ("Layer1 (RGB)", 2D) = "white" {}
	_Splat1 ("Layer2 (RGB)", 2D) = "white" {}
	_Control ("Mask (RGB)", 2D) = "white" {}
	_LightMap ("Lightmap (RGB)", 2D) = "lightmap" { LightmapMode }
	_MainTex ("Never Used", 2D) = "white" {}
}
	SubShader {
	Tags { "RenderType"="Opaque" }
	LOD 100
	Lighting Off
	
	Pass {
		BindChannels {
			Bind "Vertex", vertex
			Bind "normal", normal
			Bind "texcoord1", texcoord0 // lightmap uses 2nd uv
			Bind "texcoord1", texcoord1 // lightmap uses 2nd uv
			Bind "texcoord", texcoord2 // main uses 1st uv
		}
			SetTexture [_Splat0]
			
			SetTexture [_Control]
			{
				combine previous, texture
			}
			SetTexture [_Splat1]
			{
				combine texture lerp(previous) previous 
			}
			SetTexture [_LightMap] {
				combine  previous * texture DOUBLE
			}
		}	
	}
}
