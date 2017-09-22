using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EffectManager
{
    private static Dictionary<int, PoolObject> effectDic = new Dictionary<int, PoolObject>();
    private static List<PoolObject> effectList = new List<PoolObject>();
    /// <summary>
    /// 注册要控制的特效
    /// </summary>
    /// <param name="id"></param>
    /// <param name="effectObj"></param>
    public static void Register(int id, PoolObject effectObj)
    {
        if (!effectDic.ContainsKey(id))
        {
            effectDic.Add(id, effectObj);
        }
        else
        {
            Debug.LogError("EffectManager 特效ID已注册! id:" + id);
        }
    }
    public static void Register(PoolObject effectObj)
    {
        effectList.Add(effectObj);
    }
    public static void DestroyEffect(int id)
    {
        if (effectDic.ContainsKey(id))
        {
           PoolObject po = effectDic[id];
            if(po)
           GameObjectManager.DestroyPoolObject(po);
           effectDic.Remove(id);
        }
        else
        {
            Debug.LogError("EffectManager 特效ID未注册! id:" + id);
        }
    }
    public static void Clear()
    {
        List<int> keys =new List<int>( effectDic.Keys);
        for (int i = 0; i < keys.Count; i++)
        {
            DestroyEffect(keys[i]);
        }
        for (int j = 0; j < effectList.Count; j++)
        {
            GameObjectManager.DestroyPoolObject(effectList[j]);
        }
        effectList.Clear();
    }
    public static GameObject ShowEffect(string effectName, Vector3 pos, float showTime)
    {
       return ShowEffect(effectName, null, pos, Vector3.zero, showTime);       
    }
    public static GameObject ShowEffect(string effectName, GameObject parent, Vector3 pos, float showTime)
    {
       return ShowEffect(effectName, parent, pos, Vector3.zero, showTime);      
    }
    public static GameObject ShowEffect(string effectName, Vector3 pos, Vector3 rotation, float showTime)
    {
       return ShowEffect(effectName, null, pos, rotation, showTime);
    }
    public static GameObject ShowEffect(string effectName, GameObject parent, Vector3 pos, Vector3 rotation, float showTime)
    {
        if (effectName == "null")
            return null;
        PoolObject effectP = GameObjectManager.GetPoolObject(effectName);
        GameObject effect = effectP.gameObject;
        if (parent != null)
        {
            effect.transform.parent = parent.transform;
            effect.transform.localPosition = pos;
            effect.transform.localEulerAngles = Vector3.zero;
        }
        else
        {
            effect.transform.position = pos;
            effect.transform.eulerAngles = rotation;
        }
        
        if (Application.isPlaying)
        {
            if (showTime >= 0)
            {
                Timer.DelayCallBack(showTime, DelayDestroy, effectP);
            }
        }
        return effect;
    }

    static void DelayDestroy(params object[] objs)
    {
        GameObjectManager.DestroyPoolObject((PoolObject)objs[0]);
    }

    //掉落专用
    public static void ShowEffect(string effectName, string modelName, Vector3 fromPos, Vector3 toPos1, int characterID, float toTime1, float waitTime, float toTime2)
    {
        if (modelName == "null")
        {
            return;
        }
        float[] contralRadius = {4,1};

        GameObject model = GameObjectManager.CreateGameObjectByPool(modelName, isSetActive: false);
        PoolObject effectP = null;
        if (effectName != "null")
        {
            effectP = GameObjectManager.GetPoolObject(effectName);
            GameObject effect = effectP.gameObject;
            effect.transform.parent = model.transform;
            effect.transform.localPosition = Vector3.zero;
        }
        
        
        Animator modelAni = model.GetComponentInChildren<Animator>();
        if (modelAni != null)
        {
            modelAni.Play(0);
        }
        model.transform.position = fromPos;

        model.transform.LookAt(toPos1);//可能不需要，如果性能不好，就去掉
        model.SetActive(true);

        AnimSystem.Move(model, fromPos, toPos1, time: toTime1, interp: InterpType.OutQuart);

        
        if (toTime2 > 0)
        {
            Timer.DelayCallBack(toTime1 + waitTime, (o) =>
            {
                //if (CharacterManager.GetCharacterIsExit(characterID))
                //{
                //    //AnimSystem.BezierMove(model, from: toPos1, to: CharacterManager.GetCharacter(characterID).transform.position, time: toTime2, delayTime: 0, t_Bezier_contralRadius: contralRadius, interp: InterpType.Default);

                //    Timer.DelayCallBack( toTime2, (o2) =>
                //        {
                //            if (effectP != null)
                //            {
                //                GameObjectManager.DestroyPoolObject(effectP);
                //            }
                            

                //            GameObjectManager.DestroyGameObjectByPool(model, true);
                //        });
                //}
                //else
                //{
                //    if (effectP != null)
                //    {
                //        GameObjectManager.DestroyPoolObject(effectP);
                //    }
                //    GameObjectManager.DestroyGameObjectByPool(model, true);
                //}
            });
        }

    }
}
