using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeCounter : MonoBehaviour
{
    
    [SerializeField] TextMeshPro hourMinuteText;
    [SerializeField] TextMeshPro secondText;
    private float _currentTime;
    private int _hours;
    private int _minutes;
    private int _seconds; 
    private bool _isCounting = false;
    
    
    
    
    public void StartCounting()
    {
        _isCounting = true;
        _currentTime = 0;
        _hours = 0;
        _minutes = 0;
        _seconds = 0;

    }


    public void Update()
    {
        if (!_isCounting) return;
        _currentTime += Time.deltaTime;
        _hours = (int) _currentTime / 3600;
        _minutes = (int) (_currentTime % 3600) / 60;
        _seconds = (int) (_currentTime % 3600) % 60;
        hourMinuteText.text = $"{_hours:D2}:{_minutes:D2}";
        secondText.text = $"{_seconds:D2}";
    }
    
    public float StopCounting()
    {
        _isCounting = false;
        return _currentTime;
    }
}
