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

        public IEnumerable<string> RawText
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        private void Plot(int x, int y, FormattedTextChar c)
        {
            if (x < 0 || y < 0)
                return;

            // Expand columns if needed.
            if (x >= _width)
            {
                var columnsToAdd = _width - x + 1;
                foreach (var line in _lines)
                {
                    line.AddRange(Enumerable.Range(0, columnsToAdd)
                        .Select(i => new FormattedTextChar { Character = ' ', Highlighted = false }));
                }
                _width = x + 1;
            }

            // Expand rows if needed.
            if (y >= _height)
            {
                // TODO
            }
        }
    }
}
