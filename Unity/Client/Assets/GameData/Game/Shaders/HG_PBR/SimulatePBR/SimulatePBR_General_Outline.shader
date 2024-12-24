Shader "HeroGo/SimulatePBR/Outline" {
    Properties {
		[Header(BRDF Controller)]
		_BrightColor ("Bright Color", Color) = (1, 1, 1, 1)
        _BrightColorOffset ("Bright Color Offset", Range(-1, 1)) = 0
        _BrightColorSharpen ("Bright Color Sharpen", Range(1, 10)) = 1
        _BrightEdge ("Bright Edge", Color) = (0, 0, 0, 1)
        _BrightEdgeOffset ("Bright Edge Offset", Range(-1, 1)) = 0
        _BrightEdgeSharpen ("Bright Edge Sharpen", Range(1, 10)) = 1
        _DarkColor ("Dark Color", Color) = (0, 0, 0, 1)
        _DarkOffset ("Dark Color Offset", Range(-1, 1)) = 0
        _DarkSharpen ("Dark Color Sharpen", Range(1, 10)) = 1
        _DarkEdge ("Dark Edge", Color) = (0, 0, 0, 1)
        _DarkEdgeOffset ("Dark Edge Offset", Range(-1, 1)) = 0
        _DarkEdgeSharpen ("Dark Edge Sharpen", Range(1, 10)) = 1
		[Header(_ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ )]
		_DirectionGradualColor ("Direction Gradual", Range(0, 1)) = 1

		[Header(_________________________________________________________________________)]
        _Color ("Color", Color) = (1,1,1,1)
        _Albedo ("Albedo", 2D) = "white" {}
        _Bump ("Bump (RGB)", 2D) = "bump" {}
        _Param ("Glossy (RGB) R:Metallic G:Smoothness B:Ambient Occlusion", 2D) = "white" {}
		[Header(_________________________________________________________________________)]
		[Toggle(USE_EMISSION)] _uesEmission ("USE Emission", Float ) = 0
        _Emission ("Emission", 2D) = "white" {}
        _Noise ("Noise", 2D) = "white" {}
		[Toggle(EMISSION_GRADUAL)] _EmissionGradual ("Emission Gradual", Float ) = 0
        _GradualLocation ("Gradual Location", Range(-1, 2)) = -0.4
        _Glittering ("Emission Glittering", Range(0, 1)) = 1
        _EmissionIntensity ("Emission Intensity", Range(0, 1)) = 1
		[Header(_________________________________________________________________________)]
        _LitSphere ("Lit Sphere (RGB)", 2D) = "black" {}
		[Header(_________________________________________________________________________)]
		//OUTLINE
		_ScaleTimeLen("Scale time length", Range(0, 10)) = 0.23333331
		_ScaleWidth("Scale width", Range(1, 10)) = 3.5
		_BeginScale("Begin Scale", Range(0, 10)) = 3
		_Outline("Outline Width", Range(0.0, 0.01)) = 0.001
		_OutlineColorBegin("Begin Outline Color", Color) = (1.0, 0.0, 0.0, 1)
		_OutlineColorEnd("End Outline Color", Color) = (1.0, 1.0, 0.0, 1)
		[HideInInspector]_ElapsedTime("Elapsed time", Float) = 0
		[HideInInspector]_WorldRefPos("World ref position",Vector) = (0,0,0,0)
    }

    SubShader {
        Tags {
            "Queue"="Transparent"
			"RenderType"="Transparent"
        }
        	LOD 100
			Cull Back
			Zwrite On
			Blend SrcAlpha OneMinusSrcAlpha
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
			#include "UnityCG.cginc"
            #pragma target 2.0
			#pragma multi_compile _ USE_EMISSION
			#pragma multi_compile _ EMISSION_GRADUAL
			
			 fixed4 _BrightColor;
             fixed4 _BrightEdge;
             fixed4 _DarkEdge;
             fixed4 _DarkColor;
             fixed _DarkEdgeOffset;
             fixed _DarkEdgeSharpen;
             fixed _DarkOffset;
             fixed _DarkSharpen;
             fixed _BrightColorOffset;
             fixed _BrightColorSharpen;
             fixed _BrightEdgeOffset;
             fixed _BrightEdgeSharpen;

			 fixed _DirectionGradualColor;

             fixed4 _Color;
             sampler2D _Albedo;  
             sampler2D _Bump;  
             sampler2D _Param;  
             sampler2D _Emission;  
             sampler2D _Noise;
			 float4 _Noise_ST;
             fixed _EmissionIntensity;
             sampler2D _LitSphere;
             fixed _Glittering;
			 float4 _LightColor0;
			 fixed _GradualLocation;
				
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
            };
				
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float4 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                //float3 normalDir : TEXCOORD2;
                float4 tangentDir : TEXCOORD2;
                float4 bitangentDir : TEXCOORD3;
				float4 viewDirection : TEXCOORD4;
				float4 lightDirection : TEXCOORD5;
				float3 halfDirection : TEXCOORD6;
            };

            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
				UNITY_INITIALIZE_OUTPUT(VertexOutput, o);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
				float4 objPos = mul ( unity_ObjectToWorld, float4(0,0,0,1));
				float3 localSpace = o.posWorld.rgb - objPos.rgb;
				localSpace.y = (localSpace.y - 0.5);
			float gradualM;
			float gradualMT;
			#if USE_EMISSION
				float wave = length(localSpace + float3(0, -_GradualLocation, 0.2));
				float ratio = saturate(1 - wave);
				wave = wave * (1 - ratio + 2 * ratio);

				float waveA = frac( wave - _Time.g * 0.7);
				gradualM = abs(waveA - 0.5) * 2.0;
				
				float waveB = frac( wave - _Time.g * 0.3);
				gradualMT = abs(waveB - 0.5) * 2.0;
			#endif
                o.uv0 = float4(v.texcoord0, gradualM, localSpace.y);
                float3 normalDir = normalize(UnityObjectToWorldNormal(v.normal));
                o.tangentDir = float4(normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz ) ,normalDir.x);
                o.bitangentDir = float4(normalize(cross(normalDir, o.tangentDir) * v.tangent.w) ,normalDir.y);
				o.bitangentDir.y = abs(o.bitangentDir.y);
                o.pos = UnityObjectToClipPos( v.vertex );
                o.viewDirection = float4(normalize(_WorldSpaceCameraPos.xyz - o.posWorld.xyz) ,normalDir.z);
                o.lightDirection = float4(normalize(float3(0, 1, -1)), gradualMT);//normalize(_WorldSpaceLightPos0.xyz);
				//o.lightDirection = normalize(_WorldSpaceLightPos0.xyz * (1-_WorldSpaceLightPos0.w) + (_WorldSpaceLightPos0.xyz - o.posWorld.xyz) * _WorldSpaceLightPos0.w);
                o.halfDirection = normalize(o.viewDirection + o.lightDirection.rgb);
                return o;
            }
			
			fixed3 NormalDirection(fixed3 normalMap, fixed3 normalDir, fixed3 tangentDir, fixed3 bitangentDir){
				float3x3 tangentTransform = float3x3(tangentDir, bitangentDir, normalDir);
                return normalize(mul( normalMap, tangentTransform )); // Perturbed normals
			}

			fixed halfOptimize(fixed3 A, fixed3 B){
				return max(0.0, dot(A, B)) * 0.5 + 0.5;
			}

			fixed3 brdfModule(fixed2 brdfSpace, fixed2 position, fixed brdfOffset, fixed brdfSharpen, fixed3 col){
				fixed brdfRange = 1.0 - length(brdfSpace + position) + brdfOffset;
				brdfRange = brdfRange * brdfSharpen + 0.5 - brdfSharpen * 0.5;
				return col * max(0.0, brdfRange);
			}
			
            float4 frag(VertexOutput i) : SV_Target {
                
                fixed4 mainTex = tex2D(_Albedo, i.uv0);
                fixed4 glossy = tex2D(_Param, i.uv0);
				fixed3 normalDir = fixed3(i.tangentDir.w, i.bitangentDir.w, i.viewDirection.w);
				fixed3 normalMap = UnpackNormal(tex2D(_Bump, i.uv0));
                fixed3 normalVer = NormalDirection(normalMap, normalDir, i.tangentDir.xyz, i.bitangentDir.xyz); // Perturbed normals
				fixed Glossing = 1.0 - glossy.g;
                fixed metallica = glossy.r;
				fixed3 diffuseColor = mainTex.rgb * _Color.rgb;

		// Diffuse---<
				fixed2 brdfSpace = fixed2(halfOptimize(i.lightDirection.xyz, normalVer), halfOptimize(i.viewDirection.xyz, normalVer));
				fixed3 brightBRDF = brdfModule(brdfSpace, float2(-1, -1), _BrightColorOffset, _BrightColorSharpen, _BrightColor.rgb);
				fixed3 brEdBRDF = brdfModule(brdfSpace, float2(-1,  0), _BrightEdgeOffset, _BrightEdgeSharpen, _BrightEdge.rgb);
				fixed3 darkBRDF = brdfModule(brdfSpace, float2( 0, -1), _DarkOffset, _DarkSharpen, _DarkColor.rgb);
				fixed3 daEdBRDF = brdfModule(brdfSpace, float2( 0,  0), _DarkEdgeOffset, _DarkEdgeSharpen, _DarkEdge.rgb);
                fixed3 finalBRDF = brightBRDF + brEdBRDF + darkBRDF + daEdBRDF; // BRDF
                fixed3 diffuseCol = finalBRDF - finalBRDF * metallica;
			//--->	
				
		// reflection---<
                fixed3 matcapNormal = NormalDirection(normalMap, normalVer, i.tangentDir.xyz, i.bitangentDir.xyz); // Perturbed normals
                fixed2 matcapuv = mul( UNITY_MATRIX_V, float4( matcapNormal, 0 ) ).rg * 0.5 + 0.5; // normalSpaceUV
				fixed3 reflectMap = tex2D(_LitSphere, matcapuv).rgb;
				fixed reflecMask = Glossing * Glossing;
				fixed reflect = (reflectMap.b - reflectMap.b * reflecMask) + (reflectMap.g + reflectMap.r * 2) * reflecMask;
				reflect *= 2.5;
				reflect = reflect + reflect * saturate(normalVer.y * 3 - 1) * 0.3;
				fixed3 reflection = reflect + (brEdBRDF + daEdBRDF) * 4;// + darkBRDF;
				reflection *= metallica;
			//--->

		// specular---<
				fixed blinnPhong = max(0, dot(normalVer, i.halfDirection)); // Blinn-Phong

				fixed gloss = Glossing + 11 * Glossing;
				fixed specularShepe = pow(blinnPhong, exp2(gloss));
                fixed3 specularCol = specularShepe * Glossing * diffuseColor * 5; // Specular Contribution
			//--->		
				fixed3 finalColor = diffuseCol + reflection + specularCol;
				finalColor *= diffuseColor * _LightColor0.rgb;

		// directionGradua---<
				fixed DGMask = saturate(i.uv0.w + normalVer.y);  // DGMask = Direction Gradual Mask
				fixed directionG = (_DirectionGradualColor - _DirectionGradualColor * DGMask) + 1 * DGMask;
				fixed finalColorMask = (mainTex.r + mainTex.g + mainTex.b) * 0.33;
				finalColor = (finalColor - finalColor * finalColorMask) + finalColor * directionG * finalColorMask;
			//--->	

			#if USE_EMISSION
				fixed4 emissionCol = tex2D(_Emission, i.uv0);
                fixed3 emissive = emissionCol * _EmissionIntensity;
				#if EMISSION_GRADUAL
					float gradualM = i.uv0.z;
					float gradualMT = i.lightDirection.w;
					float gradualRes = gradualM * emissionCol.a + gradualMT * (1 - emissionCol.a);
					float gradualMove = saturate(gradualRes * 1.25 - 0.25) + 0.5;
					emissive *= gradualMove;
					i.uv0.y = i.uv0.y + _Time.b * 0.0004;
					fixed noise = tex2D(_Noise, TRANSFORM_TEX(i.uv0, _Noise)).r;

					float eff = dot(noise * normalDir, i.viewDirection.xyz);
					eff = saturate(eff * 5.0 - 2.5);  // eff = saturate(eff * 5.0 - 3);
					float emissionMask = (emissionCol.r + emissionCol.g + emissionCol.b) * 0.333;
					emissionMask = saturate((emissionMask - 0.5) * 2);
					float Gemissive = emissionCol * eff * emissionMask * 6;// * saturate(1 - gradualM * 0.7);
					emissive += Gemissive * _Glittering;
				#endif
                finalColor += emissive;
			#endif
				finalColor *= _LightColor0.rgb*1.3;
                return fixed4(finalColor, 1);
            }
            ENDCG
        }
		Pass
		{
			Name "OUTLINE_DOWNGRADE"

			Cull Back
			Lighting Off
			Fog{ Mode Off }
			//ZTest On
			Blend SrcAlpha One
			Tags { "LightMode"="ForwardBase" }
			
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest
			#include "UnityCG.cginc"

			struct a2v
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float4 color : COLOR;
				float2 texcoord0 : TEXCOORD0;
			};

			struct v2f
			{
				float4 pos : SV_POSITION;
				float2 factor : TEXCOORD0;
				float3 normal : TEXCOORD1;
				float3 ViewDir : TEXCOORD2;
				float4 color : COLOR0;
				float2 uv0 : TEXCOORD3;
			};

			half _Outline;
			half _ZSmooth;
			uniform fixed4 _OutlineColorBegin;
			uniform fixed4 _OutlineColorEnd;

			half _ElapsedTime;
			half _BeginScale;
			half _ScaleTimeLen;
			half _ScaleWidth;
			float4 _WorldRefPos;
			//float _debrisSpeed;
			//fixed _debrisSize;
			float _windSpeed;
			float _windPower;
			float _windFrequency;
			


			v2f vert(a2v v)
			{
				 v2f o;
				 o.factor.xy = saturate((_ScaleTimeLen - _ElapsedTime) / _ScaleTimeLen);
				 
				 //Correct Z artefacts
				 o.color = v.color;
				 o.uv0 = v.texcoord0;
				 fixed VerColGOneMinus = (1.0 - o.color.g);

				 float sineWave = sin(_Time.a * _windSpeed + (VerColGOneMinus * (_windFrequency * 120.0)));
                v.vertex.xyz += float3(0.8,0.4,1) * sineWave * o.color.g * _windPower * 0.06 + 0.03;

				 float4 pos = mul(unity_ObjectToWorld, v.vertex);
				 o.ViewDir = normalize(_WorldSpaceCameraPos - pos);
				 
				 pos.xyz -= _WorldRefPos;
				 pos.xyz *= (1.005 + o.factor.x * _BeginScale * half3(_ScaleWidth, 1, _ScaleWidth));
				 pos.xyz += _WorldRefPos;				
				 pos = mul(UNITY_MATRIX_V, pos);
				 
				 float3 normal = mul((float3x3)UNITY_MATRIX_IT_MV, v.normal);
				 normal.z = _ZSmooth;
				 
				 //Camera-independent size
				 float dist = distance(_WorldSpaceCameraPos, mul(unity_ObjectToWorld, v.vertex));
				 
				 

				 o.pos = mul(UNITY_MATRIX_P, pos);
				 o.normal = normalize(mul(unity_ObjectToWorld, half4(v.normal, 0)));

				return o;
			}

			float4 frag(v2f IN) : COLOR
			{
				// Outline flash
				half3 color = lerp(_OutlineColorEnd, _OutlineColorBegin, IN.factor.x);
				half fresnel = 1.0 - saturate(dot(IN.normal, IN.ViewDir));


				return half4( pow(fresnel, 2) * color * 2, fresnel * 2);
			}
			ENDCG
		}


    }
    FallBack "HeroGo/SimulatePBR/General"
}
