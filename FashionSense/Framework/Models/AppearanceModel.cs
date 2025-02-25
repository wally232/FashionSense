﻿using FashionSense.Framework.Models.Generic;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FashionSense.Framework.Models
{
    public class AppearanceModel
    {
        public Position StartingPosition { get; set; }
        public bool Flipped { get; set; }
        public bool RequireAnimationToFinish { get; set; }
        public virtual bool HideWaterLine { get; set; }
        public virtual bool HideWhileSwimming { get; set; } = true;
        public virtual bool HideWhileWearingBathingSuit { get; set; } = true;
        public bool UseBaldHead { get; set; }
        public bool HideSleeves { get; set; }
        public bool DisableGrayscale { get; set; }
        public bool DisableSkinGrayscale { get; set; }
        public bool IsPrismatic { get; set; }
        public float PrismaticAnimationSpeedMultiplier { get; set; } = 1f;
        public List<int[]> ColorMasks { get; set; } = new List<int[]>();
        public SkinToneModel SkinToneMasks { get; set; }
        public List<AnimationModel> UniformAnimation { get; set; } = new List<AnimationModel>();
        public List<AnimationModel> IdleAnimation { get; set; } = new List<AnimationModel>();
        public List<AnimationModel> MovementAnimation { get; set; } = new List<AnimationModel>();

        internal bool IsPlayerColorChoiceIgnored()
        {
            return DisableGrayscale || IsPrismatic;
        }

        internal bool IsMaskedColor(Color color)
        {
            if (!HasColorMask())
            {
                return false;
            }

            foreach (Color maskedColor in ColorMasks.Select(c => new Color(c[0], c[1], c[2])))
            {
                if (maskedColor == color)
                {
                    return true;
                }
            }

            return false;
        }

        internal bool IsSkinToneMaskColor(Color color)
        {
            if (!HasSkinToneMask() || SkinToneMasks is null)
            {
                return false;
            }

            if (SkinToneMasks.LightTone is not null && color == SkinToneMasks.Lightest)
            {
                return true;
            }
            else if (SkinToneMasks.MediumTone is not null && color == SkinToneMasks.Medium)
            {
                return true;
            }
            else if (SkinToneMasks.DarkTone is not null && color == SkinToneMasks.Darkest)
            {
                return true;
            }

            return false;
        }

        internal bool HasColorMask()
        {
            return DisableGrayscale is false && ColorMasks.Count > 0;
        }

        internal bool HasSkinToneMask()
        {
            if (SkinToneMasks is null)
            {
                return false;

            }
            return SkinToneMasks.LightTone is not null || SkinToneMasks.MediumTone is not null || SkinToneMasks.DarkTone is not null;
        }

        internal bool HasUniformAnimation()
        {
            return UniformAnimation.Count > 0;
        }

        internal bool HasIdleAnimation()
        {
            return IdleAnimation.Count > 0;
        }

        internal bool HasMovementAnimation()
        {
            return MovementAnimation.Count > 0;
        }

        private AnimationModel GetAnimationData(List<AnimationModel> animation, int frame)
        {
            return animation.FirstOrDefault(a => a.Frame == frame);
        }

        internal AnimationModel GetUniformAnimationAtFrame(int frame)
        {
            return GetAnimationData(UniformAnimation, frame);
        }

        internal AnimationModel GetIdleAnimationAtFrame(int frame)
        {
            return GetAnimationData(IdleAnimation, frame);
        }

        internal AnimationModel GetMovementAnimationAtFrame(int frame)
        {
            return GetAnimationData(MovementAnimation, frame);
        }

        internal static int GetColorIndex(int[] colorArray, int position)
        {
            if (position >= colorArray.Length)
            {
                return 255;
            }

            return colorArray[position];
        }

        internal static Color GetColor(int[] colorArray)
        {
            if (3 < colorArray.Length)
            {
                return new Color(GetColorIndex(colorArray, 0), GetColorIndex(colorArray, 1), GetColorIndex(colorArray, 2), GetColorIndex(colorArray, 3));
            }
            else
            {
                return new Color(GetColorIndex(colorArray, 0), GetColorIndex(colorArray, 1), GetColorIndex(colorArray, 2), 255);
            }
        }
    }
}
