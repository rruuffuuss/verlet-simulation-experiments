using System;
using System.Collections.Generic;
using System.Text;

using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace verlet_idk
{
    class Arc
    {
        private Single natrualLength; 
        private Single recoil; //if recoil = 0.5, arc will push a Node halfway back to correct position


        public Node n1 { get; private set; }
        public Node n2 { get; private set; }

    public Arc(ref Node Node1, ref Node Node2, Single r = 0.01f)
        {
            n1 = Node1;
            n2 = Node2;
            natrualLength = getDisplacement(n1,n2);
            recoil = r;
        }

        public void Update()
        {
            var dif = new Vector2(n1.X - n2.X, n1.Y - n2.Y);
     
            var diffScale = recoil * (dif.Length() - natrualLength)/ ( 2f * dif.Length());
   
            n1.push( dif * -diffScale);
            n2.push( dif * diffScale);
        }

        private Single getDisplacement(Node Node1, Node Node2)
        {
           return new Vector2(Node1.X - Node2.X, Node1.Y - Node2.Y).Length();
        }
    }
}
