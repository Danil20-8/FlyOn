// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

#pragma once

#include "UnityCG.cginc"

struct v2f
{
	float2 uv : TEXCOORD0;
	float4 vertex : SV_POSITION;
	float4 color : COLOR0;
};

sampler2D _MainTex;
float3 _SunPosition;
float4 _SunColor;
float _SunDot;

v2f vert(float4 vertex : POSITION, float2 uv : TEXCOORD, float3 normal : NORMAL)
{
	v2f o;
	o.vertex = UnityObjectToClipPos(vertex);
	o.uv = uv;

	float3 v = _SunPosition;
	v -= mul(unity_ObjectToWorld, vertex);
	float d = dot(normalize(v), normalize(mul(unity_ObjectToWorld, normal)));
	d *= 1 - _SunDot;
	d += _SunDot;
	o.color.rgb = _SunColor.rgb * d;
	o.color.a = 0;
	return o;
}

float4 frag(v2f i) : SV_Target
{
	float4 c = tex2D(_MainTex, i.uv);
	c.rgb *= i.color.rgb;
	return c;
}
