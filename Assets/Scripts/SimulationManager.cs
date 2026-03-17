using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulationManager : MonoBehaviour
{
    public StatsManager statsManager;
    public GameObject[] customerPrefabs;
    public Transform spawnPoint;

    public Transform cashierPoint;
    public Transform exitPoint;
    public Transform[] queuePoints;

    public float minArrivalTime = 6f;
    public float maxArrivalTime = 8f;
    public float serviceTime = 5f;
    public int maxQueueSize = 10;

    private Queue<GameObject> queue = new Queue<GameObject>();
    private bool cashierBusy = false;
    private List<float> waitingTimes = new List<float>();
    private Dictionary<GameObject, float> arrivalTimes = new Dictionary<GameObject, float>();
    private int servedCustomers = 0;

    void Start()
    {
        StartCoroutine(GenerateCustomers());
        StartCoroutine(ServiceCustomers());
    }

    IEnumerator GenerateCustomers()
    {
        while (true)
        {
            float randomTime = Random.Range(minArrivalTime, maxArrivalTime);
            yield return new WaitForSeconds(randomTime);

            GameObject prefab = customerPrefabs[Random.Range(0, customerPrefabs.Length)];
            GameObject c = Instantiate(prefab, spawnPoint.position, Quaternion.identity);

            arrivalTimes[c] = Time.time;

            if(queue.Count < maxQueueSize)
            {
                queue.Enqueue(c);
                statsManager.customersArrived++;
                UpdateQueuePositions();

                Debug.Log("Cliente llegó. Cola: " + queue.Count);
            }
            else
            {
                Destroy(c);
                Debug.Log("Cliente se fue, cola llena");
            }
        }
    }

    IEnumerator ServiceCustomers()
    {
        while (true)
        {
            if (!cashierBusy && queue.Count > 0)
            {
                GameObject customer = queue.Dequeue();

                // el cliente va al cajero
                customer.GetComponent<Customer>().SetTarget(cashierPoint.position);

                // esperar a que llegue
                while (Vector3.Distance(customer.transform.position, cashierPoint.position) > 0.5f)
                {
                    yield return null;
                }

                // 🔵 AQUI calculas el tiempo de espera
                float wait = Time.time - arrivalTimes[customer];
                waitingTimes.Add(wait);

                Debug.Log("Tiempo de espera cliente: " + wait);

                // AHORA empieza el servicio
                cashierBusy = true;

                yield return new WaitForSeconds(serviceTime);

                // servicio terminado
                cashierBusy = false;

                UpdateQueuePositions();

                // cliente se va
                customer.GetComponent<Customer>().SetTarget(exitPoint.position);

                while (Vector3.Distance(customer.transform.position, exitPoint.position) > 0.5f)
                {
                    yield return null;
                }

                statsManager.customersServed++;
                Destroy(customer);
            }

            yield return null;
        }
    }

    void UpdateQueuePositions()
        {
            int i = 0;

            foreach (GameObject customer in queue)
            {
                if (i == 0 && !cashierBusy)
                {
                    // el primero va directo al cajero
                    customer.GetComponent<Customer>().SetTarget(cashierPoint.position);
                }
                else if (i < queuePoints.Length)
                {
                    customer.GetComponent<Customer>().SetTarget(queuePoints[i].position);
                }

                i++;
            }
        }
}