using OpenTK.Mathematics;

namespace SkylineEngine
{
    public sealed class Camera : Component
    {
        private float m_fieldOfView;
        private float m_aspect;
        private float m_nearClipPlane;
        private float m_farClipPlane;
        private Color m_backgroundColor;
        private Matrix4 m_perspectiveProjection;
        private Matrix4 m_orthographicProjection;

        public Box2 area { get; set; }

        private static Camera m_mainCamera;

        public static Camera main 
        { 
            get 
            { 
                return m_mainCamera; 
            } 
        }

        public float nearClipPlane
        {
            get 
            { 
                return m_nearClipPlane; 
            }
            set 
            {
                m_nearClipPlane = value;
                Initialize(m_fieldOfView, m_aspect, m_nearClipPlane, m_farClipPlane);
            }
        }

        public float farClipPlane
        {
            get 
            { 
                return m_farClipPlane; 
            }
            set 
            { 
                m_farClipPlane = value;
                Initialize(m_fieldOfView, m_aspect, m_nearClipPlane, m_farClipPlane);
            }
        }

        public float fieldOfView
        {
            get 
            { 
                return m_fieldOfView; 
            }
            set 
            { 
                m_fieldOfView = value;
                Initialize(m_fieldOfView, m_aspect, m_nearClipPlane, m_farClipPlane);
            }
        }

        public float aspect
        {
            get 
            {
                return m_aspect;
            }
            set
            {
                m_aspect = value;
                Initialize(m_fieldOfView, m_aspect, m_nearClipPlane, m_farClipPlane);
            }
        }

        public Color backgroundColor
        {
            get 
            {
                return m_backgroundColor;
            }
            set 
            {
                m_backgroundColor = value;
            }
        }

        public Camera()
        {
            m_aspect = (float)Screen.width / (float)Screen.height;
            m_backgroundColor = new Color(71, 188, 214, 255);
            
            Initialize(70, m_aspect, 0.1f, 1000);
            
            if (m_mainCamera == null)
                m_mainCamera = this;
        }

        public void Initialize(float fieldOfView, float aspect, float near, float far)
        {
            this.m_fieldOfView = fieldOfView;
            this.m_aspect = aspect;
            this.m_nearClipPlane = near;
            this.m_farClipPlane = far;
            this.m_perspectiveProjection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(m_fieldOfView), m_aspect, m_nearClipPlane, m_farClipPlane);
            this.m_orthographicProjection = Matrix4.CreateOrthographicOffCenter(0, Screen.width, Screen.height, 0, -1, 1);
        }

        public Matrix4 GetPerspectiveProjectionMatrix()
        {
            return m_perspectiveProjection;
        }

        public Matrix4 GetOrthographicProjectionMatrix()
        {
            return m_orthographicProjection;
        }

        public Matrix4 GetViewMatrix()
        {
            var m = transform.GetViewMatrix();
            m.Invert();
            return m;
        }

        public Box2 GetViewport()
        {
            var a = this.area;
            // If we have no pixel to draw to
            if (a.Width < 1 || a.Height < 1)
                area = new Box2(0, 0, Screen.width, Screen.height);
            return area;
        }       

        public Vector3 WorldToScreenPoint(Vector3 pointInWorld)
        {
            var v = new OpenTK.Mathematics.Vector4(pointInWorld.x, pointInWorld.y, pointInWorld.z, 1);            
            var pointInNdc = v * Camera.main.GetViewMatrix() * Camera.main.GetPerspectiveProjectionMatrix();
            pointInNdc.Xyz /= pointInNdc.W;
            float screenX = (pointInNdc.X + 1) / 2f * Screen.width;
            float screenY = (1 - pointInNdc.Y) / 2f * Screen.height;
            return new Vector3(screenX, screenY, 0);
        }
    }
}
