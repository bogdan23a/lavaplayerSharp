﻿using com.sedmelluq.discord.lavaplayer.player.@event;
using com.sedmelluq.discord.lavaplayer.tools;
using java.util.concurrent;
using java.util.concurrent.atomic;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using AudioEventListener = com.sedmelluq.discord.lavaplayer.player.@event.AudioEventListener;
using TrackEndEvent = com.sedmelluq.discord.lavaplayer.player.@event.TrackEndEvent;
using TrackStartEvent = com.sedmelluq.discord.lavaplayer.player.@event.TrackStartEvent;
using AudioOutputHook = com.sedmelluq.discord.lavaplayer.player.hook.AudioOutputHook;
using AudioTrackEndReason = com.sedmelluq.discord.lavaplayer.track.AudioTrackEndReason;
using InternalAudioTrack = com.sedmelluq.discord.lavaplayer.track.InternalAudioTrack;
using TrackStateListener = com.sedmelluq.discord.lavaplayer.track.TrackStateListener;
using AudioFrame = com.sedmelluq.discord.lavaplayer.track.playback.AudioFrame;
using AudioFrameProvider = com.sedmelluq.discord.lavaplayer.track.playback.AudioFrameProvider;
using AudioTrack = com.sedmelluq.discord.lavaplayer.track.AudioTrack;
using AudioFrameProviderTools = com.sedmelluq.discord.lavaplayer.track.playback.AudioFrameProviderTools;
using System.Diagnostics;

namespace com.sedmelluq.discord.lavaplayer.player
{

    using Logger = ILogger;
    using LoggerFactory = ILoggerFactory;

    /*
    //JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
    import static com.sedmelluq.discord.lavaplayer.track.AudioTrackEndReason.CLEANUP;
    //JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
    import static com.sedmelluq.discord.lavaplayer.track.AudioTrackEndReason.FINISHED;
    //JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
    import static com.sedmelluq.discord.lavaplayer.track.AudioTrackEndReason.LOAD_FAILED;
    //JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
    import static com.sedmelluq.discord.lavaplayer.track.AudioTrackEndReason.REPLACED;
    //JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
    import static com.sedmelluq.discord.lavaplayer.track.AudioTrackEndReason.STOPPED;
    */
    /// <summary>
    /// An audio player that is capable of playing audio tracks and provides audio frames from the currently playing track.
    /// </summary>
    public class AudioPlayer : AudioFrameProvider, TrackStateListener
    {
        private static readonly LoggerFactory factory;

        private static readonly Logger log = factory.CreateLogger<AudioPlayer>();

        private volatile InternalAudioTrack activeTrack;
        private long lastRequestTime;
        private long lastReceiveTime;
        private volatile bool stuckEventSent;
        private volatile InternalAudioTrack shadowTrack;
        private readonly AtomicBoolean paused;
        private readonly DefaultAudioPlayerManager manager;
        private readonly IList<AudioEventListener> listeners;
        private readonly AtomicInteger volumeLevel;
        private readonly AudioOutputHook outputHook;
        private readonly object trackSwitchLock;

        /// <param name="manager"> Audio player manager which this player is attached to </param>
        /// <param name="outputHook"> Hook which can intercept outgoing audio frames </param>
        public AudioPlayer(DefaultAudioPlayerManager manager, AudioOutputHook outputHook)
        {
            this.manager = manager;
            this.outputHook = outputHook;
            activeTrack = null;
            paused = new AtomicBoolean();
            listeners = new IList<AudioEventListener>();
            volumeLevel = new AtomicInteger(100);
            trackSwitchLock = new object();
        }

        /// <returns> Currently playing track </returns>
        public virtual AudioTrack PlayingTrack
        {
            get
            {
                return activeTrack;
            }
        }

