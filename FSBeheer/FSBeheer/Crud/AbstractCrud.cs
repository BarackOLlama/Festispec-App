using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSBeheer.Crud
{
    class AbstractCrud
    {
        protected CustomFSContext CustomFSContext;

        public AbstractCrud(CustomFSContext customFSContext)
        {
            CustomFSContext = customFSContext;
        }
    }
}
