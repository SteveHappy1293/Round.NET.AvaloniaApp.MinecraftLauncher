using System;
using System.Media;
using NAudio.Wave;
using System.Threading;

namespace MusicPlug.Modules
{
    public static class PlayerCore
    {
        private static WaveOutEvent _outputDevice;
        public static Song MusicInfo;
        private static AudioFileReader _audioFile;
        private static float _volume = 0.5f; // 默认音量50%
        private static Timer _progressTimer;
        public static Action<TimeSpan, TimeSpan> BackAction;

        /// <summary>
        /// 初始化播放器
        /// </summary>
        static PlayerCore()
        {
            _outputDevice = new WaveOutEvent();
            _outputDevice.PlaybackStopped += (sender, args) =>
            {
                BackAction?.Invoke(_audioFile?.TotalTime ?? TimeSpan.Zero, TimeSpan.Zero);
            };
        }

        /// <summary>
        /// 接口1: 更换音频文件
        /// </summary>
        /// <param name="filePath">MP3文件路径</param>
        public static void ChangeAudio(string filePath)
        {
            try
            {
                // 如果已经有音频文件正在播放，先停止并释放资源
                if (_audioFile != null)
                {
                    StopPlayback();
                }

                _audioFile = new AudioFileReader(filePath);
                _outputDevice.Init(_audioFile);
                _audioFile.Volume = _volume;

                // 启动进度更新计时器
                _progressTimer = new Timer(UpdateProgress, null, 0, 200);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"加载音频文件失败: {ex.Message}");
                _audioFile?.Dispose();
                _audioFile = null;
            }
        }

        /// <summary>
        /// 接口2: 播放/暂停切换
        /// </summary>
        public static void TogglePlayPause()
        {
            if (_audioFile == null) return;

            switch (_outputDevice.PlaybackState)
            {
                case PlaybackState.Playing:
                    _outputDevice.Pause();
                    break;
                case PlaybackState.Paused:
                case PlaybackState.Stopped:
                    _outputDevice.Play();
                    break;
            }
        }

        /// <summary>
        /// 接口3: 调整音量
        /// </summary>
        /// <param name="volume">0.0-1.0之间的音量值</param>
        public static void SetVolume(float volume)
        {
            _volume = Math.Clamp(volume, 0f, 1f);
            if (_audioFile != null)
            {
                _audioFile.Volume = _volume;
            }
        }
        
        public static void SetPosition(long position)
        {
            if (_audioFile != null)
            {
                _audioFile.CurrentTime = TimeSpan.FromMilliseconds(position / 1000.0);
            }
        }

        /// <summary>
        /// 停止播放并释放资源
        /// </summary>
        public static void StopPlayback()
        {
            _outputDevice.Stop();
            _audioFile?.Dispose();
            _audioFile = null;
            // 不要立即销毁计时器，让它自然停止
            _progressTimer?.Change(Timeout.Infinite, Timeout.Infinite);
        }

        /// <summary>
        /// 获取当前播放状态
        /// </summary>
        public static PlaybackState GetPlaybackState()
        {
            return _outputDevice.PlaybackState;
        }

        /// <summary>
        /// 获取当前音量
        /// </summary>
        public static float GetVolume()
        {
            return _volume;
        }

        private static void UpdateProgress(object state)
        {
            if (_audioFile != null && BackAction != null)
            {
                BackAction(_audioFile.TotalTime, _audioFile.CurrentTime);
            }
        }
    }
}