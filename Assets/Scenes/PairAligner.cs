using UnityEngine;

public class PairAligner : MonoBehaviour
{
    public Transform secondImage;

    void Start()
    {
        float width = GetComponent<SpriteRenderer>().bounds.size.x;

        // B を A の右へぴったり揃える
        Vector3 pos = secondImage.position;
        pos.x = transform.position.x + width;
        secondImage.position = pos;
    }
}
