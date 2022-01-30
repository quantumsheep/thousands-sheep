using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class Swarm : MonoBehaviour
{
    public Collider2D PlayerCollider;

    public float RotationSpeed = 360.0f;
    public float Radius = 1.5f;

    public bool RandomDrones = true;

    [EnableIf("RandomDrones")]
    public int DroneCount = 5;
    [EnableIf("RandomDrones")]
    public GameObject DronePrefab;

    [DisableIf("RandomDrones")]
    public List<Drone> Drones = new List<Drone>();

    void Start()
    {
        UpdateDrones();
    }

    void Update()
    {
        transform.Rotate(Vector3.forward * (RotationSpeed * Time.deltaTime));
    }

    public void SetIsShooting(bool isShooting, Vector2 direction)
    {
        foreach (var drone in Drones)
        {
            drone.SetIsShooting(isShooting, direction);
        }
    }

    public void AddDrone(Drone drone)
    {
        drone.PlayerCollider = PlayerCollider;
        Drones.Add(drone);
    }

    [EnableIf("RandomDrones")]
    [Button]
    public void UpdateDrones()
    {
        if (RandomDrones)
        {
            if (DroneCount < Drones.Count)
            {
                for (int i = DroneCount; i < Drones.Count; i++)
                {
                    var drone = Drones[i];
                    Destroy(drone.gameObject);
                }

                Drones.RemoveRange(DroneCount, Drones.Count - DroneCount);
            }

            for (int i = Drones.Count; i < DroneCount; i++)
            {
                GameObject droneGameObject = Instantiate(DronePrefab, Vector3.zero, Quaternion.identity, transform);
                var drone = droneGameObject.GetComponent<Drone>();
                drone.PlayerCollider = PlayerCollider;

                Drones.Add(drone);
            }
        }

        for (int i = 0; i < Drones.Count; i++)
        {
            var drone = Drones[i];
            UpdateDronePosition(drone, i);
        }
    }

    private void UpdateDronePosition(Drone drone, int index)
    {
        drone.transform.localPosition = GetDronePosition(index);
    }

    private Vector3 GetDronePosition(int index)
    {
        var x = Radius * Mathf.Cos(2f * Mathf.PI * index / Drones.Count);
        var y = Radius * Mathf.Sin(2f * Mathf.PI * index / Drones.Count);
        return new Vector3(x, y, 0);
    }
}
