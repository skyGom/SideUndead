using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public enum InfoType { Exp, Level, Kill, Time, Health }
    public InfoType Type;

    private Text myText;
    private Slider mySlider;

    private void Awake()
    {
        myText = GetComponent<Text>();
        mySlider = GetComponent<Slider>();
    }

    private void LateUpdate()
    {
        switch (Type)
        {
            case InfoType.Exp:
                float curExp = GameManager.instance.Exp;
                float maxExp = GameManager.instance.NextExp[GameManager.instance.level];

                mySlider.value = curExp  / maxExp;
                break;
            case InfoType.Level:
                myText.text = string.Format("Lv.{0:F0}", GameManager.instance.level);
                break;
            case InfoType.Kill:
                myText.text = string.Format("{0:F0}", GameManager.instance.Kill);
                break;
            case InfoType.Time:
                float remainTime = GameManager.instance.MaxGameTime - GameManager.instance.GameTime;
                int min = Mathf.FloorToInt(remainTime / 60);
                int sec = Mathf.FloorToInt(remainTime % 60);

                myText.text = string.Format("{0:D2}:{1:D2}", min, sec);
                break;
            case InfoType.Health:
                float curHealth = GameManager.instance.health;
                float maxHealth = GameManager.instance.maxHealth;

                mySlider.value = curHealth / maxHealth;
                break;
        }
    }
}
