Shader "zxyzxyzxyzxyzxy/WorldTest"
{

	Properties
	{
		_MainTex ("mainTex",2D) = "white" {}
		_NormalTex ("normalTex",2D) = "white" {}
		_diff ("diffuse",Color) = (1,1,1,1)
		_specular ("specular",Range(1,200)) = 1 
		_gloss ("gloss",Range(1,200)) = 1 
		_BumpScale ("Bump",Range(-3,3)) = 1
	}

	SubShader
	{
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "Lighting.cginc"

			sampler2D _MainTex;
			fixed4 _MainTex_ST;
			sampler2D _NormalTex;
			fixed4 _NormalTex_ST;
			float _specular;
			float _gloss;
			fixed4 _diff;
			float _BumpScale;
			struct a2v
			{
				fixed4 pos :POSITION;
				fixed3 normal :NORMAL;
				fixed3 tan :Tangent;
				fixed4 texcoord :TEXCOORD0;
			};

			struct v2f
			{
				fixed4 pos : POSITION;
				fixed4 worldPos : TEXCOORD0;
				fixed3 worldNormal : TEXCOORD1;
				fixed3 worldTan : TEXCOORD2;
				fixed3 worldBin : TEXCOORD3;
				fixed4 uv : TEXCOORD4;
			};

			v2f vert(a2v v)
			{

				v2f o;
				o.pos = UnityObjectToClipPos(v.pos);
				o.worldPos = mul(unity_ObjectToWorld,v.pos);
				o.worldNormal = UnityObjectToWorldNormal(v.normal);
				o.worldTan = mul(unity_ObjectToWorld,v.tan);
				o.worldBin = normalize(cross(o.worldNormal,o.worldTan));
				o.uv.xy = v.texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;
				o.uv.zw = v.texcoord.xy *_NormalTex_ST.xy + _NormalTex_ST.zw;

				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 mainColor = tex2D(_MainTex,i.uv.xy);
				fixed4 normalColor = tex2D(_NormalTex,i.uv.zw);

				fixed3 tanNormal = UnpackNormal(normalColor);
				tanNormal *= _BumpScale;

				fixed3 worldNormalDir = fixed3( dot( i.worldTan , tanNormal),
				dot( i.worldBin , tanNormal),
				dot( i.worldNormal , tanNormal));

				fixed3 ambient;
				fixed3 diff;
				fixed3 specular = fixed3(0,0,0);

				fixed3 normalDir = worldNormalDir;
				fixed3 lightDir = UnityWorldSpaceLightDir(i.worldPos);

				fixed3 c = _LightColor0.rgb;
				ambient = UNITY_LIGHTMODEL_AMBIENT.xyz;
				diff = c * _diff.rgb * mainColor.rgb * saturate(dot(normalDir,lightDir));

				fixed3 result = ambient + diff + specular;

				return fixed4(result,1);
				
			}

			ENDCG
		}
	}

}
