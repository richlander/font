// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
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
        public LedMatrix(IMatrix matrix)
        : this(matrix, new Font5x8())
        {
        }

        /// <summary>
        /// Initialize LedMatrix.
        /// </summary>
        public LedMatrix(IMatrix matrix, BdfFont font)
        {
            _matrix = matrix;
            _font = font;
        }

        /// <summary>
        /// Draw text on the LED matrix.
        /// </summary>
        public void DrawLetter(char letter, int x = 0, int y = 0)
        {
            int width = _matrix.Width;
            int height = _matrix.Height;
            BdfFont font = _font;

            if (x is < 0 || x >= width)
            {
                throw new ArgumentException($"{nameof(x)} ({x}) is out of range.");
            }

            if (y is < 0 || y >= height)
            {
                throw new ArgumentException($"{nameof(y)} ({y}) is out of range.");
            }

            // height of letter given x offset
            int letterHeight = Math.Min(height - y, font.Height);
            // width of letter given y offset
            int letterWidth = x + font.Width >= width ? width - x : font.Width;
            // Get font data
            font.GetCharData(letter, out ReadOnlySpan<ushort> charData);

            // int b = 8 * (sizeof(ushort) - (int)Math.Ceiling(((double)font.Width) / 8)) + x;

            for (int j = 0; j < charData.Length; j++)
            {
                int bit = font.Width > 8 ? 0x8000 : 0x80;
                ushort glyph = charData[j];
                for (int i = 0; i < font.Width; i++)
                {
                    _matrix[x + i, y + j] = (glyph & bit) is 0 ? 0 : 1;
                    bit = bit >> 1;
                }
            }
        }
    }
}
