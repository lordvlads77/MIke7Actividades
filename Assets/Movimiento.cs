using UnityEngine;

public class Movimiento : MonoBehaviour
{
    public Transform endpoint;
    public int totalFrames = 60;

    private Vector3 startPosition;
    private int currentFrame = 0;
    private bool goingForward = true;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        currentFrame += goingForward ? 1 : -1;

        float t = Mathf.Clamp01((float)currentFrame / totalFrames);
        transform.position = Vector3.Lerp(startPosition, endpoint.position, t);

        if (currentFrame >= totalFrames)
            goingForward = false;
        else if (currentFrame <= 0)
            goingForward = true;
    }
}
