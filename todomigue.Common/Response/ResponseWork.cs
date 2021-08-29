using System;
using System.Collections.Generic;
using System.Text;

namespace todomigue.Common.Response
{
    public class ResponseWork
    {
        public bool IsSucces { get; set; }

        public string Message { get; set; }

        public object Result { get; set; }
    }
}
