using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen 
{
    private GameObject loadingScreen;
    private Image loadingSlider;

    public LoadingScreen(GameObject loadingScreen, Image loadingSlider) 
    {
        this.loadingScreen = loadingScreen;
        this.loadingSlider = loadingSlider;
        ConfigureSlider();
    }

    private void ConfigureSlider()
    {
        loadingSlider.type = Image.Type.Filled;
        loadingSlider.fillMethod = Image.FillMethod.Horizontal;
        loadingSlider.fillOrigin = 0;
    }

    public void SetScreenActive(bool value)
    {
        loadingScreen.SetActive(value);
        if(value)
            loadingSlider.fillAmount = 0;
    }

    public void UpdateSliderValue(float value)
    {
        AddSliderValue(value);
    }
    
    public void SetSliderValue(float value)
    {
        AddSliderValue(-loadingSlider.fillAmount + value);
    }

    private void AddSliderValue(float addValue)
    {
        if (loadingSlider.fillAmount + addValue > 1)
        {
            loadingSlider.fillAmount = 0.95f;
            return;
        }
        loadingSlider.fillAmount += addValue;
    }
}
