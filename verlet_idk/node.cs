
using System;
using System.Collections.Generic;
using System.Text;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace verlet_idk
{
    class Node
    {
        private Vector2 oldPos;
        private Vector2 curPos;
        private Vector2 newPos;

        public Single X
        {
            get
            {
                return curPos.X;
            }
        }
        public Single Y
        {
            get
            {
                return curPos.Y;
            }
        }

        public Vector2 position
        {
            get
            {
                return curPos;
            }
        }

        public Node(Vector2 pos, Vector2 vel, Single mass)
        {
            curPos = pos;
            oldPos = curPos - vel;

        }

        public void Update()
        {
            newPos = 2f * curPos - oldPos;
            oldPos = curPos;
            curPos = newPos;

            if (curPos.X < 0)
            {
                bounce(out curPos.X, out oldPos.X, 0);
            }
            else if (curPos.X > display.X)
            {
                bounce(out curPos.X, out oldPos.X, display.X);
            }

            if(curPos.Y < 0)
            {
                bounce(out curPos.Y, out oldPos.Y, 0);
            }
            else if (curPos.Y > display.Y)
            {
                bounce(out curPos.Y, out oldPos.Y, display.Y);
            }



        }

        public void bounce (out Single o, out Single n, Single bound)
        {
            o = 2f * bound - curPos.X;
            n = 2f * bound - oldPos.X;
        }

        public void push( Vector2 im)
        {
            curPos += im;
        }
    }
}
