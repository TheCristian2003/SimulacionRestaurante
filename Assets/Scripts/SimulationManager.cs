using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Globalization;
using UnityEngine;

public class SimulationManager : MonoBehaviour
{
    [Header("Referencias")]
    public StatsManager statsManager;
    public GameObject[] customerPrefabs;
    public Transform spawnPoint;
    public Transform cashierPoint;
    public Transform exitPoint;
    public Transform[] queuePoints;

    [Header("Parámetros")]
    public float minArrivalTime = 6f;   // ajustado
    public float maxArrivalTime = 8f;  // ajustado
    public float minServiceTime = 5f;   // NUEVO
    public float maxServiceTime = 5f;   // NUEVO
    public int maxQueueSize = 20;

    private Queue<GameObject> queue = new Queue<GameObject>();
    private bool cashierBusy = false;
    private int servedCustomers = 0;

    private List<CustomerRecord> records = new List<CustomerRecord>();
    private Dictionary<GameObject, CustomerRecord> recordByObject = new Dictionary<GameObject, CustomerRecord>();

    private int nextClientId = 1;

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

            int clientId = nextClientId++;

            var rec = new CustomerRecord
            {
                Run = 1,
                ClientId = clientId,
                ArrivalTime = Time.time,
                ServiceStart = -1f,
                ServiceEnd = -1f,
                ServiceTime = -1f,
                Wait = -1f,
                QueueLengthAtArrival = queue.Count,
                Rejected = false
            };

            records.Add(rec);
            recordByObject[c] = rec;

            if (queue.Count < maxQueueSize)
            {
                queue.Enqueue(c);
                statsManager.customersArrived++;
                UpdateQueuePositions();
            }
            else
            {
                rec.Rejected = true;
                recordByObject.Remove(c);
                statsManager.customersArrived++;
                Destroy(c);
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

                if (!recordByObject.TryGetValue(customer, out CustomerRecord rec))
                {
                    Destroy(customer);
                    continue;
                }

                customer.GetComponent<Customer>().SetTarget(cashierPoint.position);

                while (Vector3.Distance(customer.transform.position, cashierPoint.position) > 0.5f)
                    yield return null;

                // TIEMPO DE ESPERA
                float wait = Time.time - rec.ArrivalTime;
                rec.Wait = wait;
                rec.ServiceStart = Time.time;

                Debug.Log($"Cliente {rec.ClientId} espera: {wait:F2}s");

                cashierBusy = true;

                // SERVICIO ALEATORIO (SOLUCIÓN A LA COLA INFINITA)
                float actualServiceTime = Random.Range(minServiceTime, maxServiceTime);
                rec.ServiceTime = actualServiceTime;

                yield return new WaitForSeconds(actualServiceTime);

                rec.ServiceEnd = Time.time;
                cashierBusy = false;

                UpdateQueuePositions();

                customer.GetComponent<Customer>().SetTarget(exitPoint.position);

                while (Vector3.Distance(customer.transform.position, exitPoint.position) > 0.5f)
                    yield return null;

                statsManager.customersServed++;
                servedCustomers++;

                recordByObject.Remove(customer);
                Destroy(customer);

                // Exportar cada 20 clientes (útil para tu entrega)
                if (servedCustomers % 20 == 0)
                {
                    ExportCsv();
                }
            }

            yield return null;
        }
    }

    void UpdateQueuePositions()
    {
        int i = 0;
        foreach (GameObject customer in queue)
        {
            if (customer == null) { i++; continue; }

            if (i == 0 && !cashierBusy)
            {
                customer.GetComponent<Customer>().SetTarget(cashierPoint.position);
            }
            else if (i < queuePoints.Length)
            {
                customer.GetComponent<Customer>().SetTarget(queuePoints[i].position);
            }
            else
            {
                customer.GetComponent<Customer>().SetTarget(queuePoints[queuePoints.Length - 1].position);
            }

            i++;
        }
    }

    // EXPORTAR CSV EN CARPETA "Muestras"
    public void ExportCsv()
    {
        string folderPath = Path.Combine(Application.dataPath, "../Muestras");

        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        int fileIndex = Directory.GetFiles(folderPath, "muestra_*.csv").Length + 1;
        string fileName = "muestra_" + fileIndex + ".csv";
        string path = Path.Combine(folderPath, fileName);

        var sb = new StringBuilder();

        sb.AppendLine("ClientId,ArrivalTime,ServiceStart,ServiceEnd,Wait,ServiceTime,QueueLength,Rejected");

        foreach (var r in records)
        {
            string line = string.Format(CultureInfo.InvariantCulture,
                "{0},{1:F2},{2:F2},{3:F2},{4:F2},{5:F2},{6},{7}",
                r.ClientId,
                r.ArrivalTime,
                r.ServiceStart,
                r.ServiceEnd,
                r.Wait,
                r.ServiceTime,
                r.QueueLengthAtArrival,
                r.Rejected ? 1 : 0
            );

            sb.AppendLine(line);
        }

        File.WriteAllText(path, sb.ToString(), Encoding.UTF8);

        Debug.Log("CSV guardado en: " + path);
    }

    void OnApplicationQuit()
    {
        ExportCsv();
    }

    [System.Serializable]
    public class CustomerRecord
    {
        public int Run;
        public int ClientId;
        public float ArrivalTime;
        public float ServiceStart;
        public float ServiceEnd;
        public float Wait;
        public float ServiceTime;
        public int QueueLengthAtArrival;
        public bool Rejected;
    }
}