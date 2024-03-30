using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScObMedication", menuName = "ScriptableObjects/ScObMedication", order = 1)]
public class ScObMedication : ScriptableObject
{
    public String medicationName;
    public string treatmentPurposeDescription;
    public ScObDisease diseaseTreatment;
    public String[] sideEffects;
    public String treatmentDuration;
    [Range(0,3)]
    public int controlLevel;

}
