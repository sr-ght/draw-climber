Shader "z/ChromaKey rect"
{
	Properties{
		_MainTex("MainTex", 2D) = "white" {}
		_KeyColor("KeyColor", Color) = (1,1,1,1)
		_DChroma("D Chroma", range(0.0, 1.0)) = 0.5
		_DChromaT("D Chroma Tolerance", range(0.0, 1.0)) = 0.05
        _Area ("Area", Vector) = (0,0,0,0)
        _Radius("Radius", Range(0, 1)) = 0
	}
	CGINCLUDE
	#include "UnityCG.cginc"
	struct VS_OUT
	{
		half4 position:POSITION;
		fixed2 texcoord0:TEXCOORD0;
	};

	sampler2D _MainTex;
	fixed4 _MainTex_ST;
	
	fixed4 _KeyColor;
	fixed _DChroma;
	fixed _DChromaT;
	float4 _Area;
	float _Radius;

	VS_OUT vert(appdata_base input)
	{
		VS_OUT o;
		o.position = UnityObjectToClipPos(input.vertex);
		o.texcoord0 = TRANSFORM_TEX(input.texcoord, _MainTex);
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

		float2 center = iResolution.xy * 0.5;
		float2 hsize = iResolution.xy;

		hsize *= 0.5;
		radius = radius * min(iResolution.x, iResolution.y) * 0.5;
		radius = max(abs(radius), 0.0);
    
		float a = clamp(udRoundRect(fragCoord - center, hsize - radius, radius), 0.0, 1.0);
		return a;
	}

	fixed3 RGB_To_YCbCr(fixed3 rgb)
	{
		fixed Y = 0.299 * rgb.r + 0.587 * rgb.g + 0.114 * rgb.b;
		fixed Cb = 0.564 * (rgb.b - Y);
		fixed Cr = 0.713 * (rgb.r - Y);
		return fixed3(Cb, Cr, Y);
	}
	
	fixed4 frag(VS_OUT input) : SV_Target
	{
		fixed4 c = tex2D(_MainTex, input.texcoord0);

		fixed2 uv = input.texcoord0;
		c.a = 1.0 - GetSmoothRectAlpha(uv * _Area, _Area, _Radius);
		
		if (c.a > 0) {
			fixed3 src_YCbCr = RGB_To_YCbCr(c.rgb);
			fixed3 key_YCbCr = RGB_To_YCbCr(_KeyColor);

			fixed dChroma = distance(src_YCbCr.xy, key_YCbCr.xy);

			if (dChroma < _DChroma) {
				fixed a = 0;
				if (dChroma > _DChroma - _DChromaT) {
					a = (dChroma -_DChroma + _DChromaT) / _DChromaT;
				}
				if(c.a > a) {
					c.a = a;
				}
			}
		}

		return c;
	}
	ENDCG
	
	SubShader
	{
		Tags{ "Queue" = "Transparent" "RenderType" = "Transparent" "IgnoreProjector" = "True" }
		Lighting Off
		ZWrite Off
		AlphaTest Off
		Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
			  #pragma vertex vert
			  #pragma fragment frag
			ENDCG
		}
	}
	Fallback Off
}