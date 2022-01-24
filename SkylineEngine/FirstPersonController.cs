using System.Collections.Generic;
using SkylineEngine.InputManagement;
using DearImGui;

namespace SkylineEngine
{
    public class FirstPersonController : MonoBehaviour
    {
        public float speed = 100.0f;
        public float rotationSpeed = 0.01f;
        public float zoomSpeed = 200.0f;

        private Camera camera;
        private Vector2 newPosition;
        private Vector2 oldPosition;
        private SmoothInput forceHorizontal = new SmoothInput();
        private SmoothInput forceVertical = new SmoothInput();
        private SmoothInput forcePanning = new SmoothInput();
        private SmoothInput forceZoom = new SmoothInput();
        private OpenTK.Mathematics.Vector3 currentRotation;

        private float inputVertical;
        private float inputHorizontal;
        private float inputPanning;
        private float inputZoom;
        private bool mouseIsDown;

        void Awake()
        {
            camera = gameObject.GetComponent<Camera>();

            if(camera == null)
            {
                Debug.Log("No Camera component found on this game object");
                this.enabled = false;
            }            
        }

        void Start()
        {
            AxisInfo infoVertical = new AxisInfo();
            AxisInfo infoHorizontal = new AxisInfo();
            AxisInfo infoPanning = new AxisInfo();

            AxisKeys keysVertical = new AxisKeys();
            AxisKeys keysHorizontal = new AxisKeys();
            AxisKeys keysPanning = new AxisKeys();

            keysVertical.positive = KeyCode.W;
            keysVertical.negative = KeyCode.S;

            keysHorizontal.positive = KeyCode.D;
            keysHorizontal.negative = KeyCode.A;

            keysPanning.positive = KeyCode.R;
            keysPanning.negative = KeyCode.F;

            List<AxisKeys> vertical = new List<AxisKeys>();
            List<AxisKeys> horizontal = new List<AxisKeys>();
            List<AxisKeys> panning = new List<AxisKeys>();

            vertical.Add(keysVertical);
            horizontal.Add(keysHorizontal);
            panning.Add(keysPanning);

            infoVertical.name = "Vertical";
            infoVertical.keys = vertical.ToArray();

            infoHorizontal.name = "Horizontal";
            infoHorizontal.keys = horizontal.ToArray();

            infoPanning.name = "Panning";
            infoPanning.keys = panning.ToArray();

            List<AxisInfo> axisInfo = new List<AxisInfo>();
            axisInfo.Add(infoVertical);
            axisInfo.Add(infoHorizontal);
            axisInfo.Add(infoPanning);

            List<ActionInfo> actionInfo = new List<ActionInfo>();

            Input.RegisterAxis(actionInfo.ToArray(), axisInfo.ToArray());

            forceZoom.transitionTime = 0.01f;
        }

        void Update()
        {
            // inputVertical = 0.0f;
            // inputHorizontal = 0.0f;
            // inputPanning = 0.0f;
            // inputZoom = 0.0f;

            // if(KeyboardState.Instance.IsKeyPressed(KeyCode.W))
            //     inputVertical = forceVertical.GetValue(1.0f);
            // if(KeyboardState.Instance.IsKeyPressed(KeyCode.S))
            //     inputVertical = forceVertical.GetValue(-1.0f);

            // if(KeyboardState.Instance.IsKeyPressed(KeyCode.D))
            //     inputHorizontal = forceVertical.GetValue(1.0f);
            // if(KeyboardState.Instance.IsKeyPressed(KeyCode.A))
            //     inputHorizontal = forceVertical.GetValue(-1.0f);                

            // if(KeyboardState.Instance.IsKeyPressed(KeyCode.R))
            //     inputPanning = forceVertical.GetValue(1.0f);
            // if(KeyboardState.Instance.IsKeyPressed(KeyCode.F))
            //     inputPanning = forceVertical.GetValue(-1.0f);   



            inputVertical = forceVertical.GetValue(Input.GetAxis("Vertical"));
            inputHorizontal = forceHorizontal.GetValue(Input.GetAxis("Horizontal"));
            inputPanning = forcePanning.GetValue(Input.GetAxis("Panning"));
            inputZoom = forceZoom.GetValue(Input.GetMouseScrollDirection());
            mouseIsDown = Input.GetMouseDown(MouseButton.Right);




            //inputHorizontal = forceHorizontal.GetValue(Input.GetAxis("Horizontal"));
            //inputPanning = forcePanning.GetValue(Input.GetAxis("Panning"));
            //inputZoom = forceZoom.GetValue(Input.GetMouseScrollDirection());
            //mouseIsDown = Input.GetMouseDown(MouseButton.Right);
        }

        void LateUpdate()
        {
            if (Mathf.Abs(inputVertical) > float.Epsilon)
            {
                Move(transform.forward, inputVertical * speed * Time.deltaTime);
            }

            if (Mathf.Abs(inputHorizontal) > float.Epsilon)
            {
                Move(transform.right, inputHorizontal * speed * Time.deltaTime);
            }

            if (Mathf.Abs(Input.GetMouseScrollDirection()) > float.Epsilon)
            {
                Move(transform.forward, inputZoom * zoomSpeed * Time.deltaTime);
                Input.ClearMouseScrollDirection();
            }

            if (Mathf.Abs(inputPanning) > float.Epsilon)
            {
                Move(Vector3.up, inputPanning * speed * Time.deltaTime);
            }

            if (mouseIsDown)
            {
                Rotate();
            }
        }

        void Move(Vector3 direction, float movementSpeed)
        {
            transform.position += direction * movementSpeed;
        }

        void Rotate()
        {
            //newPosition = Input.GetMousePosition();

            SDL2.SDL.SDL_GetGlobalMouseState(out int x, out int y);

            newPosition = new Vector2(x, y);

            Vector2 mouseDelta = newPosition - oldPosition;

            if (mouseDelta.magnitude >= 50)
            {
                oldPosition = newPosition;
                return;
            }

            currentRotation.Y += -mouseDelta.x * rotationSpeed;
            currentRotation.X += -mouseDelta.y * rotationSpeed;

            //currentRotation.Y += -mouseDelta.x * rotationSpeed * Time.deltaTime;
            //currentRotation.X += -mouseDelta.y * rotationSpeed * Time.deltaTime;
            currentRotation.X = OpenTK.Mathematics.MathHelper.Clamp(currentRotation.X, OpenTK.Mathematics.MathHelper.DegreesToRadians(-90.0f), OpenTK.Mathematics.MathHelper.DegreesToRadians(90.0f));

            OpenTK.Mathematics.Quaternion rot = OpenTK.Mathematics.Quaternion.FromAxisAngle(OpenTK.Mathematics.Vector3.UnitY, currentRotation.Y) *
                                    OpenTK.Mathematics.Quaternion.FromAxisAngle(OpenTK.Mathematics.Vector3.UnitX, currentRotation.X);

            Quaternion rotation = new Quaternion(rot.X, rot.Y, rot.Z, rot.W);

            transform.rotation = rotation;

            oldPosition = newPosition;
        }
    }
}
