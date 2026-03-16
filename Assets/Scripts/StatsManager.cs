using UnityEngine;
using TMPro;

public class StatsManager : MonoBehaviour
{
    public TextMeshProUGUI statsText;

    public int customersArrived = 0;
    public int customersServed = 0;

    void Update()
    {
        statsText.text =
        "Clientes llegados: " + customersArrived +
        "\nClientes atendidos: " + customersServed;
    }
}