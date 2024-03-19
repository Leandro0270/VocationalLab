using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[CreateAssetMenu(fileName = "ScObPatientQuestion", menuName = "ScriptableObjects/ScObPatientQuestion", order = 1)]
    public class ScObPatientQuestion : ScriptableObject
    {
        public int id;
        public int categoryId;
        public bool isStarterQuestion;
        public bool isUnlocked;
        public string title;
        public string question;
        public bool isAnswered;
        public string answer;
        public AudioClip answerAudio;
        public ScObPatientQuestion[] unlockQuestions;
        public ScObPatientQuestion unlockedByQuestion;
        public string unlockHint = "";
        public QuestionOption questionOptionButton;
    }
