Shader "Custom/SpriteShadow"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _ShadowColor ("Shadow Color", Color) = (0, 0, 0, 0.5)
        _ShadowOffset ("Shadow Offset", Vector) = (0.1, -0.1, 0, 0)
    }
    SubShader
    {
        Tags {"RenderType"="Opaque" "Queue"="Transparent"}
        LOD 200

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : POSITION;
                float2 uv : TEXCOORD0;
            };

            sampler2D _MainTex;
            float4 _ShadowColor;
            float4 _ShadowOffset;  // Объявляем как float4

            v2f vert(appdata_t v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            half4 frag(v2f i) : SV_Target
            {
                // Смещение для тени
                float2 shadowUV = i.uv + _ShadowOffset.xy;
                float4 color = tex2D(_MainTex, i.uv);
                float4 shadow = tex2D(_MainTex, shadowUV);
                return lerp(color, _ShadowColor, shadow.a);  // Логика для тени
            }
            ENDCG
        }
    }
}