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
    [SerializeField] private AudioClip printerEndSound;
    //Adicionar scanner futuramente
    private TextMeshPro _textMeshPro;
    private TextMeshProUGUI _textMeshProUGUI;
    private List<GameObject> _spawnedPapers;
    private String _textToPrint;
    private bool _isPrinting;
    private int _paperSpawnlimit = 10;
    
    
    
    private void Awake()
    {
        _spawnedPapers = new List<GameObject>();
    }
    
    public void PrintText(String text)
    {
        if(_isPrinting) return;
        _textToPrint = text;
        if(_paperSpawnlimit <= _spawnedPapers.Count)
        {
            GameObject paper = _spawnedPapers[0];
            _spawnedPapers.RemoveAt(0);
            Destroy(paper);
        }
        StartCoroutine(PrintPaper());
    }
    
    private IEnumerator PrintPaper()
    {
        _isPrinting = true;
        printerAudio.clip = printerStartSound;
        printerAudio.Play();
        GameObject paper = Instantiate(paperPrefab, paperSpawnPoint.position, paperSpawnPoint.rotation);
        _spawnedPapers.Add(paper);
        _textMeshPro = paper.GetComponent<TextMeshPro>();
        if (_textMeshPro == null)
        {
            _textMeshPro = paper.GetComponentInChildren<TextMeshPro>();
            if(_textMeshPro == null)
            {
                _textMeshProUGUI = paper.GetComponent<TextMeshProUGUI>();
                if (_textMeshProUGUI == null)
                {
                    _textMeshProUGUI = paper.GetComponentInChildren<TextMeshProUGUI>();
                    if(_textMeshProUGUI == null)
                        Debug.LogError("Não achou o tmpro");
                    else
                    
                        _textMeshPro.text = _textToPrint;
                }else
                    _textMeshProUGUI.text = _textToPrint;
                
            }else
                _textMeshPro.text = _textToPrint;
        }
        else
            _textMeshPro.text = _textToPrint;
        while (Vector3.Distance(paper.transform.position, paperExitPoint.position) > 0.1f)
        {
            paper.transform.position = Vector3.MoveTowards(paper.transform.position, paperExitPoint.position, 0.1f);
            yield return null;
        }
        printerAudio.Stop();
        printerAudio.clip = printerEndSound;
        printerAudio.Play();
        
        _isPrinting = false;
    }

}
