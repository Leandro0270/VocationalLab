using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SymptomsTextPanel : MonoBehaviour
{
    [SerializeField] private GameObject check;
    [SerializeField] private TextMeshProUGUI symptomText;
    
    
    public void SetSymptom(String symptom)
    {
        symptomText.text = symptom;
        check.SetActive(false);
    }
    
    public void CheckSymptom(String symptom)
    {
        if (symptomText.text.Trim().ToLower() != symptom.Trim().ToLower()) return;
        symptomText.text = $"<s> {symptomText.text} <s>";
        if(check != null)
            check.SetActive(true);
    }
}
