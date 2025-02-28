Shader "AniSol"
{
    Properties 
    {
        _diffuse("RGB or RGBA", 2D) = "black" {}
        _spec("_spec", 2D) = "black" {}
        _Normal("_Normal", 2D) = "black" {}
    }

    SubShader 
    {
        Tags
        {
            "Queue"="Geometry"
            "IgnoreProjector"="False"
            "RenderType"="Opaque"
        }

        Cull Off
        ZWrite On
        ZTest LEqual
        ColorMask RGBA
        Fog { }

        CGPROGRAM
        #pragma surface surf BlinnPhongEditor vertex:vert
        #pragma target 2.0

        sampler2D _diffuse;
        sampler2D _spec;
        sampler2D _Normal;

        struct EditorSurfaceOutput 
        {
            half3 Albedo;
            half3 Normal;
            half3 Emission;
            half3 Gloss;
            half Specular;
            half Alpha;
            half4 Custom;
        };

        inline half4 LightingBlinnPhongEditor_PrePass (EditorSurfaceOutput s, half4 light)
        {
            half3 spec = light.a * s.Gloss;
            half4 c;
            c.rgb = (s.Albedo * light.rgb + light.rgb * spec);
            c.a = s.Alpha;
            return c;
        }

        inline half4 LightingBlinnPhongEditor (EditorSurfaceOutput s, half3 lightDir, half3 viewDir, half atten)
        {
            half3 h = normalize(lightDir + viewDir);
            half diff = max(0, dot(lightDir, s.Normal));

            float nh = max(0, dot(s.Normal, h));
            float spec = pow(nh, s.Specular * 128.0);

            half4 res;
            res.rgb = _LightColor0.rgb * diff;
            res.w = spec * Luminance(_LightColor0.rgb);
            res *= atten * 2.0;

            return LightingBlinnPhongEditor_PrePass(s, res);
        }

        struct Input 
        {
            float2 uv_diffuse;
            float2 uv_Normal;
            float2 uv_spec;
        };

        // Fixed vert function with proper initialization
        void vert(inout appdata_full v, out Input o) 
        {
            UNITY_INITIALIZE_OUTPUT(Input, o); // Ensures 'o' is properly initialized
            o.uv_diffuse = v.texcoord.xy;
            o.uv_Normal = v.texcoord.xy;
            o.uv_spec = v.texcoord.xy;
        }

        void surf(Input IN, inout EditorSurfaceOutput o) 
        {
            o.Normal = float3(0.0, 0.0, 1.0);
            o.Alpha = 1.0;
            o.Albedo = 0.0;
            o.Emission = 0.0;
            o.Gloss = 0.0;
            o.Specular = 0.0;
            o.Custom = 0.0;

            float4 Tex2D0 = tex2D(_diffuse, IN.uv_diffuse.xy);
            float4 Tex2DNormal0 = float4(UnpackNormal(tex2D(_Normal, IN.uv_Normal.xy)).xyz, 1.0);
            float4 Tex2D1 = tex2D(_spec, IN.uv_spec.xy);
            float4 Subtract0 = Tex2D0.aaaa - float4(0.5, 0.5, 0.5, 0.5);
            clip(Subtract0);

            o.Albedo = Tex2D0.rgb;
            o.Normal = Tex2DNormal0.xyz;
            o.Specular = Tex2D1.r;
            o.Gloss = Tex2D1.r;
            o.Normal = normalize(o.Normal);
        }
        ENDCG
    }
    Fallback "Diffuse"
}
