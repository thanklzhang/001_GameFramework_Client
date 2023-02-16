// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

//非真实渲染 消融效果
Shader "zxy/noise_burn_01"
{
	Properties
	{
		_BurnAmount ("BurnAmount",float) = 0
		_BurnWidth ("BurnWidth",float) = 0.1

		_FirstColor ("FirstColor",Color) = (1,1,1,1)
		_SecondColor ("SecondColor",Color) = (1,1,1,1)
		
		_MainTex ("MainTex",2D) = "white" {}
		_BumpTex ("BumpTe",2D) = "white" {}
		_BurnTex ("BurnTex",2D) = "white" {}

	}

	SubShader
	{
		//Tags { "RenderType" = "Opaque" "Queue" = "Geometry"}

		

		CGINCLUDE
		
		//#include "UnityCG.cginc"
		#include "Lighting.cginc"
		#include "AutoLight.cginc"

		struct a2v
		{
			float4 vertex : POSITION;
			float3 normal : NORMAL;
			float4 tangent : TANGENT;
			float2 texcoord : TEXCOORD0;

		};


		float _BurnAmount;
		float _BurnWidth;
		float4 _FirstColor;
		float4 _SecondColor;

		sampler2D _MainTex;
		sampler2D _BumpTex;
		sampler2D _BurnTex;

		float4 _MainTex_ST;
		float4 _BumpTex_ST;
		float4 _BurnTex_ST;
	

		struct v2f
		{
			float4 pos : SV_POSITION;
			float2 main_uv : TEXCOORD0;
			float2 bump_uv : TEXCOORD1;
			float2 burn_uv : TEXCOORD2;
			float3 tan_light_dir : TEXCOORD3;
			float3 worldPos : TEXCOORD4;


			SHADOW_COORDS(5)
		};

		v2f vert(a2v v)
		{
			v2f f;
			
			f.pos = UnityObjectToClipPos(v.vertex);

			f.main_uv = TRANSFORM_TEX(v.texcoord,_MainTex);
			f.bump_uv = TRANSFORM_TEX(v.texcoord,_BumpTex);
			f.burn_uv = TRANSFORM_TEX(v.texcoord,_BurnTex);
			
			TANGENT_SPACE_ROTATION;
			f.tan_light_dir = mul(rotation,ObjSpaceLightDir(v.vertex));
			f.worldPos = mul(unity_ObjectToWorld,v.vertex);
			TRANSFER_SHADOW(f);

			return f;
		}

		float4 frag(v2f f) : SV_TARGET
		{
			float3 burnTexColor = tex2D(_BurnTex,f.burn_uv);
			clip(burnTexColor.r - _BurnAmount);

			float3 mainTexColor = tex2D(_MainTex,f.main_uv);

			float4 bumpTexColor = tex2D(_BumpTex,f.bump_uv);
			float3 bumpNormal_In_Tan = UnpackNormal(bumpTexColor);

			float3 light_dir_in_tan = normalize(f.tan_light_dir);

			float3 albedo = mainTexColor;
			float3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz * albedo;
			float3 diffuse = _LightColor0 * albedo * max(0,dot(bumpNormal_In_Tan,light_dir_in_tan));
			

			//燃烧宽度的定位因子
			float factor = burnTexColor.r - _BurnAmount;

			//在宽度内进行插值取值 不在的话 返回 0 1
			float t = smoothstep(0,_BurnWidth,factor);

			//由于 _FirstColor 是显示中开始燃烧的颜色 也就是在正常纹理 处
			//而 _SecondColor 则表示是显示中将要消失处的颜色 所以这里反过来取值 也可以换成将上面的 t 进行 1 - t 操作
			float3 burnColor = lerp(_SecondColor,_FirstColor, t);

			UNITY_LIGHT_ATTENUATION(atten,f,f.worldPos)

			float3 mainColor = ambient + diffuse * atten;
			float3 resultColor = lerp(burnColor,mainColor,t);

			return float4(resultColor,1);
		}
		
		ENDCG

		Pass
		{
			Tags {"LightMode"="ForwardBase"}

			Cull Off
			
			CGPROGRAM
			
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fwdbase
			ENDCG
		}


	}

	FallBack "Diffuse"
	
}