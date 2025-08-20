using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [Range(0.0f, 1.0f)]
    public float time;
    public float fullDayLength; // 전체 하루 길이(초 단위) (ex. 30이면 하루가 30초)
    public float startTime = 0.4f; // 시작 시간 
    private float timeRate;
    public Vector3 noon; // Vector 90 0 0 
    private bool isNight;
    private bool isDay;
    private bool wasNight;
    private bool wasDay;


    [Header("Sun")]
    public Light sun;
    public Gradient sunColor; // 서서히 색이 변하는 걸 표현 
    public AnimationCurve sunIntensity; // 빛 강도 

    [Header("Moon")]
    public Light moon;
    public Gradient moonColor;
    public AnimationCurve moonIntensity;

    [Header("Other Lighting")]
    public AnimationCurve lightingIntensityMultiplier; // 간접광(씬 전체에 퍼지는 빛)
    public AnimationCurve reflectionIntensityMultiplier; // 반사광 

    public event Action OnNight;
    public event Action OnDay;


    void Start()
    {
        timeRate = 1.0f / fullDayLength; // ex. fullDayLength가 30이면 하루를 30초로 설정 
        time = startTime; // 초기 시간 세팅 
    }

    void Update()
    {
        time = (time + timeRate * Time.deltaTime) % 1.0f; // 시간 흘러가게 하기 

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

        // 간접광, 반사광 설정 
        RenderSettings.ambientIntensity = lightingIntensityMultiplier.Evaluate(time);
        RenderSettings.reflectionIntensity = reflectionIntensityMultiplier.Evaluate(time);
    }

    private void UpdateLighting(Light lightSource, Gradient gradient, AnimationCurve intensityCurve)
    {
        // 빛 강도 계산
        float intensity = intensityCurve.Evaluate(time); 

        // 조명의 각도 계산 
        lightSource.transform.eulerAngles = (time - (lightSource == sun ? 0.25f : 0.75f)) * noon * 4f;

        // 조명 색상 설정 
        lightSource.color = gradient.Evaluate(time);

        // 조명 강도 설정 
        lightSource.intensity = intensity;

        // 해가 넘어간 상태에서는 끄자. 
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
