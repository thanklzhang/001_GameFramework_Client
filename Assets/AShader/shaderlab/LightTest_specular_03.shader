//逐像素 Blinn - Phong 模型
Shader "zxy/specular_test_02"
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
			
				float3 _diffuse;
			float3 _mSpecular;
			float _gloss;


			struct a2v
			{
				float4 pos : POSITION;
				float3 nor : NORMAL;
			};

			struct v2f
			{
				float4 pos : SV_POSITION;
				float3 c : Color0;
				float3 p : Color1;
			};
			
			float3 GetR(float3 l,float3 n)
			{
				return normalize(reflect(l,n));
			}

			v2f vert(a2v v)
			{
				v2f f;             
				f.c = mul(unity_ObjectToWorld, v.nor);//normal
				f.pos = UnityObjectToClipPos(v.pos);
				f.p = mul(unity_ObjectToWorld, v.pos);
				return f;
			}

			float4 frag(v2f f) : SV_TARGET
			{

				float3 c_light = _LightColor0.rgb;
				float3 m_specular = _mSpecular.rgb;

				float3 l = normalize( _WorldSpaceLightPos0.xyz);

				float3 n = normalize(f.c);//normal
				float gloss = _gloss;

				//float3 viewDir = normalize(_WorldSpaceCameraPos - mul(unity_ObjectToWorld,f.p));

				float3 viewDir = normalize(UnityWorldSpaceViewDir(f.p));
				
				//float3 r = GetR(-l,n);

				float3 h = normalize(viewDir + l);

				

				float3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz;
				float3 dif = c_light * _diffuse.rgb * saturate(dot(n,l));
				float3 specular = c_light * m_specular.rgb * pow(saturate(dot(n,h)),_gloss);

				float3 c = ambient + dif + specular;



				return float4(c,1);
			}
			
			

			ENDCG

		}
	}
	
}