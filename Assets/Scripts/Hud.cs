using UnityEngine;

public class Hud : MonoBehaviour
{
    public float digitSize = 0.05f;
    public Texture2D[] digits;
    public Texture2D timeOutline;
    public Texture2D timeFill;
    public Texture2D startScreen;
    public Texture2D endScreen;
    public float fillRatio = 0.83f;
    public float fillRatio2 = 0.86f;
    
    void OnGUI()
    {
        if (Game.instance.state == Game.State.End)
        {
            GUI.DrawTexture(new Rect(0,0,Screen.width, Screen.height), endScreen);
        }

        var scoreStr = Game.instance.score.ToString();
        var digitPix = Screen.height * digitSize;
        var rect = new Rect();
        for (var i = 0; i < scoreStr.Length; ++i)
        {
            rect.x = digitPix * 0.3f + digitPix * i;
            rect.y = digitPix * 0.3f;
            rect.width = digitPix;
            rect.height = digitPix;
            var idx = scoreStr[i] - '0';
            GUI.DrawTexture(rect, digits[idx]);
        }

        if (Game.instance.state == Game.State.Game)
        {
            var timeLeft = Game.instance.timeRatioLeft;
            var timeTexScale = digitPix / timeOutline.height;

            rect.y = digitPix * 0.3f;
            rect.height = timeOutline.height * timeTexScale;

            rect.x = Screen.width - timeOutline.width * fillRatio * timeTexScale;
            rect.width = timeOutline.width * fillRatio * fillRatio2 * timeTexScale * timeLeft;
            GUI.DrawTexture(rect, timeFill);

            rect.x = Screen.width - timeOutline.width * timeTexScale;
            rect.width = timeOutline.width * timeTexScale;
            GUI.DrawTexture(rect, timeOutline);
        }
    }
}
