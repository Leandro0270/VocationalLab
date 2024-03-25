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
    private List<TextMeshProUGUI> _symptomsTexts = new List<TextMeshProUGUI>();
    private List<TextMeshProUGUI> _causesTexts = new List<TextMeshProUGUI>();

    public void SetDisease(String diseaseTitle, List<String> symptoms, List<String> causes)
    {
        diseaseName.text = diseaseTitle;
        foreach (var symptom in symptoms)
        {
            TextMeshProUGUI newSymptom = Instantiate(textPrefab, symptomsPanel.transform);
            newSymptom.text = symptom;
            _symptomsTexts.Add(newSymptom);
        }
        foreach (var cause in causes)
        {
            TextMeshProUGUI newCause = Instantiate(textPrefab, causesPanel.transform);
            newCause.text = cause;
            _causesTexts.Add(newCause);
        }
    }
    
    
    public void CheckSymptoms(String symptoms)
    {
            foreach (var text in _symptomsTexts)
            {
                if (text.text != symptoms) continue;
                String newText = $"<s> {text.text} <s>";
                text.text = newText;
            }
        
    }
    
    public void CheckCauses(String cause)
    {

            foreach (var text in _causesTexts)
            {
                if (text.text != cause) continue;
                String newText = $"<s> {text.text} <s>";
                text.text = newText;
            }
        
    }
}
