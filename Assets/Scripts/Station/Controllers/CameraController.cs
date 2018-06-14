using UnityEngine;
using System.Collections;

namespace Station
{
    public class CameraController : MonoBehaviour
    {
        public GameObject target;
        public float followSpeed;

        void Start()
        {

        }

        void FixedUpdate()
        {
            Vector3 targetPosition = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z);
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * followSpeed);
        }
    }
}

