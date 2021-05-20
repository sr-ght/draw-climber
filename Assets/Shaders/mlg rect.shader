Shader "z/mlg"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (0,0,0,0)
        _Area ("Area", Vector) = (0,0,0,0)
        _CountLines ("Count Lines", Range(0, 1024)) = 12
        _Speed ("Speed", Range(-10, 10)) = 1.0
        _Offset ("Offset", Range(0, 1)) = 0
        _Scale("Scale", Range(0, 1)) = 1
        _Radius("Radius", Range(0, 1)) = 0
        _Border("Border", Range(0, 1)) = 0
        _HideLines("HideLines", Range(0, 1)) = 0
        _OffsetColor ("Offset Color", Vector) = (0,0,0,0)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Blend SrcAlpha OneMinusSrcAlpha 

        Pass
        {
            CGPROGRAM
            #define Pi 3.14159265359
            #define Pi2 6.28318530718
            #pragma vertex vert
            #pragma fragment frag

            //#pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _CountLines;
            float _Offset;
            float4 _Area;
            float _Quality;
            float _Scale;
            float _Radius;
            float _Border;
            float _HideLines;
            fixed4 _Color;
            float2 _OffsetColor;
            float _Speed;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            float udRoundRect(float2 p, float2 b, float r)
            {
	            return length(max(abs(p) - b, 0.0)) - r;
            }

            float GetSmoothRectAlpha(float2 fragCoord, float2 iResolution, float radius)
            {
                fragCoord *= 4096;
                iResolution *= 4096;

                // Rect center and half size
                float2 center = iResolution.xy * 0.5;
                float2 hsize = iResolution.xy;

                // 
                hsize *= 0.5;
                radius = radius * min(iResolution.x, iResolution.y) * 0.5;
                radius = max(abs(radius), 0.0);
    
                // Mix content with background using rounded rectangle
	            float a = clamp(udRoundRect(fragCoord - center, hsize - radius, radius), 0.0, 1.0);
                return a;
            }

            fixed4 frag(v2f i) : SV_Target
            {   
                float ratio = _Area.x / _Area.y;
                float area = ratio > 1 ? _Area / _Area.x : _Area / _Area.y;

                fixed2 uv =  (i.uv - 0.5) / _Scale + 0.5;
                fixed2 c_uv = (uv - 0.5) * _Area;
                fixed4 col = tex2D(_MainTex, c_uv);
               
                float angle = atan2(c_uv.y * (_Speed > 0 ? 1 : -1), -c_uv.x) / Pi2 + 0.5 + (_CountLines > 0.1 ? _Offset / (_CountLines) : _Offset);
                float isLine = 1 - round(fmod((angle + _Time.x * abs(_Speed)) * _CountLines, 1.0)) * _HideLines;
                

                float alpha0 = GetSmoothRectAlpha(uv * _Area, _Area, _Radius);
                float alpha1 = GetSmoothRectAlpha(((uv - 0.5) / (1 - _Border) + 0.5) * _Area, _Area, _Radius);
                float alpha = abs(alpha1 - alpha0);

                /*
                float kf = 1; // (1 - r * 2);
                col.r = 1 - isLine * clamp(abs(fmod(angle * 6.0, 6.0) - 3.0) - 1.0, 0.0, 1.0) * kf;
                col.g = 1 - isLine * clamp(abs(fmod(angle * 6.0 + 4.0, 6.0) - 3.0) - 1.0, 0.0, 1.0) * kf;
                col.b = 1 - isLine * clamp(abs(fmod(angle * 6.0 + 2.0, 6.0) - 3.0) - 1.0, 0.0, 1.0) * kf;
                */
                float2 rg = fmod(i.uv + _OffsetColor, 2.0);
                rg = abs(rg - 1.0);
                col = float4(rg, 0.5 + 0.5 * sin(_Time.z), 1.0) + _Color;
                col.a = alpha * isLine * _Color.a;

                return col;
            }
            ENDCG
        }
    }
}
