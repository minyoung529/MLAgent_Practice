using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Random = UnityEngine.Random;

public class MummyRayStage : MonoBehaviour
{
    [Header("Item")]
    [SerializeField]
    private GameObject goodItemprefab;

    [SerializeField]
    private GameObject badItemPrefab;

    [Header("Floor")]
    [SerializeField]
    private Material goodMaterial;

    [SerializeField]
    private Material badMaterial;

    [SerializeField]
    private Material originalMaterial;

    [SerializeField]
    private Renderer floorRenderer;

    private List<GameObject> items = new List<GameObject>();

    public void OnEpisodeBegin()
    {
        DestroyItems();

        for (int i = 0; i < 30; i++)
        {
            GameObject good = Instantiate(goodItemprefab, transform);
            good.transform.localPosition = GetRandomPosition();
            good.transform.rotation = Random.rotation;

            items.Add(good);
        }

        for (int i = 0; i < 20; i++)
        {
            GameObject bad = Instantiate(badItemPrefab, transform);
            bad.transform.localPosition = GetRandomPosition();
            bad.transform.rotation = Random.rotation;

            items.Add(bad);
        }
    }

    private void DestroyItems()
    {
        for (int i = 0; i < items.Count; i++)
        {
            Destroy(items[i].gameObject);
        }

        items.Clear();
    }

    public void DestroyItem(GameObject item)
    {
        items.Remove(item);
        Destroy(item.gameObject);
    }

    public void OnEpisodeEnd()
    {
        DestroyItems();
        floorRenderer.material = originalMaterial;
    }

    public void Success(Action callBack)
    {
        StartCoroutine(ChangeCoroutine(callBack, goodMaterial));
    }

    public void Fail(Action callBack)
    {
        StartCoroutine(ChangeCoroutine(callBack, badMaterial));
    }

    private IEnumerator ChangeCoroutine(Action callBack, Material material)
    {
        floorRenderer.material = material;
        yield return new WaitForSeconds(0.2f);

        OnEpisodeEnd();
        callBack?.Invoke();
    }


    private Vector3 GetRandomPosition()
    {
        return new Vector3(Random.Range(-23f, 23f), 0f, Random.Range(-23f, 23f));
    }
}
