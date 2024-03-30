using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MonitorQuestions : MonoBehaviour
{
    private List<ScObPatientQuestion> _currentPatientQuestions;
    /* id - 1 Alimentação, atividade física, sono, vícios, higiene, saúde sexual, lazer,
     rotina */
    private List<ScObPatientQuestion> _lockedHabitsQuestions;
    private List<ScObPatientQuestion> _availableHabitsQuestions;
    //id - 2 Condições Hereditárias, Histórico de saúde, Histórico de doenças, Histórico de cirurgias, Histórico de alergias, Histórico de medicamentos,
    //Histórico de vacinas, Histórico de doenças crônicas, Histórico de doenças psicológicas, Histórico de doenças cardíacas, Histórico de doenças respiratórias, Histórico de doenças renais, Histórico de doenças hepáticas, Histórico de doenças gastrointestinais, Histórico de doenças endócrinas, Histórico de doenças neurológicas, Histórico de doenças dermatológicas, Histórico de doenças reumáticas, Histórico de doenças hematológicas, Histórico de doenças musculares, Histórico de doenças ósseas, Histórico de doenças articulares, Histórico de doenças genéticas, Histórico de doenças infecciosas, Histórico de doenças autoimunes, Histórico de doenças sexualmente transmissíveis, Histórico de doenças oncológicas, Histórico de doenças metabólicas, Histórico de doenças nutricionais, Histórico de doenças do sistema imunológico, Histórico de doenças do sistema linfático, Histórico de doenças do sistema circulatório, Histórico de doenças do sistema respiratório, Histórico de doenças do sistema digestivo, Histórico de doenças do sistema urinário, Histórico de doenças do sistema reprodutor, Histórico de doenças do sistema endócrino, Histórico de doenças do sistema nervoso, Histórico de doenças do sistema tegumentar, Histórico de doenças do sistema osteomuscular, Histórico de doenças do sistema articular, Histórico de doenças do sistema genético, Histórico de doenças do sistema infeccioso, Histórico de doenças do
    //sistema autoimune, Histórico de doenças do sistema sexual, Histórico de doenças do sistema oncológico, Histórico de doenças do sistema metabólico, Histórico de doenças do sistema nutricional, Histórico de doenças do sistema
    private List<ScObPatientQuestion> _availableClinicalHistoryQuestions;
    private List<ScObPatientQuestion> _lockedClinicalHistoryQuestions;
    //id - 3 Sintomas, Duração dos sintomas, Intensidade dos sintomas, Frequência dos sintomas, Localização dos sintomas, Características dos sintomas,
    //Fatores que pioram os sintomas, Fatores que melhoram os sintomas, Sintomas associados, Sintomas prévios, Sintomas atuais, Sintomas futuros, Sintomas passados,
    //Sintomas recorrentes, Sintomas agudos, Sintomas crônicos, Sintomas agudizados, Sintomas cronicizados, Sintomas localizados, Sintomas generalizados, Sintomas sistêmicos,
    //Sintomas específicos, Sintomas inespecíficos
    private List<ScObPatientQuestion> _availableSymptomsQuestions;
    private List<ScObPatientQuestion> _lockedSymptomsQuestions;
    //id - 4 Medicamentos, Doses, Frequência, Duração, Efeitos colaterais, Interações medicamentosas, Medicamentos atuais, Medicamentos prévios, Medicamentos futuros, Medicamentos passados,
    //Medicamentos recorrentes, Medicamentos agudos, Medicamentos crônicos, Medicamentos agudizados, Medicamentos cronicizados, Medicamentos localizados, Medicamentos generalizados, Medicamentos sistêmicos,
    //Medicamentos específicos, Medicamentos inespecíficos
    private List<ScObPatientQuestion> _availableMedicationQuestions;
    private List<ScObPatientQuestion> _lockedMedicationQuestions;
    
    //Telas
    [SerializeField] private GameObject mainPageQuestionsScreen;
    [SerializeField] private GameObject habitsQuestionsScreen;
    [SerializeField] private GameObject clinicalHistoryQuestionsScreen;
    [SerializeField] private GameObject symptomsQuestionsScreen;
    [SerializeField] private GameObject medicationQuestionsScreen;
    [SerializeField] private GameObject questionDetailsScreen;

    //cards de opções
    [SerializeField] private List<QuestionOption> habitsQuestionOptions;
    private List<QuestionOption> _availableButtonsHabits;
    [SerializeField] private List<QuestionOption> clinicalHistoryQuestionOptions;
    private List<QuestionOption> _availableButtonsClinicalHistory;
    [SerializeField] private List<QuestionOption> symptomsQuestionOptions;
    private List<QuestionOption> _availableButtonsSymptoms;
    [SerializeField] private List<QuestionOption> medicationQuestionOptions;
    private List<QuestionOption> _availableButtonsMedication;
    
    private ScObPatientQuestion _selectedQuestion;
    
    //Botões da tela principal de perguntas
    [SerializeField] private Button openHabitsQuestionsPageButton;
    [SerializeField] private GameObject habitsNotificationIcon;
    private int habitsNotificationCount;
    [SerializeField] private Button openClinicalHistoryQuestionsPageButton;
    [SerializeField] private GameObject clinicalHistoryNotificationIcon;
    private int clinicalHistoryNotificationCount;
    [SerializeField] private Button openSymptomsQuestionsPageButton;
    [SerializeField] private GameObject symptomsNotificationIcon;
    private int symptomsNotificationCount;
    [SerializeField] private Button openMedicationQuestionsPageButton;
    [SerializeField] private GameObject medicationNotificationIcon;
    private int medicationNotificationCount;
    //Other monitor scripts
    [SerializeField] private MonitorManager monitorManager;
    [SerializeField] private CurrentQuestion currentQuestionPanel;
    
    //Screen navigation
    private GameObject _lastScreen;
    private GameObject _currentScreen;
    private GameObject _nextScreen;

    private bool _setup;
    
    
    private void Awake()
    {
        openMedicationQuestionsPageButton.gameObject.SetActive(false);
        openSymptomsQuestionsPageButton.gameObject.SetActive(false);
        openClinicalHistoryQuestionsPageButton.gameObject.SetActive(false);
        openHabitsQuestionsPageButton.gameObject.SetActive(false);
        
        
        _availableButtonsHabits = new List<QuestionOption>();
        foreach (var questionButtonOption in habitsQuestionOptions)
        {
            _availableButtonsHabits.Add(questionButtonOption);
        }

        _availableButtonsClinicalHistory = new List<QuestionOption>();
        foreach (var questionButtonOption in clinicalHistoryQuestionOptions)
        {
            _availableButtonsClinicalHistory.Add(questionButtonOption);
        }
        
        _availableButtonsSymptoms = new List<QuestionOption>();
        foreach (var questionButtonOption in symptomsQuestionOptions)
        {
            _availableButtonsSymptoms.Add(questionButtonOption);
        }
        
        _availableButtonsMedication = new List<QuestionOption>();
        foreach (var questionButtonOption in medicationQuestionOptions)
        {
            _availableButtonsMedication.Add(questionButtonOption);
        }
    }

    private int _clinicalQuestionId = 1;
    private int _habitsQuestionId = 1;
    private int _symptomsQuestionId = 1;
    private int _medicationQuestionId = 1;
    

    public void OpenQuestionsPage()
    {
        _lastScreen = null;
        _currentScreen = mainPageQuestionsScreen;
        monitorManager.SelectNewScreen(_currentScreen);
        _currentScreen.SetActive(true);
    }
    public void SetCurrentPatientQuestions(ScObPatientQuestion[] questions)
    {
        if(_setup) return;
        _lockedHabitsQuestions = new List<ScObPatientQuestion>();
        _availableHabitsQuestions = new List<ScObPatientQuestion>();
        _lockedClinicalHistoryQuestions = new List<ScObPatientQuestion>();
        _availableClinicalHistoryQuestions = new List<ScObPatientQuestion>();
        _lockedSymptomsQuestions = new List<ScObPatientQuestion>();
        _availableSymptomsQuestions = new List<ScObPatientQuestion>();
        _lockedMedicationQuestions = new List<ScObPatientQuestion>();
        _availableMedicationQuestions = new List<ScObPatientQuestion>();
        
        _currentPatientQuestions = new List<ScObPatientQuestion>(questions);
        foreach (var question in _currentPatientQuestions)
        {


            switch (question.categoryId)
            {
                case 1:
                    openHabitsQuestionsPageButton.gameObject.SetActive(true);
                    if (question.isUnlocked && question.unlockedByQuestion == null &&
                        !_availableHabitsQuestions.Contains(question))
                    {
                        _availableHabitsQuestions.Add(question);
                        _availableButtonsHabits[0].SetQuestion(question, _habitsQuestionId, this);
                        _availableButtonsHabits.RemoveAt(0);
                        _habitsQuestionId++;
                    }
                    else
                    {
                        _lockedHabitsQuestions.Add(question);
                    }

                    break;
                case 2:
                    openClinicalHistoryQuestionsPageButton.gameObject.SetActive(true);
                    if (question.isUnlocked && question.unlockedByQuestion == null &&
                        !_availableClinicalHistoryQuestions.Contains(question))
                    {
                        _availableClinicalHistoryQuestions.Add(question);
                        _availableButtonsClinicalHistory[0].SetQuestion(question, _clinicalQuestionId, this);
                        _availableButtonsClinicalHistory.RemoveAt(0);
                        _clinicalQuestionId++;
                    }
                    else
                    {
                        _lockedClinicalHistoryQuestions.Add(question);
                    }

                    break;
                case 3:
                    openSymptomsQuestionsPageButton.gameObject.SetActive(true);
                    if (question.isUnlocked && question.unlockedByQuestion == null &&
                        !_availableSymptomsQuestions.Contains(question))
                    {
                        _availableSymptomsQuestions.Add(question);
                        _availableButtonsSymptoms[0].SetQuestion(question, _symptomsQuestionId, this);
                        _availableButtonsSymptoms.RemoveAt(0);
                        _symptomsQuestionId++;
                    }
                    else
                    {
                        _lockedSymptomsQuestions.Add(question);
                    }

                    break;
                case 4:
                    openMedicationQuestionsPageButton.gameObject.SetActive(true);
                    if (question.isUnlocked && question.unlockedByQuestion == null &&
                        !_availableMedicationQuestions.Contains(question))
                    {
                        _availableMedicationQuestions.Add(question);
                        _availableButtonsMedication[0].SetQuestion(question, _medicationQuestionId, this);
                        _availableButtonsMedication.RemoveAt(0);
                        _medicationQuestionId++;
                    }
                    else
                    {
                        _lockedMedicationQuestions.Add(question);
                    }

                    break;
            }
        }


        foreach (var lockedSymptoms in _lockedSymptomsQuestions)
        {
                _availableButtonsSymptoms[0].SetQuestion(lockedSymptoms,_symptomsQuestionId, this);
                _availableButtonsSymptoms.RemoveAt(0);
                _symptomsQuestionId++;
        }
        foreach (var lockedMedication in _lockedMedicationQuestions)
        {
                _availableButtonsMedication[0].SetQuestion(lockedMedication,_medicationQuestionId, this);
                _availableButtonsMedication.RemoveAt(0);
                _medicationQuestionId++;
        }
        foreach (var lockedClinical in _lockedClinicalHistoryQuestions)
        {
                _availableButtonsClinicalHistory[0].SetQuestion(lockedClinical,_clinicalQuestionId, this);
                _availableButtonsClinicalHistory.RemoveAt(0);
                _clinicalQuestionId++;
        }
        foreach (var lockedHabits in _lockedHabitsQuestions) 
        {
                _availableButtonsHabits[0].SetQuestion(lockedHabits,_habitsQuestionId, this);
                _availableButtonsHabits.RemoveAt(0);
                _habitsQuestionId++;
        }
            
        
        _setup = true;
    }
    
    public void OpenHabitsQuestionsPage()
    {
        _lastScreen = mainPageQuestionsScreen;
        _currentScreen = habitsQuestionsScreen;
        monitorManager.SelectNewScreen(_currentScreen);
        mainPageQuestionsScreen.SetActive(false);
        habitsQuestionsScreen.SetActive(true);
    }
    
    public void OpenClinicalHistoryQuestionsPage()
    {
        _lastScreen = mainPageQuestionsScreen;
        _currentScreen = clinicalHistoryQuestionsScreen;
        
        monitorManager.SelectNewScreen(_currentScreen);
        mainPageQuestionsScreen.SetActive(false);
        clinicalHistoryQuestionsScreen.SetActive(true);
    }
    
    public void OpenSymptomsQuestionsPage()
    {
        _lastScreen = mainPageQuestionsScreen;
        _currentScreen = symptomsQuestionsScreen;
        
        monitorManager.SelectNewScreen(_currentScreen);
        mainPageQuestionsScreen.SetActive(false);
        symptomsQuestionsScreen.SetActive(true);
    }
    
    public void OpenMedicationQuestionsPage()
    { 
        _lastScreen = mainPageQuestionsScreen;
        _currentScreen = medicationQuestionsScreen;
        monitorManager.SelectNewScreen(_currentScreen);
        mainPageQuestionsScreen.SetActive(false);
        medicationQuestionsScreen.SetActive(true);
    }

    public void UnlockQuestion(ScObPatientQuestion question)
    {
        question.isUnlocked = true;
        question.questionOptionButton.UnlockQuestion();
        switch (question.categoryId)
        {
            case 1:
                _availableHabitsQuestions.Add(question);
                _lockedHabitsQuestions.Remove(question);
                habitsNotificationCount++;
                habitsNotificationIcon.SetActive(true);
                break;
            case 2:
                _availableClinicalHistoryQuestions.Add(question);
                _lockedClinicalHistoryQuestions.Remove(question);
                clinicalHistoryNotificationCount++;
                clinicalHistoryNotificationIcon.SetActive(true);
                break;
            case 3:
                _availableSymptomsQuestions.Add(question);
                _lockedSymptomsQuestions.Remove(question);
                symptomsNotificationCount++;
                symptomsNotificationIcon.SetActive(true);
                break;
            case 4:
                _availableMedicationQuestions.Add(question);
                _lockedMedicationQuestions.Remove(question);
                medicationNotificationCount++;
                medicationNotificationIcon.SetActive(true);
                break;
        }
    }
    
    
    public void SelectQuestion(ScObPatientQuestion question)
    {
        if(!question.isUnlocked) return;
        _selectedQuestion = question;
        switch (question.categoryId)
        {
            case 1:
                habitsNotificationCount--;
                if (habitsNotificationCount > 0) return;
                habitsNotificationIcon.SetActive(false);
                habitsNotificationCount = 0;
                break;
            case 2:
                clinicalHistoryNotificationCount--;
                if (clinicalHistoryNotificationCount > 0) return;
                clinicalHistoryNotificationIcon.SetActive(false);
                clinicalHistoryNotificationCount = 0;
                break;
            case 3:
                symptomsNotificationCount--;
                if (symptomsNotificationCount > 0) return;
                symptomsNotificationIcon.SetActive(false);
                symptomsNotificationCount = 0;
                break;
            case 4:
                medicationNotificationCount--;
                if (medicationNotificationCount > 0) return;
                medicationNotificationIcon.SetActive(false);
                medicationNotificationCount = 0;
                break;

        }
        
        _nextScreen = questionDetailsScreen;
        monitorManager.SelectNewScreen(_nextScreen);
        questionDetailsScreen.SetActive(true);
        currentQuestionPanel.SetCurrentQuestion(_selectedQuestion);
    }
    
    public void ReturnFromDetailsScreen()
    {
        _currentScreen.SetActive(true);
        _nextScreen.SetActive(false);
        _nextScreen = null;
        if (_selectedQuestion.isAnswered)
            _selectedQuestion.questionOptionButton.MarkAsDone();
        monitorManager.SelectNewScreen(_currentScreen);
        _selectedQuestion = null;
    }
    
    public void ReturnToMainQuestionPage()
    {
        _lastScreen.SetActive(true);
        _currentScreen.SetActive(false);
        _nextScreen = _currentScreen;
        _currentScreen = _lastScreen;
        _lastScreen = null;
        monitorManager.SelectNewScreen(_currentScreen);
    }
}
