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
    [SerializeField] private AudioSource notificationAudioSource;
    [SerializeField] private AudioClip hintNotificationClip;
    [SerializeField] private AudioClip questionNotificationClip;
    private PatientBehavior _patientBehavior;
    private AudioClip _currentPatientAudioClip;
    private AudioSource _patientAudioSource;
    private bool _patientAudioPlaying;
    private AudioClip _currentDoctorAudioClip;
    [SerializeField] private AudioSource doctorAudioSource;
    private bool _doctorAudioPlaying;
    private ScObPatientQuestion _currentQuestion;

    public void SetCurrentQuestion(ScObPatientQuestion question)
    {
        cautionPanel.SetActive(true);

        _currentQuestion = question;
        questionNumberField.text = $"Pergunta: {question.id.ToString()}";
        questionField.text = question.question;
        _currentPatientAudioClip = question.answerAudio;
        _currentDoctorAudioClip = question.questionAudio;
        _patientAudioSource = monitorManager.GetPatientAudio();
        _patientBehavior = monitorManager.GetPatientBehavior();
        if (_currentQuestion.isAnswered)
        {
            askButton.interactable = false;
            transcriptAudioButton.interactable = true;
            cautionPanel.SetActive(false);
            if (_currentQuestion.unlockSymptoms.Length >0 || _currentQuestion.unlockCause.Length > 0)
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
    if (_currentQuestion.isAnswered || _doctorAudioPlaying || _patientAudioPlaying)
    {
        return;
    }

    // Definir o clipe de áudio do doutor.
    doctorAudioSource.clip = _currentDoctorAudioClip;

    // Começar a coroutine que gerencia a fala do doutor antes de seguir para o paciente.
    StartCoroutine(DoctorThenPatientDialogue());
    }

private IEnumerator DoctorThenPatientDialogue()
{
    // Inicia a fala do doutor
    _doctorAudioPlaying = true;
    doctorAudioSource.Play();

    // Espera até que o doutor termine de falar.
    yield return new WaitWhile(() => doctorAudioSource.isPlaying);
    _doctorAudioPlaying = false;

    // Após o doutor terminar de falar, prossegue com a fala do paciente.
    yield return StartCoroutine(MakeAudioQuestion());
}

private IEnumerator MakeAudioQuestion()
{
    // O resto do seu código original vem aqui, gerenciando a fala do paciente.
    questionDetailsPanel.SetActive(false);
    transcriptingAudioPanel.SetActive(true);
    returnButton.interactable = false;
    _patientAudioPlaying = true;
    _patientAudioSource.clip = _currentPatientAudioClip;
    _patientBehavior.StartTalking();
    _patientAudioSource.Play();
    microphoneModel.SetActive(true);
    yield return new WaitWhile(() => _patientAudioSource.isPlaying);
    monitorManager.AddTimesPlayerAsked();
    microphoneModel.SetActive(false);
    _patientBehavior.StopTalking();

    int unlockQuestions = _currentQuestion.unlockQuestions.Length;
    int unlockSymptoms = _currentQuestion.unlockSymptoms.Length;
    int unlockCause = _currentQuestion.unlockCause.Length;
    bool playedBoth = false;
    
     if (unlockQuestions > 0 && (unlockCause + unlockSymptoms) > 0)
     {
         playedBoth = true;
         notificationAudioSource.clip = hintNotificationClip;
         notificationAudioSource.Play();
         monitorManager.AssistantUnlockedNewQuestionOrHint(true, true);
     }

    if (unlockQuestions > 0)
    {
        foreach (ScObPatientQuestion newQuestion in _currentQuestion.unlockQuestions)
            monitorQuestions.UnlockQuestion(newQuestion);

        if (!playedBoth)
        {
            notificationAudioSource.clip = questionNotificationClip;
            notificationAudioSource.Play();
            monitorManager.AssistantUnlockedNewQuestionOrHint(false, true);
        }

        newQuestionPanel.SetActive(true);

    }
    
    if (unlockCause+unlockSymptoms > 0)
    {
        if(unlockSymptoms > 0)
            foreach (var symptom in _currentQuestion.unlockSymptoms)
                monitorManager.AddNewHint(symptom.symptomName);
        
        if (unlockCause > 0)
            foreach (var hint in _currentQuestion.unlockCause)
                monitorManager.AddNewHint(hint.causeName);
        
        if(!playedBoth)
            notificationAudioSource.clip = hintNotificationClip;
            monitorManager.AssistantUnlockedNewQuestionOrHint(true,false);
        
        newHintPanel.SetActive(true);
    }
    
    monitorManager.AssistantPrintTranscription();
    cautionPanel.SetActive(false);
    _patientAudioPlaying = false;
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
        printer.PrintText(_currentQuestion.question ,_currentQuestion.answer);
            
    }

    public void ReturnButton()
    {   newHintPanel.SetActive(false);
        newQuestionPanel.SetActive(false);
        cautionPanel.SetActive(true);
        monitorQuestions.ReturnFromDetailsScreen();
    }

}






