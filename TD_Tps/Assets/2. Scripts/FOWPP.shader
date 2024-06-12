Shader "Custom/FOWPP"
{
    Properties
    {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _FogTex ("Fog Texture", 2D) = "black" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        CGPROGRAM
        #pragma surface surf Lambert

        sampler2D _MainTex;
        sampler2D _FogTex;

        struct Input
        {
            float2 uv_MainTex;
        };

        void surf (Input IN, inout SurfaceOutput o)
        {
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
            fixed4 fog = tex2D(_FogTex, IN.uv_MainTex);
            o.Albedo = c.rgb * fog.rgb;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
