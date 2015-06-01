using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;
using Microsoft.Kinect.VisualGestureBuilder;
using System.Timers;

namespace KinectHandler {

    public enum INTERACTION { NONE, FLARES, FLARE_L, FLARE_R, FLARE_C, TIMETRAVEL_REW, TIMETRAVEL_FF, SPECTRUM, GLOW }
    public delegate void GestureTrigger(int ui_id, ulong trackingId, INTERACTION interaction, bool detected);

    class Player : IDisposable {
        private Timer _timeoutTimer;

        public int UiId { get; private set; }
        public ulong TrackingId { get; set; }
        public INTERACTION Interaction { get; set; }
        public bool Detected { get; set; }
        public bool TimmingOut { get { return _timeoutTimer.Enabled; } }
        // events
        public event GestureTrigger GestureTriggered;

        public Player(int ui_id, ulong trackingId, INTERACTION interaction = INTERACTION.NONE, double timeout_time = 250) {
            UiId = ui_id;
            TrackingId = trackingId;
            Interaction = interaction;
            Detected = false;
            _timeoutTimer = new Timer(timeout_time);
            _timeoutTimer.AutoReset = false;
            _timeoutTimer.Elapsed += OnTimeout;
            _timeoutTimer.Stop();
        }

        public void SetTimmedInteraction(INTERACTION interaction, bool detected) {
            if (!this.TimmingOut) {  // Sin hacer timeout
                if (!this.Detected) {  // Sin ningún estado detectado
                    if (detected) {
                        this.Interaction = interaction;
                        this.Detected = detected;
                        this._timeoutTimer.AutoReset = false;
                        this._timeoutTimer.Start();
                    }
                }
            } else {  // Ya estamos haciendo timeout
                if (this.Detected) {
                    this._timeoutTimer.Stop();
                    if (detected) {  // Interrumpimos Flare_L o Flare_R en favor de Flare_C
                        if ((((this.Interaction == INTERACTION.FLARE_L) || (this.Interaction == INTERACTION.FLARE_R)) && (interaction == INTERACTION.FLARE_C)) || 
                        (((this.Interaction == INTERACTION.TIMETRAVEL_REW) || (this.Interaction == INTERACTION.TIMETRAVEL_FF)) && (interaction == INTERACTION.GLOW))){
                            this.Interaction = interaction;
                            this.Detected = detected;
                            if (GestureTriggered != null)
                                GestureTriggered(UiId, TrackingId, interaction, detected);
                        }
                    }
                }
            }
        }

        public void Reset() {
            Interaction = INTERACTION.NONE;
            Detected = false;
            _timeoutTimer.Stop();
        }

        public void Dispose() {
            if (_timeoutTimer != null) {
                _timeoutTimer.Stop();
                _timeoutTimer.Dispose();
                TrackingId = 0;
                Interaction = INTERACTION.NONE;
            }
        }

        private void OnTimeout(object sender, ElapsedEventArgs e) {
            if (Detected) {
                if (GestureTriggered != null)
                    GestureTriggered(UiId, TrackingId, Interaction, Detected);
            }
            _timeoutTimer.Stop();
        }
    }

