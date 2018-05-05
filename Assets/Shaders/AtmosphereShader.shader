// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'


Shader "Custom/AtmosphereShader"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_SunPosition("Sun Position", Vector) = (0, 0, 0)
		_SunColor("Sun Color", Color) = (1, 1, 1)
		_SunDot("Sun Dot", Float) = 0
	}
		SubShader
		{
			Tags { "Queue" = "Transparent" }
			LOD 100

			Pass
		{
			Cull Front
			Blend One One
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

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

			v2f vert(float4 vertex : POSITION, float2 uv : TEXCOORD, half3 normal : NORMAL)
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
				o.color.a = d;

				return o;
			}

			float4 frag(v2f i) : SV_TARGET
			{
				float4 c = tex2D(_MainTex, i.uv);
				c.rgb *= i.color.rgb;
				c.a += i.color.a;
				return c;
			}

			ENDCG
		}
				/*Pass
				{
				Cull Front
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#include "AtmosphereShader.cginc"


				ENDCG
				}*/
				Pass
				{
					Cull Back
					Blend SrcAlpha OneMinusSrcAlpha
					CGPROGRAM
					#pragma vertex vert
					#pragma fragment frag
					#include "AtmosphereShader.cginc"
					ENDCG
				}
		}
}
