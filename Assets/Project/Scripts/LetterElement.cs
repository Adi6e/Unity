using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LetterElement : MonoBehaviour {
    private string _value = "-";
    private bool _hidden = false;
    private bool _interactable = false;
    private bool _opened = false;
    private TMP_Text _textMesh;
    private Animation _animation;

    public InteractWithLetter _interactWithLetter;

    private IEnumerator ToggleCell(bool toOpen) {
        _interactable = false;
        // FindObjectOfType<InteractWithLetter>().CanInteract = false;
        _interactWithLetter.CanInteract = false;
        string clipName = toOpen ? "element_opening-2" : "element_closing-2";
        _animation.Play(clipName);
        while (_animation.isPlaying) {
            yield return null;
        }
        _interactable = true;
        // FindObjectOfType<InteractWithLetter>().CanInteract = true;
        // FindObjectOfType<InteractWithLetter>().CheckSelectedPair();
        _interactWithLetter.CanInteract = true;
        _interactWithLetter.CheckSelectedPair();
        _opened = toOpen;
        yield return null;
    }

    private IEnumerator DestroyCell() {
        // FindObjectOfType<InteractWithLetter>().CanInteract = false;
        _interactWithLetter.CanInteract = false;
        _animation.Play("element_destroying");
        while (_animation.isPlaying) {
            yield return null;
        }
        // FindObjectOfType<InteractWithLetter>().CanInteract = true;
        _interactWithLetter.CanInteract = true;
        Destroy(gameObject);
        yield return null;
    }

    private void Awake() {
        _textMesh = GetComponentInChildren<TMP_Text>();
        _animation = GetComponent<Animation>();

        _interactWithLetter = FindObjectOfType<InteractWithLetter>();
    }

    public void SetValue(string value = "-") {
        _value = value;
        _textMesh.text = _value.ToUpper();
    }
    public string GetValue() {return _value;}

    public void SetHiddenOption(bool flag) {
        _hidden = flag;
        int eulerAngle = _hidden ? 180 : 0;
        transform.rotation = Quaternion.Euler(0, eulerAngle, 0);
    }
    public bool GetHiddenOption() {return _hidden;}

    public void SetInteractableOption() {
        transform.rotation = Quaternion.Euler(0, 180, 0);
        _interactable = true;
    }
    public bool GetInteractableOption() {return _interactable;}

    public void SetOpenedOption(bool flag = true) {
        _opened = flag;
    }
    public bool GetOpenedOption() {return _opened;}

    public void ToggleCellHandler(bool flag = true) {
        StartCoroutine(ToggleCell(flag));
    }

    public void DestroyCellHandler() {
        StartCoroutine(DestroyCell());
    }
}
