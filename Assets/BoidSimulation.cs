using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BoidSimulationControl : MonoBehaviour
{

    public enum ControlMode
    {
        Seek,
        Pursue,
        Food,
        Obstacle
    }

    public ControlMode controlMode = ControlMode.Seek;

    public GameObject boidPrefab = null;
    public GameObject foodPrefab = null;
    public GameObject targetObject = null;
    public int numBoidsToSpawn = 10;
    public List<Boid> boids = null;

    private void SeekModeControl()
    {
        for (int i = 0; i < boids.Count; i++)
        {
            Vector3 accel = boids[i].Seek(targetObject.transform.position, boids[i].acceleration);

            if (Input.GetMouseButton(0))
            {
                boids[i].rigidbody.linearVelocity += accel * Time.fixedDeltaTime;
                Debug.DrawRay(boids[i].transform.position, accel, Color.green);
            }
            else if (Input.GetMouseButton(1))
            {
                boids[i].rigidbody.linearVelocity -= accel * Time.fixedDeltaTime;
                Debug.DrawRay(boids[i].transform.position, accel, Color.green);
            }

        }

    }

    private void PursueModeControl()
    {
        for (int i = 0; i < boids.Count; i++)
        {
            Vector3 accel = boids[i].Pursue(targetObject.transform.position, boids[i].accelMax, boids[i].speedMax);
            boids[i].rigidbody.linearVelocity += accel * Time.fixedDeltaTime;
            Debug.DrawRay(boids[i].transform.position, accel, Color.green);
        }
    }

    private void Start()
    {
        targetObject = GameObject.Find("target");

        for (int i = 0; i < numBoidsToSpawn; i++)
        {
            Vector3 position = new Vector3(Random.Range(-1.4f, 1.4f), Random.Range(0f, 1.4f), Random.Range(-0.9f, 0.9f));
            Quaternion rotation = Random.rotation;

            GameObject spawnedBoid = Instantiate(boidPrefab, position, rotation);

            Boid boidComponent = spawnedBoid.GetComponent<Boid>();
            boidComponent.speedMax = Random.Range(0.5f, 1.5f);
            boidComponent.accelMax = Random.Range(0.5f, 1.5f);

            boids.Add(boidComponent);

            spawnedBoid.GetComponent<Renderer>().material.SetColor("_BaseColor", Random.ColorHSV(0, 1, 0f, 1f, 0.5f, 1f));

            spawnedBoid.GetComponent<Rigidbody>().linearVelocity = Random.onUnitSphere * 0.7f;

            spawnedBoid.transform.localScale *= Random.Range(0.9f, 3f);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            controlMode = ControlMode.Seek;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            controlMode = ControlMode.Pursue;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            controlMode = ControlMode.Food;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            controlMode = ControlMode.Obstacle;
        }

        if (Input.GetMouseButtonDown(0) && controlMode == ControlMode.Food)
        {
            SpawnFood();
        }

    }

    private void SpawnFood()
    {
        Instantiate(foodPrefab, targetObject.transform.position, Random.rotation);
    }



    private void FixedUpdate()
    {
        for (int i = 0; i < boids.Count; i++)
        {
            float foodSeekRadius = 0.5f;
            Collider[] colliders = Physics.OverlapSphere(boids[i].transform.position, foodSeekRadius);
            foreach (Collider collider in colliders)
            {
                Food food = collider.GetComponent<Food>();
                if (food != null)
                {
                    Vector3 accel = boids[i].Seek(collider.transform.position, boids[i]);
                    boids[i].rigidbody.linearVelocity += accel * Time.fixedDeltaTime;
                    Debug.DrawRay(boids[i].transform.position, accel, Color.green);
                }
            }
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitinfo;
        bool didhit = Physics.Raycast(ray, out hitinfo, 100);


        if (didhit)
        {
            targetObject.transform.position = hitinfo.point;
        }

        switch (controlMode)
        {
            case ControlMode.Seek:
                {
                    SeekModeControl();
                    break;
                }

            case ControlMode.Pursue:
                {
                    PursueModeControl();
                    break;
                }
        }
    }
}



