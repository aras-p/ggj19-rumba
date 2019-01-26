using System;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class RoomClearing : MonoBehaviour
{
    public static RoomClearing instance;

    public int rtSize = 128; 
    public Shader pointsShader;

    RenderTexture m_RT;
    Material m_Material;
    ComputeBuffer m_CB;
    Vector2[] m_CBData;

    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        InitializeRT();
    }

    void OnDestroy()
    {
        if (m_CB != null)
            m_CB.Dispose();
        m_CB = null;
    }

    void InitializeRT()
    {
        m_RT = new RenderTexture(rtSize, rtSize, 0, GraphicsFormat.R8_UNorm);
        Graphics.SetRenderTarget(m_RT);
        GL.Clear(true, true, Color.gray);
        
        m_Material = new Material(pointsShader);

        m_CB = new ComputeBuffer(rtSize * rtSize, 8, ComputeBufferType.Default);
        m_CBData = new Vector2[rtSize * rtSize];

        var bounds = Game.instance.levelSprite.bounds;
        Vector2 rayMin = bounds.min;
        var dx = bounds.size.x / rtSize;
        var dy = bounds.size.y / rtSize;
        var rayRad = (dx + dy) * 0.5f;

        var pointCount = 0;
        var rayMask = LayerMask.GetMask("Room");
        for (var iy = 0; iy < rtSize; ++iy)
        {
            for (var ix = 0; ix < rtSize; ++ix)
            {
                var rayPos = rayMin + new Vector2(dx * (ix - 0.5f), dy * (iy - 0.5f));
                if (Physics2D.OverlapCircle(rayPos, rayRad, rayMask))
                {
                    m_CBData[pointCount++] = new Vector2(ix * 1.0f / rtSize, iy * 1.0f / rtSize);
                }
            }
        }
        
        m_Material.SetBuffer("_Points", m_CB);
        DrawPoints(pointCount);
        
        Game.instance.levelSprite.material.SetTexture("_MaskTex", m_RT);
    }

    void DrawPoints(int pointCount)
    {
        Graphics.SetRenderTarget(m_RT);
        m_CB.SetData(m_CBData, 0, 0, pointCount);
        m_Material.SetPass(0);       
        Graphics.DrawProcedural(MeshTopology.Points, pointCount);        
    }

    public void DrawTrace(Vector3 pos)
    {
        var bounds = Game.instance.levelSprite.bounds;
        var pointCount = 0;
        var radius = 0.2f;
        for (var a = 0.0f; a < 360.0f; a += 60)
        {
            Vector2 pointPos;
            pointPos.x = Mathf.InverseLerp(bounds.min.x, bounds.max.x, pos.x + Mathf.Cos(Mathf.Deg2Rad * a) * radius);
            pointPos.y = Mathf.InverseLerp(bounds.min.y, bounds.max.y, pos.y + Mathf.Sin(Mathf.Deg2Rad * a) * radius);
            m_CBData[pointCount++] = pointPos;
        }

        DrawPoints(pointCount);
    }
}
