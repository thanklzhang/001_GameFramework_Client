Shader "zxy/test1"
{
	Properties
	{
		_Color ("颜色",Color) = (1,1,1,1)
	}

	SubShader
	{
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			fixed4 _Color;

			struct a2v
			{
				fixed4 pos : POSITION;
				fixed4 color : Color;
			};

			struct v2f
			{
				fixed4 pos : POSITION;
				fixed4 color : Color;
			};

			v2f vert(a2v v)
			{
				fixed4 resultPos = UnityObjectToClipPos(v.pos);
				
				v2f f;
				f.pos = resultPos;
				f.color = v.color;
				return f;
			}

			fixed4 frag(v2f f) : SV_Target
			{
				fixed d = cos(_Time.y * 4) *0.5 + 0.5;
				//return f.color * _Color * d;
				return fixed4(d,0,0,1);
			}

			ENDCG
		}
	}
}