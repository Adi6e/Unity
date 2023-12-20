using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour {
    public AudioClip Music, SoundCorrect, SoundInCorrect;

    public void ToggleMusic() {
        AudioSource aSource = gameObject.GetComponent<AudioSource>();
        int music = PlayerPrefs.GetInt("music", 1);
        aSource.clip = Music;
        if (music == 1) {aSource.Play();}
        else {aSource.Stop();}
    }

    public void ToggleSound(string soundType = "") {
        AudioSource aSource = gameObject.GetComponent<AudioSource>();
        int sound = PlayerPrefs.GetInt("sound", 1);
        if (sound == 0) {return;}
        float volume = 0.3f;
        switch (soundType) {
            case "correct":
                aSource.PlayOneShot(SoundCorrect, volume);
                break;
            case "incorrect":
                aSource.PlayOneShot(SoundInCorrect, volume);
                break;
            default:
                break;
        }
    }
}