    public class KinectHanlder : IDisposable {
        /// <summary> Active Kinect sensor </summary>
        private KinectSensor kinectSensor = null;
        /// <summary> Array for the bodies (Kinect will track up to 6 people simultaneously) </summary>
        private Body[] bodies = null;
        /// <summary> Reader for body frames </summary>
        private BodyFrameReader bodyFrameReader = null;
        /// <summary> List of gesture detectors, there will be one detector created for each potential body (max of 6) </summary>
        private List<GestureDetector> gestureDetectorList = null;
        private List<Player> _players = null;
        public event GestureTrigger OnGestureTrigger;
        public event EventHandler<IsAvailableChangedEventArgs> OnKinectAviableChange;

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
                Player p = new Player(i, (ulong)i);
                p.GestureTriggered += OnPlayerGestureTrigger;
                _players.Add(p);
            }
        }

        private void OnPlayerGestureTrigger(int ui_id, ulong trackingId, INTERACTION interaction, bool detected) {
            Player p = GetPlayer(trackingId);
            if (p != null) {
                if (OnGestureTrigger != null)
                    OnGestureTrigger(ui_id, trackingId, interaction, detected);
            }
        }

        public void Reset() {
            if (kinectSensor!= null)
                if (kinectSensor.IsOpen)
                    kinectSensor.Close();
            kinectSensor = KinectSensor.GetDefault();
            kinectSensor.Open();
            bodyFrameReader = kinectSensor.BodyFrameSource.OpenReader();
            foreach (Player p in _players) {
                p.Reset();
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

                // Asociamos gestureName con Interacciones
                INTERACTION interaction = INTERACTION.NONE;
                if (gestureName.Equals("Arm_T_Left")) {
                    interaction = INTERACTION.TIMETRAVEL_REW;
                } else if (gestureName.Equals("Arm_T_Right")) {
                    interaction = INTERACTION.TIMETRAVEL_FF;
                } else if (gestureName.Equals("Arms_T")) {
                    interaction = INTERACTION.GLOW;
                } else if (gestureName.Equals("Arm_Y_Left")) {
                    interaction = INTERACTION.FLARE_L;
                } else if (gestureName.Equals("Arm_Y_Right")) {
                    interaction = INTERACTION.FLARE_R;
                } else if (gestureName.Equals("Arms_Y")) {
                    interaction = INTERACTION.FLARE_C;
                } else if (gestureName.Equals("Egiptian")) {
                    interaction = INTERACTION.SPECTRUM;
                }
                if (interaction == INTERACTION.NONE)
                    return;

                // Player no estaba haciendo nada
                if (p.Interaction == INTERACTION.NONE) {
                    if ((!p.Detected)&&(isGestureDetected)) {  // De nada a hacer un gesto
                        // Spectrum (y las dos FULL) es la única que se puede lanzar directamente
                        if ((interaction == INTERACTION.SPECTRUM) || (interaction == INTERACTION.FLARE_C) || (interaction == INTERACTION.GLOW)) {
                            p.Detected = isGestureDetected;
                            p.Interaction = interaction;
                            if (OnGestureTrigger != null)
                                OnGestureTrigger(p.UiId, p.TrackingId, p.Interaction, p.Detected);
                        // Aqui van las interacciones que necesitan timeout
                        } else if (interaction != INTERACTION.NONE) {
                            p.SetTimmedInteraction(interaction, isGestureDetected);
                        }
                    }
                // El Player ya tiene una interacción asociada
                } else {
                    if ((((p.Interaction == INTERACTION.FLARE_L) || (p.Interaction == INTERACTION.FLARE_R)) && (interaction == INTERACTION.FLARE_C)) ||
                        (((p.Interaction == INTERACTION.TIMETRAVEL_REW) || (p.Interaction == INTERACTION.TIMETRAVEL_FF)) && (interaction == INTERACTION.GLOW))) {
                        p.SetTimmedInteraction(interaction, isGestureDetected);
                    }

                    if ((p.Detected) && (!isGestureDetected)) { // De hacer un gesto a dejar de hacerlo
                        if (interaction == p.Interaction) {
                            p.Detected = false;
                            if (OnGestureTrigger != null)
                                OnGestureTrigger(p.UiId, p.TrackingId, p.Interaction, false);
                            p.Interaction = INTERACTION.NONE;
                        }
                    }
                }

            }
        }

        /// <summary>
        /// Handles the event when the sensor becomes unavailable (e.g. paused, closed, unplugged).
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void Sensor_IsAvailableChanged(object sender, IsAvailableChangedEventArgs e) {
            if (OnKinectAviableChange != null)
                OnKinectAviableChange(sender, e);
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

                            // TODO: Necesario hacer esto????  =>> Siiiiiii
                            if (_players != null)
                                _players[i].TrackingId = bodies[i].TrackingId;
                        }
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
