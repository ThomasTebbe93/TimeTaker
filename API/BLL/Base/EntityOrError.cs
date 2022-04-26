using System;

namespace API.BLL.Base
{
    public class EntityOrError<T>
    {
        public T Value { get; set; }
        public Exception Exception { get; set; }
        
        public EntityOrError(){}

        public bool HasError() => this.Exception != null;
    }
}