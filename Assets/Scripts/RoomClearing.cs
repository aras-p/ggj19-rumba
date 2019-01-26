using System;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class RoomClearing : MonoBehaviour
{
    public static RoomClearing instance;

    public int texSize = 128; 
    Texture2D m_Texture;
    NativeArray<byte> m_Data;

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
    }

    void InitializeRT()
    {
        m_Texture = new Texture2D(texSize, texSize, GraphicsFormat.R8_UNorm, TextureCreationFlags.None);
        m_Data = m_Texture.GetRawTextureData<byte>();
        for (var i = 0; i < texSize * texSize; ++i)
            m_Data[i] = 0;
        
        var bounds = Game.instance.levelSprite.bounds;
        Vector2 rayMin = bounds.min;
        var dx = bounds.size.x / texSize;
        var dy = bounds.size.y / texSize;
        var rayRad = (dx + dy) * 0.5f;

        var rayMask = LayerMask.GetMask("Room");
        for (var iy = 0; iy < texSize; ++iy)
        {
            for (var ix = 0; ix < texSize; ++ix)
            {
                var rayPos = rayMin + new Vector2(dx * (ix + 0.5f), dy * (iy + 0.5f));
                if (Physics2D.OverlapCircle(rayPos, rayRad, rayMask))
                {
                    m_Data[iy * texSize + ix] = 255;
                }
            }
        }
        
        m_Texture.Apply();
        
        Game.instance.levelSprite.material.SetTexture("_MaskTex", m_Texture);
    }

    public void DrawTrace(Vector3 pos)
    {
        var bounds = Game.instance.levelSprite.bounds;
        var radius = 0.2f;
        for (var a = 0.0f; a < 360.0f; a += 40)
        {
            Vector2 pointPos;
            pointPos.x = Mathf.InverseLerp(bounds.min.x, bounds.max.x, pos.x + Mathf.Cos(Mathf.Deg2Rad * a) * radius);
            pointPos.y = Mathf.InverseLerp(bounds.min.y, bounds.max.y, pos.y + Mathf.Sin(Mathf.Deg2Rad * a) * radius);
            var ix = (int)(pointPos.x * texSize);
            var iy = (int)(pointPos.y * texSize);
            if (ix >= 0 && ix >= 0 && ix < texSize && iy < texSize)
            {
                var idx = iy * texSize + ix;
                if (m_Data[idx] == 0)
                    ++Game.instance.score;
                m_Data[idx] = 255;
            }
        }
        m_Texture.Apply();
    }
}
