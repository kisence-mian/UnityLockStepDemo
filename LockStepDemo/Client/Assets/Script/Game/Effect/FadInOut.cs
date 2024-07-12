using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadInOut : MonoBehaviour
{
    public float lifeCycle = 2.0f;

    public Color ghostColor;

    float startTime;
    public Material mat = null;
    public MeshFilter meshFilter;
    public MeshRenderer meshRender;

    static Shader ghostShader = null;
    Vector3 colorV3;

    public void Init(Vector3 ghostColor)
    {
        if (ghostShader == null)
        {
            ghostShader = ResourceManager.Load<Shader>("Unlit/Ghost");
        }
        colorV3 = ghostColor;
    }


    // Use this for initialization
    void OnEnable()
    {

        ghostColor = new Color(colorV3.x, colorV3.y, colorV3.z, 0.5f);
        startTime = Time.time;

        if (meshRender == null || !meshRender.material)
        {
            base.enabled = false;
        }
        else
        {
            mat = meshRender.material;
            ReplaceShader();
        }
    }
    // Update is called once per frame
    void Update()
    {
        float time = Time.time - startTime;

        if (time > lifeCycle)
        {
            
            base.enabled = false;
            GameObjectManager.DestroyGameObjectByPool(gameObject);
        }

        else
        {
            float remainderTime = lifeCycle - time;
            if (mat)
            {
                try
                {
                    Color col = mat.GetColor("_Color");
                    col.a = remainderTime / lifeCycle;
                    mat.SetColor("_Color", col);
                }
                catch
                {
                    
                }
               
            }
            
        }
    }

    void ReplaceShader()
    {
        
        if (mat != null && !mat.shader.name.Equals("Unlit/Ghost"))
        {
            mat.shader = ghostShader;
            mat.SetColor("_Color", ghostColor);
        }
        mat.renderQueue = 3000;
    }
}
