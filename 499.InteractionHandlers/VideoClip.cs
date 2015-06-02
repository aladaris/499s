using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace _499.InteractionHandlers {

    public delegate void VideoClipStoppedHandler(VideoClip video);

    public class VideoClip {
        #region Atributes
        private int _id;
        private string _name;
        private int _length;  // Length in milliseconds
        private int _layer;
        private Midi.Pitch _midiNote;
        private Timer _timer;
        // events
        public event VideoClipStoppedHandler OnVideoClipEnd;
        #endregion
        #region Properties
        public int Id { get { return _id; } }
        public string Name { get { return _name; } }
        /// <summary>
        /// Gets the videoclip time in milliseconds
        /// </summary>
        public int Length { get { return _length; } }
        public int Layer { get { return _layer; } }
        public Midi.Pitch MidiNote { get { return _midiNote; } }
        //public Timer Timer { get { return _timer; } }
        public bool IsPlaying { 
            get {
                if (_timer != null)
                    return _timer.Enabled;
                return false;
            }
        }
        #endregion
        #region Methods
        public VideoClip(int id, string name, int length, int layer, Midi.Pitch note) {
            _id = id;
            _name = name;
            _length = length;
            _layer = layer;
            _midiNote = note;
            _timer = new Timer(_length);
            _timer.AutoReset = false;
            _timer.Elapsed += TimerElapsed;
        }

        public bool Play() {
            if ((!IsPlaying)&&(_timer != null)) {
                _timer.AutoReset = false;
                _timer.Start();
                return _timer.Enabled;
            }
            return false;
        }

        public bool Stop() {
            if ((IsPlaying)&&(_timer != null)) {
                _timer.Stop();
                return _timer.Enabled;
            }
            return false;
        }

        private void TimerElapsed(object sender, ElapsedEventArgs e) {
            Timer t = sender as Timer;
            if (t != null) {
                _timer.Stop();
                if (OnVideoClipEnd != null) {
                    OnVideoClipEnd(this);
                }
            }
        }
        #endregion




    }
}
