namespace SkylineEngine
{
    public class SmoothMouseOrbit : MonoBehaviour
    {
        public enum ZoomMode
        {
            CameraFieldOfView,
            ZAxisDistance
        }

        public bool autoRotate = false;
        public float rotationSpeed = 8.0f;
        public float startRotation = 0;
        public float rotationSmoothing = 8.0f;
        public Transform target;
        public float rotationSensitivity = 0.01f;
        //public Vector2 rotationLimit = new Vector2(180, 90);
        public Vector2 rotationLimit = new Vector2(-90, 180);
        public float zAxisDistance = 20;
        public ZoomMode zoomMode = ZoomMode.ZAxisDistance;
        public Vector2 cameraZoomRangeFOV = new Vector2(10, 60);
        public Vector2 cameraZoomRangeZAxis = new Vector2(10, 60);
        public float zoomSoothness = 10.0f;
        public float zoomSensitivity = 0.5f;

        private Camera cam;
        private float cameraFieldOfView;
        private float xVelocity;
        private float yVelocity;
        private float xRotationAxis;
        private float yRotationAxis;
        private float zoomVelocity;
        private float zoomVelocityZAxis;
        private bool canRotate = true;
        private bool canControl = true;

        private void Awake()
        {
            cam = gameObject.GetComponent<Camera>();
        }

        private void Start()
        {
            cameraFieldOfView = cam.fieldOfView;
            //Sets the camera's rotation along the y axis.
            //The reason we're dividing by rotationSpeed is because we'll be multiplying by rotationSpeed in LateUpdate.
            //So we're just accouinting for that at start.
            xRotationAxis = startRotation / rotationSpeed;

            Vector3 angles = transform.rotation.eulerAngles;
            xRotationAxis = angles.y;
            //y = angles.x;
            yRotationAxis = 180.0f;


        }

        private void Update()
        {
            if(!canControl)
                return;

            ToggleCursorVisibility();
            ToggleCanRotate();
            Zoom();
        }

        private void ToggleCursorVisibility()
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                Cursor.visible = !Cursor.visible;

                if (Cursor.visible)
                    Cursor.lockState = CursorLockMode.None;
                else
                    Cursor.lockState = CursorLockMode.Locked;
            }          
        }

        public void ToggleCanRotate()
        {
            if(Input.GetKeyDown(KeyCode.L))
            {
                canRotate = !canRotate;
            }
        }

        private void LateUpdate()
        {                
            //If auto rotation is enabled, just increment the xVelocity value by the rotationSensitivity.
            //As that value's tied to the camera's rotation, it'll rotate automatically.
            if (autoRotate)
            {
                xVelocity += rotationSensitivity * Time.deltaTime;
            }
            if (target != null)
            {
                Quaternion rotation;
                Vector3 position;
                float deltaTime = Time.deltaTime;

                if (canRotate && canControl)
                {
                    float deltaX = Input.GetMouseDelta().x;
                    float deltaY = Input.GetMouseDelta().y;

                    if(Mathf.Abs(deltaX) < 2)
                        deltaX = 0;
                    
                    if(Mathf.Abs(deltaY) < 2)
                        deltaY = 0;

                    xVelocity -= deltaX * rotationSensitivity;
                    yVelocity -= deltaY * rotationSensitivity * 8;
                }


                xRotationAxis += xVelocity;
                yRotationAxis -= yVelocity;

                //Clamp the rotation along the y-axis between the limits we set. 
                //Limits of 360 or -360 on any axis will allow the camera to rotate unrestricted
                //yRotationAxis = ClampAngleBetweenMinAndMax(yRotationAxis, rotationLimit.x, rotationLimit.y);

                rotation = Quaternion.Euler(yRotationAxis, -xRotationAxis * rotationSpeed, 0);
                position = rotation * new Vector3(0f, 0f, zAxisDistance) + target.position;

                transform.rotation = rotation;
                transform.position = position;

                xVelocity = Mathf.Lerp(xVelocity, 0, deltaTime * rotationSmoothing);
                yVelocity = Mathf.Lerp(yVelocity, 0, deltaTime * rotationSmoothing);
            }
        }

        private void Zoom()
        {
            float deltaTime = Time.deltaTime;

            //Zooms the camera in using the mouse scroll wheel
            if (Input.GetMouseScrollDirection() > 0f)
            {
                if (zoomMode == ZoomMode.CameraFieldOfView)
                {
                    cameraFieldOfView = Mathf.SmoothDamp(cameraFieldOfView, cameraZoomRangeFOV.x, ref zoomVelocity, deltaTime * zoomSoothness);

                    //prevents the field of view from going below the minimum value
                    if (cameraFieldOfView <= cameraZoomRangeFOV.x)
                    {
                        cameraFieldOfView = cameraZoomRangeFOV.x;
                    }
                }
                else
                {
                    if (zoomMode == ZoomMode.ZAxisDistance)
                    {
                        zAxisDistance = Mathf.SmoothDamp(zAxisDistance, cameraZoomRangeZAxis.x, ref zoomVelocityZAxis, deltaTime * zoomSoothness);

                        //prevents the z axis distance from going below the minimum value
                        if (zAxisDistance <= cameraZoomRangeZAxis.x)
                        {
                            zAxisDistance = cameraZoomRangeZAxis.x;
                        }
                    }
                }
            }
            else
            {
                //Zooms the camera out using the mouse scroll wheel
                if (Input.GetMouseScrollDirection() < 0f)
                {
                    if (zoomMode == ZoomMode.CameraFieldOfView)
                    {
                        cameraFieldOfView = Mathf.SmoothDamp(cameraFieldOfView, cameraZoomRangeFOV.y, ref zoomVelocity, deltaTime * zoomSoothness);

                        //prevents the field of view from exceeding the max value
                        if (cameraFieldOfView >= cameraZoomRangeFOV.y)
                        {
                            cameraFieldOfView = cameraZoomRangeFOV.y;
                        }
                    }
                    else
                    {
                        if (zoomMode == ZoomMode.ZAxisDistance)
                        {
                            zAxisDistance = Mathf.SmoothDamp(zAxisDistance, cameraZoomRangeZAxis.y, ref zoomVelocityZAxis, deltaTime * zoomSoothness);

                            //prevents the z axis distance from exceeding the max value
                            if (zAxisDistance >= cameraZoomRangeZAxis.y)
                            {
                                zAxisDistance = cameraZoomRangeZAxis.y;
                            }
                        }
                    }
                }
            }

            //We're just ensuring that when we're zooming using the camera's FOV, that the FOV will be updated to match the value we got when we scrolled.
            if (Input.GetMouseScrollDirection() > 0 || Input.GetMouseScrollDirection() < 0)
            {
                cam.fieldOfView = cameraFieldOfView;
            }
        }

        public void ToggleCanControl(bool enabled)
        {
            canControl = enabled;
        }

        //Prevents the camera from locking after rotating a certain amount if the rotation limits are set to 360 degrees.
        private float ClampAngleBetweenMinAndMax(float angle, float min, float max)
        {
            if (angle < -360)
            {
                angle += 360;
            }
            if (angle > 360)
            {
                angle -= 360;
            }
            return Mathf.Clamp(angle, min, max);
        }
    }
}