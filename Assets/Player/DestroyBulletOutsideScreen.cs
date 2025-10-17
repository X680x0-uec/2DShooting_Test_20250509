using UnityEngine;

public class DestroyBulletOutsideScreen : MonoBehaviour
{
    public float margin = 3f; 

    private Vector2 screenBounds;

    void Start()
    {
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
    }

    void Update()
    {
        Vector2 currentPosition = transform.position;

        if (Mathf.Abs(currentPosition.x) > screenBounds.x + margin)
        {
            Destroy(gameObject);
        }
        
        if (Mathf.Abs(currentPosition.y) > screenBounds.y + margin)
        {
            Destroy(gameObject);
        }
    }
}
