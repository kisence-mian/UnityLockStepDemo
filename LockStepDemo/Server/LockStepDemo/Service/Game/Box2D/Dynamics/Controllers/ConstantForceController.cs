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

namespace Box2DX.Dynamics.Controllers
{

    /// <summary>
    /// This class is used to build constant force controllers
    /// </summary>
    public class ConstantForceControllerDef
    {
        /// The force to apply
        public Vec2 F;
    }

    public class ConstantForceController : Controller
    {
        /// <summary>
        /// The force to apply
        /// </summary>
        Vec2 F;

        public ConstantForceController(ConstantForceControllerDef def)
        {
            F = def.F;
        }

        public override void Step(TimeStep step)
        {
            //B2_NOT_USED(step);
            for (ControllerEdge i = _bodyList; i != null; i = i.nextBody)
            {
                Body body = i.body;
                if (body.IsSleeping())
                    continue;
                body.ApplyForce(F, body.GetWorldCenter());
            }
        }
    }
}
