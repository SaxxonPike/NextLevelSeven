using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextLevelSeven.Mapping
{
    public struct MapLocation
    {
        public int Segment;
        public string SegmentName;
        public int Field;
        public int Repetition;
        public int Component;
        public int Subcomponent;
        public bool IsValid;

        public MapLocation(string segmentName, int field = -1, int repetition = -1, int component = -1, int subcomponent = -1)
        {
            Segment = -1;
            SegmentName = segmentName;
            Field = field;
            Repetition = repetition;
            Component = component;
            Subcomponent = subcomponent;
            IsValid = true;
        }

        public MapLocation(int segment, int field = -1, int repetition = -1, int component = -1, int subcomponent = -1)
        {
            SegmentName = null;
            Segment = segment;
            Field = field;
            Repetition = repetition;
            Component = component;
            Subcomponent = subcomponent;
            IsValid = true;
        }
    }
}
