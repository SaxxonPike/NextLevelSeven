using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextLevelSeven.Core.Encoding;

namespace NextLevelSeven.Conversion
{
    public class FormattedTextBlock
    {
        private readonly IReadOnlyEncoding _encoding;
        private readonly List<List<FormattedTextChar>> _lines = new List<List<FormattedTextChar>>();
        private int _x;
        private int _y;
        private int _width;
        private int _height;

        public FormattedTextBlock(IReadOnlyEncoding encoding)
        {
            _encoding = encoding;
        }

        /// <summary>
        /// Get the lines of text without formatting such as highlighting.
        /// </summary>
        public IEnumerable<string> Text
        {
            get { return _lines.Select(line => new string(line.Select(c => c.Character).ToArray())); }
        }

        /// <summary>
        /// Get or set the column of the cursor. The viewport will be expanded as needed.
        /// </summary>
        public int CursorX
        {
            get { return _x; }
            set
            {
                _x = value;
                ExpandViewport(_x, _y);
            }
        }

        /// <summary>
        /// Get or set the row of the cursor. The viewport will be expanded as needed.
        /// </summary>
        public int CursorY
        {
            get { return _y; }
            set
            {
                _y = value;
                ExpandViewport(_x, _y);
            }
        }

        /// <summary>
        /// If true, text printed to this formatted text block will have highlighted formatting.
        /// </summary>
        public bool HighlightEnabled { get; set; }

        /// <summary>
        /// Increase the size of the viewport to accommodate a character at the specified X,Y location.
        /// </summary>
        private void ExpandViewport(int x, int y)
        {
            ExpandViewportColumns(x);
            ExpandViewportRows(y);
        }

        /// <summary>
        /// Increase the size of the viewport vertically to accommodate a character at the specified Y position.
        /// </summary>
        private void ExpandViewportRows(int y)
        {
            if (y >= _height)
            {
                var rowsToAdd = _height - y + 1;
                for (var i = 0; i < rowsToAdd; i++)
                {
                    _lines.Add(Enumerable.Repeat(new FormattedTextChar { Character = ' ' }, _width)
                        .ToList());
                }
            }
        }

        /// <summary>
        /// Increase the size of the viewport horizontally to accommodate a character at the specified X position.
        /// </summary>
        private void ExpandViewportColumns(int x)
        {
            if (x >= _width)
            {
                var columnsToAdd = _width - x + 1;
                foreach (var line in _lines)
                {
                    line.AddRange(Enumerable.Range(0, columnsToAdd)
                        .Select(i => new FormattedTextChar { Character = ' ' }));
                }
                _width = x + 1;
            }
        }

        /// <summary>
        /// Plot a character at the specified X/Y position.
        /// </summary>
        private void Plot(int x, int y, FormattedTextChar c)
        {
            if (x < 0 || y < 0)
                return;

            ExpandViewport(x, y);
            _lines[y][x] = c;

            _x = x;
            _y = y;
        }
    }
}
