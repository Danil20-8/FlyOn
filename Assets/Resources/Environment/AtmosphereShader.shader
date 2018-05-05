Shader "Unlit/AtmosphereShader"
{
	Properties{
		_Color("Main Color", Color) = (1,1,1,0)
		_SpecColor("Spec Color", Color) = (1,1,1,1)
		_Emission("Emmisive Color", Color) = (0,0,0,0)
		_Shininess("Shininess", Range(0.01, 1)) = 0.01
		_MainTex("Base (RGB)", 2D) = "white" {}
	}
		SubShader{
			Tags {"Queue" = "Transparent"}
			Blend SrcAlpha OneMinusSrcAlpha
			Material{
				Diffuse[_Color]
				Ambient[_Color]
				Shininess[_Shininess]
				Specular[_SpecColor]
				Emission[_Emission]
				}
		Lighting On
		SeparateSpecular On

		Pass{
			Cull Front
			SetTexture[_MainTex]{
				Combine Primary * texture
			}
		}

		Pass{
			Cull Back
			SetTexture[_MainTex]{
				Combine Primary * texture
			}
		}
	}
}
