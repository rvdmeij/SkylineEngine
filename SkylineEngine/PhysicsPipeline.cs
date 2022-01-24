using System.Collections.Generic;
using BulletSharp;
using BulletSharp.Math;
using SkylineEngine.Collision;

namespace SkylineEngine
{
    internal static class PhysicsPipeline
    {
        private static bool isInitialized = false;
        private static DbvtBroadphase broadphase;
        private static DefaultCollisionConfiguration collisionConfiguration;
        private static CollisionDispatcher dispatcher;
        private static SequentialImpulseConstraintSolver solver;
        private static DiscreteDynamicsWorld dynamicsWorld;
        private static BulletSharp.Math.Vector3 gravity;

        private static List<Rigidbody> bodies;
        private static List<RigidBody> rigidBodies;
        private static List<CollisionShape> collisionShapes;

        public static void Initialize()
        {
            if (isInitialized)
                return;

            bodies = new List<Rigidbody>();
            rigidBodies = new List<RigidBody>();
            collisionShapes = new List<CollisionShape>();

            gravity = new BulletSharp.Math.Vector3(0, -9.81f, 0);

            broadphase = new DbvtBroadphase();
            collisionConfiguration = new DefaultCollisionConfiguration();
            dispatcher = new CollisionDispatcher(collisionConfiguration);
            solver = new SequentialImpulseConstraintSolver();
            dynamicsWorld = new DiscreteDynamicsWorld(dispatcher, broadphase, solver, collisionConfiguration);
            dynamicsWorld.SetInternalTickCallback(WorldPreTickCallback);
            dynamicsWorld.SetGravity(ref gravity);

            isInitialized = true;

            Debug.Log("PhysicsPipeline initialized");
        }

        private static void WorldPreTickCallback(DynamicsWorld world, float timeStep)
        {
            TestManifolds(world);
        }

        private static void TestManifolds(DynamicsWorld world)
        {
            int numManifolds = world.Dispatcher.NumManifolds;

            //if (numManifolds == 1)
            //{
            //    PersistentManifold contactManifold = world.Dispatcher.GetManifoldByIndexInternal(0);
            //    CollisionObject obA = contactManifold.Body0 as CollisionObject;
            //    CollisionObject obB = contactManifold.Body1 as CollisionObject;
            //    Rigidbody rbA = obA.UserObject as Rigidbody;
            //    Rigidbody rbB = obB.UserObject as Rigidbody;

            //    rbA.OnCollision(rbB);
            //    rbB.OnCollision(rbA);
            //}

            for (int i = 0; i < numManifolds; i++)
            {
                PersistentManifold contactManifold = world.Dispatcher.GetManifoldByIndexInternal(i);
                CollisionObject obA = contactManifold.Body0 as CollisionObject;
                CollisionObject obB = contactManifold.Body1 as CollisionObject;



                int numContacts = contactManifold.NumContacts;
                for (int j = 0; j < numContacts; j++)
                {
                    ManifoldPoint pt = contactManifold.GetContactPoint(j);
                    if (pt.Distance < 0.0f)
                    {
                        var ptA = pt.PositionWorldOnA;
                        var ptB = pt.PositionWorldOnB;
                        var normalOnB = pt.NormalWorldOnB;


                    }

                    Rigidbody rbA = obA.UserObject as Rigidbody;
                    Rigidbody rbB = obB.UserObject as Rigidbody;
                }
            }

        }

        private static int fixedTimeStep = 50;
        private static float timestep = 0.0f;

        public static void Update()
        {
            if (!isInitialized)
                return;

            timestep += Time.deltaTime;

            if(timestep >= (1.0f / fixedTimeStep))
            {
                MonoBehaviourManager.FixedUpdate();
                timestep = 0;
            }

            dynamicsWorld.StepSimulation(Time.deltaTime, 7, (1.0f / fixedTimeStep));


            for (int j = 0; j < rigidBodies.Count; j++)
            {
                CollisionObject obj = dynamicsWorld.CollisionObjectArray[j];
                RigidBody body = RigidBody.Upcast(obj);
                Rigidbody g = (Rigidbody)obj.UserObject;

                if(!g.gameObject.activeSelf)
                    continue;

                Matrix transf;

                if (body != null && body.MotionState != null)
                {
                    body.MotionState.GetWorldTransform(out transf);
                }
                else
                {
                    obj.GetWorldTransform(out transf);
                }

                if (j >= 0)
                {
                    
                    g.transform.position = new Vector3(transf.Origin.X, transf.Origin.Y, transf.Origin.Z);

                    BulletSharp.Math.Quaternion rotation;
                    BulletSharp.Math.Quaternion.RotationMatrix(ref transf, out rotation);

                    Quaternion q = new Quaternion(rotation.X, rotation.Y, rotation.Z, rotation.W);

                    g.transform.rotation = q;
                }
            }
        }

        public static void PushData(Rigidbody rb)
        {
            if (rb == null)
                return;

            GameObject gameObject = rb.gameObject;

            bodies.Add(rb);

            var components = gameObject.GetComponents();

            Collider collider = null;

            if (components != null)
            {
                for (int i = 0; i < components.Length; i++)
                {
                    if (components[i].GetType().IsSubclassOf(typeof(Collider)))
                    {
                        collider = components[i] as Collider;
                        break;
                    }
                }
            }

            if(!collider.Initialize())
            {
                Debug.Log("GameObject with ID " + rb.gameObject.InstanceId + " not added to PhysicsPipeline because the Collider couldn't be initialized");
                return;
            }

            BulletSharp.Math.Vector3 localInertia = new BulletSharp.Math.Vector3();

            if (rb.mass > float.Epsilon)
                collider.shape.CalculateLocalInertia(rb.mass, out localInertia);

            Quaternion rot = gameObject.transform.rotation;
            BulletSharp.Math.Quaternion rotation = new BulletSharp.Math.Quaternion(rot.x, rot.y, rot.z, rot.w);

            Matrix orientation = Matrix.RotationQuaternion(rotation);
            Matrix position = Matrix.Translation(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);

            var t = gameObject.transform.GetViewMatrix();

            Matrix transformation = new Matrix(t.M11, t.M12, t.M13, t.M14, t.M21, t.M22, t.M23, t.M24,
                                               t.M31, t.M32, t.M33, t.M34, t.M41, t.M42, t.M43, t.M44);

            DefaultMotionState motionState = new DefaultMotionState(transformation);

            RigidBodyConstructionInfo rigidBodyCI = new RigidBodyConstructionInfo(rb.mass, motionState, collider.shape, localInertia);

            RigidBody rigidBody = new RigidBody(rigidBodyCI);

            rigidBodies.Add(rigidBody);
            collisionShapes.Add(collider.shape);

            dynamicsWorld.AddRigidBody(rigidBody);

            rigidBody.UserObject = rb;

            rb.SetRigidBody(rigidBody);

            Debug.Log("Added " + gameObject.name + " to PhysicsPipeline with ID " + rb.InstanceId);
        }

        public static void Dispose()
        {
            if (!isInitialized)
                return;
            
            for (int i = 0; i < rigidBodies.Count; i++)
            {
                dynamicsWorld.RemoveRigidBody(rigidBodies[i]);
                rigidBodies[i].MotionState.Dispose();
                rigidBodies[i].Dispose();
            }
            for (int i = 0; i < collisionShapes.Count; i++)
            {
                collisionShapes[i].Dispose();
            }

            isInitialized = false;
        }
    }
}
