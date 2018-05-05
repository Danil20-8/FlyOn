Shader "Custom/BuildShader" {

	CGINCLUDE
		sampler2D _MainTex;
		sampler2D _ProgressMap;
	struct Input {
		float2 uv_MainTex;
		float2 uv_ProgressMap;
	};

	half _Glossiness;
	half _Metallic;
	fixed4 _Color;

	float _StartTime;
	float _Duration;

	void surf(Input IN, inout SurfaceOutputStandard o) {

		fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
		o.Albedo = c.rgb;

		o.Metallic = _Metallic;
		o.Smoothness = _Glossiness;

		o.Alpha = c.a;
		fixed b = tex2D(_ProgressMap, IN.uv_ProgressMap).a;
		clip((_Time - _StartTime) / _Duration - b);
	}
	ENDCG

	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		_ProgressMap("Progress Map", 2D) = "white" {}
		_StartTime ("Start Time", Float) = 0
		_Duration ("Duration", Float) = 0
	}
	SubShader {
		Tags { "RenderType"="Transparent" "Queue"="Transparent" }
		LOD 200
		Cull Front

		CGPROGRAM

		#pragma surface surf Standard fullforwardshadows alpha:blend
		#pragma target 3.0

		ENDCG

		Cull Back
		Pass
		{
			Blend SrcAlpha OneMinusSrcAlpha
			Color[_Color]
		}

		CGPROGRAM

		#pragma surface surf Standard fullforwardshadows alpha:blend
		#pragma target 3.0
		
		ENDCG
	}
	FallBack "Diffuse"
}
