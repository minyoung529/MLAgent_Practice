using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MummyILStage : MonoBehaviour
{
    [SerializeField]
    private Renderer floor;

    [SerializeField]
    private Material successMat;

    [SerializeField]
    private Material failMat;

    [SerializeField]
    private Material originalMat;

    public void Success(System.Action callback)
    {
        StartCoroutine(ChangeCoroutine(successMat, callback));
    }

    public void Fail(System.Action callback)
    {
        StartCoroutine(ChangeCoroutine(failMat, callback));
    }

    public void OnReset()
    {
        floor.material = originalMat;
    }

    private IEnumerator ChangeCoroutine(Material mat, System.Action callback)
    {
        floor.material = mat;
        yield return new WaitForSeconds(0.2f);
        callback?.Invoke();
        OnReset();
    }
}
