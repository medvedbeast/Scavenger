using UnityEngine;
using System.Collections;

public class ShipController : MonoBehaviour
{
    public Unit unit;

    public static GameObject controlledObject;

    private GameObject aim;
    private Rigidbody body;
    private ParticleSystem exhaustL;
    private ParticleSystem exhaustR;
    private Vector3 direction;
    private float speedModifier = 50;
    private float backwardSpeedModifier = 10;
    private bool aiming = false;

    void Start()
    {
        aim = GameObject.Find("GUI").transform.FindChild("Game").FindChild("Aim").gameObject;
        body = GetComponent<Rigidbody>();
        exhaustL = transform.FindChild("engine_l").FindChild("exhaust").GetComponent<ParticleSystem>();
        exhaustR = transform.FindChild("engine_r").FindChild("exhaust").GetComponent<ParticleSystem>();
        direction = transform.forward;
        unit = GetComponent<Unit>();
        controlledObject = gameObject;

        unit.OnHpAmountChanged += OnHpChanged;
    }

    void FixedUpdate()
    {

        if (Game.gameState == GameStates.GAME)
        {
            if (controlledObject != null)
            {
                MoveAim();
            }

            if (Input.GetKey(KeyCode.A))
            {
                MoveLeft();
            }

            if (Input.GetKey(KeyCode.D))
            {
                MoveRight();
            }

            if (Input.GetKey(KeyCode.W))
            {
                MoveForward();
            }
            else
            {
                Exhaust(false);
            }

            if (Input.GetKey(KeyCode.S))
            {
                MoveBackward();
            }

            if (Input.GetKey(KeyCode.UpArrow))
            {
                ChangeFocusDistance(true);
            }

            if (Input.GetKey(KeyCode.DownArrow))
            {
                ChangeFocusDistance(false);
            }

            if (Input.GetKey(KeyCode.Z))
            {
                aiming = true;
                aim.SetActive(true);
            }
            else
            {
                aiming = false;
                aim.SetActive(false);
            }

            if (Input.GetKey(KeyCode.Space))
            {
                Shoot();
            }

            if (!Game.sector.Contains(gameObject.transform.position))
            {
                ExitSector();
            }

        }
    }

    private void MoveLeft()
    {
        transform.RotateAround(transform.position, transform.up, aiming ? -Time.deltaTime * 10 : -unit.turnSpeed);
        direction = transform.forward;
    }

    private void MoveRight()
    {
        transform.RotateAround(transform.position, transform.up, aiming ? Time.deltaTime * 10 : unit.turnSpeed);
        direction = transform.forward;
    }

    private void MoveForward()
    {
        if (unit.fuel > 0)
        {
            if (unit.carryingPower < unit.capacity)
            {
                UI.AddMessage(new Message("Ship is overloaded!", 3.0f, Color.red));
                return;
            }
            body.AddForce(direction * unit.moveSpeed * speedModifier, ForceMode.Force);
            unit.fuel -= unit.fuelSpendRate;

            GameObject.Find("Core").GetComponent<Events>().OnFuelChanged();

            if (!exhaustL.isPlaying && !exhaustR.isPlaying)
            {
                Exhaust(true);
            }
        }
        else
        {
            Exhaust(false);
            UI.AddMessage(new Message("Ship is out of fuel!", 3.0f, Color.red));
        }
    }

    private void Exhaust(bool state)
    {
        if (state)
        {
            exhaustL.Play();
            exhaustR.Play();
        }
        else
        {
            exhaustL.Stop();
            exhaustL.Clear();
            exhaustR.Stop();
            exhaustR.Clear();
        }
    }

    private void MoveBackward()
    {
        body.AddForce(-direction * unit.moveSpeed * backwardSpeedModifier, ForceMode.Force);
    }

    private void Shoot()
    {
        for (int i = 0; i < unit.modules.Count; i++)
        {
            if (unit.modules[i].referencedObject.type == Modules.WEAPON)
            {
                Weapon w = (Weapon)unit.modules[i].referencedObject;
                w.Shoot(unit.modules[i], gameObject);
            }
        }
    }

    private void MoveAim()
    {
        aim.transform.position = controlledObject.transform.position + controlledObject.transform.forward * unit.focusRange;
    }

    private void ChangeFocusDistance(bool greater)
    {
        if (greater)
        {
            if (unit.focusRange < unit.maxFocusRange)
            {
                unit.focusRange += Time.deltaTime * 35;
            }
        }
        else
        {
            if (unit.focusRange > unit.minFocusRange)
            {
                unit.focusRange -= Time.deltaTime * 35;
            }
        }
    }

    private void ExitSector()
    {
        UI.AddMessage(new Message("Exiting sector [0x291FFF]...", 1.0f, Color.blue));
        Debug.Log("Ship is out of map!");
    }

    public void OnHpChanged()
    {
        GameObject.Find("Core").GetComponent<Events>().OnHealthChanged();
    }

    public static Vector3 GetMouseDirection()
    {
        Vector3 mouse = new Vector3(Input.mousePosition.x - (Screen.width / 2), Input.mousePosition.y - (Screen.height / 2), Input.mousePosition.z);
        return new Vector3(mouse.normalized.x, mouse.normalized.z, mouse.normalized.y);
    }
}
