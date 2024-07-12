using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialManager : MonoBehaviour {


    private static Dictionary<GameObject, Material[]> historyMaterial = new Dictionary<GameObject, Material[]>();
    private static Dictionary<string, Material> HaveLoadedMatetial = new Dictionary<string, Material>();

    public static void ClearnHistory()
    {
        historyMaterial.Clear();
    }

    public static Material GetMeterial(GameObject go,int matID, Material mat, Texture[] newTex = null , string[] texID = null)
    {

        Material newMaterial = mat;

        if(newTex != null && texID != null )
        {
            for (int i = 0; i < texID.Length; i++)
            {
                if (newTex[i] != null)
                {
                    newMaterial.SetTexture(texID[i], newTex[i]);
                }
              
            }
        }

        //go.GetComponent<Renderer>().materials[matID] = newMaterial;

        return newMaterial;
    }

    public static Material[] ChangeMeterial(GameObject go, string matName,ref bool changed, params string[] texID )
    {
        
        Material newMaterial = GetMaterial(matName);
        Material[] oldMaterial = go.GetComponent<Renderer>().materials;

        Material[] newMaterials = new Material[oldMaterial.Length];
        for (int j = 0; j < oldMaterial.Length; j++)
        {
            if (oldMaterial[j].name.Contains(newMaterial.name))
            //if(newMaterial.name == oldMaterial.name.Split('\n')[0])
            {
                changed = false;
                return oldMaterial;
            }

            Texture[] oldTexture = new Texture[texID.Length];

            for (int i = 0; i < texID.Length; i++)
            {
                if (texID[i] != null)
                {
                    oldTexture[i] = oldMaterial[j].GetTexture(texID[i]);
                }
            }

            newMaterials[j] = GetMeterial(go,j, newMaterial, oldTexture, texID);

        }

        if (historyMaterial.ContainsKey(go))
        {

        }
        else
        {
            historyMaterial.Add(go, oldMaterial);
        }

        go.GetComponent<Renderer>().materials = newMaterials;

        changed = true;
        return newMaterials;
    }


    public static void Reduction(GameObject go,float delayTime = 0.01f)
    {
        if (historyMaterial.ContainsKey(go))
        {
            Timer.DelayCallBack(delayTime, (o2) =>
            {
                if (go != null)
                {
                    go.GetComponent<Renderer>().materials = historyMaterial[go];
                }
            });
            
            //Debug.Log(go.GetComponent<Renderer>().material);
        }
        else
        {
            //Debug.LogError(go + "的材质 无历史版本，无法还原");
        }
 
    }



    public static Material GetMaterial(string matName)
    {
        if (HaveLoadedMatetial.ContainsKey(matName))
        {
            
        }
        else
        {
            HaveLoadedMatetial.Add(matName, ResourceManager.Load<Material>(matName));
        }

        return Instantiate(HaveLoadedMatetial[matName]);
    }



}
