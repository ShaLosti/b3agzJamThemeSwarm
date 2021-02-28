using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;

public class Car : MonoBehaviour, ILivingEntity
{
    Vector3 moveDir = Vector3.zero;
    [SerializeField]
    AnimationCurve acceleration;
    [SerializeField]
    float slowDownRate = -.5f;
    float time;
    private bool canDrive;
    private bool slowdown;
    float speed;
    Transform somethingForward;
    float leftDestroyPosition;

    public UnityAction<Car> onCarRemove;
    WaitForSeconds waitForSeconds = new WaitForSeconds(5f);
    HashSet<Transform> walkForward = new HashSet<Transform>();
    public void Init(Vector3 directionToMove, float _leftDestroyPointX)
    {
        moveDir = directionToMove;
        leftDestroyPosition = _leftDestroyPointX - 10f;
    }
    public void Setup()
    {
        Go();
        StartCoroutine(CheckVisibality());
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (canDrive)
                Stop();
            else
                Go();
        }
        if (!canDrive)
            return;

        if(!slowdown)
            speed = acceleration.Evaluate(time);

        transform.Translate(moveDir * speed * Time.deltaTime);
        time += Time.deltaTime;
    }
    public void Stop()
    {
        StartCoroutine(SlowDown());
    }
    IEnumerator SlowDown()
    {
        slowdown = true;
        while (speed > 0)
        {
            speed += slowDownRate;
            if(speed<0)
                speed = 0;
            yield return null;
        }
        speed = 0;
        canDrive = false;
        slowdown = false;
        time = 0;
    }
    IEnumerator CheckVisibality()
    {
        while (true)
        {
            yield return waitForSeconds;
            if (transform.position.x <= leftDestroyPosition)
            {
                onCarRemove?.Invoke(this);
            }
        }
    }
    public void Go()
    {
        StopCoroutine(SlowDown());
        canDrive = true;
        slowdown = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<ILivingEntity>() == null)
            return;
        float dotProduct = Vector3.Dot((transform.localPosition - collision.transform.localPosition).normalized,
            transform.right.normalized);
        if (dotProduct > 0)
        {
            if (dotProduct > -.6f && dotProduct < .6f)
            {
                Destroy(collision.gameObject);
                return;
            }
            walkForward.Add(collision.transform);
            somethingForward = collision.gameObject.transform;
            Stop();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        float dotProduct = Vector3.Dot((transform.localPosition - collision.transform.localPosition).normalized,
            transform.right.normalized);

        if (walkForward.Contains(collision.transform))
        {
            walkForward.Remove(collision.transform);
            if (walkForward.Count == 0)
                Go();
        }
    }
}
