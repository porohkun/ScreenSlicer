using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenSlicer.Compatibility
{
    [Serializable]
    public class Rule
    {
        public string Name { get; set; }
        public ICondition[] Conditions { get; set; }
        public IActionData[] MoveWindowSequence { get; set; }
    }
}
