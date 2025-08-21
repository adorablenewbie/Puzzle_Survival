using System;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [Range(0.0f, 1.0f)]
    public float time;
    public float fullDayLength; // ��ü �Ϸ� ����(�� ����) (ex. 30�̸� �Ϸ簡 30��)
    public float startTime = 0.4f; // ���� �ð� 
    private float timeRate;
    public Vector3 noon; // Vector 90 0 0 
    public float colorChangeSpeed = 0.5f;

    private bool isNight;
    private bool isDay;
    private bool wasNight;
    private bool wasDay;

    [Header("Sun")]
    public Light sun;
    public Gradient sunColor; // ������ ���� ���ϴ� �� ǥ�� 
    public AnimationCurve sunIntensity; // �� ���� 
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
    public AnimationCurve lightingIntensityMultiplier; // ������(�� ��ü�� ������ ��)
    public AnimationCurve reflectionIntensityMultiplier; // �ݻ籤 

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
        timeRate = 1.0f / fullDayLength; // ex. fullDayLength�� 30�̸� �Ϸ縦 30�ʷ� ���� 
        time = startTime; // �ʱ� �ð� ���� 
    }

    void Update()
    {
        time = (time + timeRate * Time.deltaTime) % 1.0f; // �ð� �귯���� �ϱ� 

        // Ambient Color ���� 
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

        // ������, �ݻ籤 ���� 
        RenderSettings.ambientIntensity = lightingIntensityMultiplier.Evaluate(time);
        RenderSettings.reflectionIntensity = reflectionIntensityMultiplier.Evaluate(time);
    }

    private void UpdateLighting(Light lightSource, Gradient gradient, AnimationCurve intensityCurve)
    {
        // �� ���� ���
        float intensity = intensityCurve.Evaluate(time); 

        // ���� ���� ��� 
        lightSource.transform.eulerAngles = (time - (lightSource == sun ? 0.25f : 0.75f)) * noon * 4f;

        // ���� ���� ���� 
        lightSource.color = gradient.Evaluate(time);

        // ���� ���� ���� 
        lightSource.intensity = intensity;

        // �ذ� �Ѿ ���¿����� ����. 
        GameObject go = lightSource.gameObject;
        if (Mathf.Approximately(lightSource.intensity, 0f) && go.activeInHierarchy) // �� ������ 0�� ��������� ������Ʈ ����. 
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
