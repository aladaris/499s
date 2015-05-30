using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;
using Microsoft.Kinect.VisualGestureBuilder;
using System.Timers;

namespace KinectHandler {

    public enum INTERACTION { NONE, FLARE, GLOW, TIMETRAVEL, SPECTRUM }
    public delegate void SmoothedInput (ulong trackingId, INTERACTION interaction);

    class Player : IDisposable {
        public ulong TrackingId { get; set; }
        public INTERACTION Interaction { get; private set; }
        public Timer SmoothTimer { get; private set; }
        public event SmoothedInput OnSmoothedGesture;
        public bool Detected { get; private set; }

        public Player(ulong trackingId, INTERACTION interaction = INTERACTION.NONE, double smooth_time = 200) {
            TrackingId = trackingId;
            Interaction = interaction;
            SmoothTimer = new Timer(smooth_time);
            SmoothTimer.Elapsed += SmoothTimerElapsed;
        }

        public void SmoothInput(INTERACTION interaction, bool detected) {
            if (SmoothTimer != null) {
                // Comenzamos el timer
                if (!SmoothTimer.Enabled) {
                    Interaction = interaction;
                    SmoothTimer.Start();
                // Si el timer ya estaba funcionando, paramos todo el proceso de detección.
                } else {
                    if (Detected != detected) {
                        SmoothTimer.Stop();
                        Interaction = INTERACTION.NONE;
                    }
                }
                Detected = detected;
            }
        }

        public void Dispose() {
            if (SmoothTimer != null) {
                SmoothTimer.Stop();
                SmoothTimer.Dispose();
                TrackingId = 0;
                Interaction = INTERACTION.NONE;
            }
        }

        private void SmoothTimerElapsed(object sender, ElapsedEventArgs e) {
            if (OnSmoothedGesture != null) {
                OnSmoothedGesture(TrackingId, Interaction);
                SmoothTimer.Stop();
            }
        }
    }

    class KinectHanlder : IDisposable {
        /// <summary> Active Kinect sensor </summary>
        private KinectSensor kinectSensor = null;
        /// <summary> Array for the bodies (Kinect will track up to 6 people simultaneously) </summary>
        private Body[] bodies = null;
        /// <summary> Reader for body frames </summary>
        private BodyFrameReader bodyFrameReader = null;
        /// <summary> List of gesture detectors, there will be one detector created for each potential body (max of 6) </summary>
        private List<GestureDetector> gestureDetectorList = null;
        private List<Player> _players = null;

        public KinectHanlder() {
            // only one sensor is currently supported
            this.kinectSensor = KinectSensor.GetDefault();
            // set IsAvailableChanged event notifier
            this.kinectSensor.IsAvailableChanged += this.Sensor_IsAvailableChanged;
            // open the sensor
            this.kinectSensor.Open();
            // open the reader for the body frames
            this.bodyFrameReader = this.kinectSensor.BodyFrameSource.OpenReader();
            // set the BodyFramedArrived event notifier
            this.bodyFrameReader.FrameArrived += this.Reader_BodyFrameArrived;
            // initialize the gesture detection objects for our gestures
            this.gestureDetectorList = new List<GestureDetector>();

            _players = new List<Player>();

            // create a gesture detector for each body (6 bodies => 6 detectors)
            int maxBodies = this.kinectSensor.BodyFrameSource.BodyCount;
            for (int i = 0; i < maxBodies; ++i) {
                GestureDetector detector = new GestureDetector(this.kinectSensor);
                detector.GestureDetectedChanged += GestureChanged;
                this.gestureDetectorList.Add(detector);
                Player p = new Player((ulong)i);
                p.OnSmoothedGesture += SmoothedGesture;
                _players.Add(p);
            }
        }

        private void SmoothedGesture(ulong trackingId, INTERACTION interaction) {
            Player p = GetPlayer(trackingId);
            if (p != null) {
                if (p.Detected) {
                    // TODO: AQUÍ LANZAMOS EL GESTO
                }
            }
        }

