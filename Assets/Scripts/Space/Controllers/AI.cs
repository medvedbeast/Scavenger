using UnityEngine;
using System.Collections;
using Common;

namespace Space
{
    public class AI : MonoBehaviour
    {

        public GameObject target;
        public Vector3 destination;
        public float attackRange = 30.0f;

        private Unit unit;
        private Rigidbody body;
        private ParticleSystem exhaustL;
        private ParticleSystem exhaustR;

        private float speedModifier = 50;
        private bool isRotating = false;
        private bool isMoving = false;
        private float tolerance = 2.0f;

        void Start()
        {
            this.unit = GetComponent<Unit>();
            this.body = GetComponent<Rigidbody>();
            exhaustL = transform.FindChild("engine_l").FindChild("exhaust").GetComponent<ParticleSystem>();
            exhaustR = transform.FindChild("engine_r").FindChild("exhaust").GetComponent<ParticleSystem>();
        }

        void Update()
        {
            if (Game.gameState == GameStates.GAME)
            {
                if (target == null)
                {
                    Move(destination);
                }
                if (target != null)
                {
                    RotateTo(target.transform.position);
                    if (Vector3.Distance(this.transform.position, target.transform.position) <= attackRange)
                    {
                        if (isMoving)
                        {
                            isMoving = false;
                        }
                        Attack(target);
                    }
                    else
                    {
                        if (!isRotating)
                        {
                            Move(target.transform.position);
                        }
                    }
                }
                if (isMoving)
                {
                    if (!exhaustL.isPlaying && !exhaustR.isPlaying)
                    {
                        exhaustL.Play();
                        exhaustR.Play();
                    }
                }
                else if (!isMoving)
                {
                    if (exhaustL.isPlaying && exhaustR.isPlaying)
                    {
                        exhaustL.Stop();
                        exhaustL.Clear();
                        exhaustR.Stop();
                        exhaustR.Clear();
                    }
                }
            }
        }

        public void RotateTo(Vector3 position)
        {
            Quaternion targetRotation = Quaternion.LookRotation(position - transform.position);
            targetRotation.x = 0;
            targetRotation.z = 0;

            if (Quaternion.Angle(transform.rotation, targetRotation) > tolerance * 2)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * (unit.turnSpeed / 2));
                isRotating = true;
            }
            else
            {
                isRotating = false;
            }
        }

        public void Move(Vector3 position)
        {
            if (Vector3.Distance(transform.position, position) > attackRange - tolerance)
            {
                body.AddForce(transform.forward * unit.moveSpeed * speedModifier, ForceMode.Force);
                isMoving = true;
            }
        }

        public void Attack(GameObject target)
        {
            RaycastHit hit;
            Ray ray = new Ray(new Vector3(transform.position.x, 1, transform.position.z), transform.forward * attackRange);
            if (Physics.Raycast(ray, out hit))
            {
                for (int i = 0; i < unit.modules.Count; i++)
                {
                    if (unit.modules[i].referencedObject.type == Modules.WEAPON)
                    {
                        Weapon weapon = unit.modules[i].referencedObject as Weapon;
                        weapon.Shoot(unit.modules[i], gameObject);
                    }
                }
            }
        }

        public void OnTriggerStay(Collider other)
        {
            if (target == null && other.GetComponent<Unit>() != null)
            {
                if (other.GetComponent<Unit>().type == Units.SHIP)
                {
                    target = other.gameObject;
                }
            }
        }

        public void OnTriggerExit(Collider other)
        {
            if (target == other.gameObject)
            {
                target = null;
            }
        }
    }
}
