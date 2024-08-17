using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleableEntity : MonoBehaviour
{
    public static Color[] gizmoColors = { Color.white, Color.red, Color.blue, Color.green };

    [SerializeField] Rigidbody2D rb;
    [SerializeField] BoxCollider2D col;

    private Vector2 colOffset;
    private int lastScaleIndex;
    private int currentScaleIndex;
    public Vector2[] scales;
    public float scaleSpeed;
    public AnimationCurve scaleCurve;

    bool lerping;
    float lerp = 1f;

    private void OnDrawGizmosSelected()
    {
        Vector2 offset = col.offset;
        offset /= col.size;

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
        colOffset = col.offset;
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
            lerp += scaleSpeed * Time.fixedDeltaTime;
            lerp = Mathf.Clamp01(lerp);

            transform.GetChild(0).localScale = Vector2.Lerp(scales[lastScaleIndex], scales[currentScaleIndex], scaleCurve.Evaluate(lerp));

            if (lerp == 1f)
            {
                lerping = false;
            }
        }
    }

    private void FixedUpdate()
    {
        col.size = Vector2.Lerp(scales[lastScaleIndex], scales[currentScaleIndex], scaleCurve.Evaluate(lerp));
        col.offset = colOffset * col.size;
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
        lerp = 0f;

        lastScaleIndex = currentScaleIndex;
        currentScaleIndex++;

        if(currentScaleIndex == scales.Length)
        {
            currentScaleIndex = 0;
        }
    }
}