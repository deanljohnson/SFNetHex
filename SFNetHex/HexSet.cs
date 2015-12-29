using System;
using System.Collections.Generic;
using SFML.Graphics;
using SFML.System;

namespace SFNetHex
{
    public class HexSet : Transformable
    {
        private HashSet<Hex> m_HexSet { get; }
        protected Layout Layout { get; }

        private HexSet(Orientation o, Vector2f cellSize)
        {
            m_HexSet = new HashSet<Hex>();
            Layout = new Layout(o, cellSize, new Vector2f(0, 0));
        }

        public HexSet(int rad, Orientation o, Vector2f cellSize)
            : this(o, cellSize)
        {
            BuildHexMap(rad);
        }

        public HexSet(int x1, int x2, int y1, int y2, Orientation o, Vector2f cellSize) 
            : this(o, cellSize)
        {
            BuildParallelogramMap(x1, x2, y1, y2);
        }

        public HexSet(IEnumerable<Hex> hexes, Orientation o, Vector2f cellSize)
            : this(o, cellSize)
        {
            m_HexSet = new HashSet<Hex>(hexes);
        }

        public virtual bool Add(Hex h)
        {
            return m_HexSet.Add(h);
        }

        public virtual void UnionWith(IEnumerable<Hex> hexes)
        {
            m_HexSet.UnionWith(hexes);
        }

        public virtual void ExceptWith(IEnumerable<Hex> hexes)
        {
            m_HexSet.ExceptWith(hexes);
        }

        public virtual void Clear()
        {
            m_HexSet.Clear();
        }

        /// <summary>
        /// Returns the Position of the given Hex
        /// </summary>
        public Vector2f GetHexPosition(Hex h)
        {
            var pos = HexUtils.HexToPixel(h, Layout);
            return Transform.TransformPoint(pos);
        }

        /// <summary>
        /// Returns the nearest whole Hex coordinate that corresponds to the given Position.
        /// Will not necessarily be on the HexSet
        /// </summary>
        public Hex GetNearestWholeHex(Vector2f p)
        {
            p = InverseTransform.TransformPoint(p);
            return HexUtils.PixelToWholeHex(p, Layout);
        }

        /// <summary>
        /// Returns a List of all Hexes in the a line from start to end 
        /// that exist in this HexSet
        /// </summary>
        public HashSet<Hex> GetHexesInLine(Hex start, Hex end)
        {
            var results = Hex.GetHexesInLine(start, end);
            return TrimToInSet(results);
            
        }

        /// <summary>
        /// Returns a List of all Hexes within range of center that exist in this HexSet
        /// </summary>
        public HashSet<Hex> GetHexesInRange(Hex center, int range)
        {
            var results = Hex.GetHexesInRange(center, range);
            return TrimToInSet(results);
        }

        /// <summary>
        /// Returns a List of all Hexes that are at the given radius from the 
        /// given center that exist in this HexSet
        /// </summary>
        public HashSet<Hex> GetHexesInRing(Hex center, int radius)
        {
            var results = Hex.GetHexesInRing(center, radius);
            return TrimToInSet(results);
        }

        private HashSet<Hex> TrimToInSet(HashSet<Hex> hexes)
        {
            hexes.IntersectWith(m_HexSet);
            return hexes;
        }

        private void BuildHexMap(int rad)
        {
            for (var q = -rad; q <= rad; q++)
            {
                var r1 = Math.Max(-rad, -q - rad);
                var r2 = Math.Min(rad, -q + rad);
                for (var r = r1; r <= r2; r++)
                {
                    Add(new Hex(q, r));
                }
            }
        }

        private void BuildParallelogramMap(int q1, int q2, int r1, int r2)
        {
            for (var q = q1; q <= q2; q++)
            {
                for (var r = r1; r <= r2; r++)
                {
                    Add(new Hex(q, r));
                }
            }
        }
    }
}
