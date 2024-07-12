using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SM_UVScroller : MonoBehaviour
{
    public int targetMaterialSlot = 0;

    public float speedY = 0.5f;
    public float speedX = 0.0f;

    public float timeWentX =0;
    public float timeWentY =0;
    private Renderer m_render;
    private Material m_mat;

    //private int TexHash=0;

    Vector2 offset = new Vector2();

	// Use this for initialization
	void Start () 
    {
        m_render = GetComponent<Renderer>();
        m_mat = m_render.materials[targetMaterialSlot];
	}
	// Update is called once per frame
	void Update () 
    {
		timeWentY += Time.deltaTime*speedY;
        timeWentX += Time.deltaTime*speedX; 

        offset.x = timeWentX;
        offset.y = timeWentY;

        m_mat.SetTextureOffset("_MainTex", offset);
	}
}
