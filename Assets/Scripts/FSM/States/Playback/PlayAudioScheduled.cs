﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace StageFSM
{
    class PlayAudioScheduled : StageStateMachineBehaviour
    {

        [SerializeField]
        string stateName;
        [SerializeField]
        private PitchMode pitchMode;

        private AudioPlaybackController playbackController;
        private AudioSource audioSource;
        private AudioClip audioClip;

        private enum PitchMode
        {
            MatchSpeedLevel,
            MatchTimeScale
        }

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            AssignToolboxAndAssets(animator, stateInfo, layerIndex);
            audioClip = assetToolbox.GetAssetGroupForState(stateName, Animator).GetAsset<AudioClip>();
            if (playbackController == null)
                playbackController = toolbox.GetTool<AudioPlaybackController>();

            audioSource = playbackController.GetNextAudioSource();
            base.OnStateEnter(animator, stateInfo, layerIndex);
            if (enterCondition == EnterCondition.Early && audioClip != null)
            {
                playbackController.ShiftAudioPlaybackTimeInBeats((double)animator.GetAnimatorTransitionInfo(layerIndex).duration);

                var speedController = toolbox.GetTool<SpeedController>();
                var speed = speedController.Speed;
                var pitch = pitchMode == PitchMode.MatchSpeedLevel
                ? speedController.GetSpeedTimeScaleMult(speed)
                : Time.timeScale;
                playbackController.ScheduleClip(audioSource, audioClip, pitch);
            }
        }

        protected override void OnStateEnterOfficial()
        {
            base.OnStateEnterOfficial();
            if (audioClip != null && enterCondition == EnterCondition.Normal)
            {
                playbackController.ResetAudioPlaybackTime();
                playbackController.ScheduleClip(audioSource, audioClip, toolbox.GetTool<SpeedController>().GetSpeedTimeScaleMult());
            }
        }
    }
}
