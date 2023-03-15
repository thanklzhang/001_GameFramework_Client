Shader "zxt/ss222"
{
	Properties
	{
		_specular ("specular",Color) = (1,1,1,1)
		_diff ("diffuse",Color) = (1,1,1,1)
		_gloss ("gloss",Range(1,256)) = 20
	}
	SubShader
	{
		Pass
		{
			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag

			#include "Lighting.cginc"


			struct a2v
			{
				fixed3 vertex:POSITION;
				fixed3 normal:NORMAL;
			};

			struct v2f
			{
				fixed4 pos : SV_POSITION;
				//fixed3 color : Color;
				
				fixed3 worldPos : TEXCOORD0;
				fixed3 worldNormal : TEXCOORD1;
			};

			fixed4 _specular;
			float _gloss;
			fixed4 _diff;
			v2f vert(a2v v)
			{
				//c * m * max( (v * r)) ^ gloss
				
				

				fixed3 worldPos = mul(unity_ObjectToWorld,v.vertex).xyz;
				// fixed3 worldNormal = mul(v.normal,(fixed3x3)unity_WorldToObject);
				fixed3 worldNormal = UnityObjectToWorldNormal(v.normal);

				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.worldPos = worldPos;
				o.worldNormal = worldNormal;
				//o.color = result;
				return o;

			}

			fixed4 frag(v2f i):SV_Target
			{
				fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz;
				fixed3 c = _LightColor0.rgb;
				fixed3 m = _specular.rgb;

				// fixed3 worldLightDir = normalize(_WorldSpaceLightPos0.xyz);
				fixed3 worldLightDir = normalize(UnityWorldSpaceLightDir(i.worldPos));

				// fixed3 view = normalize(_WorldSpaceCameraPos.xyz - i.worldPos);
				fixed3 view = normalize(UnityWorldSpaceViewDir(i.worldPos));
				//fixed3 r = normalize(reflect(-worldLightDir,i.worldNormal));
				fixed gloss = _gloss;

				fixed3 h = normalize(view + worldLightDir);//

				fixed3 specular = c * m * pow(saturate(i.worldNormal * h),gloss);

				fixed3 diff = c * _diff.rgb * saturate(dot(i.worldNormal,worldLightDir));

				fixed3 result = ambient  + diff+ specular;//+ specular

				return fixed4(result,1);
			}

			// fixed4 frag(v2f i):SV_Target
			// {
			// 	fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz;
			// 	fixed3 c = _LightColor0.rgb;
			// 	fixed3 m = _specular.rgb;

			// 	fixed3 worldLightDir = normalize(_WorldSpaceLightPos0.xyz);

			// 	fixed3 view = normalize(_WorldSpaceCameraPos.xyz - i.worldPos);
			// 	fixed3 r = normalize(reflect(-worldLightDir,i.worldNormal));
			// 	fixed gloss = _gloss;

			// 	fixed3 specular = c * m * pow(saturate(view * r),gloss);

			// 	fixed3 diff = c * _diff.rgb * saturate(dot(i.worldNormal,worldLightDir));

			// 	fixed3 result = ambient + specular + diff;

			// 	return fixed4(result,1);
			// }


			// struct a2v
			// {
			// 	fixed3 vertex:POSITION;
			// 	fixed3 normal:NORMAL;
			// };

			// struct v2f
			// {
			// 	fixed4 pos : SV_POSITION;
			// 	fixed3 color : Color;
			// };

			// fixed4 _specular;
			// float _gloss;
			// fixed4 _diff;
			// v2f vert(a2v v)
			// {
			// 	//c * m * max( (v * r)) ^ gloss
				
			// 	fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz;

			// 	fixed3 c = _LightColor0.rgb;
			// 	fixed3 m = _specular.rgb;
			// 	fixed3 worldNormal = mul(v.normal,(fixed3x3)unity_WorldToObject);
			// 	fixed3 worldLightDir = normalize(_WorldSpaceLightPos0.xyz);
			// 	fixed3 view = normalize(_WorldSpaceCameraPos.xyz - mul(unity_ObjectToWorld,v.vertex).xyz);
			// 	fixed3 r = normalize(reflect(-worldLightDir,worldNormal));
			// 	fixed gloss = _gloss;

			// 	fixed3 specular = c * m * pow(saturate(view * r),gloss);

			// 	fixed3 diff = c * _diff.rgb * saturate(dot(worldNormal,worldLightDir));

			// 	fixed3 result = ambient + specular + diff;

			// 	v2f o;
			// 	o.pos = UnityObjectToClipPos(v.vertex);
			// 	o.color = result;
			// 	return o;

			// }

			// fixed4 frag(v2f i):SV_Target
			// {
			// 	return fixed4(i.color,1);
			// }

			ENDCG
		}
	}
}




















































// Shader "Unlit/zxyS1"
// {
//     Properties
//     {
//         _MainTex ("Texture", 2D) = "white" {}
//     }
//     SubShader
//     {
//         Tags { "RenderType"="Opaque" }
//         LOD 100

//         Pass
//         {
//             CGPROGRAM
//             #pragma vertex vert
//             #pragma fragment frag
//             // make fog work
//             #pragma multi_compile_fog

//             #include "UnityCG.cginc"

//             struct appdata
//             {
//                 float4 vertex : POSITION;
//                 float2 uv : TEXCOORD0;
//             };

//             struct v2f
//             {
//                 float2 uv : TEXCOORD0;
//                 UNITY_FOG_COORDS(1)
//                 float4 vertex : SV_POSITION;
//             };

//             sampler2D _MainTex;
//             float4 _MainTex_ST;

//             v2f vert (appdata v)
//             {
//                 v2f o;
//                 o.vertex = UnityObjectToClipPos(v.vertex);
//                 o.uv = TRANSFORM_TEX(v.uv, _MainTex);
//                 UNITY_TRANSFER_FOG(o,o.vertex);
//                 return o;
//             }

//             fixed4 frag (v2f i) : SV_Target
//             {
//                 // sample the texture
//                 fixed4 col = tex2D(_MainTex, i.uv);
//                 // apply fog
//                 UNITY_APPLY_FOG(i.fogCoord, col);
//                 return col;
//             }
//             ENDCG
//         }
//     }
// }
