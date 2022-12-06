using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Santa : MonoBehaviour
{
    [SerializeField]
    private float spd = 1f;

    private Vector3 santaPos;

    private bool inDistance = false;
    private bool spawn = true;

    [SerializeField]
    private GameObject present;

    [SerializeField]
    private Mesh[] meshes;

    void Update()
    {
        transform.position += transform.forward * Time.deltaTime * spd;
        if (transform.position.z > 90)
        {
            GameManager.Instance.ResetSanta(gameObject);
        }

        santaPos = new Vector3(transform.position.x, 0, transform.position.z);
        inDistance = (Vector3.Distance(santaPos, Vector3.zero) < 25f);
        if (spawn && inDistance) StartCoroutine(SpawnPresent());
    }

    IEnumerator SpawnPresent()
    {
        spawn = false;
        int random = Random.Range(0, meshes.Length);

        //if(GameManager.Instance.presents < 5)
        //{
            GameObject currentPresent = Instantiate(present);
            currentPresent.transform.position = transform.position;
            currentPresent.GetComponent<MeshFilter>().mesh = meshes[random];
            currentPresent.AddComponent<BoxCollider>();
            currentPresent.AddComponent<BoxCollider>().isTrigger = true;
        //}
        //else
        

        float randomT = Random.Range(0.5f, 3f);
        yield return new WaitForSeconds(randomT);
        spawn = true;
    }
}
