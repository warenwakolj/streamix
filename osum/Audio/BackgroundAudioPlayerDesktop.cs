
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Un4seen.Bass;
using System.IO;
using System.Runtime.InteropServices;

namespace osum.Audio
{
    class BackgroundAudioPlayerDesktop : IBackgroundAudioPlayer
    {
        private GCHandle audioHandle;
        private static int audioStream;
        private SYNCPROC endSyncProc;

        public BackgroundAudioPlayerDesktop()
        {
            BassNet.Registration("poo@poo.com", "2X25242411252422");
            Bass.BASS_Init(-1, 44100, BASSInit.BASS_DEVICE_DEFAULT, (IntPtr)0);
            Bass.BASS_SetConfig(BASSConfig.BASS_CONFIG_BUFFER, 100);
            Bass.BASS_SetConfig(BASSConfig.BASS_CONFIG_UPDATEPERIOD, 10);

            endSyncProc = new SYNCPROC(EndSync);
        }

        public float CurrentVolume
        {
            get { return 0; }
        }

        public bool Play()
        {
            Bass.BASS_ChannelPlay(audioStream, true);
            return true;
        }

        public bool Stop()
        {
            Bass.BASS_ChannelStop(audioStream);
            FreeMusic();
            return true;
        }

        public void Update()
        {
        }

        internal void FreeMusic()
        {
            if (audioStream != 0)
            {
                if (audioHandle.IsAllocated)
                    audioHandle.Free();

                Bass.BASS_ChannelStop(audioStream);
                Bass.BASS_StreamFree(audioStream);
                audioStream = 0;
            }
        }

        public bool Load(byte[] audio)
        {
            FreeMusic();
            audioHandle = GCHandle.Alloc(audio, GCHandleType.Pinned);
            audioStream = Bass.BASS_StreamCreateFile(audioHandle.AddrOfPinnedObject(), 0, audio.Length, BASSFlag.BASS_STREAM_PRESCAN);

            // Set end sync callback
            Bass.BASS_ChannelSetSync(audioStream, BASSSync.BASS_SYNC_END, 0, endSyncProc, IntPtr.Zero);

            return true;
        }

        public double CurrentTime
        {
            get
            {
                if (audioStream == 0) return 0;
                long audioTimeRaw = Bass.BASS_ChannelGetPosition(audioStream);
                return Bass.BASS_ChannelBytes2Seconds(audioStream, audioTimeRaw);
            }
        }

        public bool Pause()
        {
            Bass.BASS_ChannelPause(audioStream);
            return true;
        }

        public bool SetCurrentTime(double seconds)
        {
            if (audioStream == 0) return false;
            long position = Bass.BASS_ChannelSeconds2Bytes(audioStream, seconds);
            return Bass.BASS_ChannelSetPosition(audioStream, position);
        }

        private void EndSync(int handle, int channel, int data, IntPtr user)
        {
            OnMusicCompleted?.Invoke();
        }

        public event Action OnMusicCompleted; 
    }
}