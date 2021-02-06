Shader "zxyTest/worldTestShader"
{
    Properties
    {
        _diffuse ("diffuse",Color) = (1,1,1,1)
        _specular ("specular",Range(0,256)) = 10
        _gloss ("gloss",Range(0,256)) = 10
    }
    
    SubShader
    {
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Lighting.cginc"
            struct a2v
            {
                fixed4 vertex : POSITION;
                fixed3 normal : NORMAL;
            };

            struct v2f
            {
                fixed4 pos : POSITION;
                //fixed3 color : Color;      
                fixed3 worldPos : TEXCOORD0;
                fixed3 worldNormal : TEXCOORD1;          
            };

            fixed4 _diffuse;
            float _specular;
            float _gloss;

            v2f vert(a2v v)
            {
                
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.worldNormal = normalize(UnityObjectToWorldNormal(v.normal));
                o.worldPos = mul(unity_ObjectToWorld,v.vertex);
                return o;

            }

            fixed4 frag(v2f i) : SV_TARGET
            {
                fixed3 c = _LightColor0.rbg;
                fixed3 l = normalize(UnityWorldSpaceLightDir(i.worldPos));
                fixed3 n = normalize( i.worldNormal);
                
                fixed3 view = normalize(UnityWorldSpaceViewDir(i.worldPos));
                fixed3 halfDir = normalize(view + l);
                
                fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz;
                fixed3 diffuse = c * _diffuse.rgb * saturate(dot(n,l));
                // fixed3 specular = c * _specular * pow(max(0,dot(n,halfDir)),_gloss);
                fixed3 specular = c * _specular * pow(saturate(dot(n,halfDir)),_gloss);
                

                fixed3 result = ambient + diffuse + specular;

                return fixed4(result.rgb,1);
            }

            ENDCG
        }
    }

    Fallback "specular"
}