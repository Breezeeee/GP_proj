Shader "Unlit/MotionBlur"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_BlurAmount("Blur Amount", Float) = 1.0
	}
	SubShader
	{
		CGINCLUDE

		#include "UnityCG.cginc"
		sampler2D _MainTex;
		fixed _BlurAmount;

		struct FragmentData
		{
			float4 position : SV_POSITION;
			half2 uv : TEXCOORD0;
		};

		FragmentData vert(appdata_img v)
		{
			FragmentData o;
			o.position = UnityObjectToClipPos(v.vertex);
			o.uv = v.texcoord;
			return o;
		}

		fixed4 fragRGB(FragmentData i) : SV_Target
		{
			return fixed4(tex2D(_MainTex, i.uv).rgb, _BlurAmount);
		}

		half4 fragA(FragmentData i) : SV_Target
		{
			return tex2D(_MainTex, i.uv);
		}

		ENDCG

		ZTest Always Cull Off ZWrite Off

		Pass
		{
			Blend SrcAlpha OneMinusSrcAlpha
			ColorMask RGB
			CGPROGRAM
			#pragma vertex vert  
			#pragma fragment fragRGB  
			ENDCG
		}

		Pass 
		{
			Blend One Zero
			ColorMask A
			CGPROGRAM
			#pragma vertex vert  
			#pragma fragment fragA
			ENDCG
		}
	}
	FallBack Off
}
