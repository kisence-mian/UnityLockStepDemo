//using UnityEngine;
//using System.Collections;
//using System;

//[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
//public class CreatMesh : MonoBehaviour
//{
//    public AreaType shapeType = AreaType.Rectangle;

//    public int high = 50;
//    public int width = 50;

//    public float radius = 2;
//    public float angleDegree = 100;
//    public int segments = 10;
//    public int angleDegreePrecision = 1000;
//    public int radiusPrecision = 1000;

//    public float side = 0.5f;
//    public float Stroke = 0.02f;

//    public Material materialForBrave;
//    public Material materialForMonster;

//    public bool isMonster;

//    private MeshFilter meshFilter;
//    private MeshRenderer meshRenderer;

//    private SectorMeshCreator creator = new SectorMeshCreator();

//    [ExecuteInEditMode]
//    private void Awake()
//    {
//        isMonster = false;
//        meshFilter = GetComponent<MeshFilter>();
//        meshRenderer = GetComponent<MeshRenderer>();
//    }

//    public bool isUpdate = true;

//    private void Update()
//    {
//        if (shapeType == AreaType.Sector)
//        {
//            meshFilter.mesh = creator.CreatSector2(radius, angleDegree, segments, side, Stroke);
//        }

//        else if (shapeType == AreaType.Circle)
//        {
//            meshFilter.mesh = creator.CreatCircle(radius, segments, side, Stroke);
//        }
//        else
//        {
//            meshFilter.mesh = creator.creatRect(high, width, side);
//        }

//        if (isMonster == true)
//        {
//            meshRenderer.material = materialForMonster;
//        }
//        else
//        {
//            meshRenderer.material = materialForBrave;
//        }
//    }

//    void OnDrawGizmos()
//    {
//        Gizmos.color = Color.gray;
//        DrawMesh();
//    }

//    void OnDrawGizmosSelected()
//    {
//        Gizmos.color = Color.green;
//        DrawMesh();
//    }

//    public void SetEnemy(bool isEnemy)
//    {
//        isMonster = isEnemy;
//    }

//    public void SetArea(Area area, bool isEnemy, string textureID)
//    {
//        isUpdate = true;

//        shapeType = area.areaType;
//        radius = area.radius;
//        angleDegree = area.angle;
//        segments = (int)(area.angle / 2);
//        high     = (int)area.length;
//        width    = (int)area.Width;

//        isMonster = isEnemy;

//        SetTexture(textureID);

//        creator.verticesTmp = null;
//        creator.uvsTmp = null;
//        creator.trianglesTmp = null;
//    }

//    public void SetMesh(EntityBase entity, string areaID, bool isEnemy, string textureID)
//    {
//        MoveComponent mc = entity.GetComp<MoveComponent>();

//        AreaDataGenerate areaData = DataGenerateManager<AreaDataGenerate>.GetData(areaID);
//        Area Area = new Area();
//        Vector3 AreaDir =  SkillUtils.GetAreaDir(Area, areaData, entity, null);

//        //Debug.Log("areaData.m_SkewDistance " + areaData.m_SkewDistance);

//        Vector3 pos = mc.pos.ToVector();
//        pos.y += 0.1f;
//        transform.position = pos + AreaDir * areaData.m_SkewDistance;

//        if(areaData.m_Shape == AreaType.Rectangle)
//        {
//            transform.position = pos + AreaDir * areaData.m_SkewDistance - AreaDir * areaData.m_Length /2;
//        }

//        transform.right = AreaDir.normalized; //轴向不同，RangeTip 的前方为x轴，而player的为Z轴

//        shapeType = areaData.m_Shape;
//        radius = areaData.m_Radius;
//        angleDegree = areaData.m_Angle;
//        segments = 40;
//        high = (int)Math.Ceiling(areaData.m_Length);
//        width = (int)Math.Ceiling(areaData.m_Width);

//        isMonster = isEnemy;
//        side = 0.5f;

//        SetTexture(textureID);

//        creator.verticesTmp = null;
//        creator.uvsTmp = null;
//        creator.trianglesTmp = null;
//    }

