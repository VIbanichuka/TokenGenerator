using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TokenGenerator.Application.Dtos;

namespace TokenGenerator.Application.Interfaces.IServices
{
    public interface IEmailService
    {
        Task SendEmailAsync(EmailMessage message);
    }
}
