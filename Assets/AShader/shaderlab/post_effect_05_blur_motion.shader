//后处理 bloom 效果
Shader "zxy/post_effect_05_blur_motion"
{
	Properties
	{
		_MainTex ("MainTex",2D) = "white" {}
		_BlurAmount ("BlurAmount",Range(0,1)) = 0.9
	}
	
	SubShader
	{
		ZTest Always
		ZWrite Off
		Cull Off

		CGINCLUDE

		sampler2D _MainTex;
		float _BlurAmount;

		struct a2v
		{
			float4 vertex : POSITION;
			float2 texcoord : TEXCOORD0;
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

			f.uv = v.texcoord;
			return f;
		}

		float4 frag_rgb(v2f f) : SV_Target
		{
			float3 color = tex2D(_MainTex,f.uv);
			return float4(color,_BlurAmount);
		}

		float4 frag_a(v2f f) : SV_Target
		{
			float4 color = tex2D(_MainTex,f.uv);
			return color;
		}
		
		ENDCG

		Pass
		{
			Blend SrcAlpha OneMinusSrcAlpha
			ColorMask RGB
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag_rgb
			ENDCG
		}

		Pass
		{
			Blend One Zero
			ColorMask A
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag_a
			ENDCG
		}

	}

	FallBack Off
}