        /// <param name="track"> The track to start playing </param>
        public virtual void playTrack(AudioTrack track)
        {
            startTrack(track, false);
        }
        private static long nanoTime()
        {
            long nano = 10000L * Stopwatch.GetTimestamp();
            nano /= TimeSpan.TicksPerMillisecond;
            nano *= 100L;
            return nano;
        }
        /// <param name="track"> The track to start playing, passing null will stop the current track and return false </param>
        /// <param name="noInterrupt"> Whether to only start if nothing else is playing </param>
        /// <returns> True if the track was started </returns>
        public virtual bool startTrack(AudioTrack track, bool noInterrupt)
        {
            InternalAudioTrack newTrack = (InternalAudioTrack)track;
            InternalAudioTrack previousTrack;

            lock (trackSwitchLock)
            {
                previousTrack = activeTrack;

                if (noInterrupt && previousTrack != null)
                {
                    return false;
                }
            }
            activeTrack = newTrack;
            lastRequestTime = DateTimeHelperClass.CurrentUnixTimeMillis();
            lastReceiveTime = nanoTime();
            stuckEventSent = false;

            if (previousTrack != null)
            {
                previousTrack.stop();
                dispatchEvent(new TrackEndEvent(this, previousTrack, newTrack == null ? AudioTrackEndReason.STOPPED : AudioTrackEndReason.REPLACED));

                shadowTrack = previousTrack;
            }


            if (newTrack == null)
            {
                shadowTrack = null;
                return false;
            }
            dispatchEvent(new TrackStartEvent(this, newTrack));

            manager.executeTrack(this, newTrack, manager.Configuration, volumeLevel);
            return true;


        }

        /// <summary>
        /// Stop currently playing track.
        /// </summary>
        public void stopTrack()
        {
            stopWithReason(AudioTrackEndReason.STOPPED);
        }

        private void stopWithReason(AudioTrackEndReason reason)
        {
            shadowTrack = null;

            lock (trackSwitchLock)
            {
                InternalAudioTrack previousTrack = activeTrack;
                activeTrack = null;

                if (previousTrack != null)
                {
                    previousTrack.stop();
                    dispatchEvent(new TrackEndEvent(this, previousTrack, reason));
                }
            }
        }

        private AudioFrame provideShadowFrame()
        {
            InternalAudioTrack shadow = shadowTrack;
            AudioFrame frame = null;

            if (shadow != null)
            {
                frame = shadow.provide();

                if (frame != null && frame.Terminator)
                {
                    shadowTrack = null;
                    frame = null;
                }
            }

            return frame;
        }

        public AudioFrame provide()
        {
            return AudioFrameProviderTools.delegateToTimedProvide(this);
        }

        public AudioFrame provide(long timeout, TimeUnit unit)
        {
            AudioFrame frame = provideDirectly(timeout, unit);
            if (outputHook != null)
            {
                frame = outputHook.outgoingFrame(this, frame);
            }
            return frame;
        }

        /// <summary>
        /// Provide an audio frame bypassing hooks. </summary>
        /// <param name="timeout"> Specifies the maximum time to wait for data. Pass 0 for non-blocking mode. </param>
        /// <param name="unit"> Specifies the time unit of the maximum wait time. </param>
        /// <returns> An audio frame if available, otherwise null </returns>
        public AudioFrame provideDirectly(long timeout, TimeUnit unit)
        {
            InternalAudioTrack track;

            lastRequestTime = DateTimeHelperClass.CurrentUnixTimeMillis();

            if (timeout == 0 && paused.get())
            {
                return null;
            }

            while ((track = activeTrack) != null)
            {
                AudioFrame frame = timeout > 0 ? track.provide(timeout, unit) : track.provide();

                if (frame != null)
                {
                    lastReceiveTime = nanoTime();
                    shadowTrack = null;

                    if (frame.Terminator)
                    {

                        handleTerminator(track);
                        continue;
                    }


                }
                else if (timeout == 0)
                {

                    checkStuck(track);

                    frame = provideShadowFrame();
                }

                return frame;
            }

            return null;
        }

