/*
* Copyright (c) 2006-2007 Erin Catto http://www.gphysics.com
*
* This software is provided 'as-is', without any express or implied
* warranty.  In no event will the authors be held liable for any damages
* arising from the use of this software.
* Permission is granted to anyone to use this software for any purpose,
* including commercial applications, and to alter it and redistribute it
* freely, subject to the following restrictions:
* 1. The origin of this software must not be misrepresented; you must not
* claim that you wrote the original software. If you use this software
* in a product, an acknowledgment in the product documentation would be
* appreciated but is not required.
* 2. Altered source versions must be plainly marked as such, and must not be
* misrepresented as being the original software.
* 3. This notice may not be removed or altered from any source distribution.
*/

using Box2DX.Common;
using Math = Box2DX.Common.Math;

namespace Box2DX.Dynamics.Controllers
{
    /// This class is used to build gravity controllers
    public class GravityControllerDef
    {
        /// <summary>
        /// Specifies the strength of the gravitiation force
        /// </summary>
        public float G;

        /// <summary>
        /// If true, gravity is proportional to r^-2, otherwise r^-1
        /// </summary>
        public bool InvSqr;
    }

    public class GravityController : Controller
    {
        /// <summary>
        /// Specifies the strength of the gravitiation force
        /// </summary>
        public float G;

        /// If true, gravity is proportional to r^-2, otherwise r^-1
        public bool InvSqr;

        public GravityController(GravityControllerDef def)
        {
            G = def.G;
            InvSqr = def.InvSqr;
        }

        public override void Step(TimeStep step)
        {
            //B2_NOT_USED(step);
            if (InvSqr)
            {
                for (ControllerEdge i = _bodyList; i != null; i = i.nextBody)
                {
                    Body body1 = i.body;
                    for (ControllerEdge j = _bodyList; j != i; j = j.nextBody)
                    {
                        Body body2 = j.body;
                        Vec2 d = body2.GetWorldCenter() - body1.GetWorldCenter();
                        float r2 = d.LengthSquared();
                        if (r2 < Settings.FLT_EPSILON)
                            continue;

                        Vec2 f = G / r2 / Math.Sqrt(r2) * body1.GetMass() * body2.GetMass() * d;
                        body1.ApplyForce(f, body1.GetWorldCenter());
                        body2.ApplyForce(-1.0f * f, body2.GetWorldCenter());
                    }
                }
            }
            else
            {
                for (ControllerEdge i = _bodyList; i != null; i = i.nextBody)
                {
                    Body body1 = i.body;
                    for (ControllerEdge j = _bodyList; j != i; j = j.nextBody)
                    {
                        Body body2 = j.body;
                        Vec2 d = body2.GetWorldCenter() - body1.GetWorldCenter();
                        float r2 = d.LengthSquared();
                        if (r2 < Settings.FLT_EPSILON)
                            continue;
                        Vec2 f = G / r2 * body1.GetMass() * body2.GetMass() * d;
                        body1.ApplyForce(f, body1.GetWorldCenter());
                        body2.ApplyForce(-1.0f * f, body2.GetWorldCenter());
                    }
                }
            }
        }
    }
}
