using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DiseasePanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI diseaseName;
    [SerializeField] private TextMeshProUGUI textPrefab;
    [SerializeField] private GameObject symptomsPanel;
    [SerializeField] private GameObject causesPanel;
    [SerializeField] private GameObject uniqueCausePanel;
    private List<TextMeshProUGUI> _symptomsTexts = new List<TextMeshProUGUI>();
    private List<TextMeshProUGUI> _causesTexts = new List<TextMeshProUGUI>();
    private int _probability=0;
    public void SetDisease(String diseaseTitle, ScObSymptoms[] symptoms, ScObCause[] causes)
    {
        diseaseName.text = diseaseTitle;
        foreach (var symptom in symptoms)
        {
            TextMeshProUGUI newSymptom = Instantiate(textPrefab, symptomsPanel.transform);
            newSymptom.text = symptom.symptomName;
            _symptomsTexts.Add(newSymptom);
        }
        
        if(causes.Length == 1)
        {
            uniqueCausePanel.SetActive(true);
            causesPanel.SetActive(false);
            TextMeshProUGUI newCause = uniqueCausePanel.GetComponent<TextMeshProUGUI>();
            newCause.text = causes[0].causeName;
            _causesTexts.Add(newCause);
            return;
        }
        foreach (var cause in causes)
        {
            TextMeshProUGUI newCause = Instantiate(textPrefab, causesPanel.transform);
            newCause.text = cause.causeName;
            _causesTexts.Add(newCause);
        }
        
    }
    
    
    public void CheckSymptoms(String symptoms)
    {
            foreach (var text in _symptomsTexts)
            {
                if (text.text.Trim().ToLower() != symptoms.Trim().ToLower()) continue;
                String newText = $"<s> {text.text} <s>";
                text.text = newText;
                _probability++;
            }
        
    }
    
    public void CheckCauses(String cause)
    {

            foreach (var text in _causesTexts)
            {
                if (text.text.Trim().ToLower() != cause.Trim().ToLower()) continue;
                String newText = $"<s> {text.text} <s>";
                text.text = newText;
                _probability++;
            }
        
    }
}
