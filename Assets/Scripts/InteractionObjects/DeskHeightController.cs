using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeskHeightController : MonoBehaviour
{
    private float _initialDeskHeight;
    [SerializeField] private float maxDeskHeight;
    [SerializeField] private float minDeskHeight;
    private bool _up;
    private bool _down;
    private bool _reset;


    private void Awake()
    {
        _initialDeskHeight = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (_up)
        {
            if(transform.position.y < maxDeskHeight)
                transform.position += new Vector3(0, 0.001f, 0);
        }else if (_down)
        {
            if(transform.position.y > minDeskHeight)
                transform.position -= new Vector3(0, 0.001f, 0);
        }

        if (!_reset) return;
        if(transform.position.y > _initialDeskHeight)
            transform.position -= new Vector3(0, 0.01f, 0);
        else if(transform.position.y < _initialDeskHeight)
            transform.position += new Vector3(0, 0.01f, 0);
        else
        {
            _reset = false;
        }
    }


    public void RaiseDesk(bool up)
    {
        if (up)
        {
            StopAllCoroutines();
            _down = false;
            _reset = false;
            StartCoroutine(SetDeskHeightValue( 1f, true,false));
        }
        else
        {
            StopAllCoroutines();
            _down = false;
            _up = false;
            _reset = false;
        }
    }

    public void LowerDesk(bool down)
    { 
        if(down){
            StopAllCoroutines();
            _up = false;
            _reset = false;
            StartCoroutine(SetDeskHeightValue(1f, false, false));
        }
        else
        {
            StopAllCoroutines();
            _down = false;
            _up = false;
            _reset = false;
        }
    }
    
    public void ResetDeskPosition(bool reset)
    {
        if (reset)
        {
            StopAllCoroutines();
            _down = false;
            _up = false;
            StartCoroutine(SetDeskHeightValue(2.5f, false, true));
        }
        else
        {
            StopAllCoroutines();
            _down = false;
            _up = false;
            _reset = false;
        }
    }

    private IEnumerator SetDeskHeightValue(float delay, bool isUp, bool isReset)
    {
        yield return new WaitForSeconds(delay);
        if (isReset)
        {
            _reset = true;
        }
        else{
            if (isUp)
            {
                _up = true;
            }
            else
            {
                _down = true;
            }
        }
        
    }
}
