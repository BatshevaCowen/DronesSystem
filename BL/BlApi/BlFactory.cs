using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;

namespace BlApi
{
    public static class BlFactory
    {
        /// <summary>
        /// GetBl- returns BL object
        /// </summary>
        /// <returns></returns>
        public static IBL GetBl()
        {
            return BL.BL.instance;
        }
    }
}
