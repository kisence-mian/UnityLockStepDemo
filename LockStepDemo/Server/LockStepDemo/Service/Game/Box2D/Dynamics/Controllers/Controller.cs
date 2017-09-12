using System;

namespace Box2DX.Dynamics.Controllers
{
    /// <summary>
    /// A controller edge is used to connect bodies and controllers together
    /// in a bipartite graph.
    /// </summary>
    public class ControllerEdge
    {
        public Controller controller;		// provides quick access to other end of this edge.
        public Body body;					// the body
        public ControllerEdge prevBody;		// the previous controller edge in the controllers's joint list
        public ControllerEdge nextBody;		// the next controller edge in the controllers's joint list
        public ControllerEdge prevController;		// the previous controller edge in the body's joint list
        public ControllerEdge nextController;		// the next controller edge in the body's joint list
    }

    /// <summary>
    /// Base class for controllers. Controllers are a convience for encapsulating common
    /// per-step functionality.
    /// </summary>
    public abstract class Controller : IDisposable
    {
        internal Controller _prev;
        internal Controller _next;

        internal World _world;
        protected ControllerEdge _bodyList;
        protected int _bodyCount;

        public Controller()
        {
            _bodyList = null;
            _bodyCount = 0;
        }

        public Controller(World world)
        {
            _bodyList = null;
            _bodyCount = 0;

            _world = world;
        }

        public void Dispose()
        {
            //Remove attached bodies

            //Previus implementation:
            //while (_bodyCount > 0)
            //    RemoveBody(_bodyList.body);

            Clear();
        }

        /// <summary>
        /// Controllers override this to implement per-step functionality.
        /// </summary>
        public abstract void Step(TimeStep step);

        /// <summary>
        /// Controllers override this to provide debug drawing.
        /// </summary>
        public virtual void Draw(DebugDraw debugDraw) { }

        /// <summary>
        /// Adds a body to the controller list.
        /// </summary>
        public void AddBody(Body body)
        {
            ControllerEdge edge = new ControllerEdge();

            edge.body = body;
            edge.controller = this;

            //Add edge to controller list
            edge.nextBody = _bodyList;
            edge.prevBody = null;
            if (_bodyList != null)
                _bodyList.prevBody = edge;
            _bodyList = edge;
            ++_bodyCount;

            //Add edge to body list
            edge.nextController = body._controllerList;
            edge.prevController = null;
            if (body._controllerList != null)
                body._controllerList.prevController = edge;
            body._controllerList = edge;
        }

        /// <summary>
        /// Removes a body from the controller list.
        /// </summary>
        public void RemoveBody(Body body)
        {
            //Assert that the controller is not empty
            Box2DXDebug.Assert(_bodyCount > 0);

            //Find the corresponding edge
            ControllerEdge edge = _bodyList;
            while (edge != null && edge.body != body)
                edge = edge.nextBody;

            //Assert that we are removing a body that is currently attached to the controller
            Box2DXDebug.Assert(edge != null);

            //Remove edge from controller list
            if (edge.prevBody != null)
                edge.prevBody.nextBody = edge.nextBody;
            if (edge.nextBody != null)
                edge.nextBody.prevBody = edge.prevBody;
            if (edge == _bodyList)
                _bodyList = edge.nextBody;
            --_bodyCount;

            //Remove edge from body list
            if (edge.prevController != null)
                edge.prevController.nextController = edge.nextController;
            if (edge.nextController != null)
                edge.nextController.prevController = edge.prevController;
            if (edge == body._controllerList)
                body._controllerList = edge.nextController;

            //Free the edge
            edge = null;
        }

        /// <summary>
        /// Removes all bodies from the controller list.
        /// </summary>
        public void Clear()
        {
#warning "Check this"
            ControllerEdge current = _bodyList;
            while (current != null)
            {
                ControllerEdge edge = current;

                //Remove edge from controller list
                _bodyList = edge.nextBody;

                //Remove edge from body list
                if (edge.prevController != null)
                    edge.prevController.nextController = edge.nextController;
                if (edge.nextController != null)
                    edge.nextController.prevController = edge.prevController;
                if (edge == edge.body._controllerList)
                    edge.body._controllerList = edge.nextController;

                //Free the edge
                //m_world->m_blockAllocator.Free(edge, sizeof(b2ControllerEdge));
            }

            _bodyCount = 0;
        }

        /// <summary>
        /// Get the next body in the world's body list.
        /// </summary>
        internal Controller GetNext() { return _next; }

        /// <summary>
        /// Get the parent world of this body.
        /// </summary>
        internal World GetWorld() { return _world; }

        /// <summary>
        /// Get the attached body list
        /// </summary>
        internal ControllerEdge GetBodyList() { return _bodyList; }
    }
}