// Simplified Diffuse shader. Differences from regular Diffuse one:
// - no Main Color
// - fully supports only 1 directional light. Other lights can affect it, but it will be per-vertex/SH.

Shader "Luke/Ramp(Lightmap)" {
Properties {
	_MainTex ("Base (RGB)", 2D) = "white" {}
	_RampTex("Ramp (RGB)", 2D) = "white" {}

	_Multiplier("Multiplier (float)", Range (0.00,1.00)) = 0.5
}
SubShader{
	Tags { "RenderType" = "Opaque" }
	LOD 150

CGPROGRAM
#pragma surface surf Ramp noforwardadd

sampler2D _RampTex;
float _Multiplier;

half4 LightingRamp(SurfaceOutput s, half3 lightDir, half atten) {
	half NdotL = dot(s.Normal, lightDir);
	half diff = NdotL * 0.5 + 0.5;
	half3 ramp = tex2D(_RampTex, float2(diff,0.0)).rgb;
	half4 c;
	c.rgb = s.Albedo * _LightColor0.rgb * ramp * atten;
	c.a = s.Alpha;
	return c;
}

inline fixed4 LightingRamp_SingleLightmap(SurfaceOutput s, fixed4 color) {

	half3 lm = DecodeLightmap(color);

	//float len = length(lm) * _Multiplier;

	half r = tex2D(_RampTex, float2(lm.r * _Multiplier, 0.0)).r;
	half g = tex2D(_RampTex, float2(lm.g * _Multiplier, 0.0)).g;
	half b = tex2D(_RampTex, float2(lm.b * _Multiplier, 0.0)).b;

	half3 ramp = half3(r, g, b);

	return fixed4(ramp * color, 1);
}


struct Input {
	float2 uv_MainTex;
};

sampler2D _MainTex;

void surf (Input IN, inout SurfaceOutput o) {
	o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb;
}

ENDCG
}

Fallback "Mobile/VertexLit"
}
