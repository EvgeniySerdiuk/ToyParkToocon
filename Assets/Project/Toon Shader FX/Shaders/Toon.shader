Shader "Bytesized/Toon"
{
	Properties
	{
	    /* Color used for the material. Leave white if none */
		_Color("Color", Color) = (1,1,1,1)
		/* Texture used for the material */
		_MainTex("Main Texture", 2D) = "white" {}
		/* Ambient color to add to the light calculation */
		[HDR] _AmbientColor("Ambient Color", Color) = (0.4,0.4,0.4,1)
		/* Color to apply on the specular lighting stage */
		[HDR] _SpecularColor("Specular Color", Color) = (0.9,0.9,0.9,1)
		/* The size of the specular reflection */
		_Glossiness("Glossiness", Float) = 32
		/* The color used in the rim lighting stage */
		[HDR] _RimColor("Rim Color", Color) = (1,1,1,1)
		/* How much should the material be affected by rim lighting */
		_RimBlend("Rim Blend", Range(0, 1)) = 0.5
		/* Controls how smoothly the rim blends with other unlit parts */
		_RimThreshold("Rim Threshold", Range(0, 1)) = 0.1
		/* Controls the color transition between shadowed surfaces and non shadowed surfaces */
		_Smoothness("Smoothness", Range(0, 0.5)) = 0.025

		_OutlineColor("Outline Color", Color) = (0, 0, 0, 1)
		_OutlineSize("Outline Size", float) = 10

	}
	SubShader
	{
			Tags{ "Queue" = "Transparent" }

			// This Pass Renders the outlines
			Cull front
			ZWrite Off

			Pass
			{
				Blend SrcAlpha OneMinusSrcAlpha
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag

				#include "UnityCG.cginc"

				struct appdata
				{
					float4 vertex : POSITION;
					float3 normal : NORMAL;
				};

				struct v2f
				{
					float4 vertex : SV_POSITION;
				};

				float _OutlineSize;
				v2f vert(appdata v)
				{
					v2f o;
					float4 worldPos = mul(unity_ObjectToWorld, v.vertex);
					half3 worldNormal = UnityObjectToWorldNormal(v.normal);
					worldPos.xyz = worldPos.xyz + worldNormal * _OutlineSize * 0.001;
					o.vertex = mul(UNITY_MATRIX_VP, worldPos);
					return o;
				}

				float4 _OutlineColor;
				fixed4 frag(v2f i) : SV_Target
				{
					return _OutlineColor;
				}

				ENDCG
			}// End Outline Pass


		Pass
		{
			Cull back
		    ZWrite On
			Tags
			{
				"LightMode" = "ForwardBase"
			}

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fwdbase
			
			/* We include the lighting code for the base pass */
			#include "ToonPass.cginc"
			
			ENDCG
		}
		
		Pass {
			Tags
			{
				"LightMode" = "ForwardAdd"
			}

            Blend One One
            ZWrite Off

			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fwdadd_fullshadows

            /* We include the lighting code for the add pass */
			#include "ToonPass.cginc"

			ENDCG
		}

		/* Support for casting shadows */
        UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"
	}
}