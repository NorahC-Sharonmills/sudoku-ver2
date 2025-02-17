using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;
using DG.Tweening;

public class GridSquare : Selectable, IPointerClickHandler, ISubmitHandler, IPointerUpHandler, IPointerExitHandler
{
    public GameObject number_text;
    public List<GameObject> number_notes;
    private bool note_active = false;
    private int number_ = 0;
    private int correct_number_ = 0;
    private bool selected_ = false;
    private int square_index_ = -1;
    private bool has_default_value_ = false;
    private bool has_wrong_value_ = false;

    public int GetCorrectNumber() { return correct_number_; }
    public int GetSquareNumber() { return number_; }
    public bool IsCorrectNumberSet() { return number_ == correct_number_; }
    public bool HasWrongValue() { return has_default_value_; }
    public void SetHasDefaultValue(bool has_default) { has_default_value_ = has_default; }
    public bool GetHasDefaultValue() { return has_default_value_; }

    public bool IsSelected() { return selected_; }
    public void SetSquareIndex(int index)
    {
        square_index_ = index;
    }

    public void SetCorrectNumber(int number)
    {
        correct_number_ = number;
        has_default_value_ = false;

        if(number_ != 0 && number_ != correct_number_)
        {
            has_wrong_value_ = true;
            SetSquareColor(Color.red);
        }
        else if(number_ != 0 && number_ == correct_number_)
        {
            has_default_value_ = true;
            has_wrong_value_ = false;
        }
    }

    public void SetCorrectNumber()
    {
        number_ = correct_number_;
        SetNoteNumberValue(0);
        DisplayText();
    }

    protected override void Start()
    {
        //base.Start();
        selected_ = false;
        note_active = false;

        if (GameSettings.Instance.GetContinutePreviousGame() == false)
            SetNoteNumberValue(0);
        else
            SetClearEmptyNotes();
    }

    public List<string> GetSquareNote()
    {
        List<string> notes = new List<string>();
        number_notes.ForEach((_number_note) =>
        {
            notes.Add(_number_note.GetComponent<Text>().text);
        });

        return notes;
    }

    public void SetClearEmptyNotes()
    {
        number_notes.ForEach((_number_note) =>
        {
            if (_number_note.GetComponent<Text>().text == "0")
                _number_note.GetComponent<Text>().text = " ";
        });
    }

    private void SetNoteNumberValue(int value)
    {
        number_notes.ForEach((_number_note) =>
        {
            if (value <= 0)
                _number_note.GetComponent<Text>().text = " ";
            else
                _number_note.GetComponent<Text>().text = $"{value}";
        });
    }

    private void SetNoteSingleNumberValue(int value, bool force_update = false)
    {
        if (note_active == false && force_update == false)
            return;

        if (value < 0)
            number_notes[value - 1].GetComponent<Text>().text = " ";
        else
        {
            if (number_notes[value - 1].GetComponent<Text>().text == " " || force_update)
                number_notes[value - 1].GetComponent<Text>().text = $"{value}";
            else
                number_notes[value - 1].GetComponent<Text>().text = " ";
        }
    }

    public void SetGridNotes(List<int> notes)
    {
        notes.ForEach((_note) =>
        {
            SetNoteSingleNumberValue(_note, true);
        });
    }

    public void OnNotesActive(bool active)
    {
        note_active = active;
    }

    public void DisplayText()
    {
        if (number_ <= 0)
            number_text.GetComponent<Text>().text = " ";
        else
            number_text.GetComponent<Text>().text = number_.ToString();

        if(has_default_value_)
        {
            number_text.GetComponent<Text>().fontStyle = FontStyle.Bold;
        }
    }

    public void SetNumber(int _number)
    {
        number_ = _number;
        DisplayText();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        selected_ = true;
        GameEvents.UpdateSquareSelectedMethod(square_index_);
    }

    public void OnSubmit(BaseEventData eventData)
    {

    }

    protected override void OnEnable()
    {
        //base.OnEnable();
        GameEvents.OnUpdateSquareNumber += OnSetNumber;
        GameEvents.OnSquareSelected += OnSquareSelected;
        GameEvents.OnNotesActive += OnNotesActive;
        GameEvents.OnClearNumber += OnClearNumber;
        GameEvents.OnGameLose += OnGameLose;
    }

    protected override void OnDisable()
    {
        //base.OnDisable();
        GameEvents.OnUpdateSquareNumber -= OnSetNumber;
        GameEvents.OnSquareSelected -= OnSquareSelected;
        GameEvents.OnNotesActive -= OnNotesActive;
        GameEvents.OnClearNumber -= OnClearNumber;
        GameEvents.OnGameLose -= OnGameLose;
    }

    private void OnGameLose()
    {
        if(number_ != 0 && number_ != correct_number_)
        {
            has_default_value_ = false;
            SetSquareColor(Color.white);
            number_ = 0;
            DisplayText();
        }
    }

    public void SetCorrectValueOnHint()
    {
        SetSquareNumber(correct_number_);
        Shake();
    }

    private void Shake()
    {
        this.transform.DOKill();
        this.transform.DOShakeScale(0.3f);
    }

    public void OnSetNumber(int _number)
    {
        if (selected_ && has_default_value_ == false)
        {
            SetSquareNumber(_number);
        }    
    }

    private void SetSquareNumber(int _number)
    {
        if (note_active == true && has_default_value_ == false)
        {
            SetNoteSingleNumberValue(_number);
        }
        else if (note_active == false)
        {
            SetNoteNumberValue(0);
            SetNumber(_number);
            if (_number != correct_number_)
            {
                has_wrong_value_ = true;

                var colors = this.colors;
                colors.normalColor = Color.red;
                this.colors = colors;

                GameEvents.OnWrongNumberMethod();
            }
            else
            {
                has_wrong_value_ = false;

                has_default_value_ = true;
                var colors = this.colors;
                colors.normalColor = Color.white;
                this.colors = colors;
            }
        }
    }

    public void OnSquareSelected(int square_index)
    {
        if(square_index_ != square_index)
        {
            selected_ = false;
        }    
    }

    public void SetSquareColor(Color color)
    {
        var colors = this.colors;
        colors.normalColor = color;
        this.colors = colors;
    }

    public void OnClearNumber()
    {
        if(selected_ && !has_default_value_)
        {
            number_ = 0;
            has_wrong_value_ = false;
            SetSquareColor(Color.white);
            SetNoteNumberValue(0);
            DisplayText();
        }
    }
}
