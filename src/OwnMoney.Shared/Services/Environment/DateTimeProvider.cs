using System;
using System.Collections.Generic;
using System.Text;

namespace OwnMoney.Shared.Services.Environment
{
    public interface IDateTimeProvider
    {
        DateTime Now { get; }
    }

    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime Now => DateTime.Now;
    }
}
