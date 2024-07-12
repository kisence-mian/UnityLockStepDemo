Shader "T4MShaders/ShaderModel1/T4M 2 Textures Auto BeastLM 2DrawCall" {
Properties {
	_Splat0 ("Layer1 (RGB)", 2D) = "white" {}
	_Splat1 ("Layer2 (RGB)", 2D) = "white" {}
	_Control ("Mask (RGB)", 2D) = "white" {}
	_MainTex ("Never Used", 2D) = "white" {}
}
	SubShader {
	Tags { "RenderType"="Opaque" }
	LOD 100
	
	Pass {
		Tags { "LightMode" = "Vertex" }
			Material {
				Diffuse (1,1,1,1)
				Ambient (1,1,1,1)
			} 
			Lighting On
			
			SetTexture [_Splat0]
			
			SetTexture [_Control]
			{
				combine previous, texture
			}
					
			SetTexture [_Splat1]
			{
				combine texture lerp(previous) previous Double
				
			}
			SetTexture [_Splat0]{
				combine previous * primary 	
			} 
	}	
	
	Pass {
		Tags { "LightMode" = "VertexLM" }
		
		Lighting Off
		BindChannels {
			Bind "Vertex", vertex
			Bind "normal", normal
			Bind "texcoord1", texcoord0 // lightmap uses 2nd uv
		}
		
		SetTexture [unity_Lightmap] {
			matrix [unity_LightmapMatrix]
			combine texture 
		}
	}
	Pass {
		
		Tags { "LightMode" = "VertexLM" }
		 Blend DstColor SrcColor
		
		Lighting Off
		
		SetTexture [_Splat0]
		
		SetTexture [_Control]
		{
			combine previous, texture
		}
				
		SetTexture [_Splat1]
		{
			combine texture lerp(previous) previous
			
		}
	 
	}
		Pass {
			Tags { "LightMode" = "VertexLMRGBM" }
			
			Lighting Off
			BindChannels {
				Bind "Vertex", vertex
				Bind "normal", normal
				Bind "texcoord1", texcoord0 // lightmap uses 2nd uv
				Bind "texcoord1", texcoord1 // unused
				Bind "texcoord", texcoord2 // main uses 1st uv
			}
			
			SetTexture [unity_Lightmap] {
				matrix [unity_LightmapMatrix]
				combine  texture
			}
		}
			Pass {
		
		Tags { "LightMode" = "VertexLMRGBM" }
		 Blend DstColor SrcColor
		
		Lighting Off
		
		SetTexture [_Splat0]
		
		SetTexture [_Control]
		{
			combine previous, texture
		}
				
		SetTexture [_Splat1]
		{
			combine texture lerp(previous) previous
			
		}
	 
	}
	}
}
