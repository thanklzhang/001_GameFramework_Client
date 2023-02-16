//顶点操作 帧动画
Shader "zxy/test_vertex_01_frame_ani"
{
	Properties
	{
		_MainTex ("MainTex",2D) = "white" {}
		_Color ("Color",Color) = (1,1,1,1)
		_Speed ("Speed",float) = 10

		_X_Amount ("X_Amount",float) = 4
		_Y_Amount ("Y_Amount",float) = 4
	}

	SubShader
	{
		Tags {"Queue" = "Transparent" "IgnoreProjector"="True" "RenderType" = "Transparent"}
		Pass
		{
			Tags {"LightMode" = "ForwardBase"}
			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float4 _Color;
			float _Speed;
			float _X_Amount;
			float _Y_Amount;

			struct a2v
			{
				float4 vertex : POSITION;
				float2 coord : TEXCOORD0;
			};

			struct v2f
			{
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
			};

			v2f vert(a2v v)
			{
				v2f f;
				f.pos = UnityObjectToClipPos(v.vertex);
				f.uv = TRANSFORM_TEX(v.coord,_MainTex);

				return f;
			}

			float4 frag(v2f f) : SV_Target
			{

				//0 开始
				float index = floor(_Time.y * _Speed) % (_X_Amount * _Y_Amount);

				float row = (_Y_Amount - 1) - floor(index / _X_Amount);
				float col = index - row * 4;

				f.uv.x = f.uv.x / _X_Amount;
				f.uv.y = f.uv.y / _Y_Amount;

				//float4  albedo = tex2D(_MainTex,f.uv + float2(0,_Time.y)) * _Color;
				float2 resultUV = float2(col * (1 / _X_Amount) + f.uv.x,row * (1 / _Y_Amount) + f.uv.y);

				float4  albedo = tex2D(_MainTex,resultUV) * _Color;
				 

				float4 result = albedo;
				return result;
			}

			ENDCG

		}

	}

	FallBack "VertexLit"
}