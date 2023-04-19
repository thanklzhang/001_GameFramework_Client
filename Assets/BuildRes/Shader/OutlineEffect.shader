Shader "MyShader/OutlineEffect" {
    Properties {
        _OutlineWidth("Outline Width", Range(0, 100)) = 8
        _StartTime ("startTime", Float) = 0 // _StartTime���ڿ���ÿ��ѡ�еĶ�����ɫ���䲻ͬ��
        _OutlineColor ("OutlineColor",Color) = (1,1,1,1)
    }
 
    SubShader {
        Tags {
            // ��Ⱦ����: Background(1000, ��̨)��Geometry(2000, ������, Ĭ��)��Transparent(3000, ͸��)��Overlay(4000, ����)
            "Queue" = "Transparent+110"
            "RenderType" = "Transparent"
            "DisableBatching" = "True"
        }
 
        // ��������������Ļ�������ض�Ӧ��ģ��ֵ���Ϊ1
        Pass {
			Cull Off // �ر��޳���Ⱦ, ȡֵ��: Off��Front��Back, Off��ʾ����ͱ��涼��Ⱦ
			ZTest Always // ����ͨ����Ȳ���, ʹ�����弴ʹ���ڵ�ʱ, Ҳ������ģ��
			ZWrite Off // �ر���Ȼ���, ����������ڵ�ǰ�������
			ColorMask 0 // ����ͨ������ɫͨ��, ȡֵ��: 0��R��G��B��A��RGBA�����(RG��RGB��), 0��ʾ����Ⱦ��ɫ
 
			Stencil { // ģ�����, ֻ��ͨ��ģ����Ե����زŻ���Ⱦ
				Ref 1 // �趨�ο�ֵΪ1
				Pass Replace // ���ͨ��ģ�����, �����ص�ģ��ֵ����Ϊ�ο�ֵ(1), ģ��ֵ�ĳ�ֵΪ0, û��Comp��ʾ����ͨ��ģ�����
			}
		}
 
        // ����ģ���������������, �����͵��⻷�ϵ�����
        Pass {
            Cull Off // �ر��޳���Ⱦ, ȡֵ��: Off��Front��Back, Off��ʾ����ͱ��涼��Ⱦ
            ZTest Always // ����ͨ����Ȳ���, ʹ�����弴ʹ���ڵ�ʱ, Ҳ���������
            ZWrite Off // �ر���Ȼ���, ����������ڵ�ǰ�������
            Blend SrcAlpha OneMinusSrcAlpha // ��ϲ���, �뱳���������ɫ���
            ColorMask RGB // ����ͨ������ɫͨ��, ȡֵ��: 0��R��G��B��A��RGBA�����(RG��RGB��), 0��ʾ����Ⱦ��ɫ
 
            Stencil { // ģ�����, ֻ��ͨ��ģ����Ե����زŻ���Ⱦ
                Ref 1 // �趨�ο�ֵΪ1
                Comp NotEqual // ����ֻ��ģ��ֵΪ0�����زŻ�ͨ������, ��ֻ�����͵��⻷�ϵ�������ͨ��ģ�����
            }
 
            CGPROGRAM
            #include "UnityCG.cginc"
 
            #pragma vertex vert
            #pragma fragment frag
 
            uniform float _OutlineWidth;
            uniform float _StartTime;
            float4 _OutlineColor;
   
            struct appdata {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float3 smoothNormal : TEXCOORD3; // ƽ���ķ���, ����ͬ��������з���ȡƽ��ֵ
            };
 
            struct v2f {
                float4 position : SV_POSITION;
            };
 
            v2f vert(appdata input) {
                v2f output;
                //float3 normal = any(input.smoothNormal) ? input.smoothNormal : input.normal; // �⻬�ķ���
                float3 normal = input.normal;
                float3 viewNormal = normalize(mul((float3x3)UNITY_MATRIX_IT_MV, normal)); // �۲�����ϵ�µķ�������
                float3 viewPos = UnityObjectToViewPos(input.vertex); // �۲�����ϵ�µĶ�������
                // �ü�����ϵ�µĶ�������, �������������ŷ��߷�����������, ����Ĳ��־�����߲���
                // ����(-viewPos.z)��Ϊ�˵���͸�ӱ任��ɵ���߿�Ƚ���ԶСЧ��, ʹ���������۾��������Զ, ��߿�ȶ��������仯
                // ����1000��Ϊ�˽���߿�ȵ�λת����1mm(����Ŀ������������ϵ�еĿ��, ��������Ļ�ϵĿ��)
                output.position = UnityViewToClipPos(viewPos + viewNormal * _OutlineWidth * (-viewPos.z) / 1000);
                return output;
            }
 
            fixed4 frag(v2f input) : SV_Target {
    //            float t1 = sin(_Time.z - _StartTime); // _Time = float4(t/20, t, t*2, t*3)
				//float t2 = cos(_Time.z - _StartTime);
				//// �����ɫ��ʱ��仯, ���͸������ʱ��仯, �Ӿ��ϸо���������ͺ�����
				//return float4(t1 + 1, t2 + 1, 1 - t1, 1 - t2);

                float4 c = _OutlineColor;
                return c;
            }
 
            ENDCG
        }
    }
}
