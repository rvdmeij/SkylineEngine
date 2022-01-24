namespace SkylineEngine
{
    public class SmoothInput
    {
        public float min;
        public float max;
        public float increment;
        public float transitionTime = 1.0f;

        private float elapsedTime;
        private float lastDirection;

        public SmoothInput()
        {
            this.min = 0;
            this.max = 1;
            this.elapsedTime = 0;
            this.lastDirection = 0.0f;
        }

        public float GetValue(float inputAxisValue)
        {
            if (Mathf.Abs(inputAxisValue) <= float.Epsilon)
            {
                if (elapsedTime > 0)
                    elapsedTime -= Time.deltaTime;
                else
                {
                    elapsedTime = 0.0f;
                    lastDirection = 0.0f;
                }

                //elapsedTime = 0;

                return lastDirection * Mathf.Slerp(0.0f, 1.0f, elapsedTime / transitionTime);
            }
            else
            {
                lastDirection = inputAxisValue;

                elapsedTime += Time.deltaTime;

                if (elapsedTime >= transitionTime)
                    elapsedTime = transitionTime;

                return inputAxisValue * Mathf.Slerp(0.0f, 1.0f, elapsedTime / transitionTime);
            }
        }

        private void SetElapsedTime(float t) 
        { 
            elapsedTime = t; 
        }
    }
}