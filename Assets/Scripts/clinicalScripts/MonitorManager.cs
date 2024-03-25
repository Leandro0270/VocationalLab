using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class MonitorManager : MonoBehaviour
{
    //=====================================================================
    [SerializeField] private GameObject mainPatientScreen;
    [SerializeField] private GameObject medicationScreen;
    [SerializeField] private GameObject diagnosisScreen;
    [SerializeField] private GameObject examScreen;
    [SerializeField] private GameObject askScreen;
    [SerializeField] private GameObject observationScreen;
    //Main Patient Page======================================================================
    [SerializeField] private Button mainPage_examButton;
    [SerializeField] private Button mainPage_askButton;
    [SerializeField] private Button mainPage_diagnosisButton;
    [SerializeField] private Button mainPage_medicationButton;
    //----------------------------------------------------------------------------------------
    [SerializeField] private TextMeshProUGUI mainPage_reportedSymptomsField;
    [SerializeField] private TextMeshProUGUI mainPage_nameField;
    [SerializeField] private TextMeshProUGUI mainPage_genderField;
    [SerializeField] private TextMeshProUGUI mainPage_jobField;
    [SerializeField] private TextMeshProUGUI mainPage_useMedicationField;
    [SerializeField] private Image mainPage_imageField;
    //=======================================================================
    private GameObject _currentPatientGameObject;
    private String _currentPatientName;
    private String _currentPatientGender;
    private String _currentPatientJob;
    private Sprite _currentPatientPhoto;
    private String _currentPatientReportedSymptoms;
    private ScObMedication[] _currentPatientUsedMedication;
    private ScObPatientQuestion[] _currentPatientQuestions;
    private ScObDisease _currentPatientDisease;
    //XR=======================================================================
    [SerializeField] private XRInteractionManager xrInteractionManager;
    //Prefabs=======================================================================
    [SerializeField] private GameObject medicationPrefab;
    
    //Positions=======================================================================
    [SerializeField] private Transform[] medicationSpawnPosition;
    [SerializeField] private Transform playerPosition;
    //Instantiated Objects=======================================================================
    private List<PillCase> _instantiatedMedications;
    
    //Others=======================================================================
    [SerializeField] private HologramProjector hologramProjector;
    [SerializeField] private HintBook hintBook;
    private bool _isMonitorOn;
    private GameObject _lastScreen;
    private GameObject _currentScreen;
    //=======================================================================
    private AudioSource _patientAudio;
    private PatientBehavior _patientBehavior;
    private int _timesPlayerAsked = 0;

    private void Awake()
    {
        _isMonitorOn = true;
        _currentScreen = mainPatientScreen;
        _instantiatedMedications = new List<PillCase>();
    }

    private void Update()
    {
        if (playerPosition == null) return;
        Vector3 targetDirection = playerPosition.position - transform.position;
        float yRotation = Quaternion.LookRotation(targetDirection).eulerAngles.y;
        transform.rotation = Quaternion.Euler(0, yRotation+180, 0);
    }
    
    public void SwitchMonitorState ()
    {
        if (_isMonitorOn)
        {
            _currentScreen = mainPatientScreen.activeSelf ? mainPatientScreen : medicationScreen.activeSelf ? medicationScreen : diagnosisScreen.activeSelf ? diagnosisScreen : examScreen.activeSelf ? examScreen : askScreen.activeSelf ? askScreen : observationScreen;
            mainPatientScreen.SetActive(false);
            medicationScreen.SetActive(false);
            diagnosisScreen.SetActive(false);
            examScreen.SetActive(false);
            askScreen.SetActive(false);
            observationScreen.SetActive(false);
            _isMonitorOn = false;
        }
        else
        {
            _currentScreen.SetActive(true);
            _isMonitorOn = true;
        }
        hologramProjector.ToggleSpin(_isMonitorOn);

    }
    
    public void SelectNewScreen(GameObject newScreen)
    {
        if (!_isMonitorOn) return;
        _lastScreen = _currentScreen;
        _lastScreen.SetActive(false);
        _currentScreen = newScreen;
        _currentScreen.SetActive(true);
        
    }
    
    public void ReturnToLastScreen()
    {
        if (!_isMonitorOn) return;
        _currentScreen.SetActive(false);
        _currentScreen = _lastScreen;
        _currentScreen.SetActive(true);
    }

    public void SetNewPatient(ScObPatient newPatient, AudioSource patientAudio)
    {
        medicationScreen.SetActive(false);
        diagnosisScreen.SetActive(false);
        examScreen.SetActive(false);
        askScreen.SetActive(false);
        medicationScreen.SetActive(false);
        observationScreen.SetActive(false);
        mainPatientScreen.SetActive(true);
        _patientAudio = patientAudio;
        _currentPatientName = newPatient.fullName;
        _currentPatientGender = $"Sexo: {newPatient.gender}";
        _currentPatientJob = $"Profissão: {newPatient.job}";
        _currentPatientPhoto = newPatient.photo;
        _currentPatientDisease = newPatient.disease;
        _currentPatientReportedSymptoms = "Sintomas relatados: \n" +
                                          $" {newPatient.reportedSymptoms}";
        _currentPatientQuestions = newPatient.questions;

        _currentPatientUsedMedication = newPatient.usedMedication;
        mainPage_useMedicationField.text = _currentPatientUsedMedication.Length > 0 ? "Usa remédio: Sim" : "Usa remédio: Não";
        mainPage_reportedSymptomsField.text = _currentPatientReportedSymptoms;
        mainPage_nameField.text = _currentPatientName;
        mainPage_genderField.text = _currentPatientGender;
        mainPage_jobField.text = _currentPatientJob;
        mainPage_imageField.sprite = _currentPatientPhoto;
        mainPage_medicationButton.enabled = _currentPatientUsedMedication.Length > 0;
        
    }

    
    
    //Medication methods
    public void SpawnMedication()
    {
        
        if (_currentPatientUsedMedication.Length <= 0) return;
        if(_instantiatedMedications.Count > 0)
            foreach (var medication in _instantiatedMedications)
            {
                medication.DestroyPillCase();
            }
        if (medicationSpawnPosition.Length < 1)
        {
            
            Debug.LogError("Faltou configurar os spawn points dos remédios!");
        }
       
        var spawnPositionIndex = 0;
        foreach (var medication in _currentPatientUsedMedication)
        {
            PillCase newMedication = Instantiate(medicationPrefab, medicationSpawnPosition[spawnPositionIndex].position, Quaternion.identity).GetComponent<PillCase>();
             newMedication.SetNewMedication(medication, xrInteractionManager);
            _instantiatedMedications.Add(newMedication);
            if (spawnPositionIndex == medicationSpawnPosition.Length - 1)
                spawnPositionIndex = 0;
            else
                spawnPositionIndex++;
        }
    }
    
    
    public void AddTimesPlayerAsked()
    {
        _timesPlayerAsked++;
    }

    public void AddNewHint(String hint)
    {
        hintBook.CheckHint(hint);
    }
    
    
    public AudioSource GetPatientAudio()
    {
        return _patientAudio;
    }
    
    public void SetPatientAudio(AudioSource patientAudio)
    {
        _patientAudio = patientAudio;
    }
    
    // get e setters de patientBehavior
    public PatientBehavior GetPatientBehavior()
    {
        return _patientBehavior;
    }
    
    public void SetPatientBehavior(PatientBehavior patientBehavior)
    {
        _patientBehavior = patientBehavior;
    }
    
    
    
}
