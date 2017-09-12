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
    /// This class is used to build tensor damping controllers
    /// </summary>
    public class b2TensorDampingControllerDef
    {
        /// Tensor to use in damping model
        Mat22 T;
        /// Set this to a positive number to clamp the maximum amount of damping done.
        float maxTimestep;
    };

    public class TensorDampingController : Controller
    {

        /// <summary>
        /// Tensor to use in damping model
        /// Some examples (matrixes in format (row1; row2) )
        ///(-a 0;0 -a)		Standard isotropic damping with strength a
        ///(0 a;-a 0)		Electron in fixed field - a force at right angles to velocity with proportional magnitude
        ///(-a 0;0 -b)		Differing x and y damping. Useful e.g. for top-down wheels.
        ///By the way, tensor in this case just means matrix, don't let the terminology get you down.
        /// </summary>
        Mat22 T;

        /// <summary>
        /// Set this to a positive number to clamp the maximum amount of damping done.
        /// Typically one wants maxTimestep to be 1/(max eigenvalue of T), so that damping will never cause something to reverse direction
        /// </summary>
        float MaxTimestep;

        /// Sets damping independantly along the x and y axes
        public void SetAxisAligned(float xDamping, float yDamping)
        {
            T.Col1.X = -xDamping;
            T.Col1.Y = 0;
            T.Col2.X = 0;
            T.Col2.Y = -yDamping;
            if (xDamping > 0 || yDamping > 0)
            {
                MaxTimestep = 1 / Math.Max(xDamping, yDamping);
            }
            else
            {
                MaxTimestep = 0;
            }
        }

        public override void Step(TimeStep step)
        {
            float timestep = step.Dt;
            if (timestep <= Settings.FLT_EPSILON)
                return;
            if (timestep > MaxTimestep && MaxTimestep > 0)
                timestep = MaxTimestep;
            for (ControllerEdge i = _bodyList; i != null; i = i.nextBody)
            {
                Body body = i.body;
                if (body.IsSleeping())
                    continue;

                Vec2 damping = body.GetWorldVector(Math.Mul(T, body.GetLocalVector(body.GetLinearVelocity())));
                body.SetLinearVelocity(body.GetLinearVelocity() + timestep*damping);
            }
        }
    }
}
