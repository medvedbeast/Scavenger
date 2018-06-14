using UnityEngine;
using System.Collections;

namespace Station
{
    public class RobotController : MonoBehaviour
    {

        public float moveSpeedModifier;
        public float turnSpeedModifier;

        private Rigidbody body;

        void Start()
        {
            body = GetComponent<Rigidbody>();
        }

        void FixedUpdate()
        {
            if (Game.gameState == GameStates.GAME)
            {
                Vector3 moveDirection = Vector3.zero;
                if (Input.GetKey(KeyCode.A))
                {
                    moveDirection.x = -1;
                }
                if (Input.GetKey(KeyCode.D))
                {
                    moveDirection.x = 1;
                }
                if (Input.GetKey(KeyCode.W))
                {
                    moveDirection.z = 1;
                }
                if (Input.GetKey(KeyCode.S))
                {
                    moveDirection.z = -1;
                }
                Move(moveDirection);

                if (Input.GetKey(KeyCode.LeftArrow))
                {
                    Rotate(Common.Directions.LEFT);
                }
                if (Input.GetKey(KeyCode.RightArrow))
                {
                    Rotate(Common.Directions.RIGHT);
                }

                if (Input.GetKey(KeyCode.Space))
                {
                    Shoot();
                }

            }
        }

        private void Move(Vector3 direction)
        {
            direction *= moveSpeedModifier;
            transform.position = new Vector3(transform.position.x + direction.x, transform.position.y + direction.y, transform.position.z + direction.z);
        }

        private void Rotate(Common.Directions direction)
        {
            if (direction == Common.Directions.LEFT)
            {
                transform.Rotate(transform.up, -Time.deltaTime * turnSpeedModifier);
            }
            else if (direction == Common.Directions.RIGHT)
            {
                transform.Rotate(transform.up, Time.deltaTime * turnSpeedModifier);
            }
        }

        private void Shoot()
        {
            
        }

        public static Vector3 GetMouseDirection()
        {
            Vector3 mouse = new Vector3(Input.mousePosition.x - (Screen.width / 2), Input.mousePosition.y - (Screen.height / 2), Input.mousePosition.z);
            return new Vector3(mouse.normalized.x, mouse.normalized.z, mouse.normalized.y);
        }

    }
}

