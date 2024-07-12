Shader "T4MShaders/ShaderModel1/T4M 2 Textures ManualAdd BeastLM_1DC" {
Properties {
	_Splat0 ("Layer1 (RGB)", 2D) = "white" {}
	_Splat1 ("Layer2 (RGB)", 2D) = "white" {}
	_Control ("Mask (RGB)", 2D) = "white" {}
	_Lightmap ("LightMap", 2D) = "white" {}
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
			Bind "texcoord", texcoord1 // main uses 1st uv
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
		SetTexture [_Lightmap] {
				matrix [unity_LightmapMatrix]
				combine  previous * texture DOUBLE
		}
	 
	}
	
		Pass {
		
		Tags { "LightMode" = "VertexLMRGBM" }
		Lighting Off	 
		BindChannels {
			Bind "Vertex", vertex
			Bind "normal", normal
			Bind "texcoord1", texcoord0 // lightmap uses 2nd uv
			Bind "texcoord", texcoord1 // main uses 1st uv
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
		SetTexture [_Lightmap] {
				matrix [unity_LightmapMatrix]
				combine  previous * texture DOUBLE
		}
	 
	}
	}
}
