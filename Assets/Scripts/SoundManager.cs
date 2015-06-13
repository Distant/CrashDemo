using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour {

    [SerializeField]
    private string[] audioKeys;
    [SerializeField]
    private AudioClip[] audioClips;

    private Dictionary<string, AudioClip> sounds;

	// Use this for initialization
	void Start () {
        sounds = new Dictionary<string, AudioClip>();
        for (int i = 0; i < Mathf.Min(audioKeys.Length, audioClips.Length); i++){
            sounds.Add(audioKeys[i], audioClips[i]);
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void PlayClipAtPoint(string key, Vector3 position, float volume) {
        AudioSource.PlayClipAtPoint(sounds[key], position, volume);
    }

    public void Reset()
    {
    }
}