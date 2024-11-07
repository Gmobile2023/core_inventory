using System.Runtime.Serialization;
using Inventory.Shared.Const;
using ServiceStack;

namespace Inventory.Shared.Dtos.CommonDto
{
    [DataContract]
    public class ResponseMessageBase<T>
    {
        public ResponseMessageBase()
        {
        }

        public ResponseMessageBase(string code, string message)
        {
            ResponseStatus = new ResStatus(code, message);
        }

        [DataMember(Order = 1)] public T Results { get; set; }
        [DataMember(Order = 2)] public ResStatus ResponseStatus { get; set; }
        [DataMember(Order = 3)] public string Signature { get; set; }

        public static ResponseMessageBase<T> Error()
        {
            return new ResponseMessageBase<T>
            {
                ResponseStatus = new ResStatus(ResponseCodeConst.Error, "Error")
            };
        }

        public static ResponseMessageBase<T> WaitResult()
        {
            return new ResponseMessageBase<T>
            {
                ResponseStatus = new ResStatus(ResponseCodeConst.WaitForResult,
                    "Giao dịch đang xử lý. Vui lòng liên hệ CSKH để được hỗ trợ")
            };
        }

        public static ResponseMessageBase<T> WaitResult(string message)
        {
            return new ResponseMessageBase<T>
            {
                ResponseStatus = new ResStatus(ResponseCodeConst.WaitForResult, message)
            };
        }

        public static ResponseMessageBase<T> Error(string message)
        {
            return new ResponseMessageBase<T>
            {
                ResponseStatus = new ResStatus(ResponseCodeConst.Error, message)
            };
        }

        public static ResponseMessageBase<T> Error(string code, string message)
        {
            return new ResponseMessageBase<T>
            {
                ResponseStatus = new ResStatus(code, message)
            };
        }

        public static ResponseMessageBase<T> Error(T data)
        {
            return new ResponseMessageBase<T>
            {
                ResponseStatus = new ResStatus(ResponseCodeConst.Error, "Error"),
                Results = data
            };
        }

        public static ResponseMessageBase<T> Error(string code, string message, T data)
        {
            return new ResponseMessageBase<T>
            {
                ResponseStatus = new ResStatus(code, message),
                Results = data
            };
        }

        public static ResponseMessageBase<T> Success()
        {
            return new ResponseMessageBase<T>
            {
                ResponseStatus = new ResStatus(ResponseCodeConst.Success, "Success"),
            };
        }

        // public static ResponseMessageBase<T> Success(string message)
        // {
        //     return new ResponseMessageBase<T>
        //     {
        //         ResponseStatus = new ResStatus(ResponseCodeConst.Success, message),
        //     };
        // }

        public static ResponseMessageBase<T> Success(T data)
        {
            return new ResponseMessageBase<T>
            {
                ResponseStatus = new ResStatus(ResponseCodeConst.Success, "Success"),
                Results = data
            };
        }

        public static ResponseMessageBase<T> Success(T data, MessageInfo info)
        {
            var rs = new ResponseMessageBase<T>
            {
                ResponseStatus = new ResStatus(ResponseCodeConst.Success, "Success"),
                Results = data
            };
            rs.ResponseStatus.MessageInfo = info;
            return rs;
        }

        public static ResponseMessageBase<T> Return(string code, string message, MessageInfo info)
        {
            var rs = new ResponseMessageBase<T>
            {
                ResponseStatus = new ResStatus(code, message)
                {
                    MessageInfo = info
                }
            };
            return rs;
        }
        public static ResponseMessageBase<T> Return(T data,string code, string message, MessageInfo info)
        {
            var rs = new ResponseMessageBase<T>
            {
                ResponseStatus = new ResStatus(code, message)
                {
                    MessageInfo = info
                },
                Results = data
            };
            return rs;
        }
    }

    [DataContract]
    public class MessageInfo
    {
        [DataMember(Order = 1)] public string Title { get; set; }
        [DataMember(Order = 2)] public string Message { get; set; }
        [DataMember(Order = 3)] public string Icon { get; set; }
        [DataMember(Order = 4)] public string Button { get; set; }
        [DataMember(Order = 5)] public bool ShowIcon { get; set; }
    }

    [DataContract]
    public class ResStatus
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResponseStatus"/> class.
        /// 
        /// A response status without an errorcode == success
        /// </summary>
        public ResStatus()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResponseStatus"/> class.
        /// A response status with an errorcode == failure
        /// </summary>
        public ResStatus(string errorCode)
        {
            this.ErrorCode = errorCode;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResponseStatus"/> class.
        /// A response status with an errorcode == failure
        /// </summary>
        public ResStatus(string errorCode, string message)
            : this(errorCode)
        {
            this.Message = message;
        }

        /// <summary>
        /// Holds the custom ErrorCode enum if provided in ValidationException
        /// otherwise will hold the name of the Exception type, e.g. typeof(Exception).Name
        /// 
        /// A value of non-null means the service encountered an error while processing the request.
        /// </summary>
        [DataMember(Order = 1)]
        public string ErrorCode { get; set; }

        /// <summary>
        /// A human friendly error message
        /// </summary>
        [DataMember(Order = 2)]
        public string Message { get; set; }

        [DataMember(Order = 3)] public string TransCode { get; set; }
        [DataMember(Order = 4)] public MessageInfo MessageInfo { get; set; }
    }
}