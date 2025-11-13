using UnityEngine;
using System.Collections;

public class AutoDestroyEffect : MonoBehaviour
{
    public void DestroyOnAnimationEnd()
    {
        Destroy(gameObject);
    }
}
