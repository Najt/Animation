// <copyright file="Bezier.cs" company="GamerCats Studios">
//     GamerCats Studios All rights reserved
// </copyright>
// <author>Duna Gergely Endre</author>
namespace Game1
{
    using Microsoft.Xna.Framework;
    using System;

    /// <summary>
    /// Cubic Bezier Curve
    /// </summary>
    public class Bezier
    {
        #region Fields

        /// <summary>
        /// Bezier solve variable
        /// </summary>
        private double a, b, c, d, t;

        /// <summary>
        /// cubic solve temp variable
        /// </summary>
        private double f, g, h, i, k, m, n, r, ct, v, s;

        /// <summary>
        /// P0 Point for Bezier Curve
        /// </summary>
        private Vector2 p0;

        /// <summary>
        /// P1 Point for Bezier Curve
        /// </summary>
        private Vector2 p1;

        /// <summary>
        /// cubic solve variable
        /// </summary>
        private double solveA, solveB, solveC, solveD;

        /// <summary>
        /// P0 Vector2 for Bezier Curve
        /// </summary>
        private Vector2 vectorP0;

        /// <summary>
        /// P1 Vector2 for Bezier Curve
        /// </summary>
        private Vector2 vectorP1;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Bezier"/> class.
        /// </summary>
        /// <param name="p0">P0 point</param>
        /// <param name="vectorP0">P0 vector</param>
        /// <param name="vectorP1">P1 vector</param>
        /// <param name="p1">P1 point</param>
        public Bezier(Vector2 p0, Vector2 vectorP0, Vector2 vectorP1, Vector2 p1)
        {
            this.p0 = p0;
            this.vectorP0 = vectorP0;
            this.vectorP1 = vectorP1;
            this.p1 = p1;
            this.a = this.p0.X;
            this.b = this.p0.X + this.vectorP0.X;
            this.c = this.p1.X + this.vectorP1.X;
            this.d = this.p1.X;
            this.solveA = -this.a + (3 * this.b) - (3 * this.c) + this.d;
            this.solveB = (3 * this.a) - (6 * this.b) + (3 * this.c);
            this.solveC = (-3 * this.a) + (3 * this.b);
            this.solveD = this.a;
        }

        public Bezier(Vector2 p0, Vector2 p1)
        {
            this.p0 = p0;
            this.vectorP0 = new Vector2(0, 0);
            this.vectorP1 = new Vector2(0, 0);
            this.p1 = p1;
            this.a = this.p0.X;
            this.b = this.p0.X + this.vectorP0.X;
            this.c = this.p1.X + this.vectorP1.X;
            this.d = this.p1.X;
            this.solveA = -this.a + (3 * this.b) - (3 * this.c) + this.d;
            this.solveB = (3 * this.a) - (6 * this.b) + (3 * this.c);
            this.solveC = (-3 * this.a) + (3 * this.b);
            this.solveD = this.a;
        }

        public Bezier(Vector2 p0, float p0VectorX, float p1VectorX, Vector2 p1)
        {
            this.p0 = p0;
            this.vectorP0 = new Vector2(p0VectorX, 0);
            this.vectorP1 = new Vector2(p1VectorX, 0);
            this.p1 = p1;
            this.a = this.p0.X;
            this.b = this.p0.X + this.vectorP0.X;
            this.c = this.p1.X + this.vectorP1.X;
            this.d = this.p1.X;
            this.solveA = -this.a + (3 * this.b) - (3 * this.c) + this.d;
            this.solveB = (3 * this.a) - (6 * this.b) + (3 * this.c);
            this.solveC = (-3 * this.a) + (3 * this.b);
            this.solveD = this.a;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or sets the P0 point
        /// </summary>
        public Vector2 P0
        {
            get
            {
                return this.p0;
            }
        }

        /// <summary>
        /// Gets or sets the P1 point
        /// </summary>
        public Vector2 P1
        {
            get
            {
                return this.p1;
            }
        }

        /// <summary>
        /// Gets or sets the P0 Vector2
        /// </summary>
        public Vector2 VectorP0
        {
            get
            {
                return this.vectorP0;
            }
        }

        /// <summary>
        /// Gets or sets the P1 Vector2
        /// </summary>
        public Vector2 VectorP1
        {
            get
            {
                return this.vectorP1;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Gets the curve Y value at specific X
        /// </summary>
        /// <param name="x">The X value where we search the Y</param>
        /// <returns>Y value at X</returns>
        public double GetYFromX(double x)
        {
            if (this.solveA == 0 && this.solveB == 0)
            {
                this.t = -(this.solveD - x) / this.solveC;
            }
            else
            {
                this.t = this.CubicSolve(this.solveA, this.solveB, this.solveC, this.solveD - x);
            }

            return
                ((1 - this.t) * (1 - this.t) * (1 - this.t) * this.p0.Y) +
                (3 * (1 - this.t) * (1 - this.t) * this.t * (this.p0.Y + this.vectorP0.Y)) +
                (3 * (1 - this.t) * this.t * this.t * (this.p1.Y + this.vectorP1.Y)) +
                (this.t * this.t * this.t * this.p1.Y);
        }

        /// <summary>
        /// Cubic root finder
        /// </summary>
        /// <param name="a">a value where a*x^3+b*x^2+c*x+d</param>
        /// <param name="b">b value where a*x^3+b*x^2+c*x+d</param>
        /// <param name="c">c value where a*x^3+b*x^2+c*x+d</param>
        /// <param name="d">d value where a*x^3+b*x^2+c*x+d</param>
        /// <returns>Real Root of a*x^3+b*x^2+c*x+d</returns>
        private double CubicSolve(double a, double b, double c, double d)
        {
            this.f = (((3 * c) / a) - ((b * b) / (a * a))) / 3;
            this.g = (((2 * Math.Pow(b, 3)) / Math.Pow(a, 3)) - ((9 * b * c) / Math.Pow(a, 2)) + ((27 * d) / a)) / 27;
            this.h = (Math.Pow(this.g, 2) / 4) + (Math.Pow(this.f, 3) / 27);
            if (this.h == 0 && this.f == 0 && this.g == 0)
            {
                return -this.Root(d / a, 3.0);
            }

            if (this.h <= 0)
            {
                this.i = Math.Sqrt((Math.Pow(this.g, 2) / 4) - this.h);
                this.k = Math.Acos(-(this.g / (2 * this.i)));
                this.i = this.Root(this.i, 3);
                this.m = Math.Cos(this.k / 3);
                this.n = Math.Sqrt(3) * Math.Sin(this.k / 3);
                return (-this.i * (this.m - this.n)) - (b / (3 * a));
            }
            else
            {
                this.r = (-this.g / 2) + Math.Sqrt(this.h);
                this.ct = (-this.g / 2) - Math.Sqrt(this.h);
                this.v = this.Root(this.ct, 3.0);
                this.s = this.Root(this.r, 3.0);
                if (this.d == 0)
                {
                    return 0;
                }

                if (this.r >= 0)
                {
                    return (this.s + this.v) - (b / (3 * a));
                }
                else
                {
                    return (-this.s + this.v) - (b / (3 * a));
                }
            }
        }

        /// <summary>
        /// Root n
        /// </summary>
        /// <param name="x">The number</param>
        /// <param name="root">At root</param>
        /// <returns>x at root n</returns>
        private double Root(double x, double root)
        {
            if (x < 0 && root % 2 == 1)
            {
                return -Math.Pow(-x, 1.00 / root);
            }
            else
            {
                return Math.Pow(x, 1.00 / root);
            }
        }

        #endregion Methods
    }
}