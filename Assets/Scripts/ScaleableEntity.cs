using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleableEntity : MonoBehaviour
{
    public static Color[] gizmoColors = { Color.white, Color.red, Color.blue, Color.green };

    [SerializeField] BoxCollider2D col;
    [SerializeField] SpriteRenderer sr;
    [SerializeField] Transform contact;
    [SerializeField] GameObject handSprite;

    private Vector2 colOffset, srOffset, contactOffset;
    private int lastScaleIndex;
    private int currentScaleIndex;

    public Vector2[] scales;
    public float scaleSpeed;
    public AnimationCurve scaleCurve;

    [SerializeField] bool parentsPlayer;

    bool lerping;
    float lerp = 1f;

    private void OnDrawGizmosSelected()
    {
        Vector2 offset = col.offset + 0.125f * Vector2.up;
        offset /= scales[0];

        for (int i = 0; i < scales.Length; i++)
        {
            Color c = gizmoColors[Mathf.Clamp(i, 0, gizmoColors.Length)];
            c.a = 0.5f;
            Gizmos.color = c;

            Gizmos.DrawCube((Vector2)transform.position + (Vector2)transform.TransformVector((offset * scales[i])), transform.TransformVector(scales[i]));
        }
    }

    private void Start()
    {
        lastScaleIndex = scales.Length - 1;
        colOffset = (col.offset + 0.125f * Vector2.up) / scales[0];
        srOffset = sr.transform.localPosition / scales[0];
        contactOffset = contact.localPosition / scales[0];
    }

    private void OnMouseOver()
    {
        if (Player.instance.PlayerClicked())
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

            Vector2 s = Vector2.Lerp(scales[lastScaleIndex], scales[currentScaleIndex], scaleCurve.Evaluate(lerp));
            contact.localPosition = contactOffset * s;

            sr.size = s;
            sr.transform.localPosition = srOffset * s;

            if (lerp == 1f)
            {
                lerping = false;
                handSprite.SetActive(false);
            }
        }
    }

    private void FixedUpdate()
    {
        Vector2 s = Vector2.Lerp(scales[lastScaleIndex], scales[currentScaleIndex], scaleCurve.Evaluate(lerp));

        col.size = s - 0.25f * Vector2.up;
        col.offset = colOffset * s - 0.125f * Vector2.up;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (parentsPlayer && collision.gameObject == Player.instance.gameObject)
        {
            collision.transform.parent = contact;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (parentsPlayer && collision.gameObject == Player.instance.gameObject)
        {
            collision.transform.parent = null;
        }
    }

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

        if(currentScaleIndex == scales.Length) //Instead of resetting scale, can we decrement to the last index, so it shrinks again on click instead of resetting at the end.
        {
            currentScaleIndex = 0;
        }

        handSprite.SetActive(true);
        handSprite.transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + (scales[lastScaleIndex].magnitude < scales[currentScaleIndex].magnitude ? 180f : 0f) * Vector3.forward);
        
        SoundManager.instance.PlaySound("Scale Up");
    }
}