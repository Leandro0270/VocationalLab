using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class InteractivePage : XRGrabInteractable
{
    public float maxInteractionDistance = 2.0f;
    [SerializeField] private Transform handlePosition;
    [SerializeField] private HintBook hintBook;
    [SerializeField] private Transform pageTransform;
    [SerializeField] private BoxCollider pageCollider;
    private IXRSelectInteractor _interactor;


    private void Start()
    {
        if (pageCollider)
        {
            pageCollider = GetComponent<BoxCollider>();
        }
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        _interactor = args.interactorObject;
        if (!hintBook.IsTheNearPage(_interactor.transform, pageTransform.transform, pageCollider))
            interactionManager.SelectCancel(_interactor, this);
    }
    
    public void SetHintBook(HintBook hintBook)
    {
        this.hintBook = hintBook;
    }
    void FixedUpdate()
    {
        // Se o objeto está sendo segurado, verifica a distância para o interator
        if (!isSelected || _interactor == null) return;
        if (Vector3.Distance(_interactor.transform.position, handlePosition.transform.position) > maxInteractionDistance)
        {
            // Soltar o objeto se a distância for maior que o limite
            interactionManager.SelectCancel(_interactor, this);
        }
    }
}