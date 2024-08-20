using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScaleableEntity : MonoBehaviour
{
    public static List<ScaleableEntity> scaleableEntities = new List<ScaleableEntity>();

    public static Color[] gizmoColors = { Color.black, Color.red, Color.blue, Color.green, Color.cyan, Color.yellow, Color.magenta, Color.white };

    [SerializeField] BoxCollider2D col;
    [SerializeField] SpriteRenderer sr, indicator, indicatorBG;
    [SerializeField] Transform contact;
    [SerializeField] GameObject handSprite;
    [SerializeField] TextMeshProUGUI counterText;

    private Vector2 colOffset, srOffset, contactOffset;

    private int lastScaleIndex;
    private Vector2 lastScale;

    private int currentScaleIndex;
    private Vector2 currentScale;

    public Vector2[] scales;
    public float scaleSpeed, resetSpeed;
    public AnimationCurve scaleCurve;

    [SerializeField] bool parentsPlayer;

    bool lerping, resetting;
    float lerp = 1f;

    private void OnDrawGizmos()
    {
        Vector2 offset = col.offset + 0.125f * Vector2.up;
        offset /= scales[0];

        for (int i = 0; i < scales.Length; i++)
        {
            Color c = gizmoColors[Mathf.Clamp(i, 0, gizmoColors.Length)];
            c.a = 0.2f;
            Gizmos.color = c;

            Gizmos.DrawCube((Vector2)transform.position + (Vector2)transform.TransformVector((offset * scales[i])), transform.TransformVector(scales[i]));
        }
    }

    private void Awake()
    {
        scaleableEntities.Add(this);
    }

    private void Start()
    {
        lastScaleIndex = scales.Length - 1;
        lastScale = scales[lastScaleIndex];

        colOffset = (col.offset + 0.125f * Vector2.up) / scales[0];
        srOffset = sr.transform.localPosition / scales[0];
        contactOffset = contact.localPosition / scales[0];

        counterText.text = (currentScaleIndex + 1).ToString() + "/" + scales.Length;
        counterText.transform.rotation = Quaternion.identity;

        UpdateIndicator();
    }

    private void OnMouseOver()
    {
        Debug.Log("Mouse over " + gameObject.name);

        if (Player.instance.PlayerClicked())
        {
            Interact();
        }
    }

    private void OnMouseEnter()
    {
        indicator.gameObject.SetActive(true);
    }

    private void OnMouseExit()
    {
        indicator.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (lerping)
        {
            lerp += ((resetting ? resetSpeed : scaleSpeed) * Time.deltaTime) / Vector2.Distance(lastScale, scales[currentScaleIndex]);
            lerp = Mathf.Clamp01(lerp);

            currentScale = Vector2.Lerp(lastScale, scales[currentScaleIndex], scaleCurve.Evaluate(lerp));
            contact.localPosition = contactOffset * currentScale;

            sr.size = currentScale;
            sr.transform.localPosition = srOffset * currentScale;

            if (lerp == 1f)
            {
                UpdateCol();

                lerping = false;
                resetting = false;

                handSprite.SetActive(false);

                UpdateIndicator();
            }
        }
    }

    private void FixedUpdate()
    {
        //currentScale = Vector2.Lerp(lastScale, scales[currentScaleIndex], scaleCurve.Evaluate(lerp));

        if (lerping)
        {
            UpdateCol();
        }
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

    private void UpdateCol()
    {
        col.size = currentScale - 0.25f * Vector2.up;
        col.offset = colOffset * currentScale - 0.125f * Vector2.up;
    }

    private void UpdateIndicator()
    {
        int i = currentScaleIndex + 1;

        if(i == scales.Length)
        {
            i = 0;
        }

        Vector2 nextScale = scales[i];

        indicator.size = nextScale;
        indicator.transform.localPosition = srOffset * nextScale;

        indicatorBG.size = nextScale;
    }

    public void Interact()
    {
        Debug.Log("Interacted with" + gameObject);

        if(lerping)
        {
            return;
        }

        lerping = true;

        lerp = 0f;

        lastScaleIndex = currentScaleIndex;
        lastScale = scales[lastScaleIndex];

        currentScaleIndex++;

        if(currentScaleIndex == scales.Length) //Instead of resetting scale, can we decrement to the last index, so it shrinks again on click instead of resetting at the end.
        {
            currentScaleIndex = 0;
        }

        counterText.text = (currentScaleIndex + 1).ToString() + "/" + scales.Length;

        handSprite.SetActive(true);
        handSprite.transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + (lastScale.magnitude < scales[currentScaleIndex].magnitude ? 180f : 0f) * Vector3.forward);

        SoundManager.instance.PlaySound("Scale Up");
    }

    public void ResetScale()
    {
        if (lerping)
        {
            lastScale = currentScale;
        }
        else
        {
            if (currentScaleIndex == 0)
            {
                return;
            }

            lastScale = scales[currentScaleIndex];
        }

        lastScaleIndex = scales.Length - 1;
        currentScaleIndex = 0;

        lerping = true;

        lerp = 0f;

        counterText.text = (currentScaleIndex + 1).ToString() + "/" + scales.Length;

        handSprite.SetActive(false);

        resetting = true;
    }
}