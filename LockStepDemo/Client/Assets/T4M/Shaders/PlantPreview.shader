Shader "Hidden/PlantPreview" {
	Properties {
		_TintColor ("Tint Color", Color) = (0.5,0.5,0.5,0.1)
		_MainTex ("Texture", 2D) = "white" {}
	}
	Category {
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
		Blend SrcAlpha OneMinusSrcAlpha
		Cull Off Lighting Off ZWrite Off Fog { Color (0,0,0,0) }
		BindChannels {
			Bind "Color", color
			Bind "Vertex", vertex
			Bind "TexCoord", texcoord
		}
		SubShader {
			Pass {
				SetTexture [_MainTex] {
					constantColor [_TintColor]
					combine constant * primary
				}
				SetTexture [_MainTex] {
					combine texture * previous DOUBLE
				}
			}
		}
	}
}