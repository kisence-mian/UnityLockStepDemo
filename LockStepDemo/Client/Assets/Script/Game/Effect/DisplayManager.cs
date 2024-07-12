using UnityEngine;
using System.Collections;

public class DisplayManager 
{
    ///// <summary>
    ///// 播放某段特效
    ///// </summary>
    //static public void PlayAttackFX(CharacterBase character,string l_s_attackSkillID, SkillStatusEnum l_attackStatus,bool isFollow = false)
    //{
    //    string status = "null";
    //    switch (l_attackStatus)
    //    {
    //        case SkillStatusEnum.Before:
    //            status = DataGenerateManager<SkillDataGenerate>.GetData(l_s_attackSkillID).m_BeforeStatus;
    //            break;
    //        case SkillStatusEnum.Current:
    //            status = DataGenerateManager<SkillDataGenerate>.GetData(l_s_attackSkillID).m_CurrentStatus;
    //            break;
    //        case SkillStatusEnum.Later:
    //            status = DataGenerateManager<SkillDataGenerate>.GetData(l_s_attackSkillID).m_LaterStatus;
    //            break;
    //    }

    //    SkillStatusDataGenerate skillStatusData = DataGenerateManager<SkillStatusDataGenerate>.GetData(status);
    //    string FXName = skillStatusData.m_FXName;
    //    float FXLifeTime = skillStatusData.m_FXLifeTime;
    //    HardPointEnum FXCreatPoint = skillStatusData.m_FXCreatPoint;


    //    string followFXName = skillStatusData.m_FollowFXName;
    //    HardPointEnum followFXCreatPoint = skillStatusData.m_FollowFXCreatPoint;
    //    float followFXLifeTime = skillStatusData.m_FollowFXLifeTime;

    //    if (FXName != "null")
    //    {
    //        EffectManager.ShowEffect(FXName, character.m_hardPoint.GetHardPoint(FXCreatPoint).position, character.transform.eulerAngles, FXLifeTime);
    //    }

    //    if (followFXName != "null")
    //    {
    //        character.m_effectComp.CreateEffectInCharacter(followFXName, followFXCreatPoint,followFXLifeTime);
    //    }
    //}

    ///// <summary>
    ///// 播放音效效
    ///// </summary>
    //public static void PlaySPX(string attackSkillID, SkillStatusEnum attackStatus)
    //{
    //    string status = "null";
    //    switch (attackStatus)
    //    {
    //        case SkillStatusEnum.Before:
    //            status = DataGenerateManager<SkillDataGenerate>.GetData(attackSkillID).m_BeforeStatus;
    //            break;
    //        case SkillStatusEnum.Current:
    //            status = DataGenerateManager<SkillDataGenerate>.GetData(attackSkillID).m_CurrentStatus;
    //            break;
    //        case SkillStatusEnum.Later:
    //            status = DataGenerateManager<SkillDataGenerate>.GetData(attackSkillID).m_LaterStatus;
    //            break;
    //    }

    //    string SFXName = DataGenerateManager<SkillStatusDataGenerate>.GetData(status).m_SFXName;
    //    float delayTime = DataGenerateManager<SkillStatusDataGenerate>.GetData(status).m_SFXDelay;
    //    if (SFXName != "null")
    //    {
    //        AudioManager.PlaySound2D(SFXName, delayTime);
    //    }
    //}

    
    /// <summary>
    /// 播放某段普通攻击动画
    /// </summary>
    public static void PlaySkillAnim(CharacterBase character, string skillID, SkillStatusEnum attackAnimEnum,bool isRaise = true,bool isLater = false)
    {
        SkillDataGenerate data = DataGenerateManager<SkillDataGenerate>.GetData(skillID);
        string status = "null";

        switch (attackAnimEnum)
        {
            case SkillStatusEnum.Before:
                status = data.m_BeforeStatus;
                break;
            case SkillStatusEnum.Current:
                status = data.m_CurrentStatus;
                break;
            case SkillStatusEnum.Later:
                status = data.m_LaterStatus;
                break;
        }
        string attackAnimName = DataGenerateManager<SkillStatusDataGenerate>.GetData(status).m_AnimName;

        if (attackAnimName != "null")
        {
            if (isLater)
            {
                character.m_animComp.ResetPlayAnim(attackAnimName,0);
            }
            else if (!isRaise)
            {
                character.m_animComp.ResetPlayAnim(attackAnimName, data.m_RaiseTime);
            }
            else
            {
                character.m_animComp.ResetPlayAnim(attackAnimName, 0);
            }
        }
    }


    public static void PlayShowAni(int charactorID)
    {
        CharacterBase character = CharacterManager.GetCharacter(charactorID);

        string showAnimName = character.m_Property.m_showAnimName;
        if(showAnimName != null &&
            showAnimName != "null")
        {
            character.m_animComp.PlayAnim(showAnimName);
        }
 
    }


    /// <summary>
    /// 播放技能的镜头动画
    /// </summary>
    public static void PlayCameraSkillAnim(string skillID, SkillStatusEnum attackAnimEnum)
    {
        string status = "null";
        switch (attackAnimEnum)
        {
            case SkillStatusEnum.Before:
                status = DataGenerateManager<SkillDataGenerate>.GetData(skillID).m_BeforeStatus;
                break;
            case SkillStatusEnum.Current:
                status = DataGenerateManager<SkillDataGenerate>.GetData(skillID).m_CurrentStatus;
                break;
            case SkillStatusEnum.Later:
                status = DataGenerateManager<SkillDataGenerate>.GetData(skillID).m_LaterStatus;
                break;
        }
        string cameraAnim = DataGenerateManager<SkillStatusDataGenerate>.GetData(status).m_CameraMove;
        if (cameraAnim != "null" )//&&Random.Range(0,1000)>800
        {
            CameraService.Instance.PlaySkillAnim(cameraAnim);
        }
    }

