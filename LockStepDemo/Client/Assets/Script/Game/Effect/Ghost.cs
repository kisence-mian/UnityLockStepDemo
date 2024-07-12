using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{

    private float interval = 0.05f;
    private float lifeCycle = 0.4f;

    float lastCombinedTime = 0;
    //MeshRenderer[] meshRenderers = null;

    SkinnedMeshRenderer[] skinedMeshRenderers = null;
    List<GameObject> objs = new List<GameObject>();
    Vector3 m_ghostColor = Vector3.zero;

    // Use this for initialization
    void Start()
    {
        skinedMeshRenderers = gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();
        
    }

    public void SetColor(Vector3 ghostColor)
    {
        m_ghostColor = ghostColor;
    }

    void OnEnable()
    {
        objs = new List<GameObject>();
    }
    private void OnDisable()
    {
        objs.Clear();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - lastCombinedTime > interval)
        {
            lastCombinedTime = Time.time;

            for (int i = 0; skinedMeshRenderers != null && i < skinedMeshRenderers.Length; i++)
            {
                Mesh mesh = new Mesh();
                skinedMeshRenderers[i].BakeMesh(mesh);//取出蒙皮后的模型网格
                InitFadeInObj(mesh, skinedMeshRenderers[i], lifeCycle);
            }

        }
    }

    void InitFadeInObj(Mesh mesh, SkinnedMeshRenderer skinnedMeshRenderer, float lifeCycle)
    {
        GameObject go = GameObjectManager.CreateGameObjectByPool("GhostObj");
        FadInOut fi = go.GetComponent<FadInOut>();
        fi.Init(m_ghostColor);
        fi.meshFilter.mesh = mesh;
        fi.meshRender.material = skinnedMeshRenderer.material;//材质
        fi.lifeCycle = lifeCycle;
        
        fi.enabled = true;
        
        //go.hideFlags = HideFlags.HideAndDontSave;
        go.transform.position = skinnedMeshRenderer.transform.position;
        go.transform.rotation = skinnedMeshRenderer.transform.rotation;

        
        objs.Add(go);
    }

}
