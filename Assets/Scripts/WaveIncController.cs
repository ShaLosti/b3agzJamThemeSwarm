using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveIncController : MonoBehaviour
{
    [SerializeField]
    Animator animator;
    [SerializeField]
    CanvasGroup canvasGroup;
    private void Start()
    {
        Alert(false);
    }
    public void Alert(bool _state)
    {
        animator.enabled = _state;
        if (_state)
            animator.Play("IncAnimation");

        if (!_state)
            canvasGroup.alpha = 0;
    }
}
