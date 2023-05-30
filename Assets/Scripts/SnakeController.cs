using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeController : MonoBehaviour
{
    [SerializeField, Range(0, 1)]
    float moveInterval = 1f;

    bool isHit;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Move());
    }

    IEnumerator Move()
    {
        while (!isHit)
        {
            transform.Translate(Vector3.forward);
            yield return new WaitForSeconds(moveInterval);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Border"))
        {
            isHit = true;
            print("GameOver");
        }
    }
}
