using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    [SerializeField] private GameObject car;
    [SerializeField] private float minTimeSpawn = 10;
    [SerializeField] private float maxTimeSpawn = 50;

    private float timeSpawn;

    private void Start()
    {
        timeSpawn = Random.Range(minTimeSpawn, maxTimeSpawn);
    }
    private void Update()
    {
        timeSpawn -= Time.deltaTime;
        if (timeSpawn <= 0)
        {
            SpawnCar();
            timeSpawn = Random.Range(minTimeSpawn, maxTimeSpawn);
        }
    }

    private void SpawnCar()
    {
        GameObject newCar = Instantiate(car, transform.position, transform.rotation);
        newCar.GetComponent<CarController>().SetSpeed(Random.Range(5, 10));
    }
}
