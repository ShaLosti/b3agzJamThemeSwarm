using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GoldLose : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI moneyOnCanvas;
    [SerializeField]
    TextMeshProUGUI moneyChangeVisual;
    int gold;
    [SerializeField]
    int startMoney;

    Coroutine coroutine;
    Coroutine fadeIn;
    Coroutine fadeOut;
    WaitForSeconds waitForSeconds = new WaitForSeconds(.3f);

    private void Start()
    {
        AddGold(startMoney);
        Array.ForEach(FindObjectsOfType<Door>(), x => x.onSwarmDestroy.AddListener(OnSwarmDestroy));
    }
    /*private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            AddGold(1000);
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            AddGold(-1000);
        }
    }*/
    public void OnSwarmDestroy(Swarm swarm)
    {
        AddGold(Mathf.Abs(swarm.GoldTake));
    }
    public void AddGold(int _gold)
    {
        gold += _gold;
        moneyOnCanvas.text = gold.ToString() + "$";
        if (_gold < 0)
        {
            moneyChangeVisual.color = Color.red;
            moneyChangeVisual.text = _gold.ToString() + "$";
        }
        else
        {
            moneyChangeVisual.color = Color.green;
            moneyChangeVisual.text = '+' + _gold.ToString() + "$";
        }
        if (gold < 0)
        {
            moneyOnCanvas.color = Color.red;
        }
        else
        {
            moneyOnCanvas.color = Color.green;
        }
        if(coroutine!=null)
            StopCoroutine(coroutine);
        if (fadeIn != null)
            StopCoroutine(fadeIn);
        if (fadeOut != null)
            StopCoroutine(fadeOut);

        coroutine = StartCoroutine(ShowValueChange());
    }
    
    IEnumerator ShowValueChange()
    {
        fadeIn = StartCoroutine(Coroutins.FadeIn(moneyChangeVisual.GetComponent<CanvasGroup>(), .1f, 1f));
        yield return fadeIn;

        yield return waitForSeconds;
        fadeOut = StartCoroutine(Coroutins.FadeOut(moneyChangeVisual.GetComponent<CanvasGroup>(), .1f));
        yield return fadeOut;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.TryGetComponent(out Swarm swarm);
        if (swarm == null)
            return;

        AddGold(swarm.GoldTake);
        Destroy(swarm.gameObject);
    }
}
