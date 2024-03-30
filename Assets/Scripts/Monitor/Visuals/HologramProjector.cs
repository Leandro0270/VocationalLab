using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HologramProjector : MonoBehaviour
{
    
    [SerializeField] private Transform hologramSpinTransform;
    [SerializeField] private MeshRenderer glowMeshRenderer;
    private bool _isSpinning;
    

    private void Awake()
    {
        _isSpinning = true;
    }

    private void Update()
    {
 
        if(!_isSpinning) return;
        hologramSpinTransform.Rotate(Vector3.up, 1);
        
    }
    
    
    public void ToggleSpin(bool isOn)
    {
        _isSpinning = isOn;
        glowMeshRenderer.enabled = _isSpinning;
    }
}
