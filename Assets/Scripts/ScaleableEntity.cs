using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleableEntity : MonoBehaviour
{
    public static Color[] gizmoColors = { Color.white, Color.red, Color.blue, Color.green };

    private int lastScaleIndex;
    private int currentScaleIndex;
    public Vector2[] scales;
    public float scaleSpeed;
    public AnimationCurve scaleCurve;

    bool lerping;
    float lerp;

    private void OnDrawGizmosSelected()
    {
        Vector2 offset = transform.GetChild(0).localPosition;
        offset.x /= scales[0].x;
        offset.y /= scales[0].y;

        for (int i = 0; i < scales.Length; i++)
        {
            Color c = gizmoColors[Mathf.Clamp(i, 0, gizmoColors.Length)];
            c.a = 0.5f;
            Gizmos.color = c;

            Gizmos.DrawCube((Vector2)transform.position + (offset * scales[i]), scales[i]);
        }
    }

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

    /*private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject == Player.instance.gameObject)
        {

        }
    }*/

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