using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeController : MonoBehaviour
{
    [SerializeField, Range(0, 5)]
    float moveInterval = 1f;

    bool isHit;

    void Start()
    {
        StartCoroutine(Move());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.rotation = Quaternion.Euler(0, 90, 0);
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.rotation = Quaternion.Euler(0, 270, 0);
        }
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
