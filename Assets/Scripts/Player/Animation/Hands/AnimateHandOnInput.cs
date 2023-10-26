using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Animation.Hands
{
    
    [RequireComponent(typeof(Animator))]
    public class AnimateHandOnInput : MonoBehaviour
    {

        [SerializeField] private InputActionReference gripInputActionReference;
        [SerializeField] private InputActionReference triggerInputActionReference;
        [SerializeField] private Animator handAnimator;
        private float _gripValue;
        private float _triggerValue;

        private void Awake()
        {
            if(!handAnimator)
                handAnimator = GetComponent<Animator>();
        }

        void Update()
        {
            AnimateGrip();
            AnimateTrigger();
        }


        private void AnimateGrip()
        {
            _gripValue = gripInputActionReference.action.ReadValue<float>();
            handAnimator.SetFloat("Grip", _gripValue);
        }
    
        private void AnimateTrigger()
        {
            _triggerValue = triggerInputActionReference.action.ReadValue<float>();
            handAnimator.SetFloat("Trigger", _triggerValue);
        }
    }
}
