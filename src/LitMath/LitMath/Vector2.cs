using System;

namespace LitMath
{
    public struct Vector2
    {
        public double x;
        public double y;
        public bool isvalid;

        public Vector2(double x = 0.0, double y = 0.0, bool isvalid = true)
        {
            this.x = x;
            this.y = y;
            this.isvalid = isvalid;
        }

        public void Set(double newX, double newY)
        {
            this.x = newX;
            this.y = newY;
        }

        public override string ToString()
        {
            // return string.Format("Vector2({0}, {1})", this.x, this.y);
            return string.Format("{0:f3}, {1:f3}", this.x, this.y);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Vector2))
                return false;

            return Equals((Vector2)obj);
        }

        public bool Equals(Vector2 rhs)
        {
            return (Utils.IsEqual(x, rhs.x) && Utils.IsEqual(y, rhs.y));
        }

        public override int GetHashCode()
        {
            return x.GetHashCode() ^ y.GetHashCode();
        }

        public double length
        {
            get
            {
                return Math.Sqrt((this.x * this.x) + (this.y * this.y));
            }
        }

        public double lengthSqrd
        {
            get
            {
                return ((this.x * this.x) + (this.y * this.y));
            }
        }

        public void Normalize()
        {
            double length = this.length;
            if (length != 0.0)
            {
                this.x /= length;
                this.y /= length;
            }
        }

        public Vector2 normalized
        {
            get
            {
                double length = this.length;
                if (length != 0.0)
                {
                    return new Vector2(this.x / length, this.y / length);
                }
                return this;
            }
        }

        public static double Dot(Vector2 a, Vector2 b)
        {
            return a.x * b.x + a.y * b.y;
        }

        public static double Cross(Vector2 a, Vector2 b)
        {
            return ((a.x * b.y) - (a.y * b.x));
        }

        ///// <summary>
        ///// Returns the unsigned angle in degrees between a and b.
        ///// The smaller of the two possible angles between the two vectors is used.
        ///// The result value range: [0, 180]
        ///// </summary>
        //public static double Angle(Vector2 a, Vector2 b)
        //{
        //    return Utils.RadianToDegree(AngleInRadian(a, b));
        //}


        /// <summary>
        /// Obtains the angle of a line defined by two points.
        /// </summary>
        /// <param name="u">A Vector2.</param>
        /// <param name="v">A Vector2.</param>
        /// <returns>Angle in radians.</returns>
        public static double Angle(Vector2 u, Vector2 v)
        {
            Vector2 dir = v - u;
            return Angle(dir);
        }

        /// <summary>
        /// Obtains the angle of a vector.
        /// </summary>
        /// <param name="u">A Vector2.</param>
        /// <returns>Angle in radians.</returns>
        public static double Angle(Vector2 u)
        {
            double angle = Math.Atan2(u.y, u.x);
            if (angle < 0)
            {
                return Utils.TwoPI + angle;
            }

            return angle;
        }


        /// <summary>
        /// Returns the unsigned angle in radians between a and b.
        /// The smaller of the two possible angles between the two vectors is used.
        /// The result value range: [0, PI]
        /// </summary>
        public static double AngleInRadian(Vector2 a, Vector2 b)
        {
            double num = a.length * b.length;
            if (num == 0.0)
            {
                return 0.0;
            }
            double num2 = Dot(a, b) / num;
            return Math.Acos(Utils.Clamp(num2, -1.0, 1.0));
        }

        /// <summary>
        /// Returns the signed acute clockwise angle in degrees between from and to.
        /// The result value range: [-180, 180]
        /// </summary>
        public static double SignedAngle(Vector2 from, Vector2 to)
        {
            return Utils.RadianToDegree(SignedAngleInRadian(from, to));
        }

        /// <summary>
        /// Returns the signed acute clockwise angle in radians between from and to.
        /// The result value range: [-PI, PI]
        /// </summary>
        public static double SignedAngleInRadian(Vector2 from, Vector2 to)
        {
            double rad = AngleInRadian(from, to);
            if (Cross(from, to) < 0)
            {
                rad = -rad;
            }
            return rad;
        }

        public static double Distance(Vector2 a, Vector2 b)
        {
            Vector2 vector = b - a;
            return vector.length;
        }

        public static double AnglesDifference(double firstAngle, double secondAngle)
        {
            double difference = secondAngle - firstAngle;
            while (difference < -Utils.PI) difference += Utils.TwoPI;
            while (difference > Utils.PI) difference -= Utils.TwoPI;

            return difference;
        }


        // Evaluates if the points are clockwise.
        public static bool Clockwise(Vector2 p1, Vector2 p2, Vector2 p3)
        {
            return ((p2.x - p1.x) * (p3.y - p1.y) - (p2.y - p1.y) * (p3.x - p1.x)) < 1e-8;
        }

        public static Vector2 Polar(Vector2 u, double distance, double angle)
        {
            Vector2 dir = new Vector2(Math.Cos(angle), Math.Sin(angle));
            return u + dir * distance;
        }

        public static Vector2 Rotate(Vector2 v, double angle)
        {
            return RotateInRadian(v, Utils.DegreeToRadian(angle));
        }

        public static Vector2 Rotate(Vector2 point, Vector2 basePoint, double angle)
        {
            return RotateInRadian(point, basePoint, Utils.DegreeToRadian(angle));
        }

        public static Vector2 RotateInRadian(Vector2 v, double rad)
        {
            double x = v.x * Math.Cos(rad) - v.y * Math.Sin(rad);
            double y = v.x * Math.Sin(rad) + v.y * Math.Cos(rad);
            return new Vector2(x, y);
        }

        public static Vector2 StringToVector(string text)
        {
            bool isvalid = true;

            if (!string.IsNullOrEmpty(text) && text.Contains(","))
            {
                string[] arr = text.Split(',');

                double x = 0;
                double y = 0;
                isvalid = double.TryParse(arr[0].Replace(".",","), out x);

                if(isvalid)
                    isvalid = double.TryParse(arr[1].Replace(".", ","), out y);

                return new Vector2(x, y, isvalid);
            }
            else
            {
                return new Vector2(0, 0, false);
            }
        }

        //public static Vector2 RotateInRadian(Vector2 point, Vector2 basePoint, double rad)
        //{
        //    double cos = Math.Cos(rad);
        //    double sin = Math.Sin(rad);
        //    double x = point.x * cos - point.y * sin + basePoint.x * (1 - cos) + basePoint.y * sin;
        //    double y = point.x * sin + point.y * cos + basePoint.y * (1 - cos) + basePoint.x * sin;

        //    return new Vector2(x, y);
        //}


        /// <summary>
        /// Rotates one point around another TM
        /// </summary>
        /// <param name="pointToRotate">The point to rotate.</param>
        /// <param name="centerPoint">The center point of rotation.</param>
        /// <param name="angleInDegrees">The rotation angle in degrees.</param>
        /// <returns>Rotated point</returns>
        public static Vector2 RotateInRadian(Vector2 pointToRotate, Vector2 centerPoint, double angleInRadians)
        {
            double cosTheta = Math.Cos(angleInRadians);
            double sinTheta = Math.Sin(angleInRadians);
            return new Vector2
            {
                x = (cosTheta * (pointToRotate.x - centerPoint.x) - sinTheta * (pointToRotate.y - centerPoint.y) + centerPoint.x),
                y = (sinTheta * (pointToRotate.x - centerPoint.x) + cosTheta * (pointToRotate.y - centerPoint.y) + centerPoint.y)
            };
        }

        public static Vector2 PointOrthoMode(Vector2 last, Vector2 point, bool ortho)
        {
            if (ortho)
            {
                if (Math.Abs(point.x - last.x) > Math.Abs(point.y - last.y))
                    return new LitMath.Vector2(point.x, last.y);
                else
                    return new LitMath.Vector2(last.x, point.y);
            }
            else
            {
                return point;
            }


        }

        public static Vector2 Zero
        {
            get { return new Vector2(0.0, 0.0); }
        }

        public static Vector2 operator +(Vector2 a, Vector2 b)
        {
            return new Vector2(a.x + b.x, a.y + b.y);
        }

        public static Vector2 operator -(Vector2 a, Vector2 b)
        {
            return new Vector2(a.x - b.x, a.y - b.y);
        }

        public static Vector2 operator -(Vector2 a)
        {
            return new Vector2(-a.x, -a.y);
        }

        public static Vector2 operator *(Vector2 a, double d)
        {
            return new Vector2(a.x * d, a.y * d);
        }

        public static Vector2 operator *(double d, Vector2 a)
        {
            return new Vector2(a.x * d, a.y * d);
        }

        public static Vector2 operator /(Vector2 a, double d)
        {
            return new Vector2(a.x / d, a.y / d);
        }

        public static bool operator ==(Vector2 lhs, Vector2 rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(Vector2 lhs, Vector2 rhs)
        {
            return !(lhs == rhs);
        }
    }
}
