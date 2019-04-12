Shader "Unlit/WaveShader"
{
	Properties
	{
		_MainTex("Wave Texture", 2D) = "white" {}
		_NoiseTex("Wave Noise", 2D) = "white" {}
		_Amplitude("Amplitude", float) = 0.1
		_Indentity("Indentity", float) = 0.1
		_SpeedX("WaveSpeedX", float) = 0.08
		_SpeedY("WaveSpeedY", float) = 0.04
	}
		SubShader
		{
			Pass
			{
				CGPROGRAM
				#pragma vertex MyVertexProgram
				#pragma fragment MyFragmentProgram

				#include "unitycg.cginc"

				sampler2D _MainTex;
				float4 _MainTex_ST;
				sampler2D _NoiseTex;
				float _Amplitude;
				float _Indentity;
				float _SpeedX;
				float _SpeedY;

				struct VertexData {
					float4 position : POSITION;
					float2 uv : TEXCOORD0;
				};

				struct FragmentData {
					float4 position : SV_POSITION;
					float2 uv : TEXCOORD0;
				};

				FragmentData MyVertexProgram(VertexData v)
				{
					v.position.y += _Amplitude *sin(v.position.z + v.position.x + _Time.y);
					v.position.y += _Amplitude *sin((v.position.z - v.position.x) + _Time.w);
					FragmentData i;
					i.position = UnityObjectToClipPos(v.position);
					i.uv = TRANSFORM_TEX(v.uv, _MainTex);
					return i;
				}
				float4 MyFragmentProgram(FragmentData i) :COLOR
				{
					float2 waveOffset = (tex2D(_NoiseTex, i.uv.xy + float2(0, _Time.y * _SpeedY)).rg + tex2D(_NoiseTex, i.uv.xy + float2(_Time.w * _SpeedX, 0)).rg) - 1;
					float2 ruv = float2(i.uv.x, 1 - i.uv.y) + waveOffset * _Indentity;
					float4 res = tex2D(_MainTex, ruv);
					return res;
				}
				ENDCG
			}
		}
}
