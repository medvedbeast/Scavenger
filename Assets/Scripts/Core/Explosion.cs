using UnityEngine;

public class Explosion : MonoBehaviour
{

    private ParticleSystem system;

    public void Start()
    {
        system = GetComponentInChildren<ParticleSystem>();
    }

    public void Update()
    {
        if (system.time >= system.duration)
        {
            GameObject.Destroy(this.gameObject);
        }
    }
}
