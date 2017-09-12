using Box2DX.Common;

namespace Box2DX.Dynamics.Controllers
{
    /// <summary>
    /// This class is used to build buoyancy controllers
    /// </summary>
    public class BuoyancyControllerDef
    {
        /// The outer surface normal
        public Vec2 Normal;
        /// The height of the fluid surface along the normal
        public float Offset;
        /// The fluid density
        public float Density;
        /// Fluid velocity, for drag calculations
        public Vec2 Velocity;
        /// Linear drag co-efficient
        public float LinearDrag;
        /// Linear drag co-efficient
        public float AngularDrag;
        /// If false, bodies are assumed to be uniformly dense, otherwise use the shapes densities
        public bool UseDensity; //False by default to prevent a gotcha
        /// If true, gravity is taken from the world instead of the gravity parameter.
        public bool UseWorldGravity;
        /// Gravity vector, if the world's gravity is not used
        public Vec2 Gravity;

        public BuoyancyControllerDef()
        {
            Normal = new Vec2(0, 1);
            Offset = 0;
            Density = 0;
            Velocity = new Vec2(0, 0);
            LinearDrag = 0;
            AngularDrag = 0;
            UseDensity = false;
            UseWorldGravity = true;
            Gravity = new Vec2(0, 0);
        }
    }

    /// <summary>
    /// Calculates buoyancy forces for fluids in the form of a half plane.
    /// </summary>
    public class BuoyancyController : Controller
    {
        /// The outer surface normal
        public Vec2 Normal;
        /// The height of the fluid surface along the normal
        public float Offset;
        /// The fluid density
        public float Density;
        /// Fluid velocity, for drag calculations
        public Vec2 Velocity;
        /// Linear drag co-efficient
        public float LinearDrag;
        /// Linear drag co-efficient
        public float AngularDrag;
        /// If false, bodies are assumed to be uniformly dense, otherwise use the shapes densities
        public bool UseDensity; //False by default to prevent a gotcha
        /// If true, gravity is taken from the world instead of the gravity parameter.
        public bool UseWorldGravity;
        /// Gravity vector, if the world's gravity is not used
        public Vec2 Gravity;

        public BuoyancyController(BuoyancyControllerDef buoyancyControllerDef)
        {
            Normal = buoyancyControllerDef.Normal;
            Offset = buoyancyControllerDef.Offset;
            Density = buoyancyControllerDef.Density;
            Velocity = buoyancyControllerDef.Velocity;
            LinearDrag = buoyancyControllerDef.LinearDrag;
            AngularDrag = buoyancyControllerDef.AngularDrag;
            UseDensity = buoyancyControllerDef.UseDensity;
            UseWorldGravity = buoyancyControllerDef.UseWorldGravity;
            Gravity = buoyancyControllerDef.Gravity;
        }

        public override void Step(TimeStep step)
        {
            //B2_NOT_USED(step);
            if (_bodyList == null)
                return;

            if (UseWorldGravity)
            {
                Gravity = _world.Gravity;
            }
            for (ControllerEdge i = _bodyList; i != null; i = i.nextBody)
            {
                Body body = i.body;
                if (body.IsSleeping())
                {
                    //Buoyancy force is just a function of position,
                    //so unlike most forces, it is safe to ignore sleeping bodes
                    continue;
                }
                Vec2 areac = new Vec2(0, 0);
                Vec2 massc = new Vec2(0, 0);
                float area = 0;
                float mass = 0;
                for (Fixture shape = body.GetFixtureList(); shape != null; shape = shape.Next)
                {
                    Vec2 sc;
                    float sarea = shape.ComputeSubmergedArea(Normal, Offset, out sc);
                    area += sarea;
                    areac.X += sarea * sc.X;
                    areac.Y += sarea * sc.Y;
                    float shapeDensity = 0;
                    if (UseDensity)
                    {
                        //TODO: Expose density publicly
                        shapeDensity = shape.Density;
                    }
                    else
                    {
                        shapeDensity = 1;
                    }
                    mass += sarea * shapeDensity;
                    massc.X += sarea * sc.X * shapeDensity;
                    massc.Y += sarea * sc.Y * shapeDensity;
                }
                areac.X /= area;
                areac.Y /= area;
                //Vec2 localCentroid = Math.MulT(body.GetXForm(), areac);
                massc.X /= mass;
                massc.Y /= mass;
                if (area < Settings.FLT_EPSILON)
                    continue;
                //Buoyancy
                Vec2 buoyancyForce = -Density * area * Gravity;
                body.ApplyForce(buoyancyForce, massc);
                //Linear drag
                Vec2 dragForce = body.GetLinearVelocityFromWorldPoint(areac) - Velocity;
                dragForce *= -LinearDrag * area;
                body.ApplyForce(dragForce, areac);
                //Angular drag
                //TODO: Something that makes more physical sense?
                body.ApplyTorque(-body.GetInertia() / body.GetMass() * area * body.GetAngularVelocity() * AngularDrag);
            
            }
        }

        public override void Draw(DebugDraw debugDraw)
        {
            float r = 1000;
            Vec2 p1 = Offset * Normal + Vec2.Cross(Normal, r);
            Vec2 p2 = Offset * Normal - Vec2.Cross(Normal, r);

            Color color = new Color(0, 0, 0.8f);

            debugDraw.DrawSegment(p1, p2, color);
        }
    }
}