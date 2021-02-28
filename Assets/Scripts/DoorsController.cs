using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class DoorsController : MonoBehaviour
{
    [SerializeField]
    List<CombinedDoors> doors;
    KeyCode[] keyCodes;

    private void Start()
    {
        keyCodes = doors.Select(x => x.KeyToCloseCode).ToArray();
    }
    private void Update()
    {
        foreach (var keyCode in keyCodes)
        {
            if (Input.GetKeyDown(keyCode))
                doors.ForEach(x => x.Close(keyCode));
            else if (Input.GetKeyUp(keyCode))
                doors.ForEach(x => x.Open(keyCode));
        }
    }

    public void DestroyDoor(CombinedDoors combinedDoors)
    {
        doors.Remove(combinedDoors);
    }
}
