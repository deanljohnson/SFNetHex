using System;
using System.Collections.Generic;
using SFML.Graphics;
using SFML.System;

namespace SFNetHex
{
    public class HexMap : Transformable
    {
        protected Layout Layout { get; }
        protected HashSet<Hex> HexSet { get; set; }

        private HexMap(Orientation o, Vector2f cellSize)
        {
            HexSet = new HashSet<Hex>();
            Layout = new Layout(o, cellSize, new Vector2f(0, 0));
        }

        public HexMap(int rad, Orientation o, Vector2f cellSize)
            : this(o, cellSize)
        {
            BuildHexMap(rad);
        }

        public HexMap(int x1, int x2, int y1, int y2, Orientation o, Vector2f cellSize) 
            : this(o, cellSize)
        {
            BuildParallelogramMap(x1, x2, y1, y2);
        }

        public HexMap(IEnumerable<Hex> hexes, Orientation o, Vector2f cellSize)
            : this(o, cellSize)
        {
            HexSet = new HashSet<Hex>(hexes);
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
        /// Will not necessarily be on the HexMap
        /// </summary>
        public Hex GetNearestWholeHex(Vector2f p)
        {
            p = InverseTransform.TransformPoint(p);
            return HexUtils.PixelToWholeHex(p, Layout);
        }

        /// <summary>
        /// Returns a List of all Hexes in the a line from start to end 
        /// that exist in this HexMap
        /// </summary>
        public HashSet<Hex> GetHexesInLine(Hex start, Hex end)
        {
            var results = Hex.GetHexesInLine(start, end);
            return TrimToOnMap(results);
            
        }

        /// <summary>
        /// Returns a List of all Hexes within range of center that exist in this HexMap
        /// </summary>
        public HashSet<Hex> GetHexesInRange(Hex center, int range)
        {
            var results = Hex.GetHexesInRange(center, range);
            return TrimToOnMap(results);
        }

        /// <summary>
        /// Returns a List of all Hexes that are at the given radius from the 
        /// given center that exist in this HexMap
        /// </summary>
        public HashSet<Hex> GetHexesInRing(Hex center, int radius)
        {
            var results = Hex.GetHexesInRing(center, radius);
            return TrimToOnMap(results);
        }

        // Exposed as a virtual method so that inheritors can know when a new Hex is added
        public virtual bool Add(Hex h, bool throwOnContains = true)
        {
            if (HexSet.Contains(h))
            {
                if (throwOnContains)
                {
                    throw new ArgumentException($"{h} already exists in the HexMap");
                }
                return false;
            }

            HexSet.Add(h);
            OnAdd(h);
            return true;
        }

        public virtual void Add(IEnumerable<Hex> hexes, bool throwOnContains = true)
        {
            foreach (var hex in hexes)
            {
                Add(hex, throwOnContains);
            }
        }

        /// <summary>
        /// Override this method to be notified anytime a Hex is added to the HexMap
        /// </summary>
        protected virtual void OnAdd(Hex h)
        {
        }

        private HashSet<Hex> TrimToOnMap(HashSet<Hex> hexes)
        {
            hexes.IntersectWith(HexSet);
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
