Shader "zxy/test3"
{
	Properties
	{
		_Color ("颜色",Color) = (1,1,1,1)
		_Vector ("xx",Vector) = (0,0,0,1)
	}

	SubShader
	{
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct a2v
			{
				float4 pos : POSITION;
				float4 color : Color;
			};

			float4 _Color;
			float4 _Vector;
			struct v2f
			{
				float4 pos : SV_POSITION;
				float4 vPos : TEXCOORD0;
			};

			v2f vert(a2v a) : POSITION
			{
				v2f f;
				f.pos =  UnityObjectToClipPos( a.pos );
				f.vPos = ComputeScreenPos(f.pos);
				return f;
			}

			float4 frag(v2f f) : SV_Target
			{
				float2 xy = f.vPos.xy / f.vPos.w;
				//float x = f.pos.x;
				//float y = f.pos.y;
				//float z = f.pos.z;
				//float width = _ScreenParams.x;
				//float height = _ScreenParams.y;
				return float4(xy,0,1) * _Color.r;
			}

			ENDCG
		}
	}
}