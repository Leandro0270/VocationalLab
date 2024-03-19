using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class PhoneHandler : MonoBehaviour
{
    [SerializeField] private SphereCollider phoneCollider;
    [SerializeField] private ScObPhoneVoiceLines voiceLines;
    [SerializeField] private AudioSource phoneBaseAudioSource;
    [SerializeField] private AudioSource phoneVoiceAudioSource;
    [SerializeField] private AudioClip phoneRing;
    [SerializeField] private AudioClip phonePickUp;
    [SerializeField] private AudioClip phoneHangUp;
    [SerializeField] private float timeToImpatient = 10;
    private Coroutine _phoneRingCoroutine;


    
    
    
    private enum TutorialState
    {
        Introduction,
        Step1,
        Step2,
        Step3,
        Step4,
        Completed
    }
    
    private enum PhoneState
    {
        Ringing,
        PickedUp,
        OnBase,
        VoiceLine
    }

    private enum AssistantStatus
    {
        None,
        Talking,
        Impatient,
        CorrectAction,
        WrongAction,
        Waiting,
        HangUpResponse
    }
    
    private TutorialState _tutorialState = TutorialState.Introduction;
    private PhoneState _phoneState = PhoneState.Ringing;
    private AssistantStatus _currentAssistantStatus = AssistantStatus.None;
    
    
    private bool _isPhoneOnBase = true;
    private bool _isPlayerOnPhone = false;
     
    private AudioClip _currentVoiceLine;
    private AudioClip _nextVoiceLine = null;
    private bool _willSkipCurrentVoiceLine = false;
    private bool _isVoiceLinePlaying = false;
    private float _totalTimeOfCurrentVoiceLine;
    private float _timePlayingCurrentVoiceLine;
    
    
    private bool _startedTask = false;
    private bool _completedTask = false;
    private float _currentTaskSpendTime = 0;
    
    private bool _isVoiceLinePlayingOnBase = false;
    private bool _isVoiceLinePlayingOnPhone = false;


    private void Update()
    {
        if (_isVoiceLinePlaying)
        {


            if (_timePlayingCurrentVoiceLine >= _totalTimeOfCurrentVoiceLine)
            {
                _isVoiceLinePlaying = false;
                
                if (_startedTask && !_completedTask)
                {
                    _currentAssistantStatus = AssistantStatus.Waiting;
                }
                else
                {
                    _currentAssistantStatus = AssistantStatus.None;
                }


                if (_tutorialState != TutorialState.Introduction) return;
                
                _tutorialState = TutorialState.Step1;
                TutorialVoiceLineDecisionTree();

            }
            else
            {

                _timePlayingCurrentVoiceLine =
                    _isVoiceLinePlayingOnBase ? phoneBaseAudioSource.time : phoneVoiceAudioSource.time;
            }

            return;
        }

        if (_nextVoiceLine != null)
        {
            PlayVoiceLine(_nextVoiceLine);
            _nextVoiceLine = null;
            return;
        }

        if (_currentAssistantStatus != AssistantStatus.Waiting) return;
        _currentTaskSpendTime += Time.deltaTime;
        if (_currentTaskSpendTime >= timeToImpatient)
        {
            HandleImpatience();
            _currentTaskSpendTime = 0;
        }

    }

    private void Start() {_phoneRingCoroutine = StartCoroutine(PhoneRing());}

    private IEnumerator PhoneRing()
    {
        _isPhoneOnBase = true;
        yield return new WaitForSeconds(phoneRing.length + 2);
        phoneBaseAudioSource.clip = phoneRing;
        phoneBaseAudioSource.Play();
        _phoneState = PhoneState.Ringing;
        _phoneRingCoroutine = StartCoroutine(PhoneRing());
        
    }
    
    public void PickUpPhone()
    {
        _phoneState = PhoneState.PickedUp;
        phoneBaseAudioSource.clip = phonePickUp;
        phoneBaseAudioSource.Play();
        _isPhoneOnBase = false;
        
        if (_phoneState != PhoneState.Ringing) return;
        StopCoroutine(_phoneRingCoroutine);
        TutorialVoiceLineDecisionTree();
        StartCoroutine(DropedPhone(8));

    }
    

private AudioClip GetRandomVoiceLine(List<AudioClip> voiceLines)
{
    return voiceLines[Random.Range(0, voiceLines.Count)];
}

private void PlayCorrectActionVoiceLine()
{
    _startedTask = false;
    PlayVoiceLine(GetRandomVoiceLine(voiceLines.correctActionsVoiceLines));
}

private void PlayWrongActionVoiceLine()
{
    PlayVoiceLine(GetRandomVoiceLine(voiceLines.wrongActionsVoiceLines));
    _willSkipCurrentVoiceLine = true;
    if (_completedTask) return;
    _currentAssistantStatus = AssistantStatus.WrongAction;
}

private void PlayImpatientVoiceLine()
{
    PlayVoiceLine(GetRandomVoiceLine(voiceLines.impatientVoiceLines));
    if (_completedTask) return;
    _currentAssistantStatus = AssistantStatus.Impatient;
}

private void PlayHangUpResponseVoiceLine()
{
    PlayVoiceLine(GetRandomVoiceLine(voiceLines.hangUpVoiceLines));
}

private void TutorialVoiceLineDecisionTree()
{
    switch (_tutorialState)
    {
        case TutorialState.Introduction:
            PlayVoiceLine(voiceLines.introductionVoiceLine);
            return;
        case TutorialState.Completed:
            PlayVoiceLine(voiceLines.completedVoiceLine);
            return;
        default:
        {
            AudioClip voiceLine = null;
            switch (_currentAssistantStatus)
            {
                case AssistantStatus.CorrectAction:
                    PlayCorrectActionVoiceLine();
                    return;

                case AssistantStatus.WrongAction:
                    PlayWrongActionVoiceLine();
                    return;

                case AssistantStatus.Impatient:
                    PlayImpatientVoiceLine();
                    return;

                case AssistantStatus.HangUpResponse:
                    PlayHangUpResponseVoiceLine();
                    return;

                case AssistantStatus.None:
                    // Determine the voice line based on the tutorial step
                    switch (_tutorialState)
                    {
                        case TutorialState.Step1:
                            voiceLine = voiceLines.firstStep;
                            break;
                        case TutorialState.Step2:
                            voiceLine = voiceLines.secondStep;
                            break;
                        case TutorialState.Step3:
                            voiceLine = voiceLines.thirdStep;
                            break;
                        case TutorialState.Step4:
                            voiceLine = voiceLines.fourthStep;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    PlayVoiceLine(voiceLine);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            break;
        }
    }
}


    private void PlayVoiceLine(AudioClip voiceLine)
    {
        if (_isVoiceLinePlaying)
        {
            if (_willSkipCurrentVoiceLine)
            {
                _willSkipCurrentVoiceLine = false;
                _isVoiceLinePlaying = false;
                _isVoiceLinePlayingOnBase = false;
                _isVoiceLinePlayingOnPhone = false;
                _currentAssistantStatus = AssistantStatus.None;
                _currentVoiceLine = null;
                _nextVoiceLine = null;
                phoneBaseAudioSource.clip = null;
                phoneVoiceAudioSource.clip = null;
                PlayVoiceLine(voiceLine);
                return;
            }

            _nextVoiceLine = voiceLine;
            return;
        }

        _currentVoiceLine = voiceLine;
        _totalTimeOfCurrentVoiceLine = voiceLine.length;
        _isVoiceLinePlaying = true;
        if (_isPlayerOnPhone)
        {
            phoneVoiceAudioSource.clip = voiceLine;
            phoneVoiceAudioSource.Play();
        }
        else
        {
            phoneBaseAudioSource.clip = voiceLine;
            phoneBaseAudioSource.Play();
        }
    }




    public void HangUpPhone()
    {
        if (_isVoiceLinePlaying && _phoneState == PhoneState.PickedUp)
        {
            _currentAssistantStatus = AssistantStatus.HangUpResponse;
        }

        phoneBaseAudioSource.Pause();
        phoneVoiceAudioSource.Pause();
        phoneBaseAudioSource.clip = null;
        phoneVoiceAudioSource.clip = null;
        phoneBaseAudioSource.clip = phoneHangUp;
        phoneBaseAudioSource.Play();
        _phoneState = PhoneState.OnBase;
        StartCoroutine(PhoneRing());
        
    }

    private void HandleImpatience()
    {
        if (_currentAssistantStatus is not AssistantStatus.Waiting) return;
        _currentAssistantStatus = AssistantStatus.Impatient;
        TutorialVoiceLineDecisionTree();
    }

    public void OnWrongAction()
    {
        if(_currentAssistantStatus is AssistantStatus.HangUpResponse or AssistantStatus.Impatient or AssistantStatus.WrongAction)
            return;
        
        _currentAssistantStatus = AssistantStatus.WrongAction;
        TutorialVoiceLineDecisionTree();
    }

 
    public void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("PlayerEar")) return;
        _isPlayerOnPhone = true;
        
        if (!_isVoiceLinePlayingOnBase) return;
        
        float pausedAudioTime = phoneBaseAudioSource.time;
        AudioClip pausedAudioClip = phoneBaseAudioSource.clip;
        phoneBaseAudioSource.Pause();
        phoneVoiceAudioSource.clip = pausedAudioClip;
        phoneVoiceAudioSource.time = pausedAudioTime;
        phoneVoiceAudioSource.Play();
        phoneBaseAudioSource.clip = null;
    }

    public void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("PlayerEar")) return;
        
        _isPlayerOnPhone = false;
        StartCoroutine(DropedPhone(2));
    }

    private IEnumerator DropedPhone(int time)
    {
        yield return new WaitForSeconds(time);
        
        if (_isPlayerOnPhone) yield break;
        if (_isVoiceLinePlayingOnBase) yield break;
        var pausedAudioTime = phoneVoiceAudioSource.time;
        AudioClip pausedAudioClip = phoneVoiceAudioSource.clip;
        phoneVoiceAudioSource.Pause();
        
        phoneBaseAudioSource.clip = pausedAudioClip;
        phoneBaseAudioSource.time = pausedAudioTime;
        phoneBaseAudioSource.Play();
        _isVoiceLinePlayingOnBase = true;

    }
}
