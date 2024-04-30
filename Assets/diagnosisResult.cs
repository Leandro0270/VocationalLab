using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiagnosisResult : MonoBehaviour
{
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject loosePanel;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AssistantVoice assistantVoice;
    private AudioClip _currentPatientThanksClip;
    private AudioSource _currentPatientAudioSource;
    private PatientBehavior _patientBehavior;
    private AudioClip _winClip;
    private bool _win;



    public void SetWinClip(AudioClip clip)
    {
        _winClip = clip;
    }

    public IEnumerator SetWin(bool win)
    {
        winPanel.SetActive(false);
        loosePanel.SetActive(false);
        _win = win;
        if (_win)
        {
            winPanel.SetActive(true);
            audioSource.clip = _winClip;
            yield return new WaitForSeconds(2);
            audioSource.Play();
            yield return new WaitWhile(() => audioSource.isPlaying);
            yield return new WaitForSeconds(2);
            StartCoroutine(_patientBehavior.StartDialogue(_currentPatientThanksClip));
            yield return new WaitWhile(() => _currentPatientAudioSource.isPlaying);
            yield return new WaitForSeconds(2);
            assistantVoice.AddWinQueue();
            assistantVoice.PlayClip(2);
            
        }
        else
        {
            loosePanel.SetActive(true);
            assistantVoice.AddLooseQueue();
            assistantVoice.PlayClip(2);
        }
    }
    
    
    public void SetPatientBehavior(PatientBehavior patientBehavior)
    {
        _patientBehavior = patientBehavior;
    }
    
    public void SetPatientAudios(AudioClip clip, AudioSource patientAudioSource)
    {
        _currentPatientThanksClip = clip;
        _currentPatientAudioSource = patientAudioSource;
    }
    
}
