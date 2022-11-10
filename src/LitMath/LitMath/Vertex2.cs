using System;

namespace LitMath
{
    /// <summary>
    /// Represents a <see cref="Polyline2D">Polyline2D</see> vertex.
    /// </summary>
    public class Vertex2 : ICloneable
    {
        #region private fields

        private Vector2 position;
        private double bulge;

        #endregion

        #region constructors

        /// <summary>
        /// Initializes a new instance of the <c>Polyline2DVertex</c> class.
        /// </summary>
        public Vertex2() : this(Vector2.Zero)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <c>Polyline2DVertex</c> class.
        /// </summary>
        /// <param name="x">X coordinate.</param>
        /// <param name="y">Y coordinate.</param>
        public Vertex2(double x, double y) : this(new Vector2(x, y), 0.0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <c>Polyline2DVertex</c> class.
        /// </summary>
        /// <param name="x">X coordinate.</param>
        /// <param name="y">Y coordinate.</param>
        /// <param name="bulge">Vertex bulge  (default: 0.0).</param>
        public Vertex2(double x, double y, double bulge) : this(new Vector2(x, y), bulge)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <c>Polyline2DVertex</c> class.
        /// </summary>
        /// <param name="position">Lightweight polyline <see cref="Vector2">vertex</see> coordinates.</param>
        public Vertex2(Vector2 position) : this(position, 0.0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <c>Polyline2DVertex</c> class.
        /// </summary>
        /// <param name="position">Lightweight polyline <see cref="Vector2">vertex</see> coordinates.</param>
        /// <param name="bulge">Vertex bulge  (default: 0.0).</param>
        public Vertex2(Vector2 position, double bulge)
        {
            this.position = position;
            this.bulge = bulge;
        }

        /// <summary>
        /// Copy constructor.
        /// </summary>
        /// <param name="vertex">A Polyline2D vertex.</param>
        public Vertex2(Vertex2 vertex)
        {
            this.position = vertex.Position;
            this.bulge = vertex.Bulge;
        }

        #endregion

        #region public properties

        /// <summary>
        /// Gets or sets the light weight polyline vertex <see cref="Vector2">position</see>.
        /// </summary>
        public Vector2 Position
        {
            get { return this.position; }
            set { this.position = value; }
        }

        /// <summary>
        /// Gets or set the light weight polyline bulge.
        /// </summary>
        /// <remarks>
        /// The bulge is the tangent of one fourth the included angle for an arc segment, 
        /// made negative if the arc goes clockwise from the start point to the endpoint. 
        /// A bulge of 0 indicates a straight segment, and a bulge of 1 is a semicircle.
        /// </remarks>
        public double Bulge
        {
            get { return this.bulge; }
            set { this.bulge = value; }
        }

        #endregion

        #region overrides

        /// <summary>
        /// Converts the value of this instance to its equivalent string representation.
        /// </summary>
        /// <returns>The string representation.</returns>
        public override string ToString()
        {
            return string.Format("{0}: ({1})", "Vertex2", this.position);
        }

        /// <summary>
        /// Creates a new Polyline2DVertex that is a copy of the current instance.
        /// </summary>
        /// <returns>A new Polyline2DVertex that is a copy of this instance.</returns>
        public object Clone()
        {
            return new Vertex2(this);
        }

        #endregion
    }
}
