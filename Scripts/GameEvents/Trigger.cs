using UnityEngine;
using System.Collections;

public class Enter : MonoBehaviour
{
    [SerializeField] string tagFilter;
    [SerializeField] private GameObject prefab;
    [SerializeField] private Transform spawnPoint;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(tagFilter))
        {
            GameObject.Instantiate(prefab, spawnPoint.position, Quaternion.identity);
        }
    }
}


