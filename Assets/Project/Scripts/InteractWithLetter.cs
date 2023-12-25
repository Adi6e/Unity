using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractWithLetter : MonoBehaviour {
    private FieldsGenerator _fieldsGenerator;
    private List<LetterElement> _selectedLetters;
    private Vector3 _touchPosWorld;
    public bool CanInteract = true;

    public AudioController audioController;

    private void Awake() {
        // _fieldsGenerator = FindObjectOfType<FieldsGenerator>();
        _fieldsGenerator = gameObject.GetComponent<FieldsGenerator>();
        _selectedLetters = new List<LetterElement>();
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0) && CanInteract) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit) && _selectedLetters.Count < 2) {
                if (hit.transform.GetComponent<LetterElement>() != null) {
                    var letter = hit.transform.GetComponent<LetterElement>();
                    if (letter.GetInteractableOption()) {
                        letter.ToggleCellHandler(!letter.GetOpenedOption());
                        if (!letter.GetOpenedOption()) {_selectedLetters.Add(letter);}
                        else {_selectedLetters.Remove(letter);}
                    }
                }
            }
        }
    }

    public void CheckSelectedPair() {
        // AudioController audioController = GameObject.FindObjectOfType<AudioController>();
        if (_selectedLetters.Count != 2) {return;}
        if (_selectedLetters[0].GetValue() == _selectedLetters[1].GetValue()) {
            string foundLetter = _selectedLetters[0].GetValue();
            _selectedLetters[0].DestroyCellHandler();
            _selectedLetters[1].DestroyCellHandler();
            _fieldsGenerator.CheckOpenedLetter(foundLetter);
            audioController.ToggleSound("correct");
        } else {
            _selectedLetters[0].ToggleCellHandler(false);
            _selectedLetters[1].ToggleCellHandler(false);
            _fieldsGenerator.ChangeTimer(-3f);
            audioController.ToggleSound("incorrect");
        }
        _selectedLetters.Clear();
    }

    public void ClearSelectedLetters() {
        _selectedLetters.Clear();
    }

}
