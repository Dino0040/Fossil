Shader "Fossil/Glitch" {
	Properties{
		_MainTex("Base", 2D) = "" {}
		_glitchImage("Base (RGB)", 2D) = "" {}
		_glitchImage2("Base (RGB)", 2D) = "" {}
	}

		SubShader{
			Pass {
				ZTest Always Cull Off ZWrite Off

		CGPROGRAM
		#pragma vertex vert_img
		#pragma fragment frag
		#include "UnityCG.cginc"

		uniform sampler2D _MainTex;
		uniform sampler2D _glitchImage;
		uniform sampler2D _glitchImage2;
		uniform half _glitchStrength;
		uniform float4 _OffsetScale;

		fixed4 frag(v2f_img i) : SV_Target
		{
			fixed4 original = tex2D(_MainTex, i.uv);
			float2 glitchOffset = float2(_OffsetScale.x, _OffsetScale.y);
			fixed4 glitchPix = tex2D(_glitchImage, i.uv + glitchOffset);

			float2 glitchOffset2 = float2(_OffsetScale.z, _OffsetScale.w);
			fixed4 glitchPix2 = tex2D(_glitchImage2, i.uv + glitchOffset2);

			float2 imageOffset = float2((glitchPix.r / 255) * _glitchStrength,
										(glitchPix.g / 255) * _glitchStrength);

			float2 imageOffset2 = float2((glitchPix2.r / 255) * _glitchStrength,
										(glitchPix2.g / 255) * _glitchStrength);

			fixed4 output = tex2D(_MainTex, i.uv + imageOffset * 2 + imageOffset2 * 0.5f);
			output.r = tex2D(_MainTex, i.uv + imageOffset * 1.5f + imageOffset2 * 0.7f).r;
			return output;
		}
		ENDCG

			}
	}

		Fallback off

}
