using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class BookPage : MonoBehaviour
{
    private float _pageSize;
    private float _usedSize;

    [SerializeField] private GameObject hintPanel;
    [SerializeField] private GameObject causesPanel;

    [SerializeField] private RectTransform pageSpace;

    [SerializeField] private SymptomsTextPanel hintPrefab;
    [SerializeField] private DiseasePanel causePrefab;
    public List<Link> LinkList = new List<Link>();
    

    private bool _returnedLinks = false;

    
    private void Awake()
    {
        _pageSize = pageSpace.rect.height;
        _usedSize = 0;

    }
    
    public bool returnedLinks
    {
        get => _returnedLinks;
        set => _returnedLinks = value;
    }

    public bool AddNewHint(String hint)
    {

        _usedSize += ((hintPrefab.GetComponent<RectTransform>().rect.height+ 0.9f)/3);
        if (_usedSize >= _pageSize) return false;
        SymptomsTextPanel newHint = Instantiate(hintPrefab, hintPanel.transform);
        newHint.SetSymptom(hint);
        if (LinkList.Count > 0)
        {
            foreach (var link in LinkList)
            {
                if (link.HintName != hint) continue;
                link.Hints.Add(newHint);
            }
        }

        Link newLink = new Link();
        newLink.HintName = hint;
        newLink.Hints.Add(newHint);
        LinkList.Add(newLink);
        return true;

    }



    public bool AddNewCause(String diseaseName,ScObCause[] causes, ScObSymptoms[] hints)
    {
        DiseasePanel newCause = Instantiate(causePrefab, causesPanel.transform);
        _usedSize += (newCause.GetComponent<RectTransform>().rect.height + 1);
        if (_usedSize >= _pageSize)
        {
            Destroy(newCause.gameObject);
            return false;
        }
        
        newCause.SetDisease(diseaseName, hints, causes);
        bool foundHint = false;
        bool foundCause = false;
        if (LinkList.Count > 0)
        {
            foreach (var link in from link in LinkList from hint in hints where link.HintName == hint.symptomName select link)
            {
                link.Causes.Add(newCause);
                foundHint = true;
            }

            foreach (var link in from link in LinkList from cause in causes where link.HintName == cause.causeName select link)
            {
                link.Causes.Add(newCause);
                foundCause = true;
            }
        }

        if (!foundHint)
        {
            foreach (var hint in hints)
            {
                Link newLink = new Link();
                newLink.HintName = hint.symptomName;
                newLink.Causes.Add(newCause);
            }
        }

        if (foundCause) return true;
        
        foreach (var cause in causes)
        {
            Link newLink = new Link();
            newLink.HintName = cause.causeName;
            newLink.Causes.Add(newCause);
        }
        

        return true;
    }




    public class Link
    {
        public String HintName = "";
        public List<DiseasePanel> Causes = new();
        public List<SymptomsTextPanel> Hints = new();
    }
}



