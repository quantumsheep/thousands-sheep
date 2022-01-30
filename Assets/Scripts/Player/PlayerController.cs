using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : Entity
{
    public float Speed = 10.0f;

    public Swarm Swarm;
    public Camera PlayerCamera;
    public GameObject SpriteGameObject;

    public GameObject DronePrefab;

    private CryptoDronesContract _contract;

    public override async void Start()
    {
        base.Start();

        _contract = new CryptoDronesContract(PlayerPrefs.GetString("Account"), PlayerPrefs.GetString("Contract"));

        var drones = await _contract.GetDrones();

        foreach (var drone in drones)
        {
            Debug.Log($"Drone #{drone.id}: elements = [{string.Join(", ", drone.elements)}], speed = {drone.attacksPerSecond}, damages = {drone.attackDamages}, range = {drone.attackRange}");

            var droneGameObject = Instantiate(DronePrefab, Vector3.zero, Quaternion.identity, Swarm.transform);
            var droneComponent = droneGameObject.GetComponent<Drone>();
            droneComponent.AttacksPerSecond = drone.attacksPerSecond;
            droneComponent.BaseAttackDamages = drone.attackDamages;
            droneComponent.BaseAttackRange = drone.attackRange;

            Swarm.AddDrone(droneComponent);
        }

        Swarm.UpdateDrones();
    }

    public override void Update()
    {
        base.Update();

        PlayerCamera.transform.position = new Vector3(transform.position.x, transform.position.y, PlayerCamera.transform.position.z);

        var x = Input.GetAxis("Horizontal");
        var y = Input.GetAxis("Vertical");

        var velocity = new Vector2(x, y) * Speed;

        _rigidbody.velocity = velocity;

        var mousePosition = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var direction = (mousePosition - (Vector2)transform.position).normalized;

        Swarm.SetIsShooting(Input.GetButton("Fire1"), direction);
        SpriteGameObject.transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg, Vector3.forward);
    }
}
