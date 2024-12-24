﻿Shader "HeroGo/ElectricHair/Outline_Flash"
{
    Properties
    {
        _Albedo ("Main Texture", 2D) = "white" {}
		_MainColor ("Main Color", Color) = (1, 1, 1, 1)
		_Intensity ("Main Intensity", Range(1, 2)) = 1
		_ScrollX ("ScrollX", Range(-10, 10)) = 1
		_ScrollY ("ScrollY", Range(-10, 10)) = 1

		[Toggle(_DetailLayer)] _DetailLayer("Detail Layer", Int) = 0
		_DetailTex ("Detail Texture", 2D) = "white" {}		
		_DetailColor ("Detail Color", Color) = (1, 1, 1, 1)
		_DetailInt ("Detail Intensity", Range(1, 20)) = 5	
		_Scroll2X ("Scroll2X", Range(-10, 10)) = 1
		_Scroll2Y ("Scroll2Y", Range(-10, 10)) = 1		
		_Distort ("Distort", Range(-10, 10)) = 0

		//OUTLINE
		_FlashFreq("Flash frequncy", Range(0, 10)) = 3
		_Outline("Outline Width", Range(0.0, 0.1)) = 0.001
		_OutlineColorBegin("Begin Outline Color", Color) = (1.0, 0.0, 0.0, 1)
		_OutlineColorEnd("End Outline Color", Color) = (1.0, 1.0, 0.0, 1)

		[HideInInspector]_ElapsedTime("Elapsed time", Float) = 0
		[HideInInspector]_WorldRefPos("World ref position", Vector) = (0, 0, 0, 0)
    }
    SubShader
    {
        Tags {"LightMode" = "ForwardBase" 
		       "RenderType"="Opaque"       
		}
        LOD 100
		Pass
		{
		    Cull Back
			Lighting Off
			ZWrite On
			ZTest On

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
			#pragma multi_compile _ _DetailLayer
			#pragma target 2.0

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
				float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
				float3 normalDir : TEXCOORD1;
				float3 viewDir : TEXCOORD2;
				float Factor : TEXCOORD3;
            };

            sampler2D _Albedo;
            float4 _Albedo_ST;
			fixed4 _MainColor;
			float _Intensity;
			float _ScrollX;
			float _ScrollY;

#ifdef _DetailLayer
			sampler2D _DetailTex;
            float4 _DetailTex_ST;
			fixed4 _DetailColor;
			float _DetailInt;			
			float _Scroll2X;
			float _Scroll2Y;			
			float _Distort;
#endif
            half _Outline;
			half _ZSmooth;
			fixed4 _OutlineColorBegin;
			fixed4 _OutlineColorEnd;

			half _FlashFreq;
			half _ElapsedTime;
			float4 _WorldRefPos;

            v2f vert (appdata v)
            {
                v2f o;
				UNITY_INITIALIZE_OUTPUT(v2f, o);
                o.vertex = UnityObjectToClipPos(v.vertex);
				
                float2 uv1 = TRANSFORM_TEX(v.uv, _Albedo);
				float2 speed1 = _Time.x * float2(_ScrollX, _ScrollY);
				speed1 = frac(speed1);
				o.uv.xy = uv1 + speed1;
#ifdef _DetailLayer
				float2 uv2 = TRANSFORM_TEX(v.uv, _DetailTex);
				float2 speed2 = _Time.x * float2(_Scroll2X, _Scroll2Y);
				speed2 = frac(speed2);				
				o.uv.zw = uv2 + speed2;
#endif          
                //
                float4 worldPositon = mul(unity_ObjectToWorld, v.vertex);
				o.normalDir = UnityObjectToWorldNormal(v.normal);
				o.viewDir = normalize(_WorldSpaceCameraPos.xyz - worldPositon.xyz);

				o.Factor.x = 0.5 * sin(_ElapsedTime * _FlashFreq * 3.1415926) + 0.5;

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed3 mainCol = tex2D(_Albedo, i.uv.xy) * _MainColor.rgb * _Intensity;
#ifdef _DetailLayer
				float2 distortUV = _Distort * mainCol.r + i.uv.zw;				
				fixed3 detailCol = tex2D(_DetailTex, distortUV) * _DetailColor.rgb * _DetailInt;
				fixed3 finalCol = mainCol + detailCol;
#else
                fixed3 finalCol = mainCol;
#endif               
				fixed4 col = fixed4(finalCol, 1);

				//动画材质
		        half4 outlineCol = lerp(_OutlineColorEnd, _OutlineColorBegin, i.Factor.x);
		        float fresnel = 1.0 - saturate(dot(i.normalDir, i.viewDir));
		        col.rgb += pow(fresnel, 2) * outlineCol * 2;

                return col;
            }
            ENDCG
        }
    }
	FallBack "HeroGo/ElectricHair/General"

}
