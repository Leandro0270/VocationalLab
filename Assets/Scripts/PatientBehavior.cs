using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatientBehavior : MonoBehaviour
{
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Transform target;  
    [SerializeField] Animator animator;
    [SerializeField] private bool walkingToChair;
    [SerializeField] private AudioSource audioSource;

    private bool _isTalking = false;
    private bool _isListening = false;
    private int _currentTalkAnimation = 0;
    private float _destinationDistance = 0f;
    private bool _seated = false;
    
    
    
    [SerializeField] private bool lookAtPlayer = false;
    [SerializeField] private Transform playerPosition;
    [SerializeField] private float headWeight = 1f;
    [SerializeField] private float bodyWeight = 1f;
    
    [SerializeField] private float moveSpeed = 1.5f;
    
    [SerializeField] private float distanceToSit = 1.3f;
    [SerializeField] private float rotationAngleY = 190f;
    
    private bool _startedDialogue;
    [SerializeField] private MonitorManager monitorManager;
    
    [SerializeField] private float turnSpeed = 2f;

    private bool _patientAudioPlaying;


    public bool debug;
    public bool debug2;

    // Start is called before the first frame update
    void Start()
    {
        _destinationDistance = Vector3.Distance(transform.position, target.position);
        agent.speed = moveSpeed;
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!_startedDialogue && !walkingToChair)
        {
            _startedDialogue = true;
            StartCoroutine(monitorManager.StartTalk());
        }

        if (walkingToChair){
            _destinationDistance = Vector3.Distance(transform.position, target.position);
            if (_destinationDistance > distanceToSit)
                agent.destination = target.position;
            else
                walkingToChair = false;
        } 
        else if (!_seated)
        {
            animator.SetTrigger("TurnLeft");
            Quaternion newRotation = Quaternion.Euler(0, rotationAngleY, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, turnSpeed * Time.deltaTime);
            if (transform.rotation != newRotation) return;
            animator.SetTrigger("Sit");
            animator.SetBool("Listening", true);

            _seated = true;
            _isListening = true;
        }

    }
    
    public IEnumerator StartDialogue(AudioClip audioClip)
    {
        if(_patientAudioPlaying) yield break;
        _patientAudioPlaying = true;
        audioSource.clip = audioClip;
        StartTalking();
        audioSource.Play();
        yield return new WaitWhile(() => audioSource.isPlaying);
        StopTalking();
        _patientAudioPlaying = false;

    }
    
    
    private IEnumerator TalkAnimation()
    {
        if (_isTalking || _isListening) yield break;
        _isTalking = true;
        int oldIndex = _currentTalkAnimation;
        do
        {
            _currentTalkAnimation = UnityEngine.Random.Range(1,4);
        } while (_currentTalkAnimation == oldIndex);
        animator.SetBool("Listening", false);

        animator.SetBool("isTalking", true);
        animator.SetInteger("TalkingAnimation", _currentTalkAnimation);
        
        
        yield return new WaitForSeconds(2);
        _isTalking = false;
        StartCoroutine(TalkAnimation());
    }
    
    
    public void StartTalking()
    {
        _isListening = false;
        StartCoroutine(TalkAnimation());
    }
    public void StopTalking()
    {
        StopAllCoroutines();
        animator.SetBool("Listening", true);

        animator.SetBool("isTalking", false);

        _isListening = false;
        _isTalking = false;
        animator.SetInteger("TalkingAnimation", 0);
    }
    private void OnAnimatorIK(int layerIndex)
    {
        if (walkingToChair)
        {
            animator.SetLookAtPosition(target.position);
            animator.SetLookAtWeight(0.3f, 0.3f, headWeight, 0.5f, 0.7f);
            return;
        }
        animator.SetLookAtPosition(playerPosition.position);
        if (lookAtPlayer)
            animator.SetLookAtWeight(1, bodyWeight, headWeight, 0.5f, 0.7f);
        else
            animator.SetLookAtWeight(0);
        
    }
}
