using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleableEntity : MonoBehaviour
{
    public static Color[] gizmoColors = { Color.white, Color.red, Color.blue, Color.green };

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


            if (lerp == 1f)
            {
                lerping = false;
            }
        }
    }

    private void FixedUpdate()
    {
        transform.GetChild(0).localScale = Vector2.Lerp(scales[lastScaleIndex], scales[currentScaleIndex], scaleCurve.Evaluate(lerp));
        col.size = Vector2.Lerp(scales[lastScaleIndex], scales[currentScaleIndex], scaleCurve.Evaluate(lerp));
        col.offset = colOffset * col.size;
    }

    /*private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == Player.instance.gameObject)
        {
            collision.transform.parent = transform.GetChild(0);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject == Player.instance.gameObject)
        {
            Vector2 s = transform.GetChild(0).localScale;
            s = new Vector2(1f / s.x, 1f / s.y);
            collision.transform.localScale = s;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject == Player.instance.gameObject)
        {
            collision.transform.parent = null;
            collision.transform.localScale = Vector2.one;
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