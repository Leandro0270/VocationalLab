using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
using Random = UnityEngine.Random;

public class PillCase : MonoBehaviour
{

    private ScObMedication _currentMedication;
    
    //Cap & label
    [SerializeField] private MeshRenderer caseBaseMeshRenderer;
    [SerializeField] private MeshRenderer caseLabelMeshRenderer;
    [SerializeField] private MeshRenderer caseCapMeshRenderer;
    [SerializeField] private Material[] lowControlLevelMaterials;
    [SerializeField] private Material[] mediumControlLevelMaterials;
    [SerializeField] private Material[] highControlLevelMaterials;

    
    
    //Decal
    [SerializeField] private DecalProjector decalProjector;
    [SerializeField] private Material lowControlIconDecalMaterial;
    [SerializeField] private Material mediumControlIconDecalMaterial;
    [SerializeField] private Material highControlIconDecalMaterial;
    
    //Panel
    [SerializeField] private GameObject medicationInformationPanel;
    [SerializeField] private TextMeshProUGUI medicationNameField;
    [SerializeField] private TextMeshProUGUI medicationPurposeField;
    [SerializeField] private TextMeshProUGUI medicationSideEffectsField;
    [SerializeField] private TextMeshProUGUI medicationDurationField;
    
    
    //Player interaction
    private bool _isPlayerHolding;
    private bool _isPlayerLooking;
    
    //XR
    [SerializeField] private XRGrabInteractable xrGrabInteractable;
    

    
    
    private void Awake()
    {
        medicationInformationPanel.SetActive(false);
        caseBaseMeshRenderer.enabled = false;
        caseLabelMeshRenderer.enabled = false;
        caseCapMeshRenderer.enabled = false;
    }

    //ShakeSounds (WIP - Not implemented yet)
    //[SerializeField] private AudioClip[] shakeCaseSounds;
    //[SerializeField] private AudioSource audioSource;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void SetPlayerHolding(bool isHolding)
    {
        if (isHolding)
        {
            medicationInformationPanel.SetActive(true);
            _isPlayerHolding = true;
        }
        else
        {
            medicationInformationPanel.SetActive(false);
            _isPlayerHolding = false;
        }
    }
    public void SetNewMedication(ScObMedication newMedication, XRInteractionManager interactionManager)
    {   xrGrabInteractable.enabled = true;
        xrGrabInteractable.interactionManager = interactionManager;
        _currentMedication = newMedication;
        caseBaseMeshRenderer.enabled = true;
        caseCapMeshRenderer.enabled = true;
        caseLabelMeshRenderer.enabled = true;
        SetControlLevel(_currentMedication.controlLevel);

        medicationNameField.text = _currentMedication.medicationName;
        medicationPurposeField.text = $"Tratamento de {_currentMedication.treatmentPurposeDescription}";
        medicationSideEffectsField.text = "";
        foreach (var sideEffect in newMedication.sideEffects)
        {
            medicationSideEffectsField.text += $"-{sideEffect}\n";
        }
        medicationDurationField.text = $"Regime de tratamento {_currentMedication.treatmentDuration}";
    }
    
    private void SetControlLevel(int controlLevel)
    {
        int randomIndexMaterial = 0;
        switch (controlLevel)
        {
            case 0:
                if (lowControlIconDecalMaterial == null || lowControlLevelMaterials.Length == 0)
                {
                    Debug.LogError("Faltou configurar os materiais de controle baixo no objeto" + gameObject.name + 
                        "\n" + "O icone está vazio? " + lowControlIconDecalMaterial == null + "\n" + "A lista de materiais está vazia? " + (lowControlLevelMaterials.Length < 1));
                    DestroyPillCase();
                    return;

                };
                randomIndexMaterial = Random.Range(0, lowControlLevelMaterials.Length);
                caseLabelMeshRenderer.material = lowControlLevelMaterials[randomIndexMaterial];
                caseCapMeshRenderer.material = lowControlLevelMaterials[randomIndexMaterial];
                decalProjector.material = lowControlIconDecalMaterial;
                break;
            case 1: 
                if (mediumControlIconDecalMaterial == null || mediumControlLevelMaterials.Length == 0)
                {
                    Debug.LogError("Faltou configurar os materiais de controle médio no objeto" + gameObject.name + 
                        "\n" + "O icone está vazio? " + mediumControlIconDecalMaterial == null + "\n" + "A lista de materiais está vazia? " + (mediumControlLevelMaterials.Length < 1));
                    DestroyPillCase();
                    return;

                };
                randomIndexMaterial = Random.Range(0, lowControlLevelMaterials.Length);
                caseLabelMeshRenderer.material = mediumControlLevelMaterials[randomIndexMaterial];
                caseCapMeshRenderer.material = mediumControlLevelMaterials[randomIndexMaterial];
                decalProjector.material = mediumControlIconDecalMaterial;
                break;
            case 2:
                
                if (highControlIconDecalMaterial == null || highControlLevelMaterials.Length == 0)
                {
                    Debug.LogError("Faltou configurar os materiais de controle médio no objeto" + gameObject.name + 
                        "\n" + "O icone está vazio? " + highControlIconDecalMaterial == null + "\n" + "A lista de materiais está vazia? " + (highControlLevelMaterials.Length < 1));
                    DestroyPillCase();
                    return;

                };
                caseLabelMeshRenderer.material = highControlLevelMaterials[randomIndexMaterial];
                caseCapMeshRenderer.material = highControlLevelMaterials[randomIndexMaterial];
                decalProjector.material = highControlIconDecalMaterial;
                break;
        }
    }
    
    public void DestroyPillCase()
    {
        Destroy(gameObject);
    }
}