//    private void DrawMesh()
//    {
//        Mesh mesh ;

//        if (shapeType == AreaType.Sector)
//            mesh = creator.CreatSector2(radius, angleDegree, segments, side, Stroke);
//        else if (shapeType == AreaType.Circle)
//            mesh = creator.CreatCircle(radius, segments, side, Stroke);
//        else
//            mesh = creator.creatRect(high, width,side);

//        int[] tris = mesh.triangles;
//        for (int i = 0; i < tris.Length; i += 3)
//        {
//            Gizmos.DrawLine(convert2World(mesh.vertices[tris[i]]), convert2World(mesh.vertices[tris[i + 1]]));
//            Gizmos.DrawLine(convert2World(mesh.vertices[tris[i]]), convert2World(mesh.vertices[tris[i + 2]]));
//            Gizmos.DrawLine(convert2World(mesh.vertices[tris[i + 1]]), convert2World(mesh.vertices[tris[i + 2]]));
//        }
//    }

//    void SetTexture(string texture)
//    {

//        if (materialForMonster != null && materialForBrave != null)
//        {
//            if (texture != null && texture != "null")
//            {
//                materialForBrave.mainTexture = ResourceManager.Load<Texture>(texture);
//                materialForMonster.mainTexture = ResourceManager.Load<Texture>(texture);
//            }
//            else
//            {
//                materialForBrave.mainTexture = ResourceManager.Load<Texture>("AreaTip_fuwen_normal");
//                materialForMonster.mainTexture = ResourceManager.Load<Texture>("AreaTip_fuwen_red");
//            }
//        }

//    }

//    private Vector3 convert2World(Vector3 src)
//    {
//        return transform.TransformPoint(src);
//    }

//    private class SectorMeshCreator
//    {
//        private float radius;
//        private float angleDegree;
//        private int segments;

//        Vector3[] verticesRectTmp = new Vector3[6];
//        Vector2[] uvsRectTmp = new Vector2[6];
//        public Mesh creatRect(float width, float high, float side)
//        {
//            if (mesh == null)
//            {
//                mesh = new Mesh();
//            }

//            mesh.Clear();
//            Vector3[] vertices = verticesRectTmp;

//            vertices[0] = new Vector3(0, 0, -high / 2);
//            vertices[1] = new Vector3(width, 0, -high / 2);
//            vertices[2] = new Vector3(width, 0, high/2);
//            vertices[3] = new Vector3(0, 0, high/2);
//            vertices[4] = new Vector3(width + side, 0, -high / 2);
//            vertices[5] = new Vector3(width + side, 0, high / 2);

//            int[] triangles = new int[12];

//            triangles[0] = 2;
//            triangles[1] = 1;
//            triangles[2] = 0;
//            triangles[3] = 3;
//            triangles[4] = 2;
//            triangles[5] = 0;

//            triangles[6] = 4;
//            triangles[7] = 5;
//            triangles[8] = 1;
//            triangles[9] = 1;
//            triangles[10] = 5;
//            triangles[11] = 2;

//            uvsRectTmp[0] = new Vector2(0,0.5f);
//            uvsRectTmp[3] = new Vector2(0, 0.5f);

//            uvsRectTmp[1] = new Vector2(0.5f,0f);
//            uvsRectTmp[2] = new Vector2(0.5f, 1f);
            
//            uvsRectTmp[4] = new Vector2(1f, 0f);
//            uvsRectTmp[5] = new Vector2(1f, 1f);

//            mesh.vertices = vertices;
//            mesh.triangles = triangles;
//            mesh.uv = uvsRectTmp;

//            return mesh;
//        }


//        #region 扇形 方案1
//        Mesh mesh;
//        Vector2 c_uv15 = new Vector2(1f, 0.5f);
//        Vector2 c_uv05 = new Vector2(0, 0.5f);

//        public Vector3[] verticesTmp = null;
//        public Vector2[] uvsTmp = null;
//        public int[] trianglesTmp = null;

