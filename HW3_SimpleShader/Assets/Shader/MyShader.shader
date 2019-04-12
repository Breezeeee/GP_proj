Shader "Unlit/MyShader"
{
	Properties
	{
		_MainTex("Main Texture", 2D) = "white" {}
		_Diffuse("Diffuse", Color) = (1,1,1,1)
		_Specular("Specular", Color) = (1,1,1,1)
		_Shininess("Shininess", Range(1.0, 256)) = 10
	}
	SubShader
	{
		Pass
		{
			CGPROGRAM
			#pragma shader_feature USE_NORMAL
			#pragma shader_feature USE_TEX
			#pragma shader_feature USE_SPECULAR
			#pragma vertex MyVertexProgram
			#pragma fragment MyFragmentProgram
			sampler2D _MainTex;
			float4 _MainTex_ST;
			fixed4 _Diffuse;
			fixed4 _Specular;
			float _Shininess;
			struct VertexData {
				float4 position : POSITION;
				float3 normal : NORMAL;
				float2 uv : TEXCOORD0;
			};
							
			struct FragmentData {
				float4 position : SV_POSITION;
				float2 uv : TEXCOORD0;
				float3 normal : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
			};

			#include "UnityCG.cginc"
			#include "UnityStandardBRDF.cginc"
			FragmentData MyVertexProgram() {}
			float4 MyFragmentProgram() {}

			FragmentData MyVertexProgram(VertexData v) {
				FragmentData i;
				i.position = UnityObjectToClipPos(v.position);
				i.normal = mul(transpose((float3x3)unity_WorldToObject), v.normal);
				i.normal = normalize(i.normal);
				i.uv = TRANSFORM_TEX(v.uv, _MainTex);
				i.worldPos = mul(unity_ObjectToWorld, v.position);
				return i;
			}

			float4 MyFragmentProgram(FragmentData i) : SV_TARGET{

			#if USE_NORMAL
				return float4(i.normal, 1);

			#elif USE_TEX
				return tex2D(_MainTex, i.uv);

			#else
				//float3 lightDir = _WorldSpaceLightPos0.xyz;
				float3 lightDir = normalize(_WorldSpaceLightPos0.xyz);
				float3 lightColor = _LightColor0.rgb;
				float3 diffuse = _Diffuse.rgb * tex2D(_MainTex, i.uv).rgb * lightColor * DotClamped(lightDir, i.normal);

				float3 viewDir = normalize(_WorldSpaceCameraPos - i.worldPos);
				float3 halfVector = normalize(lightDir + viewDir);

				fixed3 specular = _Specular.rgb * lightColor * pow(DotClamped(halfVector, i.normal), _Shininess);

				fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz * tex2D(_MainTex, i.uv).rgb;

				//return float4(specular, 1);
				return float4(ambient + diffuse + specular, 1);
			#endif

			}
			ENDCG
		}
	}
	CustomEditor "CustomShaderGUI"
}
