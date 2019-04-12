Shader "Unlit/MySketchShader"
{
    Properties
    {
		_Outline("Outline", Range(0, 1)) = 0.1
		_Color("Color Tint", Color) = (1, 1, 1, 1)
		_TileFactor("Tile Factor", Range(0, 10)) = 1
		_Hatch0("Hatch 0", 2D) = "white" {}
		_Hatch1("Hatch 1", 2D) = "white" {}
		_Hatch2("Hatch 2", 2D) = "white" {}
		_Hatch3("Hatch 3", 2D) = "white" {}
		_Hatch4("Hatch 4", 2D) = "white" {}
		_Hatch5("Hatch 5", 2D) = "white" {}
	}
	SubShader
	{
		Pass
        {
            CGPROGRAM
            #pragma vertex MyVertexProgram
            #pragma fragment MyFragmentProgram

			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "AutoLight.cginc"

			fixed4 _Color;
			float _TileFactor;
			sampler2D _Hatch0;
			sampler2D _Hatch1;
			sampler2D _Hatch2;
			sampler2D _Hatch3;
			sampler2D _Hatch4;
			sampler2D _Hatch5;

			struct VertexData
			{
				float4 position : POSITION;
				float2 uv : TEXCOORD0;
				float3 normal : NORMAL;
			};

			struct FragmentData {
				float4 position : SV_POSITION;
				float2 uv : TEXCOORD0;
				fixed3 hatchWeights0 : TEXCOORD1;
				fixed3 hatchWeights1 : TEXCOORD2;
				float3 worldPos : TEXCOORD3;
			};

			FragmentData MyVertexProgram(VertexData v)
			{
				FragmentData o;
				o.position = UnityObjectToClipPos(v.position);
				o.uv = v.uv.xy * _TileFactor;
				fixed3 worldLightDir = normalize(WorldSpaceLightDir(v.position));
				fixed3 worldNormal = UnityObjectToWorldNormal(v.normal);
				fixed diffuse = max(0, dot(worldLightDir, worldNormal));
				o.hatchWeights0 = fixed3(0, 0, 0);
				o.hatchWeights1 = fixed3(0, 0, 0);
				float hatchFactor = diffuse * 7.0;

				if (hatchFactor > 6.0) 
				{
				}
				else if (hatchFactor > 5.0) 
				{
					o.hatchWeights0.x = hatchFactor - 5.0;
				}
				else if (hatchFactor > 4.0)
				{
					o.hatchWeights0.x = hatchFactor - 4.0;
					o.hatchWeights0.y = 1.0 - o.hatchWeights0.x;
				}
				else if (hatchFactor > 3.0) 
				{
					o.hatchWeights0.y = hatchFactor - 3.0;
					o.hatchWeights0.z = 1.0 - o.hatchWeights0.y;
				}
				else if (hatchFactor > 2.0) 
				{
					o.hatchWeights0.z = hatchFactor - 2.0;
					o.hatchWeights1.x = 1.0 - o.hatchWeights0.z;
				}
				else if (hatchFactor > 1.0) 
				{
					o.hatchWeights1.x = hatchFactor - 1.0;
					o.hatchWeights1.y = 1.0 - o.hatchWeights1.x;
				}
				else 
				{
					o.hatchWeights1.y = hatchFactor;
					o.hatchWeights1.z = 1.0 - o.hatchWeights1.y;
				}

				o.worldPos = mul(unity_ObjectToWorld, v.position).xyz;
				return o;
			}

			fixed4 MyFragmentProgram(FragmentData i) : SV_Target
			{
				fixed4 hatchTex0 = tex2D(_Hatch0, i.uv) * i.hatchWeights0.x;
				fixed4 hatchTex1 = tex2D(_Hatch1, i.uv) * i.hatchWeights0.y;
				fixed4 hatchTex2 = tex2D(_Hatch2, i.uv) * i.hatchWeights0.z;
				fixed4 hatchTex3 = tex2D(_Hatch3, i.uv) * i.hatchWeights1.x;
				fixed4 hatchTex4 = tex2D(_Hatch4, i.uv) * i.hatchWeights1.y;
				fixed4 hatchTex5 = tex2D(_Hatch5, i.uv) * i.hatchWeights1.z;
				fixed4 whiteColor = fixed4(1, 1, 1, 1) * (1 - i.hatchWeights0.x - i.hatchWeights0.y - i.hatchWeights0.z - i.hatchWeights1.x - i.hatchWeights1.y - i.hatchWeights1.z);

				fixed4 hatchColor = hatchTex0 + hatchTex1 + hatchTex2 + hatchTex3 + hatchTex4 + hatchTex5 + whiteColor;

				return fixed4(hatchColor.rgb * _Color.rgb, 1.0);
			}
            
            ENDCG
        }
		Pass
		{
			Cull Front
			CGPROGRAM
			#pragma vertex MyVertexProgram
			#pragma fragment MyFragmentProgram

			#include "UnityCG.cginc"

			float _Outline;

			struct VertexData
			{
				float4 position : POSITION;
				float3 normal : NORMAL;
			};

			struct FragmentData
			{
				float4 position : SV_POSITION;
			};

			FragmentData MyVertexProgram(VertexData v)
			{
				FragmentData o;
				o.position = UnityObjectToClipPos(v.position);
				float3 vnormal = mul((float3x3)UNITY_MATRIX_IT_MV, v.normal);
				float2 offset = TransformViewToProjection(vnormal.xy);
				o.position.xy += offset * _Outline;
				return o;
			}

			fixed4 MyFragmentProgram(FragmentData i) : SV_Target
			{
				return float4(0, 0, 0, 1);
			}

			ENDCG
		}
    }
}