    /// <summary>
    /// 播放胜利的镜头动画
    /// </summary>
    public static void PlayCameraWinAnim()
    {
        CameraService.Instance.PlayWinAnim();
        CharacterBase myPlyerBase =  GameLogic.GetMyPlayerCharacter();
        myPlyerBase.AimToCamera();
        myPlyerBase.ExitCurrentStatus();
        myPlyerBase.m_animComp.ChangeAnim("war_female_chaofeng");
        
    }

    /// <summary>
    /// 播放失败镜头动画
    /// </summary>
    public static void PlayCameraLoseAnim()
    {
        CameraService.Instance.PlayLoseAnim();
    }

    //镜头动画归位
    public static void CameraAnimRehome()
    {
        CameraService.Instance.AnimRehome();
    }


    #region 掉落

    /// <summary>
    /// 播放默认掉落
    /// </summary>
    public static void DropOut(int killerID,int characterID)
    {
        //CharacterBase killer = CharacterManager.GetCharacter(killerID);
       
        CharacterBase character = CharacterManager.GetCharacter(characterID);
        Vector3 hurterPos = character.transform.position;
        //Debug.Log(characterID);
        //如果没有，掉落生物默认掉落物体
        string[] dropOutID = {character.m_Property.m_dropOutID};

        DropOutGroup(dropOutID, hurterPos, killerID);
    }

    /// <summary>
    /// 掉落多个物品
    /// </summary>
    public static void DropOutGroup(string[] dropOutID, Vector3 hurterPos, int killerID)
    {
        if (dropOutID == null)
        {
            Debug.LogError("l_s_DropOutID is null !");
            return;
        }

        for (int i = 0; i < dropOutID.Length; i++)
        {
            Timer.DelayCallBack(0.2f * i, (obj) =>
            {
                DropOutOne(hurterPos, killerID, dropOutID[(int)obj[0]]);
            }, i);
        }
    }

    //单个掉落效果
    private static void DropOutOne(Vector3 hurterPos, int killerID, string dropOutID)
    {
        if (dropOutID == "null")
        {
            return;
        }
        DropOutDataGenerate DropOutInfo = DataGenerateManager<DropOutDataGenerate>.GetData(dropOutID);
        string FXName = DropOutInfo.m_FXName;
        string modelName = DropOutInfo.m_ModelName;
        int FXNum = DropOutInfo.m_FXNum;
        float FXRange = DropOutInfo.m_FXRange;
        float outTime = DropOutInfo.m_OutTime;
        float waitTime = DropOutInfo.m_WaitTime;
        float backTime = DropOutInfo.m_BackTime;
        float deltaTime = DropOutInfo.m_DeltaTime;

        for (int i = 0; i < FXNum; i++)
        {
            Timer.DelayCallBack(deltaTime * (i+1), (o2) =>
            {
                Vector3 l_v3_endPos =  Random.insideUnitSphere * FXRange  + hurterPos;
                l_v3_endPos.y = hurterPos.y;
                EffectManager.ShowEffect(FXName, modelName, hurterPos, l_v3_endPos, killerID, outTime, waitTime, backTime);
            });
        }
    }



    //单个掉落装备效果
    public static void DropOutOneEquip(Vector3 hurterPos, int killerID,string typeID,string qualityID)
    {
        if (typeID == "null"|| qualityID == "null")
        {
            return;
        }

        string FXName = DataGenerateManager<QualityDataGenerate>.GetData(qualityID).m_fxName;
        string modelName = DataGenerateManager<DropModelGenerate>.GetData(typeID).m_modelName;
        string dropOutID = DataGenerateManager<DropModelGenerate>.GetData(typeID).m_dropOutID;

        DropOutDataGenerate DropOutInfo = DataGenerateManager<DropOutDataGenerate>.GetData(dropOutID);
        int FXNum = DropOutInfo.m_FXNum;
        float FXRange = DropOutInfo.m_FXRange;
        float outTime = DropOutInfo.m_OutTime;
        float waitTime = DropOutInfo.m_WaitTime;
        float backTime = DropOutInfo.m_BackTime;
        float deltaTime = DropOutInfo.m_DeltaTime;

        for (int i = 0; i < FXNum; i++)
        {
            Timer.DelayCallBack(deltaTime * (i + 1), (o2) =>
            {
                Vector3 l_v3_endPos = Random.insideUnitSphere * FXRange + hurterPos;
                l_v3_endPos.y = hurterPos.y;
                EffectManager.ShowEffect(FXName, modelName, hurterPos, l_v3_endPos, killerID, outTime, waitTime, backTime);
            });
        }
    }

    #endregion

    /// <summary>
    /// 震动屏幕
    /// </summary>
    public static void CameraShoke(string shokeID)
    {
        if (shokeID != "null")
        {
            CameraShokeDataGenerate shokeData = DataGenerateManager<CameraShokeDataGenerate>.GetData(shokeID);
            float shokeTime = shokeData.m_TimeLength;
            float amount = shokeData.m_Amount;
            float decreaseFactor = shokeData.m_DecreaseFactor;
            Vector3 randomOffest = shokeData.m_RandomOffest;
            Vector3 weight = shokeData.m_weight;
            CameraService.Instance.ShokeMainCamera(shokeTime, amount, decreaseFactor, randomOffest, weight);
        }
    }
}
