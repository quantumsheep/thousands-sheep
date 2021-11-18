using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class Swarm : MonoBehaviour
{
    public Collider2D PlayerCollider;

    public float RotationSpeed = 360.0f;
    public float Radius = 1.5f;
    public int DroneCount = 5;
    public GameObject DronePrefab;

    private List<Drone> _drones = new List<Drone>();

    void Start()
    {
        UpdateDrones();
    }

    void Update()
    {
        transform.Rotate(Vector3.forward * (RotationSpeed * Time.deltaTime));
    }

    public void SetIsShooting(bool isShooting, Vector3 direction)
    {
        foreach (var drone in _drones)
        {
            drone.SetIsShooting(isShooting, direction);
        }
    }

    [Button]
    void UpdateDrones()
    {
        for (int i = 0; i < _drones.Count; i++)
        {
            var drone = _drones[i];
            UpdateDronePosition(drone, i);
        }

        for (int i = _drones.Count; i < DroneCount; i++)
        {
            GameObject droneGameObject = Instantiate(DronePrefab, Vector3.zero, Quaternion.identity, transform);
            var drone = droneGameObject.GetComponent<Drone>();
            drone.PlayerCollider = PlayerCollider;

            UpdateDronePosition(drone, i);

            _drones.Add(drone);
        }
    }

    private void UpdateDronePosition(Drone drone, int index)
    {
        drone.transform.localPosition = GetDronePosition(index);
    }

    private Vector3 GetDronePosition(int index)
    {
        var x = Radius * Mathf.Cos(2f * Mathf.PI * index / DroneCount);
        var y = Radius * Mathf.Sin(2f * Mathf.PI * index / DroneCount);
        return new Vector3(x, y, 0);
    }
}
