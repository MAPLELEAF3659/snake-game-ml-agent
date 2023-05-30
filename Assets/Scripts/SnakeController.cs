using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeController : MonoBehaviour
{
    [SerializeField, Range(0, 1)]
    float moveInterval = 1f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Move());
    }

    IEnumerator Move()
    {
        while (true)
        {
            transform.Translate(Vector3.forward);
            yield return new WaitForSeconds(moveInterval);
        }
    }
}
