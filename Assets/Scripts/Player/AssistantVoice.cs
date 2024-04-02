using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class AssistantVoice : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip attentionClip;
    [SerializeField] private AudioClip startClip;
    [SerializeField] private AudioClip armsUpClip;
    [SerializeField] private AudioClip pressStartButtonClip;
    [SerializeField] private AudioClip afterPatientFirstSymptomsClip;
    [SerializeField] private AudioClip bookInformationClip;
    [SerializeField] private AudioClip unlockedNewQuestionClip;
    [SerializeField] private AudioClip unlockedNewHintClip;
    [SerializeField] private AudioClip medicationClip;
    [SerializeField] private AudioClip printTranscriptionClip;
    [SerializeField] private AudioClip diagnosticClip;
    [SerializeField] private AudioClip unlockedNewHintAndQuestionClip;
    [SerializeField] private AudioClip[] unlockedNewHintRepeatClip;
    [SerializeField] private AudioClip[] unlockedNewQuestionRepeatClip;
    [SerializeField] private AudioClip wipAudioClip;
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private float timeToFirstClip = 5f;

    private bool _isPlaying;

    private bool _playedAttention;
    private bool _playedStart;
    private bool _playedArmsUp;
    private bool _playedPressStartButton;
    private bool _playedAfterPatientFirstSymptoms ;
    private bool _playedBookInformation;
    private bool _playedUnlockedNewQuestion;
    private bool _playedUnlockedNewHint;
    private bool _playedMedication;
    private bool _playedPrintTranscription;
    private bool _playedDiagnostic;
    private bool _playedUnlockedNewHintAndQuestion;
    private bool _playedWip;
    private float _timeBetweenClips = 2f;
    private List<AudioClip> _audioClipQueue = new List<AudioClip>();

    private void Start()
    {
        if (attentionClip != null || !_playedAttention)
            _audioClipQueue.Add(attentionClip);
        if (startClip != null || !_playedStart)
            _audioClipQueue.Add(startClip);
        if (armsUpClip != null || !_playedArmsUp)
            _audioClipQueue.Add(armsUpClip);
        if (pressStartButtonClip != null || !_playedPressStartButton)
            _audioClipQueue.Add(pressStartButtonClip);
        StartCoroutine(ClipPlayer(timeToFirstClip));
        _playedAttention = true;
        _playedStart = true;
        _playedArmsUp = true;
        _playedPressStartButton = true;

    }

    public void PlayClip(float timeBetweenClips)
    {
        if(_audioClipQueue.Count == 0) return;
        if (_isPlaying) return;
        _timeBetweenClips = timeBetweenClips;
        StartCoroutine(ClipPlayer(timeBetweenClips));
    }


    private IEnumerator ClipPlayer(float time)
    {
        if (_audioClipQueue.Count == 0) yield break;
        if (_isPlaying) yield return new WaitWhile(() => audioSource.isPlaying);

        yield return new WaitForSeconds(time);
        audioSource.clip = _audioClipQueue[0];
        _isPlaying = true;
        videoPlayer.Play();
        audioSource.Play();
        _audioClipQueue.RemoveAt(0);
        yield return new WaitWhile(() => audioSource.isPlaying);
        videoPlayer.frame = 0;
        videoPlayer.Stop();
        
        if (_audioClipQueue.Count > 0)
            StartCoroutine(ClipPlayer(_timeBetweenClips));
        else
            _isPlaying = false;

    }

    public void AddBookInformationQueue()
    {
        if (bookInformationClip == null) return;
        if (_playedBookInformation) return;
        _audioClipQueue.Add(bookInformationClip);
        _playedBookInformation = true;
    }

    public void AddAfterPatientFirstSymptomsQueue()
    {
        if (afterPatientFirstSymptomsClip == null) return;
        if (_playedAfterPatientFirstSymptoms) return;
        _audioClipQueue.Add(afterPatientFirstSymptomsClip);
        _playedAfterPatientFirstSymptoms = true;
    }

    public void AddUnlockedNewQuestionQueue()
    {

        if (unlockedNewQuestionClip == null) return;
        if (_playedUnlockedNewQuestion)
        {
            if(unlockedNewQuestionRepeatClip.Length < 1) return;
            _audioClipQueue.Add(unlockedNewQuestionRepeatClip[UnityEngine.Random.Range(0, unlockedNewQuestionRepeatClip.Length)]);
            return;
        }
        _audioClipQueue.Add(unlockedNewQuestionClip);
        _playedUnlockedNewQuestion = true;
    }

    public void AddUnlockedNewHintQueue()
    {
        if (unlockedNewHintClip == null) return;
        if (_playedUnlockedNewHint)
        {
            if(unlockedNewHintRepeatClip.Length < 1) return;
            _audioClipQueue.Add(unlockedNewHintRepeatClip[UnityEngine.Random.Range(0, unlockedNewHintRepeatClip.Length)]);
            return;
        }
        _audioClipQueue.Add(unlockedNewHintClip);
        _playedUnlockedNewHint = true;
    }

    public void AddMedicationQueue()
    {
        if (medicationClip == null) return;
        if (_playedMedication) return;
        _audioClipQueue.Add(medicationClip);
        _playedMedication = true;
    }

    public void AddPrintTranscriptionQueue()
    {
        if (printTranscriptionClip == null) return;
        if (_playedPrintTranscription) return;
        _audioClipQueue.Add(printTranscriptionClip);
        _playedPrintTranscription = true;
    }

    public void AddDiagnosticQueue()
    {
        if (diagnosticClip == null) return;
        if (_playedDiagnostic) return;
        _playedDiagnostic = true;
        _audioClipQueue.Add(diagnosticClip);
    }

    public void AddUnlockedNewHintAndQuestionQueue()
    {
        if (unlockedNewHintAndQuestionClip == null) return;
        if (_playedUnlockedNewHintAndQuestion) return;
        _audioClipQueue.Add(unlockedNewHintAndQuestionClip);
        _playedUnlockedNewHintAndQuestion = true;
    }
    
    public void AddWipQueue()
    {
        if (wipAudioClip == null) return;
        if (_playedWip) return;
        _audioClipQueue.Add(wipAudioClip);
        _playedWip = true;
    }
public void StopAllStartedClips()
    {
        _audioClipQueue.Clear();
        _isPlaying = false;
        StopAllCoroutines();
        audioSource.Stop();
        videoPlayer.Stop();
        videoPlayer.frame = 0;
    }
    
}
