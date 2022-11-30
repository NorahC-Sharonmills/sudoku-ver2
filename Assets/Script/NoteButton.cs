using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NoteButton : Selectable, IPointerClickHandler
{
    public GameObject note_on;
    private bool active_ = false;

    public void OnPointerClick(PointerEventData eventData)
    {
        active_ = !active_;
        note_on.SetActive(active_);
        GameEvents.OnNotesActiveMethod(active_);
    }

    protected override void Start()
    {
        base.Start();
        active_ = false;
    }
}
