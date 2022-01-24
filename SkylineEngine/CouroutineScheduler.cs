using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


namespace SkylineEngine
{
    // Scheduler
    public static class CoroutineScheduler
    {
        private static List<Coroutine> coroutines = new List<Coroutine>();

        public static Coroutine StartCoroutine(IEnumerator routine)
        {
            Coroutine coroutine = new Coroutine(routine);
            coroutines.Add(coroutine);

            return coroutine;
        }

        public static void Update()
        {
            if(coroutines.Count == 0)
                return;

            foreach (Coroutine coroutine in coroutines.Reverse<Coroutine>())
            {
                if (coroutine.routine.Current is Coroutine)
                    coroutine.waitForCoroutine = coroutine.routine.Current as Coroutine;

                if (coroutine.waitForCoroutine != null && coroutine.waitForCoroutine.finished)
                    coroutine.waitForCoroutine = null;

                if (coroutine.waitForCoroutine != null)
                    continue;

                // update coroutine

                if (coroutine.routine.MoveNext())
                {
                    coroutine.finished = false;
                }
                else
                {
                    coroutines.Remove(coroutine);
                    coroutine.finished = true;
                }
            }
        }        
    }

    public class Coroutine
    {
        public IEnumerator routine;
        public Coroutine waitForCoroutine;
        public bool finished = false;
        public float seconds;

        public Coroutine(IEnumerator routine) 
        { 
            this.routine = routine; 
        }

        public Coroutine(float seconds)
        {
            this.seconds = seconds;
            this.routine = WaitAboutSeconds(seconds);
        }

        public IEnumerator WaitAboutSeconds(float seconds)
        {
            // dumb timer
            double timer = System.DateTime.Now.Second + seconds;
            while (System.DateTime.Now.Second <= timer)
            {
                // pass
                yield return null;
            }

            yield break;
        }
    }

    public class WaitForSeconds : Coroutine
    {        
        public WaitForSeconds(float seconds) : base(seconds)
        {
            CoroutineScheduler.StartCoroutine(routine);
        }

        public WaitForSeconds(IEnumerator routine) : base(routine)
        {
        }
    }
}