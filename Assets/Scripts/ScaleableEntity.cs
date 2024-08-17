using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleableEntity : MonoBehaviour
{
    private int lastScaleIndex;
    private int currentScaleIndex;
    public Vector2[] scales;
    public float scaleSpeed;
    public AnimationCurve scaleCurve;

    bool lerping;
    float lerp;

    private void Start()
    {
        lastScaleIndex = scales.Length - 1;
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Interact();
        }
    }

    private void Update()
    {
        if (lerping)
        {
            lerp += scaleSpeed * Time.deltaTime;
            lerp = Mathf.Clamp01(lerp);

            transform.localScale = Vector2.Lerp(scales[lastScaleIndex], scales[currentScaleIndex], scaleCurve.Evaluate(lerp));

            if(lerp == 1f)
            {
                lerp = 0f;
                lerping = false;
            }
        }
    }

    public void Interact()
    {
        if(lerping)
        {
            return;
        }

        lerping = true;

        lastScaleIndex = currentScaleIndex;
        currentScaleIndex++;

        if(currentScaleIndex == scales.Length)
        {
            currentScaleIndex = 0;
        }
    }
}