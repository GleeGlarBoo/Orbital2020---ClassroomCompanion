using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{

    public float m_ScrollSpeed;
    private SpriteRenderer m_SpriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // Time.Time will be updated by Unity every frame, and will increase over time.
        m_SpriteRenderer.material.SetTextureOffset("_MainTex", new Vector2(0, Time.time * m_ScrollSpeed));
    }
}
