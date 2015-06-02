using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _499.InteractionHandlers {

    public delegate void PlayVideoClipHandler(Midi.Pitch note);  // TODO: Refactor el nombre. Algo como Send MidiOn

    public enum FLARE_SIDE { LEFT, CENTER, RIGHT }

    public class FlaresHandler {
        private int _nUsers = 0;  // Number of users triggering flares
        private readonly int _maxUsers;
        readonly private int _maxFlaresLeft;
        readonly private int _maxFlaresRight;
        private VideoClip[] _leftVideoClips;
        private VideoClip[] _rightVideoClips;
        private int _runningVideoClipsLeft = 0;  // Number of videoclips running on the left side
        private int _runningVideoClipsRight = 0;  // Number of videoclips running on the right side
        private int[] _usedLayersLeft;  // Layers used by the Left Flares (-1 for none)
        private int[] _usedLayersRight; // Layers used by the Right Flares (-1 for none)

        public int UserNum { get { return _nUsers; } }
        public int PlayingLeft { get { return _runningVideoClipsLeft; } }
        public int PlayingRight { get { return _runningVideoClipsRight; } }


        // Events
        public event PlayVideoClipHandler OnVideoClipPlay;

        public VideoClip[] LeftVideoClips { get { return _leftVideoClips; } }
        public VideoClip[] RightVideoClips { get { return _rightVideoClips; } }

        public FlaresHandler(int nFlaresLeft, int nFlaresRight, int maxFlaresLeft, int maxFlaresRight, int maxUsers = 6) {
            _maxUsers = maxUsers;
            _maxFlaresLeft = maxFlaresLeft;
            _maxFlaresRight = maxFlaresRight;
            _leftVideoClips = new VideoClip[nFlaresLeft];
            _rightVideoClips = new VideoClip[nFlaresRight];
            _usedLayersLeft = new int[_maxFlaresLeft];
            for (int i = 0; i < _maxFlaresLeft; i++)
                _usedLayersLeft[i] = -1;
            _usedLayersRight = new int[_maxFlaresRight];
            for (int i = 0; i < _maxFlaresRight; i++)
                _usedLayersRight[i] = -1;
        }

        public void Reset() {
            _nUsers = 0;
            _runningVideoClipsLeft = 0;
            _runningVideoClipsRight = 0;
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
                            clip.OnVideoClipEnd += OnLeftVideoClipStop;  // Know when the video finish
                        } else {
                            // RIGHT
                            _runningVideoClipsRight++;
                            clip.OnVideoClipEnd += OnRightVideoClipStop;  // Know when the video finish
                        }
                        return true;
                    }
                    break;
                case 2:  // Center
                    var clipL = clips[0];
                    if (clipL.Id >= 0) {  // Valid video on this position
                        if (PlayVideoClip(clipL)) {
                            _runningVideoClipsLeft++;
                            clipL.OnVideoClipEnd += OnLeftVideoClipStop;
                        }
                    }
                    var clipR = clips[1];
                    if (clipR.Id >= 0) {  // Valid video on this position
                        if (PlayVideoClip(clipR)) {
                            _runningVideoClipsRight++;
                            clipR.OnVideoClipEnd += OnRightVideoClipStop;
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

        private void OnLeftVideoClipStop(VideoClip video) {
            if (video != null) {
                video.OnVideoClipEnd -= OnLeftVideoClipStop;
                SetLayerStop(ref _usedLayersLeft, video.Layer);
                _runningVideoClipsLeft--;
            }
        }

        private void OnRightVideoClipStop(VideoClip video) {
            if (video != null) {
                video.OnVideoClipEnd -= OnRightVideoClipStop;
                SetLayerStop(ref _usedLayersRight, video.Layer);
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
            const int MAX_RETRIES = 6;  // Max attempts when selecting a videoclip
            int tryCount = 0;  // Number of tries selectiong a random videoclip
            System.Random rand = new Random();
            VideoClip nextLeft = _leftVideoClips[rand.Next(_leftVideoClips.Length)];
            VideoClip nextRight = _rightVideoClips[rand.Next(_rightVideoClips.Length)];
            VideoClip nullVideoClip = new VideoClip(-1, "null", 1, -1, Midi.Pitch.A0);  // Represents that none video is in tha position of the array.
            switch (side){
                case FLARE_SIDE.LEFT:
                    if (_runningVideoClipsLeft < _maxFlaresLeft) {
                        while (LayerIsPlaying(ref _usedLayersLeft, nextLeft.Layer)) {
                            nextLeft = _leftVideoClips[rand.Next(_leftVideoClips.Length)];
                            tryCount++;
                            if (tryCount >= MAX_RETRIES)
                                return new VideoClip[] { };
                        }
                        if (SetLayerPlaying(ref _usedLayersLeft, nextLeft.Layer))
                            return new VideoClip[] { nextLeft };
                    }
                    break;

                case FLARE_SIDE.CENTER:
                    VideoClip[] result = new VideoClip[2] {nullVideoClip, nullVideoClip};
                    if (_runningVideoClipsLeft < _maxFlaresLeft) {
                        while (LayerIsPlaying(ref _usedLayersLeft, nextLeft.Layer)) {
                            nextLeft = _leftVideoClips[rand.Next(_leftVideoClips.Length)];
                            tryCount++;
                            if (tryCount >= MAX_RETRIES) {
                                nextLeft = nullVideoClip;
                                break;
                            }
                        }
                        if (SetLayerPlaying(ref _usedLayersLeft, nextLeft.Layer))
                            result[0] = nextLeft;
                    }

                    tryCount = 0;
                    if (_runningVideoClipsRight < _maxFlaresRight) {
                        while (LayerIsPlaying(ref _usedLayersRight, nextRight.Layer)) {
                            nextRight = _rightVideoClips[rand.Next(_rightVideoClips.Length)];
                            tryCount++;
                            if (tryCount >= MAX_RETRIES) {
                                nextRight = nullVideoClip;
                                break;
                            }
                        }
                        if (SetLayerPlaying(ref _usedLayersRight, nextRight.Layer))
                            result[1] = nextRight;
                    }
                    return result;

                case FLARE_SIDE.RIGHT:
                    if (_runningVideoClipsRight < _maxFlaresRight) {
                        while (LayerIsPlaying(ref _usedLayersRight, nextRight.Layer)) {
                            nextRight = _rightVideoClips[rand.Next(_rightVideoClips.Length)];
                            tryCount++;
                            if (tryCount >= MAX_RETRIES)
                                return new VideoClip[] { };
                        }
                        if (SetLayerPlaying(ref _usedLayersRight, nextRight.Layer))
                            return new VideoClip[] { nextRight };
                    }
                    break;
            }
            return new VideoClip[] { };
        }

        /// <summary>
        /// Añade una capa al array de capas usadas seleccionado.
        /// Coloca el valor en la primera posición disponible (< 0).
        /// </summary>
        /// <param name="usedLayers"></param>
        /// <param name="layer"></param>
        /// <returns></returns>
        private bool SetLayerPlaying(ref int[] usedLayers, int layer) {
            if (layer >= 0)
                for (int i = 0; i < usedLayers.Length; i++)
                    if (usedLayers[i] < 0) {
                        usedLayers[i] = layer;
                        return true;
                    }
                    return false;
        }

        /// <summary>
        /// Elimina una capa del array de capas usadas seleccionado.
        /// </summary>
        /// <param name="usedLayers"></param>
        /// <param name="layer"></param>
        /// <returns></returns>
        private bool SetLayerStop(ref int[] usedLayers, int layer) {
            if (layer >= 0)
                for (int i = 0; i < usedLayers.Length; i++)
                    if (usedLayers[i] == layer) {
                        usedLayers[i] = -1;
                        return true;
                    }
            return false;
        }

        private bool LayerIsPlaying(ref int[] usedLayers, int layer) {
            if (layer >= 0)
                for (int i = 0; i < usedLayers.Length; i++)
                    if (layer == usedLayers[i])
                        return true;   
            return false;
        }
    }
}
