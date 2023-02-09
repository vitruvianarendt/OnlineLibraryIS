using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ISServices.Interface
{
    public interface IBackGroundEmailSender
    {
        Task DoWork();
    }
}
