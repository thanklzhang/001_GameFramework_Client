//逐顶点 高光反射模型
Shader "zxy/specular_test_01"
{
	Properties
	{
		_diffuse ("diffuse",Color) = (1,1,1,1)
		_mSpecular ("specularColor",Color) = (1,1,1,1)
		_gloss ("gloss",float) = 1
	}

	SubShader
	{
		Pass
		{
			Tags
			{
				"LightMode" = "ForwardBase"
			}

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "Lighting.cginc"
			
			struct a2v
			{
				float4 pos : POSITION;
				float3 nor : NORMAL;
			};

			float3 _diffuse;
			float3 _mSpecular;
			float _gloss;

			struct v2f
			{
				float4 pos : SV_POSITION;
				float3 c : Color0;
			};
			
			float3 GetR(float3 l,float3 n)
			{
				//return normalize(l - 2 * (dot(n , l)) * n);
				return normalize(reflect(l,n));
			}

			v2f vert(a2v v)
			{
				//c = c * m * max(0,dot(v,r))^gloss
				//r = l - 2(n * l)*n

				float3 c_light = _LightColor0.rgb;
				float3 m_specular = _mSpecular.rgb;

				float3 l = normalize( _WorldSpaceLightPos0.xyz);

				float3 n = normalize(mul(unity_ObjectToWorld,v.nor));
				float gloss = _gloss;

				//float3 r = normalize(l - 2 * (dot(n , l)) * n);

				float3 r = GetR(-l,n);


				float3 viewDir = normalize(_WorldSpaceCameraPos - mul(unity_ObjectToWorld,v.pos));
				




				float3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz;
				float3 dif = c_light * _diffuse.rgb * saturate(dot(n,l));
				float3 specular = c_light * m_specular.rgb * pow(saturate(dot(viewDir,r)),_gloss);

				float3 c = ambient + dif + specular;

				v2f f;             
				f.c = c;
				f.pos = UnityObjectToClipPos(v.pos);
				return f;
			}

			float4 frag(v2f f) : SV_TARGET
			{
				return float4(f.c,1);
			}
			
			

			ENDCG

		}
	}
	
}