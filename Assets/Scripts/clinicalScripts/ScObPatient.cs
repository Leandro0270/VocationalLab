using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[CreateAssetMenu(fileName = "ScObPatient", menuName = "ScriptableObjects/ScObPatient", order = 1)]

public class ScObPatient : ScriptableObject
{
    [Header("General Info----------------")]
    public int id;

    public AudioClip firstContactAudioClip;
    public string fullName;
    public string gender;
    public int age;
    public string job;
    public ScObDisease disease;
    public string reportedSymptoms;
    public Sprite photo;
    public ScObMedication[] usedMedication;
    public ScObPatientQuestion[] questions;
    public ScObSymptoms[] firstSymptoms;
    [Range(0,5)]
    public int diagnosisComplexity;


}
