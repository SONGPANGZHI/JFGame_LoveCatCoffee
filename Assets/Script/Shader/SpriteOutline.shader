Shader "Custom/SpriteOutline"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        _OutlineColor ("Outline Color", Color) = (1,1,1,1)
        _OutlineWidth ("Outline Width", Range(0, 4)) = 2  // 修改为0.5
        [MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
    }

    SubShader
    {
        Tags
        { 
            "Queue"="Transparent" 
            "IgnoreProjector"="True" 
            "RenderType"="Transparent" 
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile _ PIXELSNAP_ON
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex   : SV_POSITION;
                fixed4 color    : COLOR;
                float2 texcoord  : TEXCOORD0;
            };

            fixed4 _Color;
            fixed4 _OutlineColor;
            float _OutlineWidth;
            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _MainTex_TexelSize;

            v2f vert(appdata_t IN)
            {
                v2f OUT;
                OUT.vertex = UnityObjectToClipPos(IN.vertex);
                OUT.texcoord = IN.texcoord;
                OUT.color = IN.color * _Color;
                #ifdef PIXELSNAP_ON
                OUT.vertex = UnityPixelSnap (OUT.vertex);
                #endif
                return OUT;
            }

            fixed4 SampleSpriteTexture(float2 uv)
            {
                fixed4 color = tex2D(_MainTex, uv);
                #if UNITY_TEXTURE_ALPHASPLIT_ALLOWED
                if (_AlphaSplitEnabled)
                    color.a = tex2D(_AlphaTex, uv).r;
                #endif
                return color;
            }

            fixed4 frag(v2f IN) : SV_Target
            {
                fixed4 c = SampleSpriteTexture(IN.texcoord) * IN.color;
                
                // 如果当前像素已经是不透明的，直接返回
                if (c.a > 0.99) return c;
                
                // 8方向采样检测更大的描边范围
                float outline = 0;
                float2 pixelSize = _MainTex_TexelSize.xy * _OutlineWidth;
                
                outline += SampleSpriteTexture(IN.texcoord + float2(0, 1) * pixelSize).a;
                outline += SampleSpriteTexture(IN.texcoord + float2(0, -1) * pixelSize).a;
                outline += SampleSpriteTexture(IN.texcoord + float2(1, 0) * pixelSize).a;
                outline += SampleSpriteTexture(IN.texcoord + float2(-1, 0) * pixelSize).a;
                outline += SampleSpriteTexture(IN.texcoord + float2(1, 1) * pixelSize).a;
                outline += SampleSpriteTexture(IN.texcoord + float2(-1, -1) * pixelSize).a;
                outline += SampleSpriteTexture(IN.texcoord + float2(1, -1) * pixelSize).a;
                outline += SampleSpriteTexture(IN.texcoord + float2(-1, 1) * pixelSize).a;
                
                // 如果周围有任何不透明像素，则应用描边
                if (outline > 0.1 && c.a < 0.5)
                {
                    // 混合描边颜色和原始颜色(根据透明度)
                    return lerp(_OutlineColor, c, c.a);
                }
                
                return c;
            }
            ENDCG
        }
    }
}