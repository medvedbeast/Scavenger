using UnityEngine;
using System.Collections;

public class Asteroid : MonoBehaviour
{
    private static System.Random randomizer = new System.Random();

    void Start()
    {
        Rigidbody body = GetComponent<Rigidbody>();
        body.AddForce(new Vector3(randomizer.Next(1, 11) / 10 * randomizer.Next(1, 51), 0, randomizer.Next(1, 11) / 10 * randomizer.Next(1, 51)), ForceMode.Impulse);
    }

    void FixedUpdate()
    {
        if (!Game.sector.Contains(transform.position))
        {
            GameObject.Destroy(this.gameObject);
        }
    }
}
