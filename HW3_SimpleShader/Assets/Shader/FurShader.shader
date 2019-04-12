Shader "Unlit/FurShader"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_FurFactor("FurFactor", Range(0.01, 0.05)) = 0.01
	}
		SubShader
		{
			Tags { "RenderType" = "Opaque" }
			LOD 100

			Pass
			{
				CGPROGRAM
				#pragma vertex MyVertexProgram
				#pragma fragment MyFragmentProgram
				#pragma geometry MyGeometryProgram

				#include "UnityCG.cginc"

				struct VertexData
				{
					float4 position : POSITION;
					float2 uv : TEXCOORD0;
					float3 normal : NORMAL;
				};

				struct GeometryData
				{
					float4 position : POSITION;
					float3 normal : NORMAL;
					float2 uv : TEXCOORD0;
				};

				struct FragmentData
				{
					float2 uv : TEXCOORD0;
					float4 position : SV_POSITION;
					fixed4 col : COLOR;
				};

				sampler2D _MainTex;
				float4 _MainTex_ST;

				float _FurFactor;

				GeometryData MyVertexProgram(VertexData v)
				{
					GeometryData i;
					i.position = v.position;
					i.uv = TRANSFORM_TEX(v.uv, _MainTex);
					i.normal = v.normal;
					return i;
				}

				[maxvertexcount(9)]
				void MyGeometryProgram(triangle GeometryData g[3], inout TriangleStream<FragmentData> tristream)
				{
					FragmentData o;

					float3 edgeA = g[1].position - g[0].position;
					float3 edgeB = g[2].position - g[0].position;
					float3 normalFace = normalize(cross(edgeA, edgeB));

					float3 centerPos = (g[0].position + g[1].position + g[2].position) / 3;
					float2 centerTex = (g[0].uv + g[1].uv + g[2].uv) / 3;
					centerPos += float4(normalFace, 0) * _FurFactor;

					for (uint i = 0; i < 3; i++)
					{
						o.position = UnityObjectToClipPos(g[i].position);
						o.uv = g[i].uv;
						o.col = fixed4(0, 0, 0, 1);

						tristream.Append(o);

						uint index = (i + 1) % 3;
						o.position = UnityObjectToClipPos(g[index].position);
						o.uv = g[index].uv;
						o.col = fixed4(0, 0, 0, 1);

						tristream.Append(o);

						o.position = UnityObjectToClipPos(float4(centerPos, 1));
						o.uv = centerTex;
						o.col = fixed4(1.0, 1.0, 1.0, 1);

						tristream.Append(o);

						tristream.RestartStrip();
					}
				}


				fixed4 MyFragmentProgram(FragmentData i) : SV_Target
				{
					fixed4 col = tex2D(_MainTex, i.uv) * i.col;
					return col;
				}
				ENDCG
			}
		}
}
