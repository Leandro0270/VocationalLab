using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ScObPhoneVoiceLines", menuName = "ScriptableObjects/ScObPhoneVoiceLines", order = 1)]
public class ScObPhoneVoiceLines : ScriptableObject
{
//Tutorial voice lines ========================================================================================
    public AudioClip introductionVoiceLine;
    public AudioClip firstStep;
    public AudioClip secondStep;
    public AudioClip thirdStep;
    public AudioClip fourthStep;
    public AudioClip ignoredVoiceLine;
    public AudioClip completedVoiceLine;
    public List<AudioClip> impatientVoiceLines;
    
    public List<AudioClip> wrongActionsVoiceLines;
    
    
    public List<AudioClip> correctActionsVoiceLines;
    //In game voice line====================================================================================================
    public List<AudioClip> hangUpVoiceLines;
    public List<AudioClip> patientComingVoiceLines;
}
