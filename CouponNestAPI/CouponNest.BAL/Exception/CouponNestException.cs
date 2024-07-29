using CouponNestAPI.CouponNest.DM.EnumsDM;

namespace CouponNestAPI.CouponNest.BAL.Exception
{
    public class CouponNestException : System.Exception
    {
        public ExceptionTypeDM ExceptionType { get; }

        public CouponNestException(ExceptionTypeDM exceptionType, string message) 
            : base(message)
        {
            ExceptionType = exceptionType;
        }
    }
}
