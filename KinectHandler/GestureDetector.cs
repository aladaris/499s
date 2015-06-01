using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KinectHandler {
    using System;
    using System.Collections.Generic;
    using Microsoft.Kinect;
    using Microsoft.Kinect.VisualGestureBuilder;

    public delegate void DiscreteGestureDetected(string gestureName, ulong bodyTrackingId, bool isGestureDetected, float detectionConfidence);
    /*
    public class Gesto {
        private bool _detected;
        public Gesture gesture { get; set; }
        public string DataBase { get; private set; }
        public bool Detected {
            get { return _detected; }
            set {
                PreviousDetected = _detected;
                _detected = value;
            }
        }
        public bool PreviousDetected { get; private set; }

        public Gesto(string name, string database) {
            Name = name;
            DataBase = database;
            gesture = null;
            _detected = false;
            PreviousDetected = false;
        }
    }
    */
    /// <summary>
    /// Gesture Detector class which listens for VisualGestureBuilderFrame events from the service
    /// and updates the associated GestureResultView object with the latest results for the 'Seated' gesture
    /// </summary>
    public class GestureDetector : IDisposable {
        private string[] _gesturesNames = { "Egiptian", "Arms_T", "Arm_T_Left", "Arm_T_Right", "Arms_Y", "Arm_Y_Left", "Arm_Y_Right" };
        private Gesture[] _gestos = new Gesture[7];
        private bool[] _gestosDetected = new bool[7];

        private string _dataBasePath = @"Database\499_Gestos.gbd";
        /// <summary> Gesture frame source which should be tied to a body tracking ID </summary>
        private VisualGestureBuilderFrameSource vgbFrameSource = null;
        /// <summary> Gesture frame reader which will handle gesture events coming from the sensor </summary>
        private VisualGestureBuilderFrameReader vgbFrameReader = null;

        // events
        public event DiscreteGestureDetected GestureDetectedChanged;


        /// <summary>
        /// Initializes a new instance of the GestureDetector class along with the gesture frame source and reader
        /// </summary>
        /// <param name="kinectSensor">Active sensor to initialize the VisualGestureBuilderFrameSource object with</param>
        /// <param name="gestureResultView">GestureResultView object to store gesture results of a single body to</param>
        public GestureDetector(KinectSensor kinectSensor) {
            if (kinectSensor == null) {
                throw new ArgumentNullException("kinectSensor");
            }

            // create the vgb source. The associated body tracking ID will be set when a valid body frame arrives from the sensor.
            this.vgbFrameSource = new VisualGestureBuilderFrameSource(kinectSensor, 0);
            this.vgbFrameSource.TrackingIdLost += this.Source_TrackingIdLost;

            // open the reader for the vgb frames
            this.vgbFrameReader = this.vgbFrameSource.OpenReader();
            if (this.vgbFrameReader != null) {
                this.vgbFrameReader.IsPaused = true;
                this.vgbFrameReader.FrameArrived += this.Reader_GestureFrameArrived;
            }

            // load the gestures from the gesture database
            using (VisualGestureBuilderDatabase database = new VisualGestureBuilderDatabase(_dataBasePath)) {
                for (int i = 0; i < _gesturesNames.Length; i++) {
                    Gesture gesture = GetGesture(_gesturesNames[i], database);
                    if (gesture != null) {
                        _gestos[i] = gesture;
                        _gestosDetected[i] = false;
                        this.vgbFrameSource.AddGesture(gesture);
                    }
                }
            }
        }

        private Gesture GetGesture(string gesture_name, VisualGestureBuilderDatabase database) {
            foreach (Gesture gesture in database.AvailableGestures){
                if (gesture_name.Equals(gesture.Name))
                    return gesture;
            }
            return null;
        }

        /// <summary>
        /// Gets or sets the body tracking ID associated with the current detector.
        /// The tracking ID can change whenever a body comes in/out of scope.
        /// </summary>
        public ulong TrackingId {
            get {
                return this.vgbFrameSource.TrackingId;
            }

            set {
                if (this.vgbFrameSource.TrackingId != value) {
                    this.vgbFrameSource.TrackingId = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether or not the detector is currently paused
        /// If the body tracking ID associated with the detector is not valid, then the detector should be paused
        /// </summary>
        public bool IsPaused {
            get {
                return this.vgbFrameReader.IsPaused;
            }

            set {
                if (this.vgbFrameReader.IsPaused != value) {
                    this.vgbFrameReader.IsPaused = value;
                }
            }
        }

        /// <summary>
        /// Disposes all unmanaged resources for the class
        /// </summary>
        public void Dispose() {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes the VisualGestureBuilderFrameSource and VisualGestureBuilderFrameReader objects
        /// </summary>
        /// <param name="disposing">True if Dispose was called directly, false if the GC handles the disposing</param>
        protected virtual void Dispose(bool disposing) {
            if (disposing) {
                if (this.vgbFrameReader != null) {
                    this.vgbFrameReader.FrameArrived -= this.Reader_GestureFrameArrived;
                    this.vgbFrameReader.Dispose();
                    this.vgbFrameReader = null;
                }

                if (this.vgbFrameSource != null) {
                    this.vgbFrameSource.TrackingIdLost -= this.Source_TrackingIdLost;
                    this.vgbFrameSource.Dispose();
                    this.vgbFrameSource = null;
                }
            }
        }

        /// <summary>
        /// Handles gesture detection results arriving from the sensor for the associated body tracking Id
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void Reader_GestureFrameArrived(object sender, VisualGestureBuilderFrameArrivedEventArgs e) {
            VisualGestureBuilderFrameReference frameReference = e.FrameReference;
            using (VisualGestureBuilderFrame frame = frameReference.AcquireFrame()) {
                if (frame != null) {
                    // get the discrete gesture results which arrived with the latest frame
                    IReadOnlyDictionary<Gesture, DiscreteGestureResult> discreteResults = frame.DiscreteGestureResults;

                    if (discreteResults != null) {

                        // Recorremos todos los gestos para ver si se ha producido alún cambio.
                        for (int i = 0; i < _gestos.Length; i++) {
                            DiscreteGestureResult result = null;
                            discreteResults.TryGetValue(_gestos[i], out result);
                            if (result != null) {
                                // TODO: Mirar si Kinect manda el Detected False, cuando se deja de trackear a alguien. O si por el contrario hay que hacer algo si _gestosDetected[i] estaba true y result es null.
                                if (result.Detected != _gestosDetected[i]) {
                                    if (GestureDetectedChanged != null) {
                                        _gestosDetected[i] = result.Detected;
                                        GestureDetectedChanged(_gestos[i].Name, TrackingId, result.Detected, result.Confidence);
                                        break; // TODO: Mirar bien si poner esto aquí. Y en tal caso; ordenar _gestos por preferencia
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Handles the TrackingIdLost event for the VisualGestureBuilderSource object
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void Source_TrackingIdLost(object sender, TrackingIdLostEventArgs e) {
            // TODO: Algo?
        }
    }
}