        private void handleTerminator(InternalAudioTrack track)
        {
            lock (trackSwitchLock)
            {
                if (activeTrack == track)
                {
                    activeTrack = null;

                    dispatchEvent(new TrackEndEvent(this, track, track.ActiveExecutor.failedBeforeLoad() ? AudioTrackEndReason.LOAD_FAILED : AudioTrackEndReason.FINISHED));
                }
            }
        }

        private void checkStuck(AudioTrack track)
        {
            if (!stuckEventSent && nanoTime() - lastReceiveTime > manager.TrackStuckThresholdNanos)
            {
                stuckEventSent = true;
                dispatchEvent(new TrackStuckEvent(this, track, TimeUnit.NANOSECONDS.toMillis(manager.TrackStuckThresholdNanos)));
            }
        }

        public AtomicInteger Volume
        {
            get => volumeLevel;
        }

        public void setVolume(int volume)
        {
            volumeLevel.set(Math.Min(150, Math.Max(0, volume)));
        }

        /// <returns> Whether the player is paused </returns>
        public AtomicBoolean Paused
        {
            get => paused;
        }


        /// <param name="value"> True to pause, false to resume </param>
        public void setPaused(bool value)
        {
            if (paused.compareAndSet(!value, value))
            {
                if (value)
                {
                    dispatchEvent(new PlayerPauseEvent(this));
                }
                else
                {
                    dispatchEvent(new PlayerResumeEvent(this));
                    lastReceiveTime = nanoTime();
                }
            }
        }

        /// <summary>
        /// Destroy the player and stop playing track.
        /// </summary>
        public void destroy()
        {
            stopTrack();
        }

        /// <summary>
        /// Add a listener to events from this player. </summary>
        /// <param name="listener"> New listener </param>
        public void addListener(AudioEventListener listener)
        {
            lock (trackSwitchLock)
            {
                listeners.Add(listener);
            }
        }

        /// <summary>
        /// Remove an attached listener using identity comparison. </summary>
        /// <param name="listener"> The listener to remove </param>
        public void removeListener(AudioEventListener listener)
        {
            lock (trackSwitchLock)
            {
                for (IEnumerator<AudioEventListener> iterator = listeners.GetEnumerator(); iterator.MoveNext();)
                {
                    if (iterator.Current == listener)
                    {
                        //JAVA TO C# CONVERTER TODO TASK: .NET enumerators are read-only:
                        listener.remove();
                    }
                }
            }
        }
        private void dispatchEvent(AudioEvent @event)
        {
            log.LogDebug("Firing an event with class {}", @event.GetType().Name);

            lock (trackSwitchLock)
            {
                foreach (AudioEventListener listener in listeners)
                {
                    try
                    {
                        listener.onEvent(@event);
                    }
                    catch (Exception e)
                    {
                        log.LogError("Handler of event {} threw an exception.", @event, e);
                    }
                }
            }
        }

        public void onTrackException(AudioTrack track, FriendlyException exception)
        {
            dispatchEvent(new TrackExceptionEvent(this, track, exception));
        }

        public void onTrackStuck(AudioTrack track, long thresholdMs)
        {
            dispatchEvent(new TrackStuckEvent(this, track, thresholdMs));
        }

        /// <summary>
        /// Check if the player should be "cleaned up" - stopped due to nothing using it, with the given threshold. </summary>
        /// <param name="threshold"> Threshold in milliseconds to use </param>
        public virtual void checkCleanup(long threshold)
        {
            AudioTrack track = PlayingTrack;
            if (track != null && DateTimeHelperClass.CurrentUnixTimeMillis() - lastRequestTime >= threshold)
            {
                log.LogDebug("Triggering cleanup on an audio player playing track {}", track);

                stopWithReason(AudioTrackEndReason.CLEANUP);
            }
        }
    }
}

