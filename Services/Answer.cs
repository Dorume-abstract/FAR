using System;
using System.Collections.Generic;
using System.Text;

namespace FAR.Services
{
    public class Answer<T>
    {
        public string ServiceName { get; set; }
        public string Description { get; set; }
        public Result Result { get; set; }
        public Object From { get; set; }
        public Object BecauseOf { get; set; }
        public Exception Exception { get; set; }
        public T Attachment {get; set;}
        public Object[] AdditionalParams { get; set; }

    }

    public enum Result
    {
        Ok,
        OperationError,
        ValidationError,
    }
}
