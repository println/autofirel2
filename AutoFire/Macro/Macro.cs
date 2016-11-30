using System;
using System.Collections.Generic;
using System.Threading;

namespace AutoFire
{
    class Macro
    {
        private Observer observer;
        private KeySender keySender;
        private bool running;
        private List<Thread> threads = new List<Thread>();
        private DataProfile profile;

        public delegate void KeyEventHandler(object key);
        public event KeyEventHandler KeyEvent;

        public Macro(DataProfile p, bool global)
        {
            profile = p;

            keySender = KeySender.Instance;

            observer = Observer.Instance;
            observer.Add(this);
            observer.Global(global);

            running = false;
        }

        public object GetKey()
        {
            return profile.activation;
        }

        public void Notify()
        {
            if (running)
                Abort();
            else
                Run();
        }

        public bool Destroy()
        {
            Abort();
            return observer.Remove(this);
        }

        private void Run()
        {
            running = true;

            for (int i = 0; i < profile.values.Length; i++)
                CreateThread(i);

            profile.Active();

            running = profile.loop;
        }

        private void Abort()
        {
            foreach (Thread thread in threads)
                thread.Abort();

            threads.Clear();

            profile.Deactive();

            running = false;
        }

        private void CreateThread(int index)
        {
            Thread thread = new Thread(() => Sender(profile.keys[index], profile.values[index]));
            threads.Add(thread);
            thread.Start();
        }

        private void Sender(object key, int time)
        {            
            do
            {
                Thread.Sleep(time);
                keySender.SendKey(key);

            } while (profile.loop);

            threads.Remove(Thread.CurrentThread);

            if (threads.Count == 0)
            {
                Abort();
            }
        }

        ~Macro()
        {
            Abort();
        }
    }
}
