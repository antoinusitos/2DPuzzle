using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;

namespace _2DPuzzle
{
    public class SoundManager
    {
        private static SoundManager _instance;

        private static readonly object _lock = new object();

        public static SoundManager GetInstance()
        {
            // This conditional is needed to prevent threads stumbling over the
            // lock once the instance is ready.
            if (_instance == null)
            {
                // Now, imagine that the program has just been launched. Since
                // there's no Singleton instance yet, multiple threads can
                // simultaneously pass the previous conditional and reach this
                // point almost at the same time. The first of them will acquire
                // lock and will proceed further, while the rest will wait here.
                lock (_lock)
                {
                    // The first thread to acquire the lock, reaches this
                    // conditional, goes inside and creates the Singleton
                    // instance. Once it leaves the lock block, a thread that
                    // might have been waiting for the lock release may then
                    // enter this section. But since the Singleton field is
                    // already initialized, the thread won't create a new
                    // object.
                    if (_instance == null)
                    {
                        _instance = new SoundManager();
                    }
                }
            }
            return _instance;
        }

        private Song currentSong = null;

        private List<SoundEffect> soundEffects = null;

        private AudioListener listener = null;
        private AudioEmitter emitter = null;


        public void InitializeManager()
        {
            soundEffects = new List<SoundEffect>();
            currentSong = RenderManager.GetInstance().content.Load<Song>("BG_Music");

            soundEffects.Add(RenderManager.GetInstance().content.Load<SoundEffect>("Zombie1"));

            listener = new AudioListener
            {
                // 1 is left
                Position = new Microsoft.Xna.Framework.Vector3(1, 0, 0)
            };
            emitter = new AudioEmitter();
        }

        public void PlaySound(string inPath)
        {
            MediaPlayer.Play(currentSong);
            //  Uncomment the following line will also loop the song
            //  MediaPlayer.IsRepeating = true;
        }

        public void PlaySoundEffect(string inPath)
        {
            // Play that can be manipulated after the fact
            var instance = soundEffects[0].CreateInstance();
            instance.IsLooped = true;
            instance.Play();
        }

        public void PlaySoundEffect3D(string inPath)
        {
            // Play that can be manipulated after the fact
            var instance = soundEffects[0].CreateInstance();
            instance.IsLooped = true;
            instance.Play();

            // WARNING!!!!  Apply3D requires sound effect be Mono!  
            //Stereo will throw exception
            instance.Apply3D(listener, emitter);
            instance.Play();

        }
    }
}
