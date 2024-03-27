using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MonitorDiagnosis : MonoBehaviour
{
    [SerializeField] private MonitorManager monitorManager;
    [SerializeField] private GameObject diseaseOptionsPanel;
    [SerializeField] private int numberOfDiseasesToShow = 3;
    [SerializeField] private GameObject diseaseInfoPanel;
    
    [SerializeField] private TextMeshProUGUI diseaseInfoName;
    [SerializeField] private TextMeshProUGUI diseaseInfoSymptoms;
    [SerializeField] private TextMeshProUGUI diseaseInfoCauses;

    [SerializeField] private TextMeshProUGUI[] nameDiseaseOptions;
    [SerializeField] private TextMeshProUGUI[] probabilityDiseaseOptions;
    
    private ScObDisease _button1Disease;
    private ScObDisease _button2Disease;
    private ScObDisease _button3Disease;
    
    [SerializeField] private TextMeshProUGUI pageNumberText;
    private int _currentPageIndex = 0;
    private int _totalPages;
    private List<ScObDisease> _diseases;
    private List<ScObDisease> _diseasesOrganizedByProbability = new List<ScObDisease>();
    
    //infos
    private ScObDisease _currentDiseaseSelected;
    private String _currentDiseaseName;
    private List<String> _currentDiseaseSymptoms;
    private List<String> _currentDiseaseCauses;

    private void Start()
    {

        _diseases = new List<ScObDisease>(monitorManager.GetDiseases());
        foreach (var disease in _diseases)
        {
            _diseasesOrganizedByProbability.Add(disease);
        }
        _totalPages = _diseases.Count / numberOfDiseasesToShow;
        if (_diseases.Count % numberOfDiseasesToShow != 0)
        {
            _totalPages++;
        }
        pageNumberText.text = _currentPageIndex + 1 + "/" + _totalPages;
        
        nameDiseaseOptions[0].text = _diseasesOrganizedByProbability[0].diseaseName;
        nameDiseaseOptions[1].text = _diseasesOrganizedByProbability[1].diseaseName;
        nameDiseaseOptions[2].text = _diseasesOrganizedByProbability[2].diseaseName;
        
        probabilityDiseaseOptions[0].text = "Evidências coletadas: " + _diseasesOrganizedByProbability[0].currentProbability + "/" + 
                                            (_diseasesOrganizedByProbability[0].symptoms.Length + _diseasesOrganizedByProbability[0].causes.Length);
        
        probabilityDiseaseOptions[1].text = "Evidências coletadas: " + _diseasesOrganizedByProbability[1].currentProbability + "/" + 
                                    (_diseasesOrganizedByProbability[1].symptoms.Length + _diseasesOrganizedByProbability[1].causes.Length);

        probabilityDiseaseOptions[2].text = "Evidências coletadas: " + _diseasesOrganizedByProbability[2].currentProbability + "/" + 
                                    (_diseasesOrganizedByProbability[2].symptoms.Length + _diseasesOrganizedByProbability[2].causes.Length);
        
        _button1Disease = _diseasesOrganizedByProbability[0];
        _button2Disease = _diseasesOrganizedByProbability[1];
        _button3Disease = _diseasesOrganizedByProbability[2];
    }
    
    public void SelectOption(int option)
    {
        _currentDiseaseSelected = option switch
        {
            1 => _button1Disease,
            2 => _button2Disease,
            3 => _button3Disease,
            _ => _currentDiseaseSelected
        };
        _currentDiseaseName = _currentDiseaseSelected.diseaseName;
        _currentDiseaseSymptoms = new List<string>(_currentDiseaseSelected.symptoms);
        _currentDiseaseCauses = new List<string>(_currentDiseaseSelected.causes);
        diseaseInfoName.text = _currentDiseaseName;
        diseaseInfoSymptoms.text = "Sintomas: ";
        foreach (var symptom in _currentDiseaseSymptoms)
        {
            diseaseInfoSymptoms.text += symptom + ", ";
        }
        diseaseInfoCauses.text = "Causas: ";
        foreach (var cause in _currentDiseaseCauses)
        {
            diseaseInfoCauses.text += cause + ", ";
        }
        diseaseOptionsPanel.SetActive(false);
        diseaseInfoPanel.SetActive(true);
    }
    public void OpenDiagnosticPanel()
    {
        diseaseOptionsPanel.SetActive(true);
        diseaseInfoPanel.SetActive(false);
        OrganizeDiseasesByProbability();
        _currentPageIndex = 0;
        pageNumberText.text = _currentPageIndex + 1 + "/" + _totalPages;
        
        nameDiseaseOptions[0].text = _diseasesOrganizedByProbability[0].diseaseName;
        nameDiseaseOptions[1].text = _diseasesOrganizedByProbability[1].diseaseName;
        nameDiseaseOptions[2].text = _diseasesOrganizedByProbability[2].diseaseName;
        
        probabilityDiseaseOptions[0].text = "Evidências coletadas: " + _diseasesOrganizedByProbability[0].currentProbability + "/" + 
                                            (_diseasesOrganizedByProbability[0].symptoms.Length + _diseasesOrganizedByProbability[0].causes.Length);
        
        probabilityDiseaseOptions[1].text = "Evidências coletadas: " + _diseasesOrganizedByProbability[1].currentProbability + "/" + 
                                            (_diseasesOrganizedByProbability[1].symptoms.Length + _diseasesOrganizedByProbability[1].causes.Length);

        probabilityDiseaseOptions[2].text = "Evidências coletadas: " + _diseasesOrganizedByProbability[2].currentProbability + "/" + 
                                            (_diseasesOrganizedByProbability[2].symptoms.Length + _diseasesOrganizedByProbability[2].causes.Length);
        
        _button1Disease = _diseasesOrganizedByProbability[0];
        _button2Disease = _diseasesOrganizedByProbability[1];
        _button3Disease = _diseasesOrganizedByProbability[2];
    }
    public void NextPage()
    {
        if (_currentPageIndex == _totalPages - 1) return;
        _currentPageIndex++;
        pageNumberText.text = _currentPageIndex + 1 + "/" + _totalPages;
        nameDiseaseOptions[0].text = _diseasesOrganizedByProbability[_currentPageIndex * numberOfDiseasesToShow].diseaseName;
        nameDiseaseOptions[1].text = _diseasesOrganizedByProbability[_currentPageIndex * numberOfDiseasesToShow + 1].diseaseName;
        nameDiseaseOptions[2].text = _diseasesOrganizedByProbability[_currentPageIndex * numberOfDiseasesToShow + 2].diseaseName;
        
        probabilityDiseaseOptions[0].text = "Evidências coletadas: " + _diseasesOrganizedByProbability[_currentPageIndex * numberOfDiseasesToShow].currentProbability + "/" + 
                                            (_diseasesOrganizedByProbability[_currentPageIndex * numberOfDiseasesToShow].symptoms.Length + _diseasesOrganizedByProbability[_currentPageIndex * numberOfDiseasesToShow].causes.Length);
        
        probabilityDiseaseOptions[1].text = "Evidências coletadas: " + _diseasesOrganizedByProbability[_currentPageIndex * numberOfDiseasesToShow + 1].currentProbability + "/" + 
                                            (_diseasesOrganizedByProbability[_currentPageIndex * numberOfDiseasesToShow + 1].symptoms.Length + _diseasesOrganizedByProbability[_currentPageIndex * numberOfDiseasesToShow + 1].causes.Length);
        
        probabilityDiseaseOptions[2].text = "Evidências coletadas: " + _diseasesOrganizedByProbability[_currentPageIndex * numberOfDiseasesToShow + 2].currentProbability + "/" + 
                                            (_diseasesOrganizedByProbability[_currentPageIndex * numberOfDiseasesToShow + 2].symptoms.Length + _diseasesOrganizedByProbability[_currentPageIndex * numberOfDiseasesToShow + 2].causes.Length);
        
        _button1Disease = _diseasesOrganizedByProbability[_currentPageIndex * numberOfDiseasesToShow];
        _button2Disease = _diseasesOrganizedByProbability[_currentPageIndex * numberOfDiseasesToShow + 1];
        _button3Disease = _diseasesOrganizedByProbability[_currentPageIndex * numberOfDiseasesToShow + 2];
    }
    
    public void PreviousPage()
    {
        if (_currentPageIndex == 0) return;
        _currentPageIndex--;
        pageNumberText.text = _currentPageIndex + 1 + "/" + _totalPages;
        nameDiseaseOptions[0].text = _diseasesOrganizedByProbability[_currentPageIndex * numberOfDiseasesToShow].diseaseName;
        nameDiseaseOptions[1].text = _diseasesOrganizedByProbability[_currentPageIndex * numberOfDiseasesToShow + 1].diseaseName;
        nameDiseaseOptions[2].text = _diseasesOrganizedByProbability[_currentPageIndex * numberOfDiseasesToShow + 2].diseaseName;
        
        probabilityDiseaseOptions[0].text = "Evidências coletadas: " + _diseasesOrganizedByProbability[_currentPageIndex * numberOfDiseasesToShow].currentProbability + "/" + 
                                            (_diseasesOrganizedByProbability[_currentPageIndex * numberOfDiseasesToShow].symptoms.Length + _diseasesOrganizedByProbability[_currentPageIndex * numberOfDiseasesToShow].causes.Length);
        
        probabilityDiseaseOptions[1].text = "Evidências coletadas: " + _diseasesOrganizedByProbability[_currentPageIndex * numberOfDiseasesToShow + 1].currentProbability + "/" + 
                                            (_diseasesOrganizedByProbability[_currentPageIndex * numberOfDiseasesToShow + 1].symptoms.Length + _diseasesOrganizedByProbability[_currentPageIndex * numberOfDiseasesToShow + 1].causes.Length);
        
        probabilityDiseaseOptions[2].text = "Evidências coletadas: " + _diseasesOrganizedByProbability[_currentPageIndex * numberOfDiseasesToShow + 2].currentProbability + "/" + 
                                            (_diseasesOrganizedByProbability[_currentPageIndex * numberOfDiseasesToShow + 2].symptoms.Length + _diseasesOrganizedByProbability[_currentPageIndex * numberOfDiseasesToShow + 2].causes.Length);
        
        _button1Disease = _diseasesOrganizedByProbability[_currentPageIndex * numberOfDiseasesToShow];
        _button2Disease = _diseasesOrganizedByProbability[_currentPageIndex * numberOfDiseasesToShow + 1];
        _button3Disease = _diseasesOrganizedByProbability[_currentPageIndex * numberOfDiseasesToShow + 2];
    }

    

    public void OrganizeDiseasesByProbability()
    {
        //Compara toda a lista de doenças e organiza pelo número de probabilidade maior para o menor
        _diseasesOrganizedByProbability.Sort((disease1, disease2) => disease2.currentProbability.CompareTo(disease1.currentProbability));
    }

    
    public void ReturnToOptions()
    {
        diseaseInfoPanel.SetActive(false);
        diseaseOptionsPanel.SetActive(true);
        _currentDiseaseSelected = null;
    }
    
    public void ConfirmDiagnosis()
    {
        monitorManager.ConfirmDiagnosis(_currentDiseaseSelected);
    }



}
