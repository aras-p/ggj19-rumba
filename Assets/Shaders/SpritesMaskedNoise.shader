Shader "Sprites/MaskedNoise"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        [HideInInspector] _RendererColor ("RendererColor", Color) = (1,1,1,1)
        [HideInInspector] _Flip ("Flip", Vector) = (1,1,1,1)
        _NoiseTex ("Noise Texture", 2D) = "gray" {}
        _MaskTex ("Mask Texture", 2D) = "white" {}
        _NoiseTile ("Noise Tile", Float) = 4.0
        _TexSize ("Texture Size", Float) = 128
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
        Blend One OneMinusSrcAlpha

        Pass
        {
        CGPROGRAM
            #pragma vertex SpriteVert
            #pragma fragment frag
            #pragma multi_compile_instancing
            #include "UnitySprites.cginc"

sampler2D _MaskTex;
sampler2D _NoiseTex;
float _NoiseTile;
float _TexSize;

half4 BetterSampleMask(float2 uv)
{
	float d = 0.7 / _TexSize;
	return (
	    tex2D(_MaskTex, uv) +
	    tex2D(_MaskTex, uv + float2(-d,-d)) +
	    tex2D(_MaskTex, uv + float2(-d, d)) +
	    tex2D(_MaskTex, uv + float2( d,-d)) +
	    tex2D(_MaskTex, uv + float2( d, d))) / 5.0;
}

            
fixed4 frag(v2f IN) : SV_Target
{
    fixed4 c = SampleSpriteTexture (IN.texcoord) * IN.color;
    fixed3 noi = tex2D(_NoiseTex, IN.texcoord * _NoiseTile).rgb;
    fixed mask = BetterSampleMask(IN.texcoord).r;
    mask = max(0.4, mask);    
    c.rgb *= lerp(noi, fixed3(1,1,1), mask);
    c.rgb *= c.a;
    return c;
}
            
        ENDCG
        }
    }
}
