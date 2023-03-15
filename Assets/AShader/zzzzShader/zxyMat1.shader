Shader "zxy/normal1"
{
	Properties
	{
		_MainTex ("MainTex",2D) = "white" {}
		_MainNormal ("MainNormal",2D) = "white" {}
		_BumpScale ("Bump",Range(-3,3)) = 1
	}
	SubShader
	{
		Pass
		{
			Tags {"LightMode" = "ForwardBase"}
			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			#include "Lighting.cginc"

			sampler2D _MainTex;
			float4 _MainTex_ST;
			sampler2D _MainNormal;
			float4 _MainNormal_ST;

			float _BumpScale;

			struct a2v
			{
				fixed4 pos : Position;
				fixed3 normal : Normal;
				fixed4 tangent : Tangent;
				fixed4 texcoord : TEXCOORD;
			};

			struct v2f
			{
				fixed4 pos : SV_Position;
				float4 uv : TEXCOORD0;
				//fixed3 tanNormal : TEXCOORD1;
				fixed3 tanView : TEXCOORD1;
				fixed3 tanLightDir : TEXCOORD2;
			};


			v2f vert(a2v v)
			{
				v2f o;
				o.uv.xy = v.texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;
				o.uv.zw = v.texcoord.xy * _MainNormal_ST.xy + _MainNormal_ST.zw;

				fixed3 tanX = normalize(v.tangent);
				fixed3 tanZ = normalize(v.normal);
				fixed3 tanY = normalize(cross(tanX,tanZ) * v.tangent.w);
				fixed3x3 tanMatrix = fixed3x3(tanX,tanY,tanZ);

				fixed3 tanView = normalize(mul(tanMatrix,ObjSpaceViewDir(v.pos)));
				fixed3 tanLightDir = normalize(mul(tanMatrix,ObjSpaceLightDir(v.pos)));

				
				o.pos = UnityObjectToClipPos(v.pos);
				o.tanView = tanView;
				o.tanLightDir = tanLightDir;
				

				return o;
			}

			fixed4 frag(v2f i):SV_Target
			{
				fixed4 packageNormal = tex2D(_MainNormal,i.uv.zw);
//				fixed3 normalRgb = sample2D(_MainNormal,i.uv);
//				
				fixed3 tanNormal = UnpackNormal(packageNormal);
				//tanNormal.
				tanNormal.xy *= _BumpScale;
				//tanNormal.z = sqrt(1 - saturate( dot(tanNormal.xy,tanNormal.xy)));
				

				fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz;
				fixed3 diff = _LightColor0.rgb * tex2D(_MainTex,i.uv.xy).rgb * (saturate(dot(tanNormal,i.tanLightDir)));

				fixed3 result = diff + ambient;
				return fixed4(result,1);
			}

			ENDCG
		}
	}	
}




















































// Shader "zxt/ss222"
// {
// 	Properties
// 	{
// 		_specular ("specular",Color) = (1,1,1,1)
// 		_diff ("diffuse",Color) = (1,1,1,1)
// 		_gloss ("gloss",Range(1,256)) = 20
// 	}
// 	SubShader
// 	{
// 		Pass
// 		{
// 			CGPROGRAM

// 			#pragma vertex vert
// 			#pragma fragment frag

// 			#include "Lighting.cginc"


// 			struct a2v
// 			{
// 				fixed3 vertex:POSITION;
// 				fixed3 normal:NORMAL;
// 			};

// 			struct v2f
// 			{
// 				fixed4 pos : SV_POSITION;
// 				//fixed3 color : Color;
				
// 				fixed3 worldPos : TEXCOORD0;
// 				fixed3 worldNormal : TEXCOORD1;
// 			};

// 			fixed4 _specular;
// 			float _gloss;
// 			fixed4 _diff;
// 			v2f vert(a2v v)
// 			{
// 				//c * m * max( (v * r)) ^ gloss
				
				

// 				fixed3 worldPos = mul(unity_ObjectToWorld,v.vertex).xyz;
// 				// fixed3 worldNormal = mul(v.normal,(fixed3x3)unity_WorldToObject);
// 				fixed3 worldNormal = UnityObjectToWorldNormal(v.normal);

// 				v2f o;
// 				o.pos = UnityObjectToClipPos(v.vertex);
// 				o.worldPos = worldPos;
// 				o.worldNormal = worldNormal;
// 				//o.color = result;
// 				return o;

// 			}

// 			fixed4 frag(v2f i):SV_Target
// 			{
// 				fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz;
// 				fixed3 c = _LightColor0.rgb;
// 				fixed3 m = _specular.rgb;

// 				// fixed3 worldLightDir = normalize(_WorldSpaceLightPos0.xyz);
// 				fixed3 worldLightDir = normalize(UnityWorldSpaceLightDir(i.worldPos));

// 				// fixed3 view = normalize(_WorldSpaceCameraPos.xyz - i.worldPos);
// 				fixed3 view = normalize(UnityWorldSpaceViewDir(i.worldPos));
// 				//fixed3 r = normalize(reflect(-worldLightDir,i.worldNormal));
// 				fixed gloss = _gloss;

// 				fixed3 h = normalize(view + worldLightDir);//

// 				fixed3 specular = c * m * pow(saturate(i.worldNormal * h),gloss);

// 				fixed3 diff = c * _diff.rgb * saturate(dot(i.worldNormal,worldLightDir));

// 				fixed3 result = ambient  + diff+ specular;//+ specular

// 				return fixed4(result,1);
// 			}

// 			