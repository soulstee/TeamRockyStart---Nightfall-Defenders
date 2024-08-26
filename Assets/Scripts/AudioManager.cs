using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public GameObject audioPrefab;

    private void Awake(){
        instance = this;
    }

    public void PlayNoise(string name){
        AudioClip clip = Resources.Load("Audio/" + name, typeof(AudioClip)) as AudioClip;

        GameObject audObj = Instantiate(audioPrefab, Vector2.zero, Quaternion.identity);
        audObj.GetComponent<AudioSource>().clip = clip;
        audObj.GetComponent<AudioSource>().Play();

        Destroy(audObj.gameObject, clip.length+1f);
    }
}
