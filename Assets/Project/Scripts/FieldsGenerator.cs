using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FieldsGenerator : MonoBehaviour {
    private const string _charsEN = "abcdefghijklmnopqrstuvwxyz";
    private int _cellsAmount = 0;
    private Transform _letterElementParent;
    private int _wordPoints = 0;
    private float _timer = 0;
    private bool _timerOn = false;

    public Transform WordPanel, LettersPanel, LetterElement;
    public List<LetterElement> WordLetters;
    public Image WordObjImg;
    public Text TextScore, TextScoreBest, TextTimer;

    private IEnumerator RestartGameTemp(float secs = 0f) {
        _timerOn = false;
        TextTimer.text = "--";
        TextTimer.color = new Color32(255, 255, 255, 255);
        TextScore.text = "SCORE: " + PlayerPrefs.GetInt("score", 0);
        TextScoreBest.text = "BEST: " + PlayerPrefs.GetInt("score_best", 0);
        FindObjectOfType<InteractWithLetter>().CanInteract = false;
        yield return new WaitForSeconds(secs);
        RenderWordField();
        yield return new WaitForSeconds(secs / 2f);
        _timer = 45f;
        _timerOn = true;
        FindObjectOfType<InteractWithLetter>().CanInteract = true;
        yield return null;
    }

    private void Awake() {
        _letterElementParent = FindObjectOfType<LetterElementParent>().transform;
        WordLetters = new List<LetterElement>();
    }

    private void Start() {
        PlayerPrefs.SetInt("score", 0);
        StartCoroutine(RestartGameTemp(0.5f));
    }

    private void Update() {
        if (_timerOn) {
            TextTimer.text = "" + Mathf.Round(_timer);
            if (_timer > 0f) {
                _timer -= Time.deltaTime;
                if (Mathf.Round(_timer) == 10f) {
                    TextTimer.color = new Color32(255, 0, 0, 255);
                }
            } else {
                _timerOn = false;
                int cPoints = PlayerPrefs.GetInt("score", 0);
                if (cPoints > PlayerPrefs.GetInt("score_best", 0)) {
                    PlayerPrefs.SetInt("score_best", cPoints);
                }
                PlayerPrefs.SetInt("score", 0);
                StartCoroutine(RestartGameTemp(0.5f));
            }
        }
    }

    private void RenderWordField() {
        WordLetters.Clear();
        LetterElement[] lettersObjs = GameObject.FindObjectsOfType<LetterElement>();
        foreach (LetterElement obj in lettersObjs) {Destroy(obj.gameObject);}
        FindObjectOfType<InteractWithLetter>().ClearSelectedLetters();

        string langType = "en";
        List<int> savedInds = new List<int>();
        for (int i = 0; i < WordsHandler.Words.words.Length; i++) {
            var item = WordsHandler.Words.words[i];
            if (item.Language == langType) {
                savedInds.Add(i);
            }
        }
        if (savedInds.Count == 0) {return;}
        int randIndex = savedInds[UnityEngine.Random.Range(0, savedInds.Count)];

        var currentItem = WordsHandler.Words.words[randIndex];
        string currentWord = currentItem.TextSeq;
        int wordLength = currentWord.Length;
        _wordPoints = currentItem.Points;
        
        float positionDelta = 0.1f;
        float letterSize = 3.3f / wordLength;
        float initPosX = -((int)(wordLength / 2 - (1 - wordLength % 2)) * (letterSize + positionDelta) + (1 - wordLength % 2) * (letterSize + positionDelta) * 0.5f);
                
        float cPosX = initPosX;
        for (int i = 0; i < wordLength; i++) {
            var letter = Instantiate(LetterElement, new Vector3(cPosX, WordPanel.transform.position.y, -2f), Quaternion.identity);
            letter.transform.parent = _letterElementParent.GetChild(0);
            letter.transform.localScale = new Vector3(letterSize, letterSize, 0.2f);
            letter.GetComponent<LetterElement>().SetValue(currentWord[i] + "");
            WordLetters.Add(letter.GetComponent<LetterElement>());
            cPosX += letterSize + positionDelta;
        }
        int hiddenLettersCount = wordLength / 2;
        List<int> indexesList = new List<int>();
        List<string> charsList = new List<string>();
        for (int i = 0; i < hiddenLettersCount; i++) {
            int rndIndex = UnityEngine.Random.Range(0, wordLength);
            while (indexesList.Contains(rndIndex)) {
                rndIndex = UnityEngine.Random.Range(0, wordLength);
            }
            WordLetters[rndIndex].SetHiddenOption(true);
            indexesList.Add(rndIndex);
            charsList.Add(currentWord[rndIndex] + "");
        }
        for (int i = 0; i < 8 - hiddenLettersCount; i++) {
            string letter = _charsEN[UnityEngine.Random.Range(0, _charsEN.Length)] + "";
            while (charsList.Contains(letter)) {
                letter = _charsEN[UnityEngine.Random.Range(0, _charsEN.Length)] + "";
            }
            charsList.Add(letter);
        }
        var sprite = Resources.Load<Sprite>($"WordObjsSprites/{currentItem.PrefabName}");
        WordObjImg.sprite = sprite;
        RenderLettersField(charsList.ToArray());
    }

    private void RenderLettersField(string[] lettersSeq) {
        int lettersLen = lettersSeq.Length * 2;
        string[] letters = new string[lettersLen];
        for (int i = 0; i < lettersLen; i++) {
            letters[i] = lettersSeq[i % (lettersLen / 2)];
        }
        Utils.Shuffle(letters);

        float positionDelta = 1.2f;
        float letterSize = 1f;
        Vector2 initPos = new Vector2(-1.8f, -1.4f);
        for (int i = 0; i < 4; i++) {
            float posYDelta = i * positionDelta + initPos.y;
            for (int j = 0; j < 4; j++) {
                float posXDelta = j * positionDelta + initPos.x;
                var letter = Instantiate(LetterElement, new Vector3(LettersPanel.transform.position.x + posXDelta, LettersPanel.transform.position.y + posYDelta, -2f), Quaternion.identity);
                letter.transform.parent = _letterElementParent.GetChild(1);
                letter.transform.localScale = new Vector3(letterSize, letterSize, 0.2f);
                letter.GetComponent<LetterElement>().SetValue(letters[i * 4 + j] + "");
                letter.GetComponent<LetterElement>().SetInteractableOption();
            }
        }
    }

    public void CheckOpenedLetter(string letter = "") {
        bool isLetterElementFound = false;
        int hiddenLettersCount = 0;
        foreach (LetterElement element in WordLetters) {
            if (element.GetHiddenOption()) {
                if (element.GetValue() == letter) {
                    element.SetHiddenOption(false);
                    isLetterElementFound = true;
                } else {
                    hiddenLettersCount ++;
                }
            }
        }
        int currentPoints = PlayerPrefs.GetInt("score", 0);
        if (!isLetterElementFound) {
            currentPoints += 1;
        }
        if (hiddenLettersCount == 0) {
            currentPoints += _wordPoints;
            StartCoroutine(RestartGameTemp(2f));
        }
        TextScore.text = "SCORE: " + currentPoints;
        PlayerPrefs.SetInt("score", currentPoints);
    }
}
