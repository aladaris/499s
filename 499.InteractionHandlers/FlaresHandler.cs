using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _499.InteractionHandlers {

    public enum FLARE_SIDE { LEFT, CENTER, RIGHT }

    public class FlaresHandler {
        private int _nUsers = 0;  // Number of users triggering flares
        private readonly int _maxUsers;
        readonly private int _maxFlaresLeft;
        readonly private int _maxFlaresright;
        private VideoClip[] _leftVideoClips;
        private VideoClip[] _rightVideoClips;
        private int _runningVideoClipsLeft = 0;  // Number of videoclips running on the left side
        private int _runningVideoClipsRight = 0;  // Number of videoclips running on the right side

        public int UserNum { get { return _nUsers; } }
        public int PlayingLeft { get { return _runningVideoClipsLeft; } }
        public int PlayingRight { get { return _runningVideoClipsRight; } }


        // Events
        public delegate void PlayVideoClipHandler(Midi.Pitch note);
        public event PlayVideoClipHandler OnVideoClipPlay;

        public VideoClip[] LeftVideoClips { get { return _leftVideoClips; } }
        public VideoClip[] RightVideoClips { get { return _rightVideoClips; } }

        public FlaresHandler(int nFlaresLeft, int nFlaresRight, int maxFlaresLeft, int maxFlaresRight, int maxUsers = 6) {
            _maxUsers = maxUsers;
            _maxFlaresLeft = maxFlaresLeft;
            _maxFlaresright = maxFlaresRight;
            _leftVideoClips = new VideoClip[nFlaresLeft];
            _rightVideoClips = new VideoClip[nFlaresRight];
        }

        public bool NewUser(FLARE_SIDE side) {
            if (_nUsers + 1 > _maxUsers)
                return false;
            _nUsers++;
            VideoClip[] clips = GetCorrespondingVideoClips(side);
            switch (clips.Length) {
                case 0:
                    return false;
                case 1:  // Left or Right
                    var clip = clips[0];
                    if (PlayVideoClip(clip)) {
                        if (side == FLARE_SIDE.LEFT) {
                            // LEFT
                            _runningVideoClipsLeft++;
                            clip.Timer.Elapsed += OnLeftVideoClipStop;  // Know when the video finish
                        } else {
                            // RIGHT
                            _runningVideoClipsRight++;
                            clip.Timer.Elapsed += OnRightVideoClipStop;  // Know when the video finish
                        }
                        return true;
                    }
                    break;
                case 2:  // Center
                    var clipL = clips[0];
                    if (clipL.Id >= 0) {  // Valid video on this position
                        if (PlayVideoClip(clipL)) {
                            _runningVideoClipsLeft++;
                            clipL.Timer.Elapsed += OnLeftVideoClipStop;
                        }
                    }
                    var clipR = clips[1];
                    if (clipR.Id >= 0) {  // Valid video on this position
                        if (PlayVideoClip(clipR)) {
                            _runningVideoClipsRight++;
                            clipR.Timer.Elapsed += OnRightVideoClipStop;
                        }
                    }
                    return true;
            }
            return false;
        }

        public bool RemoveUser() {
            if (_nUsers - 1 < 0)
                return false;
            _nUsers--;
            return true;
        }

        private bool PlayVideoClip(VideoClip clip) {
            if (clip.Play()) {
                if (OnVideoClipPlay != null) {
                    OnVideoClipPlay(clip.MidiNote);
                    return true;
                }
            }
            clip.Stop();
            return false;
        }

        private void OnLeftVideoClipStop(object sender, System.Timers.ElapsedEventArgs e) {
            var timer = sender as System.Timers.Timer;
            if (timer != null) {
                timer.Stop();
                timer.Elapsed -= OnLeftVideoClipStop;
                _runningVideoClipsLeft--;
            }
        }

        private void OnRightVideoClipStop(object sender, System.Timers.ElapsedEventArgs e) {
            var timer = sender as System.Timers.Timer;
            if (timer != null) {
                timer.Stop();
                timer.Elapsed -= OnRightVideoClipStop;
                _runningVideoClipsRight--;
            }
        }

        /// <summary>
        /// Devuelve un array de videos.
        /// El array será de longitud 1, para acciones a derecha o izquierda.
        /// El array será de longitud 2, para aciones al centro. Este puede tener elementos 'nullVideoClip', si no se pueden lanzar debido al maximo establecido.
        /// El array será de longitud 0, si algo falla.
        /// </summary>
        /// <param name="side">Triggered side</param>
        /// <returns></returns>
        private VideoClip[] GetCorrespondingVideoClips(FLARE_SIDE side) {
            VideoClip nextLeft = _leftVideoClips[_nUsers % _leftVideoClips.Length];
            VideoClip nextRight = _rightVideoClips[_nUsers % _rightVideoClips.Length];
            VideoClip nullVideoClip = new VideoClip(-1, "null", 1, 0, Midi.Pitch.A0);  // Represents that none video is in tha position of the array.
            switch (side){
                case FLARE_SIDE.LEFT:
                    if (_runningVideoClipsLeft < _maxFlaresLeft) {
                        return new VideoClip[] { nextLeft };
                    }
                    break;

                case FLARE_SIDE.CENTER:
                    VideoClip[] result = new VideoClip[2] {nullVideoClip, nullVideoClip};
                    if (_runningVideoClipsLeft < _maxFlaresLeft)
                        result[0] = nextLeft;
                    if (_runningVideoClipsRight < _maxFlaresright)
                        result[1] = nextRight;
                    return result;

                case FLARE_SIDE.RIGHT:
                    if (_runningVideoClipsRight < _maxFlaresright) {
                        return new VideoClip[] { nextRight };
                    }
                    break;
            }
            return new VideoClip[] { };
        }
    }
}
