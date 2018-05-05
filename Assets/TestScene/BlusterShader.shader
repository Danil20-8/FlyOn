Shader "Unlit/BlusterShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{

		Tags{ "PreviewType" = "Plane" "Queue" = "Transparent" }
		LOD 100

		Pass
		{
			Cull Off
			Blend SrcAlpha OneMinusSrcAlpha
			SetTexture[_MainTex]{combine texture, texture }
		}
	}
}
