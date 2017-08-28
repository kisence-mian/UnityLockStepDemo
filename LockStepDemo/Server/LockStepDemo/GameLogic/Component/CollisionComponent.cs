using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class CollisionComponent : ComponentBase
{
    public bool isCanAcross = false;
    public Area area = new Area();

    public bool isEnterCollision = false;

    private List<EntityBase> collisionList = new List<EntityBase>();

    public List<EntityBase> CollisionList
    {
        get
        {
            return collisionList;
        }

        set
        {
            collisionList = value;
        }
    }
}