//        public Mesh CreatSector(float radius, float angleDegree, int segments, float side, float Stroke)
//        {
//            if (segments == 0)
//            {
//                segments = 1;
//            }

//            if (mesh == null)
//            {
//                mesh = new Mesh();
//            }

//            mesh.Clear();

//            //if (verticesTmp ==null)
//            {
//                verticesTmp = new Vector3[6 + 2 * segments + 1];
//                uvsTmp = new Vector2[verticesTmp.Length];
//                trianglesTmp = new int[4 * 3 + segments * 3 * 3];
//            }

//            Vector3[] vertices = verticesTmp;
//            Vector2[] uvs = uvsTmp;
//            vertices[0] = new Vector3(-Stroke, 0, 0);
//            vertices[1] = Vector3.zero;
//            vertices[vertices.Length - 1] = new Vector3(-Stroke, 0, 0);

//            uvs[0] = c_uv05;
//            uvs[1] = c_uv05;
//            uvs[vertices.Length - 1] = c_uv15;

//            float angle = Mathf.Deg2Rad * angleDegree;
//            float currAngle = angle / 2;
//            float deltaAngle = angle / segments;

//            float tmp = angleDegree / 90; 

//            int i = 0;
//            int vi = 0;
//            for (i = 2; i + 1 < vertices.Length - 2; i += 2)
//            {
//                vertices[i]     = new Vector3(Mathf.Cos(currAngle) * radius      , 0, Mathf.Sin(currAngle) * radius);
//                vertices[i + 1] = new Vector3(Mathf.Cos(currAngle) * (radius + side), 0, Mathf.Sin(currAngle) * (radius + side));

//                float v = (float)(i-2) / (float)(vertices.Length - 7);
//                int count = 0;
//                float tmp2 = v * tmp;

//                while (tmp2 > 1)
//                {
//                    tmp2 -= 1;
//                    count++;
//                }

//                if (count % 2 == 1)
//                {
//                    uvs[i] = new Vector2(0.5f, (tmp2) % 1);
//                    uvs[i + 1] = new Vector2(1, (tmp2) % 1);
//                }
//                else
//                {
//                    uvs[i] = new Vector2(0.5f, (1 - (tmp2 % 1)));
//                    uvs[i + 1] = new Vector2(1, (1 - (tmp2 % 1)));
//                }
//                currAngle -= deltaAngle;
//            }

//            float angle_a = Mathf.Atan2(Stroke, radius);

//            uvs[i] = c_uv15;
//            vertices[i++] = new Vector3(Mathf.Cos(-angle_a - angle / 2) * (radius + side), 0, Mathf.Sin(-angle_a - angle / 2) * (radius + side));

//            uvs[i] = c_uv15;
//            vertices[i] = new Vector3(Mathf.Cos(angle / 2 + angle_a) * (radius + side), 0, Mathf.Sin(angle / 2 + angle_a) * (radius + side));

//            int[] triangles = trianglesTmp;
//            for (i = 0, vi = 2; i < triangles.Length - 12; i += 9, vi+=2)
//            {
//                triangles[i] = 1;
//                triangles[i + 1] = vi;
//                triangles[i + 2] = vi + 2;

//                triangles[i + 3] = vi;
//                triangles[i + 4] = vi + 1;
//                triangles[i + 5] = vi + 2;

//                triangles[i + 6] = vi + 2;
//                triangles[i + 7] = vi + 1;
//                triangles[i + 8] = vi + 3;
//            }

//            triangles[i++] = 0;
//            triangles[i++] = vertices.Length - 3;
//            triangles[i++] = vertices.Length - 1;

//            triangles[i++] = vertices.Length - 1;
//            triangles[i++] = vertices.Length - 3;
//            triangles[i++] = vertices.Length - 4;

//            triangles[i++] = 0;
//            triangles[i++] = vertices.Length - 1;
//            triangles[i++] = vertices.Length - 2;

//            triangles[i++] = vertices.Length - 1;
//            triangles[i++] = vertices.Length - 2;
//            triangles[i++] = 3;

//            mesh.vertices = vertices;
//            mesh.triangles = triangles;

