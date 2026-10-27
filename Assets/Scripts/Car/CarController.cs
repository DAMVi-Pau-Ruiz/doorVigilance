using UnityEngine;

public class CarController : MonoBehaviour
{
    [SerializeField] private float speed;

    private void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CarDestroy"))
        {
            Destroy(gameObject);
        }
    }
}
