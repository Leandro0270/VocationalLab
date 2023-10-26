using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SetPokeToFingerAttachPoint : MonoBehaviour
{
    
    
    [SerializeField] private Transform pokeAttachPoint;
    
    private XRPokeInteractor _xrPokeInteractor;
    // Start is called before the first frame update
    void Start()
    {
        _xrPokeInteractor = transform.parent.parent.GetComponentInChildren<XRPokeInteractor>();
        SetPokeAttachPoint();
    }

    private void SetPokeAttachPoint()
    {
        if(pokeAttachPoint == null)
            Debug.LogError("PokeAttachPoint is null");
        if(_xrPokeInteractor == null)
            Debug.LogError("XRPokeInteractor is null");
        
        _xrPokeInteractor.attachTransform = pokeAttachPoint;
    }
}
