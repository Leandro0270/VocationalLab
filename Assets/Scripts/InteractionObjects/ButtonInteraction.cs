using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonInteraction : MonoBehaviour
{
    [SerializeField] private GameObject button;
    [SerializeField] private UnityEvent onPress;
    [SerializeField] private UnityEvent onRelease;
    private GameObject _presser;
    private bool _isPressed;

    private void OnTriggerEnter(Collider other)
    {
        if (_isPressed) return;
        button.transform.localPosition = new Vector3(0,0.01f,0);
        onPress.Invoke();
        _isPressed = true;

    }
    
    private void OnTriggerExit(Collider other)
    {
            button.transform.localPosition = new Vector3(0,0.044f,0);
            onRelease.Invoke();
            _isPressed = false;
    }


}
