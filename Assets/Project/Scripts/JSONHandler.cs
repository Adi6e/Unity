using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class JSONHandler : MonoBehaviour {
    public TextAsset textJSON;
    
    private void Awake() {
        WordsHandler.Words = JsonUtility.FromJson<WordsHandler.WordsList>(textJSON.text);
    }

}
