using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deleteinfo : MonoBehaviour
{
    [SerializeField]
    GameObject[] objToDelete;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(10);
        foreach (var item in objToDelete)
        {
            Destroy(item.gameObject);
        }
        Destroy(gameObject);
    }
}
