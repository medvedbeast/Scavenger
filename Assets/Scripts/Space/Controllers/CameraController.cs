using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Common;

namespace Space
{
    public class CameraController : MonoBehaviour
    {
        static GameObject target;
        static float minSize = 20;
        static float currentSize;
        static float maxSize = 125;

        void Start()
        {
            currentSize = minSize;
        }

        void FixedUpdate()
        {
            if (Game.gameState == GameStates.GAME)
            {
                if (Input.mouseScrollDelta.y != 0)
                {
                    if (Input.mouseScrollDelta.y < 0)
                    {
                        if (currentSize < maxSize)
                        {
                            currentSize += 5;
                        }
                    }
                    else
                    {
                        if (currentSize > minSize)
                        {
                            currentSize -= 5;
                        }
                    }
                    Camera.main.orthographicSize = currentSize;
                }

                if (target != null)
                {
                    transform.position = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z);
                }
            }
        }

        public static void Assign(GameObject target)
        {
            CameraController.target = target;
        }
    }

}
