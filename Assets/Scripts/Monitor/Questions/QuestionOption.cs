using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class QuestionOption : MonoBehaviour
{
    [SerializeField] private GameObject questionDetailsPanel;
    [SerializeField] private GameObject lockedPanel;
    [SerializeField] private TextMeshProUGUI questionNumber;
    [SerializeField] private TextMeshProUGUI questionText;
    [SerializeField] private GameObject notificationIcon;
    [SerializeField] private GameObject checkIcon;
    [SerializeField] private GameObject questionIcon;
    private bool _isUnlocked;
    private ScObPatientQuestion _question;
    private int _questionNumber;
    private bool _questionDone;
    
    private MonitorQuestions _monitorQuestions;
    
    
    
    
    public void SetQuestion(ScObPatientQuestion question, int questionNumberId, MonitorQuestions monitorQuestions)
    {
        _questionNumber = questionNumberId;
        _question = question;
        _monitorQuestions = monitorQuestions;
        _question.questionOptionButton = this;
        questionNumber.text = $"{_questionNumber.ToString()} -";
        questionText.text = _question.title != "" ? _question.title : _question.question;
        _isUnlocked = _question.isUnlocked;
        questionDetailsPanel.SetActive(_isUnlocked);
        lockedPanel.SetActive(!_isUnlocked);
        notificationIcon.SetActive(!_isUnlocked);
        checkIcon.SetActive(!_isUnlocked);
        questionIcon.SetActive(_isUnlocked);
    }


    public void UnlockQuestion()
    {
        _isUnlocked = true;
        questionDetailsPanel.SetActive(_isUnlocked);
        lockedPanel.SetActive(!_isUnlocked);
        notificationIcon.SetActive(true);
        checkIcon.SetActive(!_isUnlocked);
        questionIcon.SetActive(_isUnlocked);
    }

    public void SelectQuestion()
    {
        if(!_isUnlocked) return;
        notificationIcon.SetActive(false);
        questionIcon.SetActive(true);
        _monitorQuestions.SelectQuestion(_question);
        
    }
    
    public void MarkAsDone()
    {
        _questionDone = true;
        checkIcon.SetActive(_questionDone);
        questionIcon.SetActive(!_questionDone);
        notificationIcon.SetActive(!_questionDone);
    }
    
    
}
