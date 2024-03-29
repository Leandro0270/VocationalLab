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
    [SerializeField] private TextMeshProUGUI mainPage_ageField;
    //=======================================================================
    private GameObject _currentPatientGameObject;
    private String _currentPatientName;
    private String _currentPatientAge;
    private String _currentPatientGender;
    private String _currentPatientJob;
    private Sprite _currentPatientPhoto;
    private String _currentPatientReportedSymptoms;
    private ScObMedication[] _currentPatientUsedMedication;
    private ScObPatientQuestion[] _currentPatientQuestions;
    private ScObDisease _currentPatientDisease;
    private ScObPatient _currentPatient;
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
    [SerializeField] private PatientBehavior patientBehavior;
    private int _timesPlayerAsked = 0;
    [SerializeField] private MonitorDiagnosis monitorDiagnosis;
    [SerializeField] private ScObDisease[] diseases;
    [SerializeField] private MonitorQuestions monitorQuestions;
    
    [SerializeField] private AudioSource patientAudioSource;
    [SerializeField] private AudioSource doctorAudioSource;
    [SerializeField] private AudioClip welcomeDoctorAudio;
    private AudioClip _currentPatientWelcomeAudioClip;
    private bool _doctorAudioPlaying;
    private bool _patientAudioPlaying;
    [SerializeField] private ScObPatient debugPatient;
    private void Awake()
    {
        _isMonitorOn = true;
        _currentScreen = mainPatientScreen;
        _instantiatedMedications = new List<PillCase>();
    }

    private void Start()
    {
    
        foreach (var disease in diseases)
        {  
            foreach (var symptom in disease.symptoms)
            {
                hintBook.AddHint(symptom.symptomName);

            }
        }
        
        foreach (var disease in diseases)
        {   
            hintBook.AddCause(disease.diseaseName, disease.causes, disease.symptoms);
            foreach (var symptom in disease.symptoms)
            {
                hintBook.AddHint(symptom.symptomName);

            }
        }
        
        StartAppointment(debugPatient, patientAudioSource);
    }
    
    
    private void StartAppointment(ScObPatient newPatient, AudioSource patientAudio)
    {
        SetNewPatient(newPatient, patientAudio);
        mainPage_diagnosisButton.interactable = false;
        mainPage_askButton.interactable = false;
        mainPage_medicationButton.interactable = false;
        foreach (var symptom in _currentPatient.firstSymptoms)
        {
            AddNewHint(symptom.symptomName);
        }
    }

    public IEnumerator StartTalk()
    {
        {
            if (_doctorAudioPlaying || _patientAudioPlaying)
            {
                if(_doctorAudioPlaying)
                    yield return new WaitWhile(() => doctorAudioSource.isPlaying);
            }

            doctorAudioSource.clip = welcomeDoctorAudio;
            StartCoroutine(DoctorThenPatientDialogue());
        }
    }
    
    
    private IEnumerator DoctorThenPatientDialogue()
    {
        _doctorAudioPlaying = true;
        doctorAudioSource.Play();
        yield return new WaitWhile(() => doctorAudioSource.isPlaying);
        _doctorAudioPlaying = false;
        yield return new WaitForSeconds(2);
        StartCoroutine(patientBehavior.StartDialogue(_currentPatientWelcomeAudioClip));
        yield return new WaitWhile(() => patientAudioSource.isPlaying);
        mainPage_diagnosisButton.interactable = true;
        mainPage_askButton.interactable = true;
        mainPage_medicationButton.interactable = true;

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
            _currentScreen = mainPatientScreen.activeSelf ? mainPatientScreen :
                medicationScreen.activeSelf ? medicationScreen :
                diagnosisScreen.activeSelf ? diagnosisScreen :
                examScreen.activeSelf ? examScreen :
                askScreen.activeSelf ? askScreen : mainPatientScreen;
            mainPatientScreen.SetActive(false);
            medicationScreen.SetActive(false);
            diagnosisScreen.SetActive(false);
            examScreen.SetActive(false);
            askScreen.SetActive(false);
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
        mainPatientScreen.SetActive(true);
        patientAudioSource= patientAudio;
        _currentPatientWelcomeAudioClip = newPatient.firstContactAudioClip;
        _currentPatient = newPatient;
        _currentPatientName = newPatient.fullName;
        _currentPatientGender = $"Sexo: {newPatient.gender}";
        _currentPatientJob = $"Profissão: {newPatient.job}";
        _currentPatientPhoto = newPatient.photo;
        _currentPatientAge = $"Idade {newPatient.age}";
        _currentPatientDisease = newPatient.disease;
        _currentPatientReportedSymptoms = "Sintomas relatados: \n" +
                                          $" {newPatient.reportedSymptoms}";
        _currentPatientQuestions = newPatient.questions;
        _currentPatientUsedMedication = newPatient.usedMedication;
        mainPage_useMedicationField.text = _currentPatientUsedMedication.Length > 0 ? "Usa remédio: Sim" : "Usa remédio: Não";
        mainPage_reportedSymptomsField.text = _currentPatientReportedSymptoms;
        mainPage_nameField.text = _currentPatientName;
        mainPage_ageField.text = _currentPatientAge;
        mainPage_genderField.text = _currentPatientGender;
        mainPage_jobField.text = _currentPatientJob;
        mainPage_imageField.sprite = _currentPatientPhoto;
        mainPage_medicationButton.enabled = _currentPatientUsedMedication.Length > 0;
        
        
    }

    public void OpenDiagnosticScreen()
    {
        diagnosisScreen.SetActive(true);
        mainPatientScreen.SetActive(false);
        monitorDiagnosis.OpenDiagnosticPanel();
        monitorDiagnosis.OrganizeDiseasesByProbability();
    }
    
    public void OpenQuestionScreen()
    {
        askScreen.SetActive(true);
        mainPatientScreen.SetActive(false);
        monitorQuestions.SetCurrentPatientQuestions(_currentPatientQuestions);
        monitorQuestions.OpenQuestionsPage();
        
    }
    
    
    //Medication methods
    public void SpawnMedication()
    {
        
        if (_currentPatientUsedMedication.Length < 1) return;
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
    
    public void ConfirmDiagnosis(ScObDisease disease)
    {
        Debug.Log(disease == _currentPatientDisease ? "Acertou" : "Errou");
    }
    public void AddTimesPlayerAsked()
    {
        _timesPlayerAsked++;
    }

    public void AddNewHint(String hint)
    {
        hintBook.CheckHint(hint);
        monitorDiagnosis.OrganizeDiseasesByProbability();
    }
    
    
    public AudioSource GetPatientAudio()
    {
        return patientAudioSource;
    }
    
    public void SetPatientAudioSource(AudioSource patientAudio)
    {
        patientAudioSource = patientAudio;
    }
    
    // get e setters de patientBehavior
    public PatientBehavior GetPatientBehavior()
    {
        return patientBehavior;
    }
    
    public ScObDisease GetPatientDisease()
    {
        return _currentPatientDisease;
    }
    
    public List<ScObDisease> GetDiseases()
    {
        return new List<ScObDisease>(diseases);
    }
    
    
    
    public void SetPatientBehavior(PatientBehavior patientBehavior)
    {
        this.patientBehavior = patientBehavior;
    }
    
    
    
}
