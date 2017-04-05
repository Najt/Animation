// <copyright file="Animation.cs" company="GamerCats Studios">
//     GamerCats Studios All rights reserved
// </copyright>
// <author>Duna Gergely Endre/Najt</author>
namespace Game1
{
    using Microsoft.Xna.Framework;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;

    /// <summary>
    /// Possible states of the animation
    /// </summary>
    public enum AnimStates
    {
        /// <summary>
        /// The Animation is Idle
        /// </summary>
        Waiting,

        /// <summary>
        /// The Animation is Running
        /// </summary>
        Running,

        /// <summary>
        /// The Animation is Ended
        /// </summary>
        Ended,

        /// <summary>
        /// The Animation is Paused
        /// </summary>
        Paused
    }

    /// <summary>
    /// Animation with Bezier Curves
    /// </summary>
    public class Animation
    {
        #region Fields

        /// <summary>
        /// Index for the <see cref="beziers"/>
        /// </summary>
        private int beizerIndex = 0;

        /// <summary>
        /// Array of the bezier curves
        /// </summary>
        private List<Bezier> beziers;

        /// <summary>
        /// Elapsed time in the animation in milliseconds
        /// </summary>
        private float elapsed = 0;

        /// <summary>
        /// state of the Animation
        /// </summary>
        private AnimStates state = AnimStates.Waiting;

        /// <summary>
        /// Current value of the animation
        /// </summary>
        private float value = 0;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Animation"/> class.
        /// </summary>
        /// <param name="animFilePath">File path for the animation file</param>
        public Animation(string animFilePath)
        {
            try
            {
                if (!animFilePath.StartsWith("Anims\\"))
                {
                    animFilePath = "Anims\\" + animFilePath;
                }

                StreamReader reader = new StreamReader(animFilePath);
                List<Bezier> a = new List<Bezier>();
                CultureInfo info = new CultureInfo("en");
                while (!reader.EndOfStream)
                {
                    string[] line = reader.ReadLine().Split(' ');
                    a.Add(new Bezier(new Vector2(float.Parse(line[0], info), float.Parse(line[1], info)), new Vector2(float.Parse(line[2], info), float.Parse(line[3], info)), new Vector2(float.Parse(line[4], info), float.Parse(line[5], info)), new Vector2(float.Parse(line[6], info), float.Parse(line[7], info))));
                }

                this.beziers = a;
                this.value = (float)this.beziers[0].GetYFromX(0);
            }
            catch (FileNotFoundException e)
            {
                Log.L(e.Message);
            }
        }

        public Animation(List<Bezier> beziers)
        {
            this.beziers = beziers;
            this.value = (float)this.beziers[0].GetYFromX(0);
        }

        public Animation(Bezier bezier)
        {
            beziers = new List<Bezier>() { bezier };
            value = (float)beziers[0].GetYFromX(0);
        }

        public Animation(float from, float to, float duration, float fromX = 0, float toX = 0)
        {
            beziers = new List<Bezier>() { new Bezier(new Vector2(0, from), fromX, -toX, new Vector2(duration, to)) };
            value = (float)beziers[0].GetYFromX(0);
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the bezier curves of this animation
        /// </summary>
        public List<Bezier> Beziers
        {
            get
            {
                return this.beziers;
            }
        }

        /// <summary>
        /// Gets or sets the elapsed time of this animation
        /// </summary>
        public float ElapsedTime
        {
            get
            {
                return this.elapsed;
            }

            set
            {
                this.elapsed = value;
            }
        }

        /// <summary>
        /// Gets or sets the delegate which runs when the animation is ended
        /// </summary>
        public Action OnEnded
        {
            get;

            set;
        }

        /// <summary>
        /// Gets or sets the delegate which runs when the animation is paused
        /// </summary>
        public Action OnPaused
        {
            get;

            set;
        }

        /// <summary>
        /// Gets or sets the delegate which runs when the animation is about updating
        /// </summary>
        public Action OnPreUpdate
        {
            get;

            set;
        }

        /// <summary>
        /// Gets or sets the delegate which runs when the animation is repeated
        /// </summary>
        public Action OnRepeat
        {
            get;

            set;
        }

        /// <summary>
        /// Gets or sets the delegate which runs when the animation is resumed
        /// </summary>
        public Action OnResumed
        {
            get;

            set;
        }

        /// <summary>
        /// Gets or sets the delegate which runs when the animation is started
        /// </summary>
        public Action OnStarted
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the delegate which runs when the animation is updated
        /// </summary>
        public Action<float> OnUpdated
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the Animation has to repeat
        /// </summary>
        public bool Repeat { get; set; }

        /// <summary>
        /// Gets the state of this animation
        /// </summary>
        public AnimStates State
        {
            get
            {
                return this.state;
            }
        }

        /// <summary>
        /// Gets the current value of the animation
        /// </summary>
        public float Value
        {
            get { return this.value; }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Pauses the animation
        /// </summary>
        public void Pause()
        {
            if (this.state == AnimStates.Running)
            {
                this.state = AnimStates.Paused;
                this.OnPaused?.Invoke();
            }
        }

        /// <summary>
        /// Resumes this animation if paused
        /// </summary>
        public void Resume()
        {
            if (this.state == AnimStates.Paused)
            {
                this.state = AnimStates.Running;
                this.OnResumed?.Invoke();
            }
        }

        /// <summary>
        /// Starts the animation if its ended or idle
        /// </summary>
        public void Start()
        {
            if (this.state == AnimStates.Ended || this.state == AnimStates.Waiting)
            {
                this.elapsed = 0;
                this.beizerIndex = 0;
                this.state = AnimStates.Running;
                this.OnStarted?.Invoke();
            }
        }

        /// <summary>
        /// Stops the animation if its running
        /// </summary>
        public void Stop()
        {
            if (this.state == AnimStates.Running)
            {
                this.elapsed = 0;
                this.beizerIndex = 0;
                this.state = AnimStates.Ended;
                this.OnEnded?.Invoke();
            }
        }

        /// <summary>
        /// Updates this animation
        /// </summary>
        /// <param name="elapsedTime">Elapsed time since last call</param>
        public void Update(float elapsedTime)
        {
            if (this.state == AnimStates.Running && this.beziers != null)
            {
                this.elapsed += elapsedTime;
                while (this.beizerIndex < this.beziers.Count && this.elapsed > this.beziers[this.beizerIndex].P1.X)
                {
                    this.beizerIndex++;
                }

                if (this.beizerIndex == this.beziers.Count)
                {
                    if (this.Repeat)
                    {
                        this.OnPreUpdate?.Invoke();
                        this.value = (float)this.beziers[this.beizerIndex - 1].GetYFromX(this.elapsed);
                        this.OnUpdated?.Invoke(value);
                        this.Start();
                        this.OnRepeat?.Invoke();
                        this.beizerIndex = 0;
                        this.elapsed = 0;
                    }
                    else
                    {
                        this.OnPreUpdate?.Invoke();
                        this.value = (float)this.beziers[this.beizerIndex - 1].GetYFromX(beziers[this.beizerIndex - 1].P1.X);
                        this.OnUpdated?.Invoke(value);
                        this.Stop();
                    }
                }
                else
                {
                    this.OnPreUpdate?.Invoke();
                    this.value = (float)this.beziers[this.beizerIndex].GetYFromX(this.elapsed);
                    this.OnUpdated?.Invoke(value);
                }
            }
        }

        #endregion Methods
    }
}
