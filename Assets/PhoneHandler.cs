using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PhoneHandler : MonoBehaviour
{
    [SerializeField] private SphereCollider phoneCollider;
    [SerializeField] private ScObPhoneVoiceLines voiceLines;
    [SerializeField] private AudioSource phoneBaseAudioSource;
    [SerializeField] private AudioSource phoneVoiceAudioSource;
    [SerializeField] private AudioClip phoneRing;
    [SerializeField] private AudioClip phonePickUp;
    [SerializeField] private AudioClip phoneHangUp;
    [SerializeField] private float timeToComplain = 10;
    private int _sarahAngerLevel = 0;
    private bool _willIncreaseAnger = false;
    private bool _isHangUp = false;
    private AudioClip _currentVoiceLine;
    
    
    private bool _isPhonePickedUp = false;
    private bool _isPlayerOnPhone = false;
    
    
    private bool _completedTask = false;
    private bool _willPlayImpatientVoiceLine = false;
    private bool _isTutorial = true;
    private bool _isTutorialStep1 = false;
    private bool _isTutorialStep2 = false;
    private bool _isTutorialStep3 = false;
    private bool _isTutorialStep4 = false;
    private bool _isFirstIgnored = false;
    private bool _isPlayingOnBase = false;

    
    private void Start()
    {
        StartCoroutine(PhoneRing());
    }
    
    private IEnumerator PhoneRing()
    {
        yield return new WaitForSeconds(phoneRing.length + 2);
        phoneBaseAudioSource.clip = phoneRing;
        phoneBaseAudioSource.Play();
    }

    private void ChooseAudioClip()
    {
        if(_isTutorial)
            _currentVoiceLine = voiceLines.introductionVoiceLine;
        else if (_isTutorialStep1)
        {
            _currentVoiceLine = voiceLines.firstStep;
        }
    }

    public void PickUpPhone()
    {
        StopCoroutine(PhoneRing());
        phoneBaseAudioSource.clip = phonePickUp;
        _isPhonePickedUp = true;
    }
    
    public void HangUpPhone()
    {
        if ((phoneBaseAudioSource.isPlaying || phoneVoiceAudioSource.isPlaying) && _willIncreaseAnger &&
            _isPhonePickedUp)
        {
            _isHangUp = true;
            _sarahAngerLevel++;
        }

        phoneBaseAudioSource.Pause();
        phoneVoiceAudioSource.Pause();
        phoneBaseAudioSource.clip = null;
        phoneVoiceAudioSource.clip = null;
        phoneBaseAudioSource.clip = phoneHangUp;
        phoneBaseAudioSource.Play();
        _willIncreaseAnger = false;
        _isPhonePickedUp = false;
        if(_isHangUp) StartCoroutine(PhoneRing());
        
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerOnPhone = true;
            if (_isPlayingOnBase)
            {
                float pausedAudioTime = phoneBaseAudioSource.time;
                AudioClip pausedAudioClip = phoneBaseAudioSource.clip;
                phoneBaseAudioSource.Pause();
                phoneVoiceAudioSource.clip = pausedAudioClip;
                phoneVoiceAudioSource.time = pausedAudioTime;
                phoneVoiceAudioSource.Play();
                phoneBaseAudioSource.clip = null;
            }
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerOnPhone = false;
            StartCoroutine(DropedPhone(5));
        }
    }

    private IEnumerator DropedPhone(int time)
    {
        yield return new WaitForSeconds(time);
        if (!_isPhonePickedUp || _isPlayerOnPhone) yield break;
        float pausedAudioTime = phoneVoiceAudioSource.time;
        AudioClip pausedAudioClip = phoneVoiceAudioSource.clip;
        phoneVoiceAudioSource.Pause();

        if (_isFirstIgnored)
        {
            phoneBaseAudioSource.clip = voiceLines.ignoredVoiceLine;
            phoneBaseAudioSource.Play();
            _isFirstIgnored = false;
            StartCoroutine(DropedPhone(2));
        }
        
        phoneBaseAudioSource.clip = pausedAudioClip;
        phoneBaseAudioSource.time = pausedAudioTime;
        phoneBaseAudioSource.Play();
        _isPlayingOnBase = true;
        
    }
}