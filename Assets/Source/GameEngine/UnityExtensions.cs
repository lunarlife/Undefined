using System;
using System.Linq;
using TMPro;
using UndefinedNetworking.GameEngine;
using UndefinedNetworking.GameEngine.Input;
using UndefinedNetworking.GameEngine.Scenes.UI.Enums;
using UndefinedNetworking.GameEngine.Scenes.UI.Structs;
using UndefinedNetworking.GameEngine.Scenes.UI.Windows;
using UnityEngine;
using UnityEngine.UI;
using Utils.Dots;
using FontStyle = UndefinedNetworking.GameEngine.Scenes.UI.Enums.FontStyle;
using Rect = Utils.Rect;
using TextAlignment = UndefinedNetworking.GameEngine.Scenes.UI.Enums.TextAlignment;

namespace GameEngine
{
    public static class UnityExtensions
    {
        public static Color ToUnityColor(this Utils.Color color)
        {
            return new(color.R / 255f, color.G / 255f, color.B / 255f, color.A / 255f);
        }

        public static Vector4 ToUnityPadding(this Margins margins)
        {
            return new(margins.Left, margins.Bottom, margins.Right, margins.Top);
        }

        public static (VerticalAlignmentOptions vertical, HorizontalAlignmentOptions horizontal) ToUnityTextAlignment(
            this TextAlignment alignment)
        {
            return alignment switch
            {
                TextAlignment.Top => (VerticalAlignmentOptions.Top, HorizontalAlignmentOptions.Left),
                TextAlignment.Bottom => (VerticalAlignmentOptions.Bottom, HorizontalAlignmentOptions.Left),
                TextAlignment.Left => (VerticalAlignmentOptions.Top, HorizontalAlignmentOptions.Left),
                TextAlignment.Right => (VerticalAlignmentOptions.Top, HorizontalAlignmentOptions.Right),
                TextAlignment.TopLeft => (VerticalAlignmentOptions.Top, HorizontalAlignmentOptions.Left),
                TextAlignment.TopCenter => (VerticalAlignmentOptions.Top, HorizontalAlignmentOptions.Geometry),
                TextAlignment.TopRight => (VerticalAlignmentOptions.Top, HorizontalAlignmentOptions.Right),
                TextAlignment.TopJustified => (VerticalAlignmentOptions.Top, HorizontalAlignmentOptions.Justified),
                TextAlignment.TopFill => (VerticalAlignmentOptions.Top, HorizontalAlignmentOptions.Flush),
                TextAlignment.CenterLeft => (VerticalAlignmentOptions.Middle, HorizontalAlignmentOptions.Left),
                TextAlignment.Center => (VerticalAlignmentOptions.Middle, HorizontalAlignmentOptions.Geometry),
                TextAlignment.CenterRight => (VerticalAlignmentOptions.Middle, HorizontalAlignmentOptions.Right),
                TextAlignment.CenterJustified => (VerticalAlignmentOptions.Middle,
                    HorizontalAlignmentOptions.Justified),
                TextAlignment.CenterFill => (VerticalAlignmentOptions.Middle, HorizontalAlignmentOptions.Flush),
                TextAlignment.BottomLeft => (VerticalAlignmentOptions.Bottom, HorizontalAlignmentOptions.Left),
                TextAlignment.BottomCenter => (VerticalAlignmentOptions.Bottom, HorizontalAlignmentOptions.Geometry),
                TextAlignment.BottomRight => (VerticalAlignmentOptions.Bottom, HorizontalAlignmentOptions.Right),
                TextAlignment.BottomJustified => (VerticalAlignmentOptions.Bottom,
                    HorizontalAlignmentOptions.Justified),
                TextAlignment.BottomFill => (VerticalAlignmentOptions.Bottom, HorizontalAlignmentOptions.Flush),
                _ => throw new ArgumentOutOfRangeException(nameof(alignment), alignment, null)
            };
        }

