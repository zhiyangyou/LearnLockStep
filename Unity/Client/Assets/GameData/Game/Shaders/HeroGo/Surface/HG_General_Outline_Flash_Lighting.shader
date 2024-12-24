﻿// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "HeroGo/Surface/General_Outline_Flash"
{
	Properties
	{
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_BumpMap("Bump (RGB)", 2D) = "white" {}
		_SpecMap("Specular (RGB)", 2D) = "gray" {}

		_FresnelPow("Fresnel Pow",Range(1, 6)) = 1
		_FresnelOffset("Fresnel Offset",Range(-1, 1)) = 0
		_SpecularPow("Specular Pow",Range(1, 6)) = 1
		_ColorStrength("Color Strength",Range(0, 1)) = 0.3
		_LightIntensity("Light Intensity",Range(0, 2)) = 1
		_AmbientColor("Ambient Color",Color) = (0.35,0.35,0.35,0.35)
		_FresnelColor("Fresnel Color",Color) = (1,1,1,1)

		//OUTLINE
		_FlashFreq("Flash frequncy", Range(0, 10)) = 3
		_Outline("Outline Width", Range(0.0, 0.1)) = 0.001
		_OutlineColorBegin("Begin Outline Color", Color) = (1.0, 0.0, 0.0, 1)
		_OutlineColorEnd("End Outline Color", Color) = (1.0, 1.0, 0.0, 1)

		[HideInInspector]_ElapsedTime("Elapsed time", Float) = 0
		[HideInInspector]_WorldRefPos("World ref position",Vector) = (0,0,0,0)
	}

	SubShader
	{
		Tags
		{
			"LightMode" = "ForwardBase"
			"RenderType" = "Opaque"
		}
		LOD 100

		Pass
		{
			Name "GENERAL_OUTLINE_FLASH"

			ZWrite On
			ZTest On
			Stencil
			{
				Ref 1
				Comp Always
				Pass Replace
				ZFail Keep
			}

			Cull Back
			Lighting Off
			ZWrite On

			Fog{ Mode Off }

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"
			#include "AutoLight.cginc"
			#include "Lighting.cginc"
			#include "../Inc/Base.cginc"
			#include "../Inc/Toon.cginc"
			#include "../../Includes/TMShaderSupport.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float4 tangent : TANGENT;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv_MainTex		: TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float3 WorldNormal		: TEXCOORD2;
				float3 WorldTangent		: TEXCOORD3;
				float3 WorldBinormal	: TEXCOORD4;
				float3 WorldPosition	: TEXCOORD5;
				float3 WorldViewDir		: TEXCOORD6;
				float  Factor : TEXCOORD7;

				float4 HPosition		: SV_POSITION;
			};

			sampler2D _MainTex;
			sampler2D _BumpMap;
			sampler2D _SpecMap;
			float4 _MainTex_ST;
			
			real _LightIntensity;
			real _FresnelPow;
			real _SpecularPow;
			real _FresnelOffset;
			real _ColorStrength;
			real3 _FresnelColor;
			real3 _AmbientColor;

			half _Outline;
			half _ZSmooth;
			uniform fixed4 _OutlineColorBegin;
			uniform fixed4 _OutlineColorEnd;

			half _FlashFreq;
			half _ElapsedTime;
			float4 _WorldRefPos;

			v2f vert(appdata v)
			{
				v2f o;
				o.HPosition = UnityObjectToClipPos(v.vertex);
				half3 binormal = cross(v.normal, v.tangent.xyz) * v.tangent.w;
				o.WorldNormal = mul(unity_ObjectToWorld, real4(v.normal,0));
				o.WorldTangent = mul(unity_ObjectToWorld, real4(v.tangent.xyz, 0));
				o.WorldBinormal = mul(unity_ObjectToWorld, real4(binormal, 0));

				o.WorldPosition = normalize(mul(unity_ObjectToWorld, v.vertex)).xyz;
				o.WorldViewDir = normalize(_WorldSpaceCameraPos.xyz - mul(unity_ObjectToWorld, v.vertex).xyz);
				o.uv_MainTex = TRANSFORM_TEX(v.uv, _MainTex);

				o.Factor.x = 0.5 * sin(_ElapsedTime * _FlashFreq * 3.1415926) + 0.5;
				UNITY_TRANSFER_FOG(o,o.HPosition);

				return o;
			}

			fixed4 frag(v2f IN) : SV_Target
			{
				/// Albedo comes from a texture tinted by color
				fixed4 albedo = tex2D(_MainTex, IN.uv_MainTex);
				fixed4 specular = tex2D(_SpecMap, IN.uv_MainTex);

				/// Normal map
				real3 bump = UnpackNormal(tex2D(_BumpMap, IN.uv_MainTex));
				//real3 bump = 2.0f * tex2D(_BumpMap, IN.uv_MainTex) -1.0f;
				//bump.y = -bump.y;

				real3 Nn = normalize(IN.WorldNormal);
				real3 Tn = normalize(IN.WorldTangent);
				real3 Bn = normalize(IN.WorldBinormal);

				Nn = Nn * bump.z + bump.x * Tn + bump.y * Bn;
				Nn = normalize(Nn);
				real3 Vn = IN.WorldViewDir;
				real3 Ln = normalize(_WorldSpaceLightPos0.xyz - IN.WorldPosition);
				real3 Hn = normalize(Vn + Ln);

				fixed4 col = 0;
				
				float attenuation = LIGHT_ATTENUATION(i);
				float3 attenColor = attenuation * _LightColor0.xyz;
				float3 specularColor = specular.rgb * _LightIntensity;
				float3 directSpecular = saturate(attenColor * pow(max(0, dot(Hn,Nn)), _SpecularPow) * specularColor);

				fixed3 fresnel = rim_lighting_term(Nn, Vn, _FresnelPow, _FresnelOffset);
				col.xyz += fresnel * max(1- dot(real3(0,-1,0),Nn),0) * specular.rgb;
				col.rgb += ((dot(Nn,Ln) * 0.5 + 0.5) * _LightColor0 + _AmbientColor) * albedo;
				col.rgb += directSpecular;
				col.rgb = lerp(col.rgb,col.rgb * SafeNormalize(col.rgb), _ColorStrength);

				// Outline flash
				half4 outlineCol = lerp(_OutlineColorEnd, _OutlineColorBegin, IN.Factor.x);
				real fresnelE = 1.0 - saturate(dot(Nn, Vn));
				col.rgb += pow(fresnelE, 2) * outlineCol * 3;

				col.a = 1;
				// apply fog
				UNITY_APPLY_FOG(IN.fogCoord, col);
				return col;
			}
			ENDCG
		}
	}
	FallBack "Diffuse"
}
