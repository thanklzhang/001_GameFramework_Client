//后处理 高斯模糊
Shader "zxy/post_effect_03_gassia_blur"
{
	Properties
	{
		_MainTex ("MainTex",2D) = "white" {}
		_BlurSize ("BlurSize",Float) = 0.6
	}
	
	SubShader
	{
		ZTest Always
		ZWrite Off
		Cull Off

		CGINCLUDE

		sampler2D _MainTex;
		float4 _MainTex_TexelSize;
		float _BlurSize;



		struct a2v
		{
			float4 vertex : POSITION;
			float2 texcoord : TEXCOORD0;
		};

		struct v2f
		{
			float4 pos : SV_POSITION;
			float2 uv[5] : TEXCOORD0;
		};

		//垂直
		v2f vert_vertical(a2v v)
		{
			v2f f;
			f.pos = UnityObjectToClipPos(v.vertex);

			f.uv[0] = v.texcoord;
			f.uv[1] = v.texcoord + float2(0,_MainTex_TexelSize.y) * _BlurSize;
			f.uv[2] = v.texcoord - float2(0,_MainTex_TexelSize.y) * _BlurSize;
			f.uv[3] = v.texcoord + float2(0,_MainTex_TexelSize.y * 2) * _BlurSize;
			f.uv[4] = v.texcoord - float2(0,_MainTex_TexelSize.y * 2) * _BlurSize;
			return f;
		}

		//水平
		v2f vert_horizontal(a2v v)
		{
			v2f f;
			f.pos = UnityObjectToClipPos(v.vertex);

			f.uv[0] = v.texcoord;
			f.uv[1] = v.texcoord + float2(_MainTex_TexelSize.x,0) * _BlurSize;
			f.uv[2] = v.texcoord - float2(_MainTex_TexelSize.x	,0) * _BlurSize;
			f.uv[3] = v.texcoord + float2(_MainTex_TexelSize.x * 2,0) * _BlurSize;
			f.uv[4] = v.texcoord - float2(_MainTex_TexelSize.x * 2,0) * _BlurSize;
			return f;
		}

		float4 frag(v2f f) : SV_Target
		{
			float3 sum = float3(0,0,0);
			float weight[3] = {0.4026,0.2442,0.0545};
			sum += tex2D(_MainTex,f.uv[0]) * weight[0];

			//for(int i = 1;i < 3;++i)
			//{
			//	sum += tex2D(_MainTex,f.uv[2 * i - 1]) * weight[i];
			//	sum += tex2D(_MainTex,f.uv[2 * i]) * weight[i];
			//}
			sum += tex2D(_MainTex,f.uv[1]) * weight[1];
			sum += tex2D(_MainTex,f.uv[2]) * weight[1];
			sum += tex2D(_MainTex,f.uv[3]) * weight[2];
			sum += tex2D(_MainTex,f.uv[4]) * weight[2];

			return float4(sum,1);
		}
		ENDCG



		Pass
		{
			Name "GAUSSIA_BLUR_VERTICAL"
			CGPROGRAM
			#pragma vertex vert_vertical
			#pragma fragment frag
			ENDCG
		}

		Pass
		{
			Name "GAUSSIA_BLUR_HORIZONTAL"
			CGPROGRAM
			#pragma vertex vert_horizontal
			#pragma fragment frag
			ENDCG
		}

		

	}

	FallBack Off
}
