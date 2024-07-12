using UnityEngine;
using System.Collections;

public class CharacterMaterialCompmoent : CompmoentBase
{
    public SkinnedMeshRenderer m_meshRender;

    public Material m_highLightMaterial;

    /// <summary>
    /// 隐身材质
    /// </summary>
    public Material m_cloakingMaterial;

    /// <summary>
    /// 默认材质球
    /// </summary>
    public Material m_defaultMaterial;

    public override void OnCreate()
    {
        base.OnCreate();

        if (m_meshRender == null)
        {
            return;
        }

        if (m_meshRender.sharedMaterials.Length > 1)
        {
            m_defaultMaterial = m_meshRender.materials[0];
            m_highLightMaterial = m_meshRender.materials[1];
            m_highLightMaterial.SetColor("_TintColor", new Color(1, 1, 1, 0));
        }
    }


    private float m_n_nowHitLightOffsetY;
    private float m_n_nowHitLightAlpha = 0;
    /// <summary>
    /// 播放高光特效
    /// </summary>
    public void PlayHighLightFX()
    {
        if (m_highLightMaterial != null && m_n_nowHitLightAlpha == 0)
        {
            AnimSystem.CustomMethodToFloat(HitLightAlpha, 0, 1, 0.15f, callBack: ValueToFunction);
            AnimSystem.CustomMethodToFloat(HitLightOffest, m_n_nowHitLightOffsetY, m_n_nowHitLightOffsetY - 0.5f, 0.5f);
        }
    }

    void ValueToFunction(object[] obj)
    {
        AnimSystem.CustomMethodToFloat(HitLightAlpha, m_n_nowHitLightAlpha, 0, 0.3f);
    }

    Vector2 v2Temp = new Vector2();
    void HitLightOffest(float l_n_nowHitLightAlpha)
    {
        m_n_nowHitLightOffsetY = l_n_nowHitLightAlpha;

        v2Temp.y = l_n_nowHitLightAlpha;

        m_highLightMaterial.mainTextureOffset = v2Temp;
    }

    Color colTemp = new Color();
    void HitLightAlpha(float l_n_nowHitLightAlpha)
    {
        m_n_nowHitLightAlpha = l_n_nowHitLightAlpha;

        colTemp.r = 1;
        colTemp.g = 1;
        colTemp.b = 1;
        colTemp.a = l_n_nowHitLightAlpha;

        m_highLightMaterial.SetColor("_TintColor", colTemp);
    }

    public virtual void SetVisible(VisibleEnum visible)
    {
        //Debug.Log("VisibleEnum : " + visible);


        if (m_meshRender == null)
        {
            return;
        }
        if (visible == VisibleEnum.inVisible)
        {
            if (m_meshRender.gameObject.activeSelf)
            {
                m_meshRender.gameObject.SetActive(false);
            }
        }
        else
        {
            if (m_meshRender == null)
            {
                return;
            }

            if (!m_meshRender.gameObject.activeSelf)
            {
                m_meshRender.gameObject.SetActive(true);
            }

            if (visible == VisibleEnum.Visible)
            {
                if (m_defaultMaterial != null && m_meshRender.material != m_defaultMaterial)
                {
                    m_meshRender.material = m_defaultMaterial;
                }
            }
            else
            {
                if (m_cloakingMaterial != null && m_meshRender.material != m_cloakingMaterial)
                {
                    m_meshRender.material = m_cloakingMaterial;
                }
            }
        }
    }



}
