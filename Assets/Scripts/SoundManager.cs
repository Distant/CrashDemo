using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour {

    [SerializeField]
    private string[] audioKeys;
    [SerializeField]
    private AudioClip[] audioClips;

    private Transform player;
    private Transform mainCamera;

    private Dictionary<string, AudioClip> sounds;
    private AudioSource source;
    public float minDist;
    public float maxDist;

    // Use this for initialization
    void Start () {
        Reset();
        sounds = new Dictionary<string, AudioClip>();
        for (int i = 0; i < Mathf.Min(audioKeys.Length, audioClips.Length); i++){
            sounds.Add(audioKeys[i], audioClips[i]);
        }
        source = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void PlayClipAtPoint(string key, Vector3 position, float volume) {
        if (sounds.ContainsKey(key))
        {
            GameObject tempGO = new GameObject("OneShotAudio");
            CheckIfNull();
            tempGO.transform.position = position - (player.position - mainCamera.position);
            AudioSource aSource = tempGO.AddComponent<AudioSource>();
            aSource.clip = sounds[key];
            aSource.spatialBlend = 1;
            aSource.volume = volume;
            aSource.rolloffMode = AudioRolloffMode.Linear;
            aSource.dopplerLevel = 0;
            aSource.minDistance = minDist;
            aSource.maxDistance = maxDist;
            aSource.Play();
            Destroy(tempGO, sounds[key].length);
        }
    }

    public void Reset()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").transform;
    }

    private void CheckIfNull() {
        if (player == null) player = GameObject.FindGameObjectWithTag("Player").transform;
        if (mainCamera == null) mainCamera = GameObject.FindGameObjectWithTag("MainCamera").transform;
    }
}