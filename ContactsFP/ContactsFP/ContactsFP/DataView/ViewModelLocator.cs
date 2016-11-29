using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsFP.DataView
{
    public class ViewModelLocator
    {
        private static AddViewModel _addModel;
        public static AddViewModel AddModel
        {
            get
            {
                if (_addModel == null)
                    _addModel = new AddViewModel();
                return _addModel;
            }
        }

        private static SendViewModel _sendModel;
        public static SendViewModel SendModel
        {
            get
            {
                if (_sendModel == null)
                    _sendModel = new SendViewModel();
                return _sendModel;
            }
        }
    }
}
