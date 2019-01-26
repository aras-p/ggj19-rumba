using UnityEngine;

public class Hud : MonoBehaviour
{
    public float digitSize = 0.05f;
    public Texture2D[] digits;
    
    void OnGUI()
    {
        var scoreStr = Game.instance.score.ToString();
        var digitPix = Screen.height * digitSize;
        var rect = new Rect();
        for (var i = 0; i < scoreStr.Length; ++i)
        {
            rect.x = digitPix * 0.3f + digitPix * i;
            rect.y = digitPix * 0.3f;
            rect.width = digitPix;
            rect.height = digitPix;
            var idx = (int)(scoreStr[i] - '0');
            GUI.DrawTexture(rect, digits[idx]);
        }
    }
}
