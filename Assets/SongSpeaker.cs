using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongSpeaker : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip[] songs;
    [SerializeField] private GameObject currentDisk;
    private bool _isPlaying = false;

    private void Start()
    {
        _isPlaying = true;
    }
    private void Update()
    {
        if(!_isPlaying) return;
        currentDisk.transform.Rotate(Vector3.up, 100 * Time.deltaTime);
    }
    
    
    public void ChangeVolume(float volume)
    {
        audioSource.volume = volume;
    }
}
