using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [Range(0.0f, 1.0f)]
    public float time;
    public float fullDayLength; // ��ü �Ϸ� ����(�� ����) (ex. 30�̸� �Ϸ簡 30��)
    public float startTime = 0.4f; // ���� �ð� 
    private float timeRate;
    public Vector3 noon; // Vector 90 0 0 
    private bool isNight;
    private bool isDay;
    private bool wasNight;
    private bool wasDay;


    [Header("Sun")]
    public Light sun;
    public Gradient sunColor; // ������ ���� ���ϴ� �� ǥ�� 
    public AnimationCurve sunIntensity; // �� ���� 

    [Header("Moon")]
    public Light moon;
    public Gradient moonColor;
    public AnimationCurve moonIntensity;

    [Header("Other Lighting")]
    public AnimationCurve lightingIntensityMultiplier; // ������(�� ��ü�� ������ ��)
    public AnimationCurve reflectionIntensityMultiplier; // �ݻ籤 

    public event Action OnNight;
    public event Action OnDay;


    void Start()
    {
        timeRate = 1.0f / fullDayLength; // ex. fullDayLength�� 30�̸� �Ϸ縦 30�ʷ� ���� 
        time = startTime; // �ʱ� �ð� ���� 
    }

    void Update()
    {
        time = (time + timeRate * Time.deltaTime) % 1.0f; // �ð� �귯���� �ϱ� 

        bool nowNight = (time >= 0.75f || time < 0.25f);
        bool nowDay = (time >= 0.25 && time < 0.75f);

        if(nowNight && !wasNight)
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

        // ������ ���� ��� 
        lightSource.transform.eulerAngles = (time - (lightSource == sun ? 0.25f : 0.75f)) * noon * 4f;

        // ���� ���� ���� 
        lightSource.color = gradient.Evaluate(time);

        // ���� ���� ���� 
        lightSource.intensity = intensity;

        // �ذ� �Ѿ ���¿����� ����. 
        GameObject go = lightSource.gameObject;
        if(lightSource.intensity == 0 && go.activeInHierarchy) 
        {
            go.SetActive(false);
        }
        else if(lightSource.intensity > 0 && !go.activeInHierarchy)
        {
            go.SetActive(true);
        }
    }

}
