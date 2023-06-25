using RequestManager.Core.Domain.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RequestManager.Core.Application.Interfaces
{
    public interface IEmailSender
    {
        void SendMessage(Message message);

        Task SendMessageAsync(Message message);
    }
}