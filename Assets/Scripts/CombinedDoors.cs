using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CombinedDoors : MonoBehaviour
{
    Door leftDoor;
    Door rightDoor;

    [SerializeField]
    string keyToClose;
    [SerializeField]
    KeyCode keyToCloseCode;
    [SerializeField]
    SpriteRenderer buttonSprite;
    [SerializeField]
    SpriteRenderer doorHealthSprite;
    float maxDoorHealthWidth = 0;
    [SerializeField]
    float maxDoorHealth = 100f;
    [SerializeField]
    float currentDoorHealth = 100f;
    [SerializeField]
    int doorDmg = -2;
    [SerializeField]
    float maxAddHpTime = 1f;
    float addHPTime = 0;
    [SerializeField]
    float addHP = 10;
    private int btnPressed;

    public KeyCode KeyToCloseCode
    {
        get
        {
            if (keyToCloseCode == KeyCode.None)
                keyToCloseCode = (KeyCode)Enum.Parse(typeof(KeyCode), keyToClose);
            return keyToCloseCode;
        }
    }
    private void Start()
    {
        leftDoor = GetComponentInChildren<LeftDoor>();
        rightDoor = GetComponentInChildren<RightDoor>();
        leftDoor.onSwarmDestroy.AddListener(DoDmg);
        maxDoorHealthWidth = doorHealthSprite.size.x;
        Open(keyToCloseCode);
    }

    private void DoDmg(Swarm swarm)
    {
        currentDoorHealth += doorDmg;
        addHPTime = 0;
        if (currentDoorHealth <= 0)
        {
            transform.parent.TryGetComponent(out DoorsController doorsController);
            if (doorsController != null)
                doorsController.DestroyDoor(this);

            Destroy(gameObject);
            MusicController.instance.PlayDoorBreak();
            return;
        }
        RecalculateHealthWidth();
    }
    private void Update()
    {
        addHPTime += Time.deltaTime;
        if(addHPTime >= maxAddHpTime)
        {
            addHPTime = 0;
            AddHP();
        }
    }

    private void AddHP()
    {
        currentDoorHealth += addHP;
        if (currentDoorHealth >= maxDoorHealth)
            currentDoorHealth = maxDoorHealth;
        RecalculateHealthWidth();
    }

    private void RecalculateHealthWidth()
    {
        Vector2 doorSize = doorHealthSprite.size;
        doorSize.x = maxDoorHealthWidth * (currentDoorHealth / maxDoorHealth);
        doorHealthSprite.size = doorSize;
    }

    public virtual void Open(KeyCode _key)
    {
        if (!keyToCloseCode.Equals(_key))
            return;

        if (btnPressed <= 3)
            buttonSprite.gameObject.SetActive(true);
        leftDoor.Open();
        rightDoor.Open();
        MusicController.instance.PlayDoorOpenSound();
    }
    public virtual void Close(KeyCode _key)
    {
        if (!keyToCloseCode.Equals(_key))
            return;
        if (btnPressed <= 3)
            buttonSprite.gameObject.SetActive(false);
        leftDoor.Close();
        rightDoor.Close();
        MusicController.instance.PlayDoorCloseSound();
        btnPressed++;
    }
}
