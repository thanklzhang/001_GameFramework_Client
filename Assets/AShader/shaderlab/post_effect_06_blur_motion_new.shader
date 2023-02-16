//后处理 bloom 效果
Shader "zxy/post_effect_06_blur_motion_new"
{
	Properties
	{
		_MainTex ("MainTex",2D) = "white" {}
		_BlurAmount ("BlurAmount",Range(0,1)) = 0.9
		_Iteration ("Iteration",Range(1,10)) = 1
	}
	
	SubShader
	{
		ZTest Always
		ZWrite Off
		Cull Off

		CGINCLUDE

		sampler2D _MainTex;
		float2 _MainTex_TexelSize;
		sampler2D _CameraDepthTexture;
		float4x4 _PreToViewProjection_Matrix;
		float4x4 _CurrToViewProjection_Inverse_Matrix;
		float _BlurAmount;
		int _Iteration;

		struct a2v
		{
			float4 vertex : POSITION;
			float2 texcoord : TEXCOORD0;
		};

		struct v2f
		{
			float4 pos : SV_POSITION;
			float2 uv : TEXCOORD0;
			float2 uv_depth : TEXCOORD1;
		};

	
		v2f vert(a2v v)
		{
			v2f f;
			f.pos = UnityObjectToClipPos(v.vertex);

			f.uv = v.texcoord;
			f.uv_depth = v.texcoord;

			#if UNITY_UV_STARTS_AT_TOP
			if(_MainTex_TexelSize.y < 0)
			{
				f.uv_depth.y = 1 - f.uv_depth.y;
			}
			#endif

			return f;
		}

		float4 frag(v2f f) : SV_Target
		{

			float d = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture,f.uv_depth);

			//to ndc
			float x = f.uv.x * 2 - 1;
			float y = f.uv.y * 2 - 1;
			float z = d * 2 - 1;

			float4 P = mul(_CurrToViewProjection_Inverse_Matrix,float4(x,y,z,1));
			float4 worldPos = P / P.w;

			float4 preProjectionPos = mul(_PreToViewProjection_Matrix,worldPos);
			float4 preNDCPos = preProjectionPos / preProjectionPos.w;

			float2 currPos = float2(x,y);
			float2 prePos = preNDCPos.xy;

			float2 velocity = (currPos - prePos) / 2.0f;

			float2 uv = f.uv;
			float3 c = float3(0,0,0);
			for(int i = 0;i < _Iteration;++i)
			{
				float3 color = tex2D(_MainTex,uv);

				c += color;

				uv += velocity * _BlurAmount;
			}

			c /= _Iteration;
			
			return float4(c,1);
		}

	
		ENDCG

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			ENDCG
		}

	}

	FallBack Off
}
