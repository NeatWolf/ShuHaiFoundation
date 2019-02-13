using System;
using UnityEngine;

namespace ShuHai.Unity
{
    public class CurveAnimation : ScriptableObject
    {
        public static CurveAnimation Create() { return CreateInstance<CurveAnimation>(); }

        #region Time

        public const float DefaultDuration = 1;

        public float BeginTime { get; protected set; }
        public float EndTime { get { return BeginTime + Duration; } }
        public float Duration = DefaultDuration;
        public float ElaspsedTime { get { return CurrentTime - BeginTime; } }

        public float CurrentTime { get { return TimeSource != null ? TimeSource() : DefaultTimeSource(); } }

        public Func<float> TimeSource;

        public static float DefaultTimeSource() { return Time.realtimeSinceStartup; }

        #endregion

        public enum FinalState
        {
            RemainCurrent,
            SameAsBegin,
            SameAsEnd
        }

        public float Value { get; protected set; }

        public bool IsPlaying { get; private set; }

        public AnimationCurve Curve;

        public event Action AboutToPlay;
        public event Action Playing;
        public event Action Stopped;

        public void Play(float duration)
        {
            Duration = duration;
            Play();
        }

        public void Play()
        {
            if (IsPlaying)
                Stop();

            BeginTime = CurrentTime;

            AboutToPlay?.Invoke();

            PlayImpl();

            IsPlaying = true;

            Update();
        }

        public void Stop(FinalState finalState = FinalState.RemainCurrent)
        {
            IsPlaying = false;

            switch (finalState)
            {
                case FinalState.RemainCurrent:
                    // Nothing to do.
                    break;
                case FinalState.SameAsBegin:
                    BeginTime = CurrentTime - Duration;
                    break;
                case FinalState.SameAsEnd:
                    BeginTime = CurrentTime;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(finalState), finalState, null);
            }
            UpdateImpl();

            StopImpl();

            Stopped?.Invoke();
        }

        public void Update()
        {
            if (!IsPlaying)
                return;

            if (EndTime > CurrentTime)
            {
                UpdateImpl();

                Playing?.Invoke();
            }
            else
            {
                Stop();
            }
        }

        protected virtual void PlayImpl() { }

        protected virtual void StopImpl() { }

        protected virtual void UpdateImpl() { Value = Evaluate(Curve, Value); }

        protected float Evaluate(AnimationCurve curve, float fallback)
        {
            return curve?.Evaluate(ElaspsedTime) ?? fallback;
        }
    }
}