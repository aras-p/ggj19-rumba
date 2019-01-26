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
            
fixed4 frag(v2f IN) : SV_Target
{
    fixed4 c = SampleSpriteTexture (IN.texcoord) * IN.color;
    fixed3 noi = tex2D(_NoiseTex, IN.texcoord * _NoiseTile).rgb;
    fixed mask = tex2D(_MaskTex, IN.texcoord).r;
    //mask = 0;
    c.rgb *= lerp(noi, fixed3(1,1,1), mask);
    c.rgb *= c.a;
    return c;
}
            
        ENDCG
        }
    }
}
