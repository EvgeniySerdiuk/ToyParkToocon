using TMPro;
using UnityEngine;

public class FPSCounter : MonoBehaviour
{
    public float updateInterval = 0.5f;
    private float accum = 0.0f;
    private int frames = 0;
    private float timeLeft;
    private TextMeshProUGUI TMPtext;

    private void Awake()
    {
        TMPtext = GetComponent<TextMeshProUGUI>();
    }
    private void Start()
    {
        timeLeft = updateInterval;
    }

    private void Update()
    {
        timeLeft -= Time.deltaTime;
        accum += Time.timeScale / Time.deltaTime;
        frames++;

        if (timeLeft <= 0.0)
        {
            float fps = accum / frames;
            string format = System.String.Format("{0:F2} FPS", fps);
            TMPtext.text = format;

            timeLeft = updateInterval;
            accum = 0.0f;
            frames = 0;
        }
    }
}