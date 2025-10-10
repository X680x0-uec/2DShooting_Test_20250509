using UnityEngine;

public class SineWaveBullet : MonoBehaviour
{
    private Vector2 direction;
    private float speed;
    private float frequency;
    private float amplitude;
    private Vector3 startPos;
    private float time;

    public void SetWave(Vector2 dir, float spd, float freq, float amp)
    {
        direction = dir.normalized;
        speed = spd;
        frequency = freq;
        amplitude = amp;
        startPos = transform.position;
        time = 0f;
    }

    private void Update()
    {
        time += Time.deltaTime;
        Vector2 perp = new Vector2(-direction.y, direction.x);
        Vector3 offset = perp * Mathf.Sin(time * frequency) * amplitude;
        transform.position = startPos + (Vector3)(direction * speed * time) + offset;
    }
}
