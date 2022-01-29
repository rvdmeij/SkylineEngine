using BulletSharp;
using SkylineEngine.Collision;

namespace SkylineEngine
{

    public enum ForceMode
    {
        Force,          //Add a continuous force to the rigidbody, using its mass.
        Impulse        //Add an instant force impulse to the rigidbody, using its mass.
    }

    public sealed class Rigidbody : Component
    {
        private float m_mass = 1.0f;
        private Vector3 m_velocity = Vector3.zero;
        private RigidBody m_rigidBody;

        public RigidBody rigidBody
        {
            get
            {
                return m_rigidBody;
            }
        }

        public float mass
        {
            get
            {
                return m_mass;
            }
            set
            {
                m_mass = value;
                if(m_rigidBody != null)
                {
                    SetMass();
                }
            }
        }

        public Vector3 velocity
        {
            get 
            {
                if (m_rigidBody == null)
                    return Vector3.zero;
                return new Vector3(m_rigidBody.LinearVelocity.X, m_rigidBody.LinearVelocity.Y, m_rigidBody.LinearVelocity.Z);
            }
            set 
            { 
                m_velocity = value; 
                if(m_rigidBody != null)
                {
                    SetVelocity();
                }
            }
        }

        public Vector3 centerOfMass
        {
            get
            {
                if (m_rigidBody == null)
                    return Vector3.zero;
                return new Vector3(m_rigidBody.CenterOfMassPosition.X, m_rigidBody.CenterOfMassPosition.Y, m_rigidBody.CenterOfMassPosition.Z); 
            }
            set
            {
                if (m_rigidBody != null)
                {
                    CreateTranslationMatrix(value);
                }
            }
        }

        public override void InitializeComponent()
        {
            Initialize();
        }

        public void SetBounciness(float bounciness)
        {
            m_rigidBody.Restitution = bounciness;
        }

        public void SetStiffnessAndDamping(float stiffness, float damping)
        {
            //m_rigidBody.SetContactStiffnessAndDamping(stiffness, damping);
        }

        public void MovePosition(Vector3 position)
        {
            Activate();
            m_rigidBody.Translate(new BulletSharp.Math.Vector3(position.x, position.y, position.z));
        }

        public void MoveRotation(Quaternion rotation)
        {
            Activate();
            var rot = rotation.eulerAngles;
            var v = new BulletSharp.Math.Vector3(rot.x, rot.y, rot.z);
            m_rigidBody.ApplyTorque(v);
        }

        public float GetFriction()
        {
            if (m_rigidBody == null)
                return 0;
            return m_rigidBody.Friction;
        }

        public void SetFriction(float f)
        {
            if (m_rigidBody == null)
                return;
            m_rigidBody.Friction = f;
        }

        internal void SetRigidBody(RigidBody rb)
        {
            this.m_rigidBody = rb;
        }

        internal void Initialize()
        {
            PhysicsPipeline.PushData(this);
        }

        public void AddForce(Vector3 force, ForceMode forceMode = ForceMode.Impulse)
        {
            Activate();
            if(forceMode == ForceMode.Impulse)
                m_rigidBody.ApplyCentralImpulse(new BulletSharp.Math.Vector3(force.x, force.y, force.z));
            else if(forceMode == ForceMode.Force)
                m_rigidBody.ApplyCentralForce(new BulletSharp.Math.Vector3(force.x, force.y, force.z));
        }

        public void AddRelativeForce(Vector3 force)
        {
            Activate();
        }

        public void AddTorque(Vector3 torque, ForceMode forceMode = ForceMode.Impulse)
        {
            Activate();
            if(forceMode == ForceMode.Impulse)
                m_rigidBody.ApplyTorqueImpulse(new BulletSharp.Math.Vector3(torque.x, torque.y, torque.z));
            else if(forceMode == ForceMode.Force)
                m_rigidBody.ApplyTorque(new BulletSharp.Math.Vector3(torque.x, torque.y, torque.z));
            
        }

        public void AddRelativeTorque(Vector3 torque)
        {
            Activate();
        }

        private void SetVelocity()
        {
            Activate();
            m_rigidBody.LinearVelocity = new BulletSharp.Math.Vector3(m_velocity.x, m_velocity.x, m_velocity.x);
        }

        private void SetMass()
        {
            var inertia = m_rigidBody.LocalInertia;
            m_rigidBody.SetMassProps(m_mass, inertia);
        }

        private void Activate()
        {
            switch (m_rigidBody.ActivationState)
            {
                case ActivationState.Undefined:
                    break;
                case ActivationState.ActiveTag:
                    break;
                case ActivationState.IslandSleeping:
                    m_rigidBody.Activate(true);
                    break;
                case ActivationState.WantsDeactivation:
                    break;
                case ActivationState.DisableDeactivation:
                    break;
                case ActivationState.DisableSimulation:
                    break;
                default:
                    break;
            }
        }

        internal void OnCollision(Rigidbody other)
        {
            Debug.Log(gameObject.name + " made contact with " + other.gameObject.name);
        }

        private BulletSharp.Math.Matrix CreateTranslationMatrix(Vector3 position)
        {
            var v = position.ToOpenTKVector();
            var t = OpenTK.Mathematics.Matrix4.CreateTranslation(v);

            BulletSharp.Math.Matrix transformation = new BulletSharp.Math.Matrix(t.M11, t.M12, t.M13, t.M14, t.M21, t.M22, t.M23, t.M24,
                                                                                 t.M31, t.M32, t.M33, t.M34, t.M41, t.M42, t.M43, t.M44);

            return transformation;
        }
    }
}
