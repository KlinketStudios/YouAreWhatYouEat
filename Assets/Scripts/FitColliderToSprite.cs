using UnityEngine;


public class FitColliderToSprite : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private BoxCollider bc;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bc.size = sr.sprite.bounds.size;
    }
    
}
