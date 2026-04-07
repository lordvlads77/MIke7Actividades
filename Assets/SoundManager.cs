using System;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance {get; private set;}
    
    AudioSource _selfAudioSource;
    [SerializeField] private GameObject _audioPrefab;
    List<AudioSource> _audioSources = new List<AudioSource>();
    List<AudioSource> _audioS3D = new List<AudioSource>();
    private void Awake()
    {
        Instance = this;
        if (Instance != this)
        {
            Destroy(gameObject);
        }
        _selfAudioSource = GetComponent<AudioSource>();
    }

    public void SetAudio(AudioClip clip) // 2D
    {
        _selfAudioSource.PlayOneShot(clip);
    }

    public void SetAudio(AudioClip clip, Vector3 position)
    {
        AudioSource newAudio = GetAudio3DFromPool();
        newAudio.clip = clip;
        newAudio.transform.position = position;
        newAudio.gameObject.SetActive(true);
        newAudio.Play();
    }

    AudioSource GetAudio3DFromPool()
    {
        for (int i = 0; i < _audioS3D.Count; i++)
        {
            if (!_audioS3D[i].gameObject.activeInHierarchy)
            {
                return _audioS3D[i];
            }
        }
        GameObject newAudioObject = Instantiate(_audioPrefab);
        AudioSource newAudioSource = newAudioObject.GetComponent<AudioSource>();
        _audioS3D.Add(newAudioSource);
        return newAudioSource;
    }
}
