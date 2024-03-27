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
    [SerializeField] private float pageSpaceAngle;
    [SerializeField] private Transform bookCover;
    private List<HingeJoint> _pages = new List<HingeJoint>();
    private List<BookPage> _bookPages = new List<BookPage>();
    private List<BookPage.Link> _links;
    private List<String> _hints = new List<string>();
    private bool _importNext;


    

    private void Awake()
    {
        for (int i = 0; i < numberOfPages; i++)
        {
            HingeJoint page = Instantiate(pagePrefab, transform);
            _pages.Add(page);
            page.transform.localPosition = new Vector3(pagePositionX, startPagePositionY -(pageSpacing*_pages.Count) ,pagePositionZ);
            page.connectedBody = bookRigidbody;
            page.limits = new JointLimits {min = 0, max = page.limits.max - pageSpaceAngle*(_pages.Count-1)};
            PageSideLink sides = page.GetComponent<PageSideLink>();
            _bookPages.Add(sides.bookPageSideA);
            _bookPages.Add(sides.bookPageSideB);
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
            
            foreach (var checkHint in link.Causes)
                checkHint.CheckSymptoms(hint);

        }
    }
    public void AddHint(String hint)
    {
        if(_hints.Contains(hint.Trim().ToLower())) return;
        _hints.Add(hint.Trim().ToLower());
        foreach (var bookPage in _bookPages)
        {
            if (_importNext)
            {
                foreach (var link in _links)
                {
                    bookPage.LinkList.Add(link);
                }
                _importNext = false;
            }
            if (bookPage.AddNewHint(hint))
            {
                return;
            }

            if (bookPage.returnedLinks) continue;
            _links = bookPage.LinkList;
            bookPage.returnedLinks = true;
            _importNext = true;
        }
    }

    public void AddCause(String diseaseName, String[] causes, String[] hints)
    {
        while (true)
        {
            foreach (var bookPage in _bookPages)
            {
                if (_importNext)
                {
                    foreach (var link in _links)
                    {
                        bookPage.LinkList.Add(link);
                    }

                    _importNext = false;
                }

                bool conseguiu = bookPage.AddNewCause(diseaseName, causes, hints);
                if (conseguiu)
                {
                    return;
                }


                if (bookPage.returnedLinks) continue;
                _links = bookPage.LinkList;
                bookPage.returnedLinks = true;
                _importNext = true;
            }

            AddMorePages();
        }
    }


    private void AddMorePages()
    {
        HingeJoint page = Instantiate(pagePrefab, transform);
        _pages.Add(page);
        page.transform.localPosition =
            new Vector3(pagePositionX, startPagePositionY - (pageSpacing * _pages.Count), pagePositionZ);
        page.connectedBody = bookRigidbody;
        page.limits = new JointLimits { min = 0, max = page.limits.max - pageSpaceAngle * (_pages.Count - 1) };
        PageSideLink sides = page.GetComponent<PageSideLink>();
        _bookPages.Add(sides.bookPageSideA);
        _bookPages.Add(sides.bookPageSideB);
    }

}
