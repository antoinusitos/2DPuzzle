using System.Collections.Generic;

namespace _2DPuzzle
{
    public class CollisionManager
    {
        private static CollisionManager _instance;

        private static readonly object _lock = new object();

        public static CollisionManager GetInstance()
        {
            // This conditional is needed to prevent threads stumbling over the
            // lock once the instance is ready.
            if (_instance == null)
            {
                // Now, imagine that the program has just been launched. Since
                // there's no Singleton instance yet, multiple threads can
                // simultaneously pass the previous conditional and reach this
                // point almost at the same time. The first of them will acquire
                // lock and will proceed further, while the rest will wait here.
                lock (_lock)
                {
                    // The first thread to acquire the lock, reaches this
                    // conditional, goes inside and creates the Singleton
                    // instance. Once it leaves the lock block, a thread that
                    // might have been waiting for the lock release may then
                    // enter this section. But since the Singleton field is
                    // already initialized, the thread won't create a new
                    // object.
                    if (_instance == null)
                    {
                        _instance = new CollisionManager();
                    }
                }
            }
            return _instance;
        }

        private List<PhysicsComponent> registeredComponents = new List<PhysicsComponent>();

        public void RegisterPhysicsComponent(PhysicsComponent inPhysicsComponent)
        {
            registeredComponents.Add(inPhysicsComponent);
        }

        public void ProcessPhysics()
        {
            for(int physicsComponentIndex = 0; physicsComponentIndex < registeredComponents.Count; physicsComponentIndex++)
            {
                if (registeredComponents[physicsComponentIndex].collisionType == CollisionType.DYNAMIC)
                {
                    registeredComponents[physicsComponentIndex].owner.transformComponent.position += registeredComponents[physicsComponentIndex].velocity;
                    for (int againsPhysicsComponentIndex = 0; againsPhysicsComponentIndex < registeredComponents.Count; againsPhysicsComponentIndex++)
                    {
                        if (registeredComponents[physicsComponentIndex] == registeredComponents[againsPhysicsComponentIndex])
                        {
                            continue;
                        }
                        if(!registeredComponents[physicsComponentIndex].rectangle.Intersects(registeredComponents[againsPhysicsComponentIndex].rectangle))
                        {
                            continue;
                        }
                        float bottom = registeredComponents[physicsComponentIndex].rectangle.Bottom;
                        float top = registeredComponents[againsPhysicsComponentIndex].rectangle.Top;

                        if(bottom >= top)
                        {
                            Debug.Log("collision !");
                        }


                        //registeredComponents[physicsComponentIndex].owner.transformComponent.position -= registeredComponents[physicsComponentIndex].velocity;
                    }
                }
            }
        }
    }
}
