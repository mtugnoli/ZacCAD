using System;

namespace LitMath
{
    public struct Vector2b
    {
        public double x;
        public double y;
        public double b;
        public bool isvalid;

        public Vector2b(double x = 0.0, double y = 0.0, double b = 0.0, bool isvalid = true)
        {
            this.x = x;
            this.y = y;
            this.b = b;
            this.isvalid = isvalid;
        }

        public void Set(double newX, double newY, double newB)
        {
            this.x = newX;
            this.y = newY;
            this.b = newB;
        }

        public override string ToString()
        {
            return string.Format("{0:f3}, {1:f3}, {2:f3}", this.x, this.y, this.b);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Vector2b))
                return false;

            return Equals((Vector2b)obj);
        }

        public bool Equals(Vector2b rhs)
        {
            return (Utils.IsEqual(x, rhs.x) && Utils.IsEqual(y, rhs.y) && Utils.IsEqual(b, rhs.b));
        }

        public override int GetHashCode()
        {
            return x.GetHashCode() ^ y.GetHashCode() ^ b.GetHashCode();
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

        public Vector2b normalized
        {
            get
            {
                double length = this.length;
                if (length != 0.0)
                {
                    return new Vector2b(this.x / length, this.y / length);
                }
                return this;
            }
        }

        public static double Dot(Vector2b a, Vector2b b)
        {
            return a.x * b.x + a.y * b.y;
        }

        public static double Cross(Vector2b a, Vector2b b)
        {
            return ((a.x * b.y) - (a.y * b.x));
        }

        /// <summary>
        /// Obtains the angle of a line defined by two points.
        /// </summary>
        /// <param name="u">A Vector2b.</param>
        /// <param name="v">A Vector2b.</param>
        /// <returns>Angle in radians.</returns>
        public static double Angle(Vector2b u, Vector2b v)
        {
            Vector2b dir = v - u;
            return Angle(dir);
        }

        /// <summary>
        /// Obtains the angle of a vector.
        /// </summary>
        /// <param name="u">A Vector2b.</param>
        /// <returns>Angle in radians.</returns>
        public static double Angle(Vector2b u)
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
        public static double AngleInRadian(Vector2b a, Vector2b b)
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
        public static double SignedAngle(Vector2b from, Vector2b to)
        {
            return Utils.RadianToDegree(SignedAngleInRadian(from, to));
        }

        /// <summary>
        /// Returns the signed acute clockwise angle in radians between from and to.
        /// The result value range: [-PI, PI]
        /// </summary>
        public static double SignedAngleInRadian(Vector2b from, Vector2b to)
        {
            double rad = AngleInRadian(from, to);
            if (Cross(from, to) < 0)
            {
                rad = -rad;
            }
            return rad;
        }

        public static double Distance(Vector2b a, Vector2b b)
        {
            Vector2b vector = b - a;
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
        public static bool Clockwise(Vector2b p1, Vector2b p2, Vector2b p3)
        {
            return ((p2.x - p1.x) * (p3.y - p1.y) - (p2.y - p1.y) * (p3.x - p1.x)) < 1e-8;
        }

        public static Vector2b Polar(Vector2b u, double distance, double angle)
        {
            Vector2b dir = new Vector2b(Math.Cos(angle), Math.Sin(angle));
            return u + dir * distance;
        }

        public static Vector2b Rotate(Vector2b v, double angle)
        {
            return RotateInRadian(v, Utils.DegreeToRadian(angle));
        }

        public static Vector2b Rotate(Vector2b point, Vector2 basePoint, double angle)
        {
            return RotateInRadian(point, basePoint, Utils.DegreeToRadian(angle));
        }

        public static Vector2b RotateInRadian(Vector2b v, double rad)
        {
            double x = v.x * Math.Cos(rad) - v.y * Math.Sin(rad);
            double y = v.x * Math.Sin(rad) + v.y * Math.Cos(rad);
            return new Vector2b(x, y);
        }

        public static Vector2b StringToVector(string text)
        {
            bool isvalid = true;

            if (!string.IsNullOrEmpty(text) && text.Contains(","))
            {
                string[] arr = text.Split(',');

                double x = 0;
                double y = 0;
                double b = 0;
                isvalid = double.TryParse(arr[0].Replace(".", ","), out x);

                if (isvalid)
                    isvalid = double.TryParse(arr[1].Replace(".", ","), out y);

                if (isvalid)
                    isvalid = double.TryParse(arr[2].Replace(".", ","), out b);


                return new Vector2b(x, y, b, isvalid);
            }
            else
            {
                return new Vector2b(0, 0, 0, false);
            }
        }



        /// <summary>
        /// Rotates one point around another TM
        /// </summary>
        /// <param name="pointToRotate">The point to rotate.</param>
        /// <param name="centerPoint">The center point of rotation.</param>
        /// <param name="angleInDegrees">The rotation angle in degrees.</param>
        /// <returns>Rotated point</returns>
        public static Vector2b RotateInRadian(Vector2b pointToRotate, Vector2 centerPoint, double angleInRadians)
        {
            double cosTheta = Math.Cos(angleInRadians);
            double sinTheta = Math.Sin(angleInRadians);
            return new Vector2b
            {
                x = (cosTheta * (pointToRotate.x - centerPoint.x) - sinTheta * (pointToRotate.y - centerPoint.y) + centerPoint.x),
                y = (sinTheta * (pointToRotate.x - centerPoint.x) + cosTheta * (pointToRotate.y - centerPoint.y) + centerPoint.y)
            };
        }


        public static Vector2b PointOrthoMode(Vector2b last, Vector2b point, bool ortho)
        {
            if (ortho)
            {
                if (Math.Abs(point.x - last.x) > Math.Abs(point.y - last.y))
                    return new LitMath.Vector2b(point.x, last.y);
                else
                    return new LitMath.Vector2b(last.x, point.y);
            }
            else
            {
                return point;
            }


        }

        public static Vector2b Zero
        {
            get { return new Vector2b(0.0, 0.0); }
        }

        public static Vector2b operator +(Vector2b a, Vector2b b)
        {
            return new Vector2b(a.x + b.x, a.y + b.y);
        }

        public static Vector2b operator -(Vector2b a, Vector2b b)
        {
            return new Vector2b(a.x - b.x, a.y - b.y);
        }

        public static Vector2b operator -(Vector2b a)
        {
            return new Vector2b(-a.x, -a.y);
        }

        public static Vector2b operator *(Vector2b a, double d)
        {
            return new Vector2b(a.x * d, a.y * d);
        }

        public static Vector2b operator *(double d, Vector2b a)
        {
            return new Vector2b(a.x * d, a.y * d);
        }

        public static Vector2b operator /(Vector2b a, double d)
        {
            return new Vector2b(a.x / d, a.y / d);
        }

        public static bool operator ==(Vector2b lhs, Vector2b rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(Vector2b lhs, Vector2b rhs)
        {
            return !(lhs == rhs);
        }
    }
}
