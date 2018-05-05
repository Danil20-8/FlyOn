Shader "Unlit/UnlitTranparentColor"
{
	Properties
	{
		_Color ("Color", Color) = (1, 1, 1, .5)
	}
	SubShader
	{
		Tags { "Queue" = "Transparent" }
		LOD 100

		Blend SrcAlpha OneMinusSrcAlpha
		Pass
		{
			Color [_Color]
		}
	}
}
