using System;
using System.Collections;
using System.Collections.Generic;
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
        _usedSize = _pageSize;

    }
    
    public bool returnedLinks
    {
        get => _returnedLinks;
        set => _returnedLinks = value;
    }

    public bool AddNewHint(String hint)
    {
        _usedSize += hintPrefab.GetComponent<RectTransform>().rect.height;
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
        LinkList.Add(newLink);
        return true;

    }



    public bool AddNewCause(List<String> causes, List<String> hints)
    {
        _usedSize += causePrefab.GetComponent<RectTransform>().rect.height;
        if (_usedSize >= _pageSize) return false;
        DiseasePanel newCause = Instantiate(causePrefab, causesPanel.transform);
        newCause.SetDisease("Disease", hints, causes);
        bool foundHint = false;
        bool foundCause = false;
        if (LinkList.Count > 0)
        {
            var index = 0;
            foreach (var link in LinkList)
            {
                if (link.HintName == hints[index])
                {
                    link.Causes.Add(newCause);
                    foundHint = true;
                }

                ;
                index++;
            }

            index = 0;
            foreach (var link in LinkList)
            {
                if (link.HintName == causes[index])
                {
                    link.Causes.Add(newCause);
                    foundCause = true;
                }

                ;
                index++;
            }
        }

        if (!foundHint)
        {
            foreach (var hint in hints)
            {
                Link newLink = new Link();
                newLink.HintName = hint;
                newLink.Causes.Add(newCause);
            }
        }

        if (foundCause) return true;
        
        foreach (var cause in causes)
        {
            Link newLink = new Link();
            newLink.HintName = cause;
            newLink.Causes.Add(newCause);
        }
        

        return true;
    }




    public class Link
    {
        public String HintName;
        public List<DiseasePanel> Causes;
        public List<SymptomsTextPanel> Hints;

    }
}



