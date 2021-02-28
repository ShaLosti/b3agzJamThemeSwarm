using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Door : MonoBehaviour
{
    [SerializeField]
    protected float moveToX;
    Vector2 currentPos;
    bool doorIsOpen = false;
    [SerializeField]
    public OnSwarmEvent onSwarmDestroy;

    protected void Start()
    {
        currentPos = transform.localPosition;
    }
    public virtual void Open()
    {
        currentPos = transform.localPosition;
        currentPos.x += moveToX;
        transform.localPosition = currentPos;
        doorIsOpen = true;
    }
    public virtual void Close()
    {
        currentPos = transform.localPosition;
        currentPos.x -= moveToX;
        transform.localPosition = currentPos;
        doorIsOpen = false;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (doorIsOpen)
            return;

        onSwarmDestroy?.Invoke(collision.GetComponent<Swarm>());
        MusicController.instance.PlayPunchSound();
        Destroy(collision.gameObject);
    }
}
[System.Serializable]
public class OnSwarmEvent : UnityEvent<Swarm> { }