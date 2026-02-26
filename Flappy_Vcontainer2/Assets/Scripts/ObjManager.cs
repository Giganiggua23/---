using UnityEngine;
using System.Collections;
using VContainer;
using VContainer.Unity;

public class ObjManager : MonoBehaviour
{
    [SerializeField] private GameObject objectA;
    [SerializeField] private GameObject objectB;
    [SerializeField] private float offsetY = 2f;
    [SerializeField] private float spawnInterval = 1f;

    private IObjectResolver resolver;

    [Inject]
    public void Construct(IObjectResolver resolver)
    {
        this.resolver = resolver;
    }

    void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            resolver.Instantiate(objectA, transform.position, Quaternion.identity);

            Vector3 spawnPositionB = new Vector3(
                transform.position.x,
                transform.position.y - offsetY,
                transform.position.z
            );
            resolver.Instantiate(objectB, spawnPositionB, Quaternion.identity);

            yield return new WaitForSeconds(spawnInterval);
        }
    }
}