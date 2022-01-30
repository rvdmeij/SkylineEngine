using System.Collections.Generic;
using OpenTK.Mathematics;

namespace SkylineEngine
{
    public delegate void TransformChangedEvent(ref OpenTK.Mathematics.Matrix4 m);

    public sealed class Transform : Component
    {
        private Transform m_parent;
        private List<Transform> m_children = new List<Transform>();
        private Vector3 m_position;
        private Quaternion m_rotation;
        private Vector3 m_scale;
        private Vector3 m_oldPosition;
        private Quaternion m_oldRotation;
        private Vector3 m_oldScale;
        private Vector3 m_forward;
        private Vector3 m_right;
        private Vector3 m_up;
        private Matrix4 m_translationMatrix;
        private Matrix4 m_rotationMatrix;
        private Matrix4 m_scaleMatrix;
        private Matrix4 m_parentMatrix;

        internal event TransformChangedEvent onTransformChanged;

        public Transform parent
        {
            get 
            { 
                return m_parent; 
            }
            set 
            {
                if (m_parent == null)
                {
                    m_parent = value;

                    if(m_parent != null)
                        m_parent.AddChild(this);

                }
                else
                {
                    if (value == null)
                    {
                        m_parent.RemoveChild(this);
                    }
                    else
                    {
                        m_parent.AddChild(this);
                    }

                    m_parent = value;

                }
            }
        }

        public List<Transform> children
        {
            get { return m_children; }
        }

        public Vector3 position
        {
            get
            {
                return m_position;
            }
            set
            {
                m_position = value;
                SetTranslationMatrix();
            }
        }

        public Vector3 localPosition
        {
            get
            {
                if(parent != null)
                {
                    Vector3 pos = m_position - parent.position;
                    return pos;
                }
                else
                {
                    return m_position;
                }
            }
            set
            {
                if(parent != null)
                {
                    Vector3 pos = parent.position + value;
                    m_position = pos;
                    SetTranslationMatrix();
                }
                else
                {
                    m_position = value;
                    SetTranslationMatrix();
                }
                
            }
        }

        public Quaternion rotation
        {
            get { return m_rotation; }
            set
            {
                m_rotation = value;
                SetRotationMatrix();
            }
        }

        public Quaternion localRotation
        {
            get
            {
                if(parent == null)
                    return m_rotation;
                
                var rot = m_rotation * Quaternion.Inverse(parent.rotation);
                return rot;
            }
            set
            {
                Quaternion rot = value;
                if(parent != null)
                    m_rotation = rot * Quaternion.Inverse(parent.rotation);
                else
                    m_rotation = value;
                SetRotationMatrix();
            }
        }

        public Vector3 scale
        {
            get { return m_scale; }
            set 
            {
                m_scale = value; 
                SetScaleMatrix(); 
            }
        }

        public Vector3 eulerAngles
        {
            get { return rotation.eulerAngles; }
        }

        public Vector3 up { get { return m_up; } }
        public Vector3 right { get { return m_right; } }
        public Vector3 forward { get { return m_forward; } }

        public Transform()
        {
            m_translationMatrix = Matrix4.Identity;
            m_rotationMatrix = Matrix4.Identity;
            m_scaleMatrix = Matrix4.Identity;

            m_up = Vector3.up;
            m_right = Vector3.right;
            m_forward = -Vector3.forward;

            position = new Vector3();
            rotation = Quaternion.identity;
            scale = new Vector3(1, 1, 1);
            parent = null;
            m_parentMatrix = Matrix4.Identity;
            m_oldPosition = Vector3.zero;
            m_oldRotation = Quaternion.identity;
            m_oldScale = Vector3.zero;
            rot = rotation.eulerAngles;
    }

        internal void AddChild(Transform child)
        {
            //Check if transform already is a child, if so then return
            for(int i = 0; i < m_children.Count; i++)
            {
                if (m_children[i].InstanceId == child.InstanceId)
                    return;
            }

            m_children.Add(child);
        }

        internal void RemoveChild(Transform child)
        {
            for (int i = m_children.Count -1; i > 0; i--)
            {
                if(m_children[i].InstanceId == child.InstanceId)
                {
                    m_children.RemoveAt(i);
                    break;
                }
            }
        }

        public void SetParent(Transform t)
        {
            parent = t;
        }

        public void SetPosition(Vector3 position)
        {
            m_position = position;
            SetTranslationMatrix();
        }

        public void Translate(Vector3 translation)
        {
            m_position += translation;
            SetTranslationMatrix();
        }

        public void SetTranslationMatrix()
        {
            OpenTK.Mathematics.Vector3 v = m_position.ToOpenTKVector();
            m_translationMatrix = Matrix4.CreateTranslation(v);

            if (m_position != m_oldPosition)
            {
                m_oldPosition = m_position;
                OnChanged();
            }
        }

        public void SetRotationMatrix()
        {
            var q = new OpenTK.Mathematics.Quaternion(m_rotation.x, m_rotation.y, m_rotation.z, m_rotation.w);

            m_rotationMatrix = Matrix4.CreateFromQuaternion(q);

            Vector3 rot;
            m_rotation.ToEulerAngles(out rot);

            m_forward = new Vector3(-Mathf.Sin(rot.y), Mathf.Cos(rot.y) * Mathf.Sin(rot.x), -(Mathf.Cos(rot.x) * Mathf.Cos(rot.y)));
            m_right = Vector3.Normalize(Vector3.Cross(m_forward, Vector3.up));
            m_up = Vector3.Normalize(Vector3.Cross(m_right, m_forward));

            if (m_rotation != m_oldRotation)
            {
                m_oldRotation = m_rotation;
                OnChanged();
            }
        }