        public static FontStyles ToUnityStyle(this FontStyle style)
        {
            var styles = style.GetUniqueFlags();
            return styles.Aggregate(FontStyles.Normal, (current, fontStyle) => current | fontStyle switch
            {
                FontStyle.Bold => FontStyles.Bold,
                FontStyle.Italic => FontStyles.Italic,
                FontStyle.Underline => FontStyles.Underline,
                FontStyle.LowerCase => FontStyles.LowerCase,
                FontStyle.UpperCase => FontStyles.UpperCase,
                FontStyle.SmallCaps => FontStyles.SmallCaps,
                FontStyle.Strikethrough => FontStyles.Strikethrough,
                FontStyle.Superscript => FontStyles.Superscript,
                FontStyle.Subscript => FontStyles.Subscript,
                FontStyle.Highlight => FontStyles.Highlight,
                _ => throw new ArgumentOutOfRangeException()
            });
        }

        public static Image.FillMethod ToUnityFillMethod(this FillMethod method)
        {
            return method switch
            {
                FillMethod.Horizontal => Image.FillMethod.Horizontal,
                FillMethod.Vertical => Image.FillMethod.Vertical,
                FillMethod.Radial90 => Image.FillMethod.Radial90,
                FillMethod.Radial180 => Image.FillMethod.Radial180,
                FillMethod.Radial360 => Image.FillMethod.Radial360,
                _ => throw new ArgumentOutOfRangeException(nameof(method), method, null)
            };
        }

        public static TextOverflowModes ToUnityOverflow(this TextOverflow overflow)
        {
            return overflow switch
            {
                TextOverflow.Overflow => TextOverflowModes.Overflow,
                TextOverflow.Ellipsis => TextOverflowModes.Ellipsis,
                TextOverflow.Masking => TextOverflowModes.Masking,
                TextOverflow.Cut => TextOverflowModes.Truncate,
                _ => throw new ArgumentOutOfRangeException(nameof(overflow), overflow, null)
            };
        }

        public static GridLayoutGroup.Axis ToUnityAxis(this Axis axis)
        {
            return axis switch
            {
                Axis.Horizontal => GridLayoutGroup.Axis.Horizontal,
                Axis.Vertical => GridLayoutGroup.Axis.Vertical,
                _ => throw new ArgumentOutOfRangeException(nameof(axis), axis, null)
            };
        }

        public static MouseKey? ToMouseKey(this KeyCode code)
        {
            return code switch
            {
                KeyCode.Mouse0 => MouseKey.Left,
                KeyCode.Mouse1 => MouseKey.Right,
                KeyCode.Mouse2 => MouseKey.Middle,
                /*KeyCode.Mouse3 => MouseKey.Left,
            KeyCode.Mouse4 => MouseKey.Left,
            KeyCode.Mouse5 => MouseKey.Left,
            KeyCode.Mouse6 => MouseKey.Left,*/
                _ => null
            };
        }

        public static KeyboardKey? ToKeyboardKey(this KeyCode code)
        {
            return code switch
            {
                KeyCode.A => KeyboardKey.A,
                KeyCode.B => KeyboardKey.B,
                KeyCode.C => KeyboardKey.C,
                KeyCode.D => KeyboardKey.D,
                KeyCode.E => KeyboardKey.E,
                KeyCode.F => KeyboardKey.F,
                KeyCode.G => KeyboardKey.G,
                KeyCode.H => KeyboardKey.H,
                KeyCode.I => KeyboardKey.I,
                KeyCode.J => KeyboardKey.J,
                KeyCode.K => KeyboardKey.K,
                KeyCode.L => KeyboardKey.L,
                KeyCode.M => KeyboardKey.M,
                KeyCode.N => KeyboardKey.N,
                KeyCode.O => KeyboardKey.O,
                KeyCode.P => KeyboardKey.P,
                KeyCode.Q => KeyboardKey.Q,
                KeyCode.R => KeyboardKey.R,
                KeyCode.S => KeyboardKey.S,
                KeyCode.T => KeyboardKey.T,
                KeyCode.U => KeyboardKey.U,
                KeyCode.V => KeyboardKey.V,
                KeyCode.W => KeyboardKey.W,
                KeyCode.X => KeyboardKey.X,
                KeyCode.Y => KeyboardKey.Y,
                KeyCode.Z => KeyboardKey.Z,
                KeyCode.Exclaim => KeyboardKey.Exclaim,
                KeyCode.Alpha0 => KeyboardKey.D0,
                KeyCode.LeftShift => KeyboardKey.LeftShift,
                KeyCode.RightShift => KeyboardKey.RightShift,
                /*KeyCode. => KeyboardKey.D0,
            KeyCode.Exclaim => KeyboardKey.D0,
            KeyCode.Exclaim => KeyboardKey.D0,
            KeyCode.Exclaim => KeyboardKey.D0,
            KeyCode.Alpha0 => KeyboardKey.D0,
            KeyCode. => KeyboardKey.D1,
            KeyCode. => KeyboardKey.D2,
            KeyCode. => KeyboardKey.D3,
            KeyCode. => KeyboardKey.D4,
            KeyCode. => KeyboardKey.D5,
            KeyCode. => KeyboardKey.D6,
            KeyCode. => KeyboardKey.D7,
            KeyCode. => KeyboardKey.D8,
            KeyCode. => KeyboardKey.D9,*/
                _ => null
            };
        }

