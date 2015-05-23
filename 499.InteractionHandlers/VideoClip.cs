using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _499.InteractionHandlers {
    public struct VideoClip {
        #region Atributes
        private int _id;
        private string _name;
        private int _length;  // Length in seconds
        private int _layer;
        private Midi.Pitch _midiNote;
        private System.Timers.Timer _timer;
        #endregion
        #region Properties
        public int Id { get { return _id; } }
        public string Name { get { return _name; } }
        /// <summary>
        /// Gets the videoclip time in seconds
        /// </summary>
        public int Length { get { return _length; } }
        public int Layer { get { return _layer; } }
        public Midi.Pitch MidiNote { get { return _midiNote; } }
        public System.Timers.Timer Timer { get { return _timer; } }
        public bool IsPlaying { get { return _timer.Enabled; } }
        #endregion
        #region Methods
        public VideoClip(int id, string name, int length, int layer, Midi.Pitch note) {
            _id = id;
            _name = name;
            _length = length;
            _layer = layer;
            _midiNote = note;
            _timer = new System.Timers.Timer(_length * 1000);
            _timer.AutoReset = false;
        }

        public bool Play() {
            if (!IsPlaying) {
                _timer.Start();
                return _timer.Enabled;
            }
            return false;
        }

        public bool Stop() {
            if (IsPlaying) {
                _timer.Stop();
                return _timer.Enabled;
            }
            return false;
        }
        #endregion




    }
}
