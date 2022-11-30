using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;

public class GridSquare : Selectable, IPointerClickHandler, ISubmitHandler, IPointerUpHandler, IPointerExitHandler
{
    public GameObject number_text;
    private int number_ = 0;
    private int correct_number_ = 0;
    private bool selected_ = false;
    private int square_index_ = -1;

    public bool IsSelected() { return selected_; }
    public void SetSquareIndex(int index)
    {
        square_index_ = index;
    }

    public void SetCorrectNumber(int number)
    {
        correct_number_ = number;
    }

    protected override void Start()
    {
        //base.Start();
        selected_ = false;
    }

    public void DisplayText()
    {
        if (number_ <= 0)
            number_text.GetComponent<Text>().text = " ";
        else
            number_text.GetComponent<Text>().text = number_.ToString();
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
    }

    protected override void OnDisable()
    {
        //base.OnDisable();
        GameEvents.OnUpdateSquareNumber -= OnSetNumber;
        GameEvents.OnSquareSelected -= OnSquareSelected;
    }

    public void OnSetNumber(int _number)
    {
        if(selected_)
        {
            SetNumber(_number);
            if (_number != correct_number_)
            {
                var colors = this.colors;
                colors.normalColor = Color.red;
                this.colors = colors;

                GameEvents.OnWrongNumberMethod();
            }
            else
            {
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
}