        public static GridLayoutGroup.Constraint ToUnityConstraint(this Constraint constraint)
        {
            return constraint switch
            {
                Constraint.Flexible => GridLayoutGroup.Constraint.Flexible,
                Constraint.FixedColumnCount => GridLayoutGroup.Constraint.FixedColumnCount,
                Constraint.FixedRowCount => GridLayoutGroup.Constraint.FixedRowCount,
                _ => throw new ArgumentOutOfRangeException(nameof(constraint), constraint, null)
            };
        }

        public static TextAnchor ToUnityAlingnment(this Alignment alignment)
        {
            return alignment switch
            {
                Alignment.TopLeft => TextAnchor.UpperLeft,
                Alignment.TopCenter => TextAnchor.UpperCenter,
                Alignment.TopRight => TextAnchor.UpperRight,
                Alignment.BottomLeft => TextAnchor.LowerLeft,
                Alignment.BottomCenter => TextAnchor.LowerCenter,
                Alignment.BottomRight => TextAnchor.LowerRight,
                Alignment.LeftCenter => TextAnchor.LowerCenter,
                Alignment.RightCenter => TextAnchor.MiddleRight,
                Alignment.Center => TextAnchor.MiddleCenter,
                _ => throw new ArgumentOutOfRangeException(nameof(alignment), alignment, null)
            };
        }

        public static GridLayoutGroup.Corner ToUnityCorner(this Corner alignment)
        {
            return alignment switch
            {
                Corner.TopLeft => GridLayoutGroup.Corner.UpperLeft,
                Corner.TopRight => GridLayoutGroup.Corner.UpperRight,
                Corner.BottomLeft => GridLayoutGroup.Corner.LowerLeft,
                Corner.BottomRight => GridLayoutGroup.Corner.LowerRight,
                _ => throw new ArgumentOutOfRangeException(nameof(alignment), alignment, null)
            };
        }

        public static Vector2 ToUnityVector(this Dot2 dot2)
        {
            return new(dot2.X, dot2.Y);
        }

        public static Vector2 ToUnityVector(this Dot2Int dot2)
        {
            return new(dot2.X, dot2.Y);
        }

        public static Vector4 ToUnityVector(this Rect rect)
        {
            return new(rect.Position.X, rect.Position.Y, rect.Width, rect.Height);
        }

        public static Dot2 ToDot(this Vector2 dot2)
        {
            return new(dot2.x, dot2.y);
        }

        public static Dot2Int ToDot(this Vector2Int dot2)
        {
            return new(dot2.x, dot2.y);
        }

        public static Dot2 ToDot(this Vector3 dot2)
        {
            return new(dot2.x, dot2.y);
        }

        public static Dot2Int ToDot(this Vector3Int dot2)
        {
            return new(dot2.x, dot2.y);
        }
    }
}