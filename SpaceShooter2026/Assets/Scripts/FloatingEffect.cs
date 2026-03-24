using UnityEngine;

public class TitleFloat : MonoBehaviour
{
    // set in inspector
    public float pulseSpeed = 2f;
    public float pulseAmount = 0.03f;
    public float bobSpeed = 1.5f;
    public float bobAmount = 6f;

    // private fields
    private Vector3 startScale;
    private Vector3 startPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startScale = transform.localScale;
        startPosition = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        float scaleOffset = 1f + Mathf.Sin(Time.time * pulseSpeed) * pulseAmount;
        transform.localScale = startScale * scaleOffset;

        float yOffset = Mathf.Sin(Time.time * bobSpeed) * bobAmount;
        transform.localPosition = new Vector3(
            startPosition.x,
            startPosition.y + yOffset,
            startPosition.z
        );
    }
}
