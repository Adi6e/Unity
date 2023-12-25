using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {
    public Button BtnMusic, BtnSound;

    public AudioController audioController;
    public FieldsGenerator fieldsGenerator;

    private void Start() {
        ToggleGameSettings();
    }


    private void ToggleGameSettings() {
        int music = PlayerPrefs.GetInt("music", 1);
        int sound = PlayerPrefs.GetInt("sound", 1);
        Image musicImageCancel = BtnMusic.transform.GetChild(1).GetComponent<Image>();
        Image soundImageCancel = BtnSound.transform.GetChild(1).GetComponent<Image>();
        musicImageCancel.gameObject.SetActive(music == 0);
        soundImageCancel.gameObject.SetActive(sound == 0);
        // AudioController audioController = GameObject.FindObjectOfType<AudioController>();
        audioController.ToggleMusic();
    }

    public void OnButtonStartClicked() {
        gameObject.SetActive(false);
        // FieldsGenerator fieldsGenerator = GameObject.FindObjectOfType<FieldsGenerator>();
        fieldsGenerator.RestartGame();
    }
    public void OnButtonMenuClicked() {
        gameObject.SetActive(true);
        // FieldsGenerator fieldsGenerator = GameObject.FindObjectOfType<FieldsGenerator>();
        fieldsGenerator.ExitGame();

    }
    public void OnButtonExitClicked() {
        Application.Quit();
    }
    public void OnButtonMusicClicked() {
        int music = PlayerPrefs.GetInt("music", 1);
        PlayerPrefs.SetInt("music", music == 1 ? 0 : 1);
        ToggleGameSettings();
    }
    public void OnButtonSoundClicked() {
        int sound = PlayerPrefs.GetInt("sound", 1);
        PlayerPrefs.SetInt("sound", sound == 1 ? 0 : 1);
        ToggleGameSettings();
    }
}
