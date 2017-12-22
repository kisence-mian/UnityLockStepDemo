using FastCollections;
using Lockstep;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class QuadTree
{
    const int MAX_OBJECTS = 3; //四叉树最大数量
    const int MAX_DEPTH = 5;   //四叉树最大深度
    const int MAX_CARVE_DEPTH = 3;   //分裂最大深度

    public FastList<CollisionComponent> m_objectList; //子对象
    public FastList<QuadTree> m_childList;   //子节点
    public Body m_body; //范围,为性能考虑形状只支持不旋转的矩形(标准矩形)
    public int m_depth;

    int m_objectListCount = 0;
    int m_childListCount = 0;

    public QuadTree(Body body, int depth)
    {
        m_objectList = new FastList<CollisionComponent>();
        m_childList = new FastList<QuadTree>();
        m_depth = depth;
        m_body = body;
    }

    /*
     * 如果某一个象限（节点）内存储的物体数量超过了MAX_OBJECTS最大数量，则需要对这个节点进行划分
     * 
     */
    public void Split()
    {
        long x = m_body.position.x;
        long y = m_body.position.y;
        long sl = m_body.length.Div(FixedMath.Create(2));
        long sw = m_body.width.Div(FixedMath.Create(2));

        //Debug.Log("Split " + x.ToFloat() + " " + y.ToFloat() + " " + sw.ToFloat() + " " + sl.ToFloat());

        m_childList.Add(new QuadTree(new Body(x - sl, x, y - sw, y), m_depth + 1));
        m_childList.Add(new QuadTree(new Body(x - sl, x, y, y + sw), m_depth + 1));
        m_childList.Add(new QuadTree(new Body(x, x + sl, y - sw, y), m_depth + 1));
        m_childList.Add(new QuadTree(new Body(x, x + sl, y, y + sw), m_depth + 1));

        m_childListCount = 4;
    }

    /*
      插入功能：
        - 如果当前节点[ 存在 ]子节点，则检查物体到底属于哪个子节点，如果能匹配到子节点，则将该物体插入到该子节点中
        - 如果当前节点[ 不存在 ]子节点，将该物体存储在当前节点。随后，检查当前节点的存储数量，
        - 如果超过了最大存储数量，则对当前节点进行划分，划分完成后，将当前节点存储的物体重新分配到四个子节点中。
    */
    public void Insert(CollisionComponent coll)
    {
        //如果存在子节点，则插入子节点
        if (m_childListCount > 0)
        {
            int index = GetIndex(coll.area);

            //Debug.Log("index " + index + " depth " + m_depth);

            //如果都不包含则存储在父节点内
            if (index == -1)
            {
                m_objectList.Add(coll);
                m_objectListCount++;
            }
            else
            {
                m_childList[index].Insert(coll);
            }
        }

        //否则则存储在当前节点
        else
        {
            m_objectList.Add(coll);
            m_objectListCount++;

            //如果当前节点储存的数量超过了上限，则分裂
            if (m_objectListCount > MAX_OBJECTS
                && m_depth < MAX_DEPTH)
            {
                Split();

                //把当前节点下对象的放入子节点内
                for (int i = 0; i < m_objectListCount; i++)
                {
                    int index = GetIndex(m_objectList[i].area);

                    if (index != -1)
                    {
                        CollisionComponent tmp = m_objectList[i];
                        m_objectList.RemoveAt(i);
                        i--;

                        m_childList[index].Insert(tmp);
                        m_objectListCount--;
                    }
                }
            }
        }
    }

    //从四叉树中移除一个对象
    public void Remove(CollisionComponent coll)
    {
        if (m_objectList.Contains(coll))
        {
            m_objectList.Remove(coll);
            m_objectListCount--;
        }
        else
        {
            for (int i = 0; i < m_childListCount; i++)
            {
                m_childList[i].Remove(coll);
            }
        }
    }

    public int GetIndex(Body body)
    {
        int count = 0;
        int index = -1;

        for (int i = 0; i < 4; i++)
        {
            if (m_childList[i].m_body.CheckIsInnner(body))
            {
                count++;
                index = i;
            }
        }

        if (count > 1)
        {
            string error = "GetIndex Faill ! " + body.position + " shape " + body.bodyType + " length " + body.Length + " widh " + body.Width + " Radius " + body.Radius;

            for (int i = 0; i < m_childList.Count; i++)
            {
                error += "\n " + i + " -> " + m_childList[i].m_body.position + " shape " + m_childList[i].m_body.bodyType + " length " + m_childList[i].m_body.Length + " widh " + m_childList[i].m_body.Width + " Radius " + m_childList[i].m_body.Radius;
            }

            error += "\n ";

            throw new Exception(error);
        }
        else if (count == 1)
        {
            return index;
        }
        else
        {
            return -1;
        }
    }

    bool[] indexListCache = new bool[4];

    public bool[] GetRetrieveIndex(Body body)
    {
        //indexListCache.Clear();

        for (int i = 0; i < 4; i++)
        {
            indexListCache[i] = m_childList[i].m_body.CheckCollide(body);
        }

        //if (indexListCache.Count > 0)
        //{
        //    return indexListCache;
        //}
        //else
        //{
        //    string error = "GetIndex Faill ! " + body.position + " shape " + body.bodyType + " length " + body.Length + " widh " + body.Width + " Radius " + body.Radius;

        //    for (int i = 0; i < m_childList.Count; i++)
        //    {
        //        error += "\n " + i + " -> " + m_childList[i].m_body.position + " shape " + m_childList[i].m_body.bodyType + " length " + m_childList[i].m_body.Length + " widh " + m_childList[i].m_body.Width + " Radius " + m_childList[i].m_body.Radius;
        //    }

        //    error += "\n ";

        //    throw new Exception(error);
        //}

        return indexListCache;
    }

    FastList<CollisionComponent> listCache = new FastList<CollisionComponent>();

    /*
      检索功能：
        给出一个物体对象，该函数负责将该物体可能发生碰撞的所有物体选取出来。该函数先查找物体所属的象限，该象限下的物体都是有可能发生碰撞的，然后再递归地查找子象限..
    */
    public FastList<CollisionComponent> Retrieve(CollisionComponent coll)
    {
        listCache.Clear();

        //Debug.Log("m_childListCount " + m_childListCount);

        if (m_childListCount != 0)
        {
            bool[] indexList = GetRetrieveIndex(coll.area);

            for (int i = 0; i < 4; i++)
            {
                if(indexList[i])
                {
                    listCache.AddRange(m_childList[i].Retrieve(coll));
                }
            }
        }

        listCache.AddRange(m_objectList);

        //Debug.Log("Retrieve " + listCache.Count + " depth " + m_depth);

        return listCache;
    }

    /*
      动态更新：
        从根节点深入四叉树，检查四叉树各个节点存储的物体是否依旧属于该节点（象限）的范围之内，如果不属于，则重新插入该物体。
    */
    public void Refresh(QuadTree root = null)
    {
        if (root == null)
        {
            root = this;
        }

        for (int i = 0; i < m_objectListCount; i++)
        {
            CollisionComponent cc = m_objectList[i];
            if (!cc.isStatic)
            {
                if (!m_body.CheckIsInnner(cc.area))
                {
                    m_objectList.RemoveAt(i);
                    m_objectListCount--;
                    i--;

                    root.Insert(cc);
                }
            }
        }

        for (int i = 0; i < m_childListCount; i++)
        {
            m_childList[i].Refresh(root);
        }
    }

    /// <summary>
    /// 分割矩形以优化碰撞
    /// </summary>
    public List<Body> Carve(Body body)
    {
        List<Body> list = new List<Body>();

        //只优化标准矩形
        if(!(body.bodyType == BodyType.Rectangle && body.isStandard == true))
        {
            list.Add(body);
            return list;
        }

        //分裂自己
        if(m_childList.Count == 0 
            && m_depth < MAX_CARVE_DEPTH 
            && m_depth < MAX_DEPTH)
        {
            Split();
        }

        //截取一个重叠面积
        body = GetOverLapBody(m_body,body);
        if (m_childList.Count > 0)
        {
            bool[] indexList = GetRetrieveIndex(body);
            for (int i = 0; i < 4; i++)
            {
                if(indexList[i])
                {
                    list.AddRange(m_childList[i].Carve(body));
                }
            }
        }
        else
        {
            list.Add(body);
        }

        return list;
    }

    public Body GetOverLapBody(Body b1, Body b2)
    {
        if (!(b1.bodyType == BodyType.Rectangle
            && b2.bodyType == BodyType.Rectangle
            && b1.isStandard == true
            && b2.isStandard == true)

            || !b1.CheckCollide(b2))
        {
            throw new Exception("GetOverLapBody error!");
        }

        long l = b1.LeftBound > b2.LeftBound ? b1.LeftBound : b2.LeftBound;
        long r = b1.RightBound < b2.RightBound ? b1.RightBound : b2.RightBound;
        long u = b1.UpBound < b2.UpBound ? b1.UpBound : b2.UpBound;
        long d = b1.DownBound > b2.DownBound ? b1.DownBound : b2.DownBound;

        //Debug.Log(l.ToFloat() + " " + r.ToFloat() + " " + u.ToFloat() + " " + d.ToFloat());

        Body result = new Body(l + 1,r - 1,d + 1,u -1);

        result.Draw();

        //b1.PrintBound();
        //b2.PrintBound();
        //result.PrintBound();

        //Debug.Log(b1 + "\n" + b2 + "\n" + result);

        return result;
    }

    public void Show(int depth = -1)
    {
        if(m_depth == depth || depth == -1)
        {
            for (int i = 0; i < m_objectList.Count; i++)
            {
                m_objectList[i].area.Draw();
            }
        }

        for (int i = 0; i < m_childList.Count; i++)
        {
            m_childList[i].Show(depth);
        }
    }
}