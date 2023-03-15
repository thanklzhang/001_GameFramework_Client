// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "zxy/test light"//逐顶点 Phong 光照
{
	Properties
	{
		diff("diffuse",Vector) = (1,1,1,1)
	}

		Subshader
	{
		Pass
		{

			CGPROGRAM
			#pragma vertex vert ;
			#pragma fragment frag ;

			#include "Lighting.cginc"

			// c m   max(0 n · l)

			fixed4 diff;
		struct v2f
		{
			fixed4 pos : Position;
			fixed3 color : Color;
		};

		v2f vert(appdata_base v)
		{

			fixed di;
			fixed3 ambient;

			fixed3 cColor = _LightColor0.rgb;
			fixed3 m = diff.rgb;

			fixed3 normalDir = v.normal;


			fixed3 worldNormalDir = normalize(mul(normalDir, unity_WorldToObject));
			fixed3 worldLightDir = normalize(_WorldSpaceLightPos0.xyz);

			di = cColor * m * saturate(dot(worldNormalDir, worldLightDir));
			ambient = UNITY_LIGHTMODEL_AMBIENT.xyz;

			v2f f;
			f.pos = UnityObjectToClipPos(v.vertex);
			f.color = di + ambient;
			return f;
		}
		fixed3 frag(v2f i) :SV_Target
		{
			return i.color;
		}

		ENDCG



	}
	}
}