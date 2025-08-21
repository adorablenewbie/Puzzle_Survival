using System;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [Range(0.0f, 1.0f)]
    public float time;
    public float fullDayLength; // 전체 하루 길이(초 단위) (ex. 30이면 하루가 30초)
    public float startTime = 0.4f; // 시작 시간 
    private float timeRate;
    public Vector3 noon; // Vector 90 0 0 
    public float colorChangeSpeed = 0.5f;

    private bool isNight;
    private bool isDay;
    private bool wasNight;
    private bool wasDay;

    [Header("Sun")]
    public Light sun;
    public Gradient sunColor; // 서서히 색이 변하는 걸 표현 
    public AnimationCurve sunIntensity; // 빛 강도 
    public float sunriseTime;
    public float noonTime;
    public float sunsetTime;

    [Header("Moon")]
    public Light moon;
    public Gradient moonColor;
    public AnimationCurve moonIntensity;
    public float moonriseTime;
    public float moonsetTime;

    [Header("Other Lighting")]
    public AnimationCurve lightingIntensityMultiplier; // 간접광(씬 전체에 퍼지는 빛)
    public AnimationCurve reflectionIntensityMultiplier; // 반사광 

    [Header("AmbientColor")]
    public Color nightAmbientColor;
    public Color dayAmbientColor;

    public event Action OnNight;
    public event Action OnDay;

    private void Awake()
    {
        RenderSettings.skybox.SetFloat("_Exposure", 2.34f);

        // ambient color initialize 
        nightAmbientColor = Color.black;
        dayAmbientColor = new Color(0.691f, 0.584f, 0.447f, 1.000f);

        sunIntensity = new AnimationCurve();
        sunIntensity.AddKey(sunriseTime, 0.0f);
        sunIntensity.AddKey(noonTime, 1.0f);
        sunIntensity.AddKey(sunsetTime, 0.0f);

        moonIntensity = new AnimationCurve();
        moonIntensity.AddKey(moonriseTime, 0.0f);
        moonIntensity.AddKey(moonsetTime, 0.0f);
    }

    void Start()
    {
        timeRate = 1.0f / fullDayLength; // ex. fullDayLength가 30이면 하루를 30초로 설정 
        time = startTime; // 초기 시간 세팅 
    }

    void Update()
    {
        time = (time + timeRate * Time.deltaTime) % 1.0f; // 시간 흘러가게 하기 

        // Ambient Color 변경 
        ChangeAmbientColor();

        //bool nowNight = (time >= 0.75f || time < 0.25f);
        bool nowNight = (time >= sunsetTime || time <= sunriseTime);
        //bool nowDay = (time >= 0.25f && time < 0.75f);
        bool nowDay = (sunriseTime <= time && time <= sunsetTime);

        if (nowNight && !wasNight)
        {
            isNight = true;
            isDay = false;
            OnNight?.Invoke();
        }

        if (nowDay && !wasDay)
        {
            isDay = true;
            isNight = false;
            OnDay?.Invoke();
        }


        wasNight = nowNight;
        wasDay = nowDay;

        UpdateLighting(sun, sunColor, sunIntensity);
        UpdateLighting(moon, moonColor, moonIntensity);

        // 간접광, 반사광 설정 
        RenderSettings.ambientIntensity = lightingIntensityMultiplier.Evaluate(time);
        RenderSettings.reflectionIntensity = reflectionIntensityMultiplier.Evaluate(time);
    }

    private void UpdateLighting(Light lightSource, Gradient gradient, AnimationCurve intensityCurve)
    {
        // 빛 강도 계산
        float intensity = intensityCurve.Evaluate(time); 

        // 조명 각도 계산 
        lightSource.transform.eulerAngles = (time - (lightSource == sun ? 0.25f : 0.75f)) * noon * 4f;

        // 조명 색상 설정 
        lightSource.color = gradient.Evaluate(time);

        // 조명 강도 설정 
        lightSource.intensity = intensity;

        // 해가 넘어간 상태에서는 끄자. 
        GameObject go = lightSource.gameObject;
        if (Mathf.Approximately(lightSource.intensity, 0f) && go.activeInHierarchy) // 빛 강도가 0에 가까워지면 오브젝트 끄기. 
        {
            go.SetActive(false);
        }
        else if (!Mathf.Approximately(lightSource.intensity, 0f) && !go.activeInHierarchy)
        {
            go.SetActive(true);
        }
    }

    private void ChangeAmbientColor()
    {
        //if (0.75f <= time || time <= 0.25f) // time >= sunsetTime || time <= sunriseTime
        if (time >= sunsetTime || time <= sunriseTime) 
        {
            RenderSettings.skybox.SetFloat("_Exposure", Mathf.Lerp(RenderSettings.skybox.GetFloat("_Exposure"), 0f, Time.deltaTime * colorChangeSpeed));
            RenderSettings.ambientLight = Color.Lerp(RenderSettings.ambientLight, nightAmbientColor, Time.deltaTime * colorChangeSpeed);
        }
        else
        {
            RenderSettings.skybox.SetFloat("_Exposure", Mathf.Lerp(RenderSettings.skybox.GetFloat("_Exposure"), 1f, Time.deltaTime * colorChangeSpeed));
            RenderSettings.ambientLight = Color.Lerp(RenderSettings.ambientLight, dayAmbientColor, Time.deltaTime * colorChangeSpeed);
        }
    }
}
