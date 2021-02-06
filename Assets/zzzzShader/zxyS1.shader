// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

Shader "zxy/shaderS1"
{
	Properties
	{
		_diff ("Diffuse",Color) = (1,1,1,1)
	}
	
	SubShader
	{
		Pass
		{
			CGPROGRAM

			#pragma vertex vert 
			#pragma fragment frag 

			// #include "UnityCG.inc";
			#include "Lighting.cginc"

			fixed4 _diff;

			// struct a2v
			// {
			// 	fixed4 vertex : POSITION;
			// 	fixed3 normal : NORMAL;
			// };

			// struct v2f
			// {
			// 	fixed4 pos : POSITION;
			// 	fixed3 color : Color;
			// };

			// v2f vert(a2v v)
			// {
			// 	fixed3 c = _LightColor0.xyz;
			// 	fixed3 m = _diff.rgb;
			// 	fixed3 n = normalize( mul(v.normal,(fixed3x3)unity_WorldToObject ));
			// 	fixed3 l = normalize(_WorldSpaceLightPos0.xyz);
			// 	fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.rgb;

			// 	fixed3 diff = c * m * saturate(dot(n , l));

			// 	v2f result;
			// 	result.pos = UnityObjectToClipPos(v.vertex);
			// 	result.color = ambient + diff;

			// 	return result;
			// }

			// fixed4 frag(v2f i):SV_Target
			// {
			// 	return fixed4(i.color,1);
			// }


			// struct a2v
			// {
			// 	fixed4 vertex : POSITION;
			// 	fixed3 normal : NORMAL;
			// };

			// struct v2f
			// {
			// 	fixed4 pos : POSITION;
			// 	// fixed3 color : Color;
			// 	fixed3 tex : TEXCOORD0;
			// };

			// v2f vert(a2v v)
			// {
				
			// 	fixed3 n = normalize( mul(v.normal,(fixed3x3)unity_WorldToObject ));
				
				

			// 	v2f result;
			// 	result.pos = UnityObjectToClipPos(v.vertex);
			// 	result.tex = n;

			// 	return result;
			// }

			// fixed4 frag(v2f i):SV_Target
			// {
			// 	fixed3 c = _LightColor0.xyz;
			// 	fixed3 m = _diff.rgb;

			// 	fixed3 l = normalize(_WorldSpaceLightPos0.xyz);
			// 	fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.rgb;

			// 	fixed3 diff = c * m * saturate(dot(i.tex , l));

			// 	fixed3 color = ambient + diff;

			// 	return fixed4(color,1);
			// }

			struct a2v
			{
				fixed4 vertex : POSITION;
				fixed3 normal : NORMAL;
			};

			struct v2f
			{
				fixed4 pos : POSITION;
				// fixed3 color : Color;
				fixed3 tex : TEXCOORD0;
			};

			v2f vert(a2v v)
			{
				
				fixed3 n = normalize( mul(v.normal,(fixed3x3)unity_WorldToObject ));
				
				

				v2f result;
				result.pos = UnityObjectToClipPos(v.vertex);
				result.tex = n;

				return result;
			}

			fixed4 frag(v2f i):SV_Target
			{
				fixed3 c = _LightColor0.xyz;
				fixed3 m = _diff.rgb;

				fixed3 l = normalize(_WorldSpaceLightPos0.xyz);
				fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.rgb;

				fixed3 diff = c * m * (0.5 * dot(i.tex , l) +0.5);

				fixed3 color = ambient + diff;

				return fixed4(color,1);
			}

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
