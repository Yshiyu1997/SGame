using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class DragImage : MonoBehaviour
{
    private Vector3 initObjPos;
    private Vector3 initMousePos;

    public float limtVal_Left = -256.5454f;
    public float limtVal_Right = 544.5454f;

    void Start()
    {
        EventTrigger et = this.GetComponent<EventTrigger>();
        AddETEvent(et, EventTriggerType.BeginDrag, StartDrag);
        AddETEvent(et, EventTriggerType.Drag, Draging);
    }

    private void AddETEvent(EventTrigger et, EventTriggerType ei, UnityAction<BaseEventData> ua)
    {
        UnityAction<BaseEventData> action = ua;
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = ei;
        entry.callback.AddListener(action);
        et.triggers.Add(entry);
    }

    private void StartDrag(BaseEventData data)
    {
        initObjPos = transform.GetComponent<RectTransform>().localPosition;
        initMousePos = Input.mousePosition;
    }


    private void Draging(BaseEventData data)
    {
        Vector3 temp = Input.mousePosition - initMousePos;
        if (initObjPos.x + temp.x <= limtVal_Left || initObjPos.x + temp.x >= limtVal_Right) return;
        transform.GetComponent<RectTransform>().localPosition = new Vector3(initObjPos.x + temp.x, initObjPos.y, initObjPos.z);
    }
}
