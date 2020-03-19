using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trail : MonoBehaviour
{

    public float dimSpeed;
    public void NewTrail()
    {
        Instantiate(gameObject, transform.parent);

        TrailRenderer trail = GetComponent<TrailRenderer>();
        StartCoroutine(Dim50(trail));
    }


    IEnumerator Dim50(TrailRenderer trail)
    {
        float a = 1;
        while (a > 0.5f)
        {

            a -= dimSpeed * Time.deltaTime;
            Gradient newGradient = new Gradient();
            newGradient.SetKeys(
                new GradientColorKey[] { new GradientColorKey(Color.white, 0f), new GradientColorKey(Color.white, 1f) }, 
                new GradientAlphaKey[] { new GradientAlphaKey(a, 0f), new GradientAlphaKey(a, 1f) }
                );
            trail.colorGradient = newGradient;
            yield return null;
        }
    }

    public void DimAndDestroySelf()
    {
        StartCoroutine(DimAndDestroy(GetComponent<TrailRenderer>()));
    }

    IEnumerator DimAndDestroy(TrailRenderer trail)
    {
        float a = trail.colorGradient.alphaKeys[0].alpha;
        while (a > 0f)
        {

            a -= dimSpeed * Time.deltaTime;
            Gradient newGradient = new Gradient();
            newGradient.SetKeys(
                new GradientColorKey[] { new GradientColorKey(Color.white, 0f), new GradientColorKey(Color.white, 1f) },
                new GradientAlphaKey[] { new GradientAlphaKey(a, 0f), new GradientAlphaKey(a, 1f) }
                );
            trail.colorGradient = newGradient;
            yield return null;
        }
        Destroy(gameObject);
    }
}
