using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct RegisteredWeapons
{
    public Weapon real;
    public Weapon hidden;
}

public class ShootTrajectory : MonoBehaviour
{

    public static bool charging;
    public GameObject weapon;
    public Transform referenceWeapon;

    public GameObject marker;
    private List<GameObject> markers = new List<GameObject>();
    private Dictionary<string, RegisteredWeapons> allWeapons = new Dictionary<string, RegisteredWeapons>();
    public GameObject objectsToSpawn;

    private void Start()
    {
        
    }

    private void FixedUpdate()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            ShowTrajectory();
        }
    }

    public void RegisterWeapon(Weapon weapon)
    {
        if (!allWeapons.ContainsKey(weapon.gameObject.name))
            allWeapons[weapon.gameObject.name] = new RegisteredWeapons();
        var weapons = allWeapons[weapon.gameObject.name];

        weapons.real = weapon;
        allWeapons[weapon.gameObject.name] = weapons;
    }

    public void CreateMovementMarkers()
    {

    }

    public void ShowTrajectory()
    {

    }

    public void SyncArrows()
    {

    }
}
