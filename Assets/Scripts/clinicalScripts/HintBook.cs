using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class HintBook : MonoBehaviour
{   [SerializeField] Rigidbody bookRigidbody;
    [SerializeField] private HingeJoint pagePrefab;
    [SerializeField] private int numberOfPages;
    [SerializeField] private float pageSpacing;
    [SerializeField] private float pagePositionZ;
    [SerializeField] private float pagePositionX;
    [SerializeField] private float startPagePositionY;
    [SerializeField] private Transform bookCover;
    private List<HingeJoint> _pages = new List<HingeJoint>();
    private List<BookPage> _bookPages = new List<BookPage>();
    private List<BookPage.Link> _links;
    

    

    private void Awake()
    {
        for (int i = 0; i < numberOfPages; i++)
        {
            HingeJoint page = Instantiate(pagePrefab, transform);
            _pages.Add(page);
            page.transform.localPosition = new Vector3(pagePositionX, startPagePositionY +(pageSpacing*_pages.Count) ,pagePositionZ);
            page.connectedBody = bookRigidbody;
            page.anchor = new Vector3(page.anchor.x + (pageSpacing*_pages.Count), page.anchor.y + (pageSpacing*_pages.Count), page.anchor.z);
            float currentMaxLimit = page.limits.max;
            page.limits = new JointLimits {min = 0, max = currentMaxLimit - (pageSpacing*_pages.Count*100)};
            _bookPages.Add(page.GetComponent<BookPage>());
        }
    }
    

    public void CheckHint(String hint)
    {
        foreach (var link in _links.Where(link => link.HintName == hint))
        {
            foreach (var checkHint in link.Hints)
                checkHint.CheckSymptom(hint);
            

            foreach (var checkHint in link.Causes)
                checkHint.CheckCauses(hint);
        }
    }
    public void AddHint(String hint)
    {
        bool importNext= false;
        foreach (var bookPage in _bookPages)
        {
            if (importNext)
            {
                foreach (var link in _links)
                {
                    bookPage.LinkList.Add(link);
                }
                importNext = false;
            }
            if (bookPage.AddNewHint(hint))
            {
                return;
            }

            if (bookPage.returnedLinks) continue;
            _links = bookPage.LinkList;
            bookPage.returnedLinks = true;
            importNext = true;
        }
    }
    
    public void AddCause(List<String> causes, List<String> hints)
    {
        bool importNext= false;
        foreach (var bookPage in _bookPages)
        {
            if (importNext)
            {
                foreach (var link in _links)
                {
                    bookPage.LinkList.Add(link);
                }
                importNext = false;
            }
            if (bookPage.AddNewCause(causes, hints))
            {
                return;
            }

            if (bookPage.returnedLinks) continue;
            _links = bookPage.LinkList;
            bookPage.returnedLinks = true;
            importNext = true;
        }
    }
}