        public void Dispose() {
            if (this.bodyFrameReader != null) {
                // BodyFrameReader is IDisposable
                this.bodyFrameReader.FrameArrived -= this.Reader_BodyFrameArrived;
                this.bodyFrameReader.Dispose();
                this.bodyFrameReader = null;
            }
            if (this.gestureDetectorList != null) {
                // The GestureDetector contains disposable members (VisualGestureBuilderFrameSource and VisualGestureBuilderFrameReader)
                foreach (GestureDetector detector in this.gestureDetectorList) {
                    detector.Dispose();
                }

                this.gestureDetectorList.Clear();
                this.gestureDetectorList = null;
            }
            if (this.kinectSensor != null) {
                this.kinectSensor.IsAvailableChanged -= this.Sensor_IsAvailableChanged;
                this.kinectSensor.Close();
                this.kinectSensor = null;
            }
            if (_players != null){
                foreach (Player p in _players){
                    p.Dispose();
                }
                _players.Clear();
            }
        }

        private void GestureChanged(string gestureName, ulong trackingId, bool isGestureDetected, float detectionConfidence) {
            Player p = GetPlayer(trackingId);
            if (p != null) {
                INTERACTION interaction = INTERACTION.NONE;  // TODO: Poner en función de 'gestureName'
                // Comenzamos el proceso de filtrado. Si nunca recibimos el evento 'OnSmoothedGesture' nunca lanzaremos el gesto.
                p.SmoothInput(interaction, isGestureDetected);
            }
        }

        /// <summary>
        /// Handles the event when the sensor becomes unavailable (e.g. paused, closed, unplugged).
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void Sensor_IsAvailableChanged(object sender, IsAvailableChangedEventArgs e) {
            // TODO: Usar para dar estabilidad!
        }

        /// <summary>
        /// Handles the body frame data arriving from the sensor and updates the associated gesture detector object for each body
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void Reader_BodyFrameArrived(object sender, BodyFrameArrivedEventArgs e) {
            bool dataReceived = false;
            using (BodyFrame bodyFrame = e.FrameReference.AcquireFrame()) {
                if (bodyFrame != null) {
                    if (this.bodies == null) {
                        // creates an array of 6 bodies, which is the max number of bodies that Kinect can track simultaneously
                        this.bodies = new Body[bodyFrame.BodyCount];
                    }
                    // The first time GetAndRefreshBodyData is called, Kinect will allocate each Body in the array.
                    // As long as those body objects are not disposed and not set to null in the array,
                    // those body objects will be re-used.
                    bodyFrame.GetAndRefreshBodyData(this.bodies);
                    dataReceived = true;
                }
            }
            if (dataReceived) {
                // we may have lost/acquired bodies, so update the corresponding gesture detectors
                if (this.bodies != null) {
                    // loop through all bodies to see if any of the gesture detectors need to be updated
                    int maxBodies = this.kinectSensor.BodyFrameSource.BodyCount;
                    for (int i = 0; i < maxBodies; ++i) {
                        Body body = this.bodies[i];
                        ulong trackingId = body.TrackingId;
                        // if the current body TrackingId changed, update the corresponding gesture detector with the new value
                        if (trackingId != this.gestureDetectorList[i].TrackingId) {
                            this.gestureDetectorList[i].TrackingId = trackingId;
                            // if the current body is tracked, unpause its detector to get VisualGestureBuilderFrameArrived events
                            // if the current body is not tracked, pause its detector so we don't waste resources trying to get invalid gesture results
                            this.gestureDetectorList[i].IsPaused = trackingId == 0;
                        }



                        // TODO: Necesario hacer esto????
                        if (_players != null)
                            _players[i].TrackingId = bodies[i].TrackingId;
                    }
                }
            }
        }

        private Player GetPlayer(ulong trackingId) {
            if (_players != null) {
                foreach (Player p in _players) {
                    if (p.TrackingId == trackingId)
                        return p;
                }
            }
            return null;
        }
    }
}
