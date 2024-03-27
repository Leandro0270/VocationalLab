using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "ScObDisease", menuName = "ScriptableObjects/ScObDisease", order = 2)]

public class ScObDisease : ScriptableObject
{
    public int id;
    public String diseaseName;
    public String[] symptoms;
    public String[] causes;
    public AudioClip treatmentExplication;
    
    
    //Tools
    [Header("Tools ----------------")]
    public bool requireStethoscope = false;
    public bool requireSphygmomanometer = false;
    public bool requireThermometer = false;
    public bool requireOphthalmoscope = false;
    public bool requireOtoscope = false;
    public bool requireReflexHammer = false;
    public bool requirePenlight = false;

    //Exams
    [Header("Exams----------------")]
    public bool requireUltrasound = false;
    public bool requireXray = false;
    public bool requireUrineTests = false;
    public bool requireBloodTest = false;
    public bool requireElectrocardiogram = false;
    
    [Header("Exams Results----------------")]
    public String electrocardiogramResult = "Resultados dentro dos padrões";
    public String urineTestResult = "Resultados dentro dos padrões";
    public String xrayResult = "Resultados dentro dos padrões";
    public String ultrasoundResult = "Resultados dentro dos padrões";
    public String bloodTestResult = "Resultados dentro dos padrões";
    
    public int currentProbability = 0;









}
