using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Printer : MonoBehaviour
{
    [SerializeField] private GameObject paperPrefab;
    [SerializeField] private Transform paperSpawnPoint;
    [SerializeField] private Transform paperExitPoint;
    [SerializeField] private Transform hitPoint;
    [SerializeField] private AudioSource printerAudio;
    [SerializeField] private AudioClip printerStartSound;
    [SerializeField] private AudioClip printerMidSound;
    [SerializeField] private AudioClip printerEndSound;
    //Adicionar scanner futuramente
    private List<TextMeshPro> _spawnedPapers;
    private String _textToPrint;
    private bool _isPrinting;
    [SerializeField] private int paperSpawnlimit = 10;
    [SerializeField] private float printSpeed = 0.1f;
    [SerializeField]private bool debug;
    [SerializeField] private String debugText;

    private void Update()
    {
        if (debug)
        {
            PrintText(debugText,debugText);
            debug = false;
        }
    }

    private void Awake()
    {
        _spawnedPapers = new List<TextMeshPro>();
    }
    
    public void PrintText(String question, String answer )
    {
        if(_isPrinting) return;
        _textToPrint = $"<color=red>Você: {question}</color=red>\n \n<color=blue>Paciente: {answer}</color=blue>";
        if(paperSpawnlimit <= _spawnedPapers.Count)
        {
            foreach (var paperText in _spawnedPapers)
            {
                if (_textToPrint != paperText.text) continue;
                GameObject paper = paperText.gameObject.transform.parent.gameObject;
                _spawnedPapers.Remove(paperText);
                Destroy(paper);
                break;
            } ;
        }
        StartCoroutine(PrintPaper());
    }
    
    private IEnumerator PrintPaper()
    {
        float totalDistance = Vector3.Distance(paperSpawnPoint.position, paperExitPoint.position);
        float speed = totalDistance / printSpeed;
        _isPrinting = true;
        printerAudio.clip = printerStartSound;
        printerAudio.Play();
        GameObject paper = Instantiate(paperPrefab, paperSpawnPoint.position, paperSpawnPoint.rotation);
        TextMeshPro paperText = paper.GetComponentInChildren<TextMeshPro>();
        _spawnedPapers.Add(paperText);
        paperText.text = _textToPrint;
        
        while (Vector3.Distance(paper.transform.position, paperExitPoint.position) > 0.1f)
        {
            if(printerAudio.clip == printerStartSound && printerAudio.isPlaying == false)
            {
                printerAudio.clip = printerMidSound;
                printerAudio.Play();
                printerAudio.loop = true;
            }
            else if (printerAudio.clip== printerMidSound)
            {
                paper.transform.position = Vector3.MoveTowards(paper.transform.position, paperExitPoint.position, speed * Time.deltaTime);
            }

            yield return null;
        }
        printerAudio.loop = false;
        printerAudio.clip = printerEndSound;
        printerAudio.Play();
        
        _isPrinting = false;
    }

}
