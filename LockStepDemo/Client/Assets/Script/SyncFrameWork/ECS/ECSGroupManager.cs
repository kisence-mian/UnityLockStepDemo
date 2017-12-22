using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ECSGroupManager
{
    //private WorldBase world;
    private Dictionary<int, ECSGroup> allGroupDic = new Dictionary<int, ECSGroup>();
    private Dictionary<ECSGroup, List<EntityBase>> groupToEntityDic = new Dictionary<ECSGroup, List<EntityBase>>();
    private Dictionary<EntityBase, List<ECSGroup>> entityToGroupDic = new Dictionary<EntityBase, List<ECSGroup>>();

    public Dictionary<int, ECSGroup> AllGroupDic
    {
        get
        {
            return allGroupDic;
        }
    }

    public Dictionary<ECSGroup, List<EntityBase>> GroupToEntityDic
    {
        get
        {
            return groupToEntityDic;
        }
    }

    public ECSGroupManager(WorldBase world)
    {

        for (int i = 0; i < world.m_systemList.Count; i++)
        {
            SystemBase system = world.m_systemList[i];
            int key = StringArrayToInt(system.Filter);
            if (allGroupDic.ContainsKey(key))
            {
                //Debug.Log("System :"+ system.GetType().FullName+ "  Filter :" + string.Join(",", system.Filter) + " Dic :"+string.Join(",",allGroupDic[key].Components));
                continue;
            }
            ECSGroup group = new ECSGroup(key, system.Filter);
            allGroupDic.Add(key, group);
                groupToEntityDic.Add(group, new List<EntityBase>());
        }
        for (int i = 0; i < world.m_recordList.Count; i++)
        {
            RecordSystemBase system = world.m_recordList[i];
          
            int key = StringArrayToInt(system.Filter);
            if (allGroupDic.ContainsKey(key))
            {
                //Debug.Log("System :" + system.GetType().FullName + "  Filter :" + string.Join(",", system.Filter) + " Dic :" + string.Join(",", allGroupDic[key].Components));
                continue;
            }

            ECSGroup group = new ECSGroup(key, system.Filter);
            allGroupDic.Add(key, group);
            groupToEntityDic.Add(group, new List<EntityBase>());
        }
    }

    public int StringArrayToInt(string[] arr)
    {
        Array.Sort(arr);
        string tempS = string.Join("&", arr);
        return tempS.ToHash();
    }
    public List<EntityBase> GetEntityByFilter(int key, string[] filters)
    {
        ECSGroup group;
        if (allGroupDic.TryGetValue(key, out group))
        {

        }
        else
        {
            AddGroup(key, filters);
            allGroupDic.TryGetValue(key, out group);
        }
        List<EntityBase> list;
        if (groupToEntityDic.TryGetValue(group, out list))
        {
            if (list != null)
                return list;
        }
        return new List<EntityBase>();
    }
    public List<EntityBase> GetEntityByFilter(string[] filters)
    {
        int key = StringArrayToInt(filters);
      
        return GetEntityByFilter(key,filters);
    }

    public ECSGroup[] GetGroupByEntity(EntityBase entity)
    {
        List<ECSGroup> list;
        if (entityToGroupDic.TryGetValue(entity, out list))
        {
            if (list != null)
                return list.ToArray();
        }

        return new ECSGroup[0];
    }

    private bool AddGroup(int key,string[] filters)
    {
        if( filters==null || filters.Length == 0)
        {
            Debug.LogError("AddGroup 失败，参数不能为空！");
            return false;
        }
        if (allGroupDic.ContainsKey(key))
        {
            Debug.LogError("AddGroup 失败，名字重复！");
            return false;
        }

        ECSGroup group = new ECSGroup(key, filters);
        allGroupDic.Add(key, group);

        List<EntityBase> newListEntity = new List<EntityBase>();

        List<EntityBase> listEntity = new List<EntityBase>(entityToGroupDic.Keys);
        for (int i = 0; i < listEntity.Count; i++)
        {
            EntityBase entity = listEntity[i];
            //List<ECSGroup> newGroupList = GetEntitySuportGroup(entity);
            bool isContains = true;
            for (int j = 0; j < filters.Length; j++)
            {
                if (!entity.GetExistComp(filters[j]))
                {
                    isContains = false;
                }
            }
            if (isContains)
            {
                newListEntity.Add(entity);
                entityToGroupDic[entity].Add(group);
            }
        }
        groupToEntityDic.Add(group, newListEntity);

        return true;
    }

    public void OnEntityCreate(EntityBase entity)
    {
        List<ECSGroup> newGroupList = GetEntitySuportGroup(entity);

        if (!entityToGroupDic.ContainsKey(entity))
                entityToGroupDic.Add(entity, newGroupList);
            else
                entityToGroupDic[entity] = newGroupList;
        
        for (int i = 0; i < newGroupList.Count; i++)
        {
            List<EntityBase> list = groupToEntityDic[newGroupList[i]];
            if (!list.Contains(entity))
                list.Add(entity);
        }

    }
    public void OnEntityDestroy(EntityBase entity)
    {
        List<ECSGroup> list = entityToGroupDic[entity];
        for (int i = 0; i < list.Count; i++)
        {
            groupToEntityDic[list[i]].Remove(entity);
        }
        entityToGroupDic.Remove(entity);
    }

    public void OnEntityComponentChange(EntityBase entity, int compIndex, ComponentBase component)
    {
        //Debug.Log("OnEntityComponentChange");
        List<ECSGroup> oldSystems;
        if (!entityToGroupDic.TryGetValue(entity, out oldSystems))
        {
            oldSystems = new List<ECSGroup>();
            entityToGroupDic.Add(entity, oldSystems);
        }

        List<ECSGroup> newGroupList = GetEntitySuportGroup(entity);


        entityToGroupDic[entity] = newGroupList;

        for (int i = 0; i < newGroupList.Count; i++)
        {
            ECSGroup sys = newGroupList[i];
            if (!oldSystems.Contains(sys))
            {
                List<EntityBase> list = groupToEntityDic[sys];
                if (list == null)
                {
                    list = new List<EntityBase>();
                }
                list.Add(entity);
            }
        }

        for (int i = 0; i < oldSystems.Count; i++)
        {
            ECSGroup sys = oldSystems[i];
            if (!newGroupList.Contains(sys))
            {
                List<EntityBase> list = groupToEntityDic[sys];
                if (list == null)
                {
                    list = new List<EntityBase>();
                }

                list.Remove(entity);
            }
        }
    }

    public List<ECSGroup> GetEntitySuportGroup(EntityBase entity)
    {
        List<ECSGroup> groupNames = new List<ECSGroup>();
        List<ECSGroup> groups = new List<ECSGroup>(allGroupDic.Values);

        for (int i = 0; i < groups.Count; i++)
        {
            string[] filterCom = groups[i].Components;
            bool isContains = true;
            for (int j = 0; j < filterCom.Length; j++)
            {
                if (!entity.GetExistComp(filterCom[j]))
                {
                    isContains = false;
                    break;
                }
            }
            if (isContains)
            {
                groupNames.Add(groups[i]);
            }
        }

        return groupNames;
    }
}

