using System;
using System.Collections.Generic;
using System.Text;

namespace OwnMoney.Shared.Domains.Environment
{
    public interface IDateTimeProvider
    {
        ///<summary> Get the current date time </summary>
        DateTime Now { get; }
    }

    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime Now => DateTime.Now;
    }
}
