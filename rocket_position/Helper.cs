using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rocket_position
{
    public class Helper
    {

        public enum ePositionStatus
        {
            Clash = 1,
            OkForLanding = 2,
            OutOfPlatform = 3
        }
    }
}
