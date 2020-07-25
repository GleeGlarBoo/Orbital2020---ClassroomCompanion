using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroller2 : MonoBehaviour
{
    public float m_ScrollSpeed = 0.5f;

    private SpriteRenderer m_SpriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        m_SpriteRenderer.material.SetTextureOffset("_MainTex", new Vector2(Time.time * m_ScrollSpeed, 0));
    }
}
