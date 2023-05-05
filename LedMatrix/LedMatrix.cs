﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Threading;
using Iot.Device.Graphics;

namespace Iot.Device.Matrix
{
    /// <summary>
    /// Provides basic functionality for IMatrix
    /// </summary>
    public class LedMatrix
    {
        private IMatrix _matrix;

        /// <summary>
        /// Initialize LedMatrix.
        /// </summary>
        public LedMatrix(IMatrix matrix)
        {
            _matrix = matrix;
        }

        // Scroll text from right to left
        // Example:
        // text: 20 characters * 10 width font
        // matrix: 100 width
        // viewport = matrix-width / font-width
        // 
        public bool ScrollText(ReadOnlySpan<char> text, BdfFont font, Direction direction, int index, int y = 0)
        {
            int columns = (font.Width * text.Length) + _matrix.Width;
            int cursor = direction is Direction.RightToLeft ? _matrix.Width - index : index;
            int negativeWidth = direction is Direction.RightToLeft ? 0 - font.Width : _matrix.Width + font.Width;
            int start = 0;

            if (index > columns)
            {
                return false;
            }

            if (cursor < negativeWidth && direction is Direction.RightToLeft)
            {
                start = cursor / negativeWidth;
                cursor += (font.Width * start);
            }
            else if (cursor > negativeWidth && direction is Direction.LeftToRight)
            {
                int w1 = cursor - _matrix.Width;
                int w2 = negativeWidth - _matrix.Width;
                start = w1 / w2;
                cursor -= (font.Width * start); 
            }

            // bool reverse = direction is Direction.RightToLeft ? false : true;
            DrawText(text.Slice(start), font, false, cursor, y);
            return true;
        }

        /// <summary>
        /// Draw text on the LED matrix.
        /// </summary>
        public void DrawText(ReadOnlySpan<char> value, BdfFont font, bool reverse = false, int x = 0, int y = 0)
        {
            int rollingX = x;
            int negativeWidth = font.Width * -1;

            foreach(char c in value)
            {
                if (rollingX >= _matrix.Width)
                {
                    break;
                }
                
                DrawLetter(c, font, rollingX, y);
                rollingX += reverse ? negativeWidth : font.Width;
            }
        }

        /// <summary>
        /// Draw text on the LED matrix.
        /// </summary>
        public void DrawLetter(char value, BdfFont font, int x = 0, int y = 0)
        {
            int width = _matrix.Width;
            int height = _matrix.Height;

            int firstColumnToDraw = x < 0 ? Math.Abs(x) : 0;
            int lastColumnToDraw = x + font.Width > width ? width - x : font.Width;
            // Get font data
            font.GetCharData(value, out ReadOnlySpan<ushort> charData);

            for (int j = 0; j < charData.Length; j++)
            {
                int bit = font.Width > 8 ? 0x8000 : 0x80;
                ushort glyph = charData[j];

                if (firstColumnToDraw > 0)
                {
                    bit >>= firstColumnToDraw;
                }

                for (int i = firstColumnToDraw; i < lastColumnToDraw; i++)
                {
                    _matrix[x + i, y + j] = (glyph & bit) is 0 ? 0 : 1;
                    bit >>= 1;
                }
            }
        }
    }
}
