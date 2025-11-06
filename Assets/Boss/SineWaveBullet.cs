using UnityEngine;

public class SineWaveBullet : MonoBehaviour
{
    private Vector2 direction;
    private float speed;
    private float amplitude;
    private float frequency;
    private float phase; // 位相

    private float startY;
    private float lifetime = 10f;

    public void SetWave(Vector2 dir, float spd, float amp, float freq, float ph)
    {
        direction = dir.normalized;
        speed = spd;
        amplitude = amp;
        frequency = freq;
        phase = ph;

        startY = transform.position.y;

        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        // 左方向に移動しつつ、上下に正弦波運動
        float newY = startY + Mathf.Sin(Time.time * frequency + phase) * amplitude;
        transform.position += (Vector3)(direction * speed * Time.deltaTime);
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}
