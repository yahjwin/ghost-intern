using UnityEngine;
using UnityEngine.UI;

public class BackgroundMove : MonoBehaviour
{
    public RectTransform background;

    public float zoomSpeed = 0.05f;
    public float maxZoom = 1.15f;

    public float swayAmount = 5f;
    public float swaySpeed = 1.5f;

    private Vector3 startScale;
    private Vector2 startPos;

    void Start()
    {
        startScale = background.localScale;
        startPos = background.anchoredPosition;
    }

    void Update()
    {
        float scale =
            1f +
            Mathf.PingPong(
                Time.time * zoomSpeed,
                maxZoom - 1f);

        background.localScale =
            startScale * scale;

        float offsetX =
            Mathf.Sin(Time.time * swaySpeed)
            * swayAmount;

        background.anchoredPosition =
            startPos +
            new Vector2(offsetX, 0);
    }
}