//            mesh.uv = uvs;

//            return mesh;
//        }

//        #endregion

//        #region 扇形 方案2

//        public Mesh CreatSector2(float radius, float angleDegree, int segments, float side, float Stroke)
//        {
//            if (segments == 0)
//            {
//                segments = 1;
//            }

//            if (mesh == null)
//            {
//                mesh = new Mesh();
//            }

//            mesh.Clear();

//            //if (verticesTmp ==null)
//            {
//                verticesTmp = new Vector3[2 + 3 * (segments + 1) + 1];
//                uvsTmp = new Vector2[verticesTmp.Length];
//                trianglesTmp = new int[4 * 3 + (segments + 1) * 3 * 3];
//            }

//            Vector3[] vertices = verticesTmp;
//            Vector2[] uvs = uvsTmp;
//            vertices[0] = new Vector3(-Stroke, 0, 0);
//            vertices[1] = Vector3.zero;
//            vertices[vertices.Length - 1] = new Vector3(-Stroke, 0, 0);

//            uvs[0] = c_uv05;
//            uvs[1] = c_uv05;
//            uvs[vertices.Length - 1] = c_uv15;

//            float angle = Mathf.Deg2Rad * angleDegree;
//            float currAngle = angle / 2;
//            float deltaAngle = angle / segments;

//            float tmp = angleDegree / 90;

//            //Debug.Log("================");
//            //Debug.Log("vertices.Length : " + vertices.Length);

//            int i = 0;
//            int i2 = 0;
//            for (i = 1, i2 = 0; i < vertices.Length - 4; i += 3, i2 ++)
//            {
//                vertices[i] = new Vector3(Mathf.Cos(currAngle) * radius, 0, Mathf.Sin(currAngle) * radius);
//                vertices[i + 1] = vertices[i];
//                vertices[i + 2] = new Vector3(Mathf.Cos(currAngle) * (radius + side), 0, Mathf.Sin(currAngle) * (radius + side));

//                float v = (float)i2 / (float)(segments);
//                int count = 0;
//                float tmp2 = v * tmp;

//                while (tmp2 > 1)
//                {
//                    tmp2 -= 1;
//                    count++;
//                }

//                uvs[i] = new Vector2(0.5f, v);
//                //uvs[i + 1] = new Vector2(0.5f, v);
//                //uvs[i + 2] = new Vector2(1f, v);

//                if (count % 2 == 1)
//                {
//                    uvs[i + 1] = new Vector2(0.5f, (tmp2) % 1);
//                    uvs[i + 2] = new Vector2(1, (tmp2) % 1);
//                }
//                else
//                {
//                    uvs[i + 1] = new Vector2(0.5f, (1 - (tmp2 % 1)));
//                    uvs[i + 2] = new Vector2(1, (1 - (tmp2 % 1)));
//                }
//                currAngle -= deltaAngle;
//            }

//            float angle_a = Mathf.Atan2(Stroke, radius);

//            uvs[i] = c_uv05;
//            vertices[i++] = new Vector3(Mathf.Cos(-angle_a - angle / 2) * (radius + side), 0, Mathf.Sin(-angle_a - angle / 2) * (radius + side));

//            uvs[i] = c_uv05;
//            vertices[i] = new Vector3(Mathf.Cos(angle_a + angle / 2) * (radius + side), 0, Mathf.Sin(angle / 2 + angle_a) * (radius + side));

//            //Debug.Log("i1: " + i);
//            //Debug.Log(trianglesTmp.Length);

//            int[] triangles = trianglesTmp;
//            int vi = 0;
//            for (i = 0, vi = 1; i < triangles.Length - 21; i += 9, vi += 3)
//            {
//                //内圈
//                triangles[i] = 0;
//                triangles[i + 1] = vi;
//                triangles[i + 2] = vi + 3;

//                //外圈1
//                triangles[i + 3] = vi + 1;
//                triangles[i + 4] = vi + 2;
//                triangles[i + 5] = vi + 4;

