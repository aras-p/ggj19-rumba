Shader "Unlit/RenderPoints"
{
    SubShader
    {
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma require compute
            
            StructuredBuffer<float2> _Points;

            float4 vert (uint vid : SV_VertexID) : SV_POSITION
            {
                float4 p;
                p.x = _Points[vid].x * 2.0f - 1.0f;
                p.y = (1.0f - _Points[vid].y) * 2.0f - 1.0f;
                p.z = 0.5f;
                p.w = 1.0f;
                return p;
            }

            fixed4 frag () : SV_Target
            {
                return 1;
            }
            ENDCG
        }
    }
}