        public void SetScaleMatrix()
        {
            m_scaleMatrix = Matrix4.CreateScale(scale.x, scale.y, scale.z);

            if (m_scale != m_oldScale)
            {
                m_oldScale = m_scale;
                OnChanged();
            }
        }

        public Matrix4 GetViewMatrix()
        {
            Matrix4 m = m_scaleMatrix * m_rotationMatrix * m_translationMatrix;

            if (parent != null)
                return m * m_parentMatrix;
            else
                return m;
        }

        private void OnChanged()
        {
            if (m_children.Count == 0)
                return;

            Matrix4 transformation = m_scaleMatrix * m_rotationMatrix * m_translationMatrix;
            //Matrix4 transformation = GetViewMatrix();

            for (int i = 0; i < m_children.Count; i++)
                m_children[i].SetParentMatrix(transformation);

            if (onTransformChanged != null)
                onTransformChanged(ref transformation);
        }

        private void SetParentMatrix(Matrix4 m)
        {
            m_parentMatrix = m;

            if (onTransformChanged != null)
                onTransformChanged(ref m_parentMatrix);
        }

        public void SetScale(Vector3 scale)
        {
            m_scale = scale;
            SetScaleMatrix();
        }

        public void Rotate(Quaternion rotation)
        {
            m_rotation = rotation;
            SetRotationMatrix();
        }

        public void Rotate(Vector3 rotation)
        {
            m_rotation = Quaternion.Euler(rotation.x, rotation.y, rotation.z);
            SetRotationMatrix();
        }

        private Vector3 rot = Vector3.zero;

        public void Rotate(float x, float y, float z)
        {
            var oldRotation = rot;

            rot.x += x;
            rot.y += y;
            rot.z += z;

            var rotationDelta = rot - oldRotation;

            var rott = m_rotation.ToOpenTKQuaternion();

            rott = rott * OpenTK.Mathematics.Quaternion.FromAxisAngle(OpenTK.Mathematics.Vector3.UnitX, rotationDelta.x * Mathf.Deg2Rad);
            rott = rott * OpenTK.Mathematics.Quaternion.FromAxisAngle(OpenTK.Mathematics.Vector3.UnitY, rotationDelta.y * Mathf.Deg2Rad);
            rott = rott * OpenTK.Mathematics.Quaternion.FromAxisAngle(OpenTK.Mathematics.Vector3.UnitZ, -rotationDelta.z * Mathf.Deg2Rad);

            Quaternion q = new Quaternion(rott.X, rott.Y, rott.Z, rott.W);            
            m_rotation = q;
            SetRotationMatrix();
        }

        public void LookAt(Transform target)
        {
            this.LookAt(target, Vector3.up);
        }

        public void LookAt(Transform target, Vector3 worldUp)
        {
            if (target == null)
                return;
            this.LookAt(target.position, worldUp);
        }

        /// <summary>
        ///   <para>Rotates the transform so the forward vector points at worldPosition.</para>
        /// </summary>
        /// <param name="worldPosition">Point to look at.</param>
        /// <param name="worldUp">Vector specifying the upward direction.</param>
        public void LookAt(Vector3 worldPosition, Vector3 worldUp)
        {
            var mat = Matrix4.LookAt(position.ToOpenTKVector(), worldPosition.ToOpenTKVector(), worldUp.ToOpenTKVector());
            var q = OpenTK.Mathematics.Quaternion.FromMatrix(new OpenTK.Mathematics.Matrix3(mat));
            Quaternion rot = new Quaternion(q.X, q.Y, q.Z, q.W);
            Rotate(rot);
        }

        public Vector3 InverseTransformDirection(Vector3 direction)
        {
            var v = direction.ToOpenTKVector();
            var dir = new OpenTK.Mathematics.Matrix3(GetViewMatrix().Inverted()) * v;
            return new Vector3(dir.X, dir.Y, dir.Z);
        }

        public Vector3 TransformDirection(Vector3 direction)
        {
            Vector3 v = transform.rotation * direction;
            return v;
        }

        public static void TransformDirection(ref Vector3 vector, ref Quaternion rotation, out Vector3 result)
        {
            float x = rotation.x + rotation.x;
            float y = rotation.y + rotation.y;
            float z = rotation.z + rotation.z;
            float wx = rotation.w * x;
            float wy = rotation.w * y;
            float wz = rotation.w * z;
            float xx = rotation.x * x;
            float xy = rotation.x * y;
            float xz = rotation.x * z;
            float yy = rotation.y * y;
            float yz = rotation.y * z;
            float zz = rotation.z * z;

            result = new Vector3(vector.x * (1.0f - yy - zz) + vector.y * (xy - wz) + vector.z * (xz + wy),
                                 vector.x * (xy + wz) + vector.y * (1.0f - xx - zz) + vector.z * (yz - wx),
                                 vector.x * (xz - wy) + vector.y * (yz + wx) + vector.z * (1.0f - xx - yy));
        }  

     
    }
}
