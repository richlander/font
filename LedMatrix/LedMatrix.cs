// Licensed to the .NET Foundation under one or more agreements.
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
        private BdfFont _font;

        /// <summary>
        /// Initialize LedMatrix.
        /// </summary>
        public LedMatrix(IMatrix matrix, BdfFont font)
        {
            _matrix = matrix;
            _font = font;
        }

        // Scroll text from right to left
        // Example:
        // text: 20 characters * 10 width font
        // matrix: 100 width
        // viewport = matrix-width / font-width
        // 
        public void ScrollingText(ReadOnlySpan<char> text, int delay)
        {
            int viewport = _matrix.Width / _font.Width;
            int columns = (_font.Width * text.Length) + _matrix.Width;
            int width = _matrix.Width;
            int negativeWidth = 0 - _font.Width;
            int start = 0;
            for (int i = 0; i < columns; i++)
            {
                width -= 1;

                if (width <= negativeWidth)
                {
                    width += _font.Width;
                    start++;
                }

                DrawText(text.Slice(start), width);
                Thread.Sleep(delay);
            }
        }

        /// <summary>
        /// Draw text on the LED matrix.
        /// </summary>
        public void DrawText(ReadOnlySpan<char> value, int x = 0, int y = 0)
        {
            int rollingX = x;
            int negativeWidth = _font.Width * -1;

            foreach(char c in value)
            {
                if (rollingX >= _matrix.Width)
                {
                    break;
                }
                
                DrawLetter(c, rollingX, y);
                rollingX += _font.Width;
            }
        }

        /// <summary>
        /// Draw text on the LED matrix.
        /// </summary>
        public void DrawLetter(char value, int x = 0, int y = 0)
        {
            int width = _matrix.Width;
            int height = _matrix.Height;
            BdfFont font = _font;

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
