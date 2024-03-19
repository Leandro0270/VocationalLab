using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CurrentQuestion : MonoBehaviour
{
    [SerializeField] private MonitorManager monitorManager;
    [SerializeField] private MonitorQuestions monitorQuestions;
    [SerializeField] private GameObject questionDetailsPanel;
    [SerializeField] private GameObject transcriptingAudioPanel;
    [SerializeField] private TextMeshProUGUI questionNumberField;
    [SerializeField] private TextMeshProUGUI questionField;
    [SerializeField] private Button askButton;
    [SerializeField] private Button transcriptAudioButton;
    [SerializeField] private Button returnButton;
    [SerializeField] private GameObject cautionPanel;
    [SerializeField] private GameObject newQuestionPanel;
    [SerializeField] private GameObject newHintPanel;
    [SerializeField] private GameObject microphoneModel;
    [SerializeField] private Printer printer;
    private AudioClip _currentAudioClip;
    private AudioSource _audioSource;
    private bool _audioPlaying;
    private ScObPatientQuestion _currentQuestion;

    public void SetCurrentQuestion(ScObPatientQuestion question)
    {
        cautionPanel.SetActive(true);

        _currentQuestion = question;
        questionNumberField.text = $"Pergunta: {question.id.ToString()}";
        questionField.text = question.question;
        _currentAudioClip = question.answerAudio;
        _audioSource = monitorManager.GetPatientAudio();
        if (_currentQuestion.isAnswered)
        {
            askButton.interactable = false;
            transcriptAudioButton.interactable = true;
            cautionPanel.SetActive(false);
            if (_currentQuestion.unlockHint != "")
            {
                cautionPanel.SetActive(false);

                newHintPanel.SetActive(true);
            }
            else if (_currentQuestion.unlockQuestions.Length > 0)
            {
                cautionPanel.SetActive(false);
                newQuestionPanel.SetActive(true);
            }
        }
        else
        {
            askButton.interactable = true;
            transcriptAudioButton.interactable = false;
        }
    }





    public void MakeQuestion()
    {
        if (_currentQuestion.isAnswered || _audioPlaying)
        {
            return;
        }
        _audioSource.clip = _currentAudioClip;
        StartCoroutine(MakeAudioQuestion());
    }




    private IEnumerator MakeAudioQuestion()
    {
        questionDetailsPanel.SetActive(false);
        transcriptingAudioPanel.SetActive(true);
        microphoneModel.SetActive(true);
        returnButton.interactable = false;
        _audioPlaying = true;
        _audioSource.Play();
        yield return new WaitWhile(() => _audioSource.isPlaying);
        monitorManager.AddTimesPlayerAsked();
        microphoneModel.SetActive(false);

        if (_currentQuestion.unlockQuestions.Length > 0)
        {
            foreach (ScObPatientQuestion newQuestion in _currentQuestion.unlockQuestions)
            {
                monitorQuestions.UnlockQuestion(newQuestion);
            }

            newQuestionPanel.SetActive(true);
            cautionPanel.SetActive(false);

        }
        else if (_currentQuestion.unlockHint != "")
        {
            monitorManager.AddNewHint(_currentQuestion.unlockHint);
            newHintPanel.SetActive(true);
            cautionPanel.SetActive(false);
        }
        else
        {
            cautionPanel.SetActive(false);
        }
        _audioPlaying = false;
        transcriptingAudioPanel.SetActive(false);
        questionDetailsPanel.SetActive(true);
        askButton.interactable = false;
        transcriptAudioButton.interactable = true;
        _currentQuestion.isAnswered = true;
        returnButton.interactable = true;

    }
    
    public void PrintTranscription()
    {
        if (!_currentQuestion.isAnswered) return;
        printer.PrintText(_currentQuestion.answer);
            
    }

    public void ReturnButton()
    {
        monitorQuestions.ReturnFromDetailsScreen();
    }

}






