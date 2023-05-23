using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MummyStage : MonoBehaviour
{
    [SerializeField]
    private Renderer floorRenderer;

    [SerializeField]
    private Material redMaterial;
    [SerializeField]
    private Material blueMaterial;
    [SerializeField]
    private Material grayMaterial;

    public void StartEpisode()
    {
        floorRenderer.material = grayMaterial;
    }

    public void Success(Action callback)
    {
        StartCoroutine(ChangeCoroutine(blueMaterial, callback));
    }

    public void Fail(Action callback)
    {
        StartCoroutine(ChangeCoroutine(redMaterial, callback));
    }

    private IEnumerator ChangeCoroutine(Material material, Action callback)
    {
        floorRenderer.material = material;
        yield return new WaitForSeconds(1f);
        callback?.Invoke();
    }
}
