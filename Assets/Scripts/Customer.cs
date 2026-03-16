
using UnityEngine;

public class Customer : MonoBehaviour
{
    public float speed = 4f;
    private Vector3 target;

    public void SetTarget(Vector3 position)
    {
        target = position;
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            target,
            speed * Time.deltaTime
        );

        Vector3 direction = target - transform.position;

        if(direction != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(direction),
                5f * Time.deltaTime
            );
        }
    }

    void Start()
    {
        int randomModel = Random.Range(0, transform.childCount);

        for(int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(i == randomModel);
        }
    }
}