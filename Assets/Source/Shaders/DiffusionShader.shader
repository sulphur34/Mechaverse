Shader "Custom/EdgeBlurShader"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}  // Текстура спрайта
        _Color ("Color", Color) = (1,1,1,1)  // Цветовой множитель
        _EdgeBlurRadius ("Edge Blur Radius", Range(0, 1)) = 0.2  // Радиус размытия краев
    }

    SubShader
    {
        Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha  // Прозрачность
        ZWrite Off
        Cull Off
        Lighting Off

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
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            sampler2D _MainTex;  // Текстура спрайта
            float4 _Color;  // Цветовой множитель
            float _EdgeBlurRadius;  // Радиус размытия краев

            v2f vert(appdata_t v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            half4 frag(v2f i) : SV_Target
            {
                // Получаем текстуру с альфа-каналом
                half4 texColor = tex2D(_MainTex, i.uv) * _Color;

                // Рассчитываем расстояние до центра спрайта по UV координатам
                float2 center = float2(0.5, 0.5);
                float dist = distance(i.uv, center);

                // Применяем эффект размытия краев
                float edgeFade = smoothstep(0.5 - _EdgeBlurRadius, 0.5, dist);

                // Умножаем альфа-канал на edgeFade для плавного размытия
                texColor.a *= 1.0 - edgeFade;

                return texColor;
            }
            ENDCG
        }
    }

    FallBack "Transparent/Cutout/VertexLit"
}