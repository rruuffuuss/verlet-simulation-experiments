using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using System.Linq;

namespace verlet_idk
{
    class Body
    {
        private Node[] _nodes;
        private Arc[] _arcs;

        public Texture2D nodeTexture { get; set; }
        public Texture2D arcTexture { get; set; }
        public Body(int segNo, int[][] segStructure, Single[] segRad, Vector2 centre, Vector2 vel)
        {

            //setup nodes
            var nodesPerSeg = segRad.Length;
            _nodes = new Node[segNo * nodesPerSeg + 1];
            _nodes[_nodes.Length - 1] = new Node(centre, vel, 0);
            for (int i = 0; i < segNo; i++)
            {
                for (int l = 0; l < nodesPerSeg; l++)
                {
                    var angle = (2 * MathF.PI / segNo) * i;
                    var x = segRad[l] * MathF.Cos(angle);
                    var y = segRad[l] * MathF.Sin(angle);
                    _nodes[i * nodesPerSeg + l] = new Node(centre + new Vector2(x, y), vel, 0);
                }
            }


            //setup arcs between outer nodes arcs
            var circArcs = new Arc[segNo * lengthofJagged(segStructure)];
            var arcNo = 0;
            for (int i = 0; i < segNo - 1; i++)
            {

                for (int l = 0; l < segStructure.Length; l++)
                {
                    for (int o = 0; o < segStructure[l].Length; o++)
                    {
                        var n1 = i * nodesPerSeg + segStructure[l][o];
                        var n2 = (i + 1) * nodesPerSeg + l;
                        circArcs[arcNo] = new Arc(ref _nodes[n1], ref _nodes[n2]);
                        arcNo++;
                    }
                }
            }
            for (int l = 0; l < segStructure.Length; l++)
            {
                for (int o = 0; o < segStructure[l].Length; o++)
                {
                    var n1 = segStructure[l][o];
                    var n2 = (segNo - 1) * nodesPerSeg + l;
                    circArcs[arcNo] = new Arc(ref _nodes[n1], ref _nodes[n2]);
                    arcNo++;
                }
            }

            //setup radial nodes

            //between all nodes
            var radArcs = new Arc[segNo * nodesPerSeg];
            for (int i = 0; i < segNo; i++)
            {
                radArcs[i * nodesPerSeg] = new Arc(ref _nodes[_nodes.Length - 1], ref _nodes[nodesPerSeg * i]);
                for (int l = 1; l < nodesPerSeg; l++) {
                    radArcs[i * nodesPerSeg + l] = new Arc(ref _nodes[i * nodesPerSeg + l], ref _nodes[i * nodesPerSeg + l + 1]);
                }
            }

            //between radial nodes and centre
            var centralArcs = new Arc[_nodes.Length - 1];
            for(int i = 0; i < _nodes.Length - 1; i++)
            {
                centralArcs[i] = new Arc(ref _nodes[i], ref _nodes[_nodes.Length - 1]);
            }


            _arcs = new Arc[radArcs.Length + circArcs.Length + centralArcs.Length];
            radArcs.CopyTo(_arcs, 0);
            circArcs.CopyTo(_arcs, radArcs.Length);
            centralArcs.CopyTo(_arcs, radArcs.Length + circArcs.Length);
            
        }

    
        private int lengthofJagged(int[][] jagged)
        {
            var total = 0;
            foreach(int[] o_list in jagged)
            {
                total += o_list.Length;
            }
            return total;
        }

        public void Update()
        {
            foreach(Node n in _nodes)
            {
                n.Update();
            }

            foreach(Arc a in _arcs)
            {
                a.Update();
            }
        }

        public void Draw(SpriteBatch _spriteBatch)
        {
            foreach(Node n in _nodes)
            {
                _spriteBatch.Draw(nodeTexture, getNodeRectangle(n),Color.White);
            }


            if (_arcs.Length > 0)
            {

                foreach (Arc a in _arcs)
                {
                    //object reference not set to an instance of an object
                    DrawLine(_spriteBatch, a.n1.position, a.n2.position);
                }
            }
            
        }

        private Rectangle getNodeRectangle(Node n)
        {
            return new Rectangle(Convert.ToInt16(n.X - arcTexture.Width / 2), Convert.ToInt16(n.Y - arcTexture.Height / 2), nodeTexture.Width, nodeTexture.Height);
        }

        private void DrawLine(SpriteBatch spriteBatch, Vector2 begin, Vector2 end, int width = 1)
        {
            Rectangle r = new Rectangle((int)begin.X, (int)begin.Y, (int)(end - begin).Length() + width, width);
            Vector2 v = Vector2.Normalize(begin - end);
            float angle = (float)Math.Acos(Vector2.Dot(v, -Vector2.UnitX));
            if (begin.Y > end.Y) angle = MathHelper.TwoPi - angle;
            spriteBatch.Draw(arcTexture, r, null, Color.White, angle, Vector2.Zero, SpriteEffects.None, 0);
        }
    }
}