//                //外圈2
//                triangles[i + 6] = vi + 5;
//                triangles[i + 7] = vi + 2;
//                triangles[i + 8] = vi + 4;
//            }

//            //triangles[i++] = 0;
//            //triangles[i++] = vertices.Length - 1;
//            //triangles[i++] = 3;

//            //triangles[i++] = 0;
//            //triangles[i++] = vertices.Length - 2;
//            //triangles[i++] = 0;

//            //triangles[i++] = 0;
//            //triangles[i++] = vertices.Length - 3;
//            //triangles[i++] = vertices.Length - 2;

//            //triangles[i++] = 0;
//            //triangles[i++] = 4;
//            //triangles[i++] = vertices.Length - 1;

//            //Debug.Log("i:" + i);

//            mesh.vertices = vertices;
//            mesh.triangles = triangles;

//            mesh.uv = uvs;

//            return mesh;
//        }

//        #endregion

//        #region 圆形

//        public Mesh CreatCircle(float radius, int segments, float side, float Stroke)
//        {
//            if (segments == 0)
//            {
//                segments = 1;
//            }

//            if (mesh == null)
//            {
//                mesh = new Mesh();
//            }

//            mesh.Clear();

//            if (verticesTmp == null)
//            {
//                verticesTmp = new Vector3[2 * segments + 1];
//                uvsTmp = new Vector2[verticesTmp.Length];
//                trianglesTmp = new int[segments * 3 * 3];
//            }

//            Vector3[] vertices = verticesTmp;
//            Vector2[] uvs = uvsTmp;

//            vertices[0] = Vector3.zero;
//            uvs[0] = c_uv05;

//            float angle = Mathf.Deg2Rad * 360;
//            float currAngle = angle / 2;
//            float deltaAngle = angle / segments;

//            int i = 0;
//            int vi = 0;

//            for (i = 1; i < vertices.Length ; i += 2)
//            {
//                vertices[i] = new Vector3(Mathf.Cos(currAngle) * radius, 0, Mathf.Sin(currAngle) * radius);
//                vertices[i + 1] = new Vector3(Mathf.Cos(currAngle) * (radius + side), 0, Mathf.Sin(currAngle) * (radius + side));

//                float v = (float)(i - 1) / (float)(vertices.Length );
//                float tmp = v * 4;
//                int count = 0;

//                while (tmp > 1)
//                {
//                    tmp -= 1;
//                    count++;
//                }
//                if (count % 2 == 1)
//                {
//                    uvs[i] = new Vector2(0.5f, (tmp) % 1);
//                    uvs[i + 1] = new Vector2(1, (tmp) % 1);
//                }
//                else
//                {
//                    uvs[i] = new Vector2(0.5f, (1 - (tmp% 1) ));
//                    uvs[i + 1] = new Vector2(1, (1 - (tmp % 1)));
//                }

//                currAngle -= deltaAngle;
//            }

//            int[] triangles = trianglesTmp;
//            for (i = 0, vi = 1; i < triangles.Length - 9; i += 9, vi += 2)
//            {
//                triangles[i] = 0;
//                triangles[i + 1] = vi;
//                triangles[i + 2] = vi + 2;

//                triangles[i + 3] = vi;
//                triangles[i + 4] = vi + 1;
//                triangles[i + 5] = vi + 2;

//                triangles[i + 6] = vi + 2;
//                triangles[i + 7] = vi + 1;
//                triangles[i + 8] = vi + 3;
//            }

//            //Debug.Log(triangles.Length + "  " + i + "  " + segments);

//            triangles[i] = 0;
//            triangles[i + 1] = vi;
//            triangles[i + 2] = 1;

//            triangles[i + 3] = vi;
//            triangles[i + 4] = vi + 1;
//            triangles[i + 5] = 1;

//            triangles[i + 6] = 1;
//            triangles[i + 7] = vi + 1;
//            triangles[i + 8] = 2;

//            mesh.vertices = vertices;
//            mesh.triangles = triangles;

//            mesh.uv = uvs;

//            return mesh;
//        }

//        #endregion
//    }
//}
