using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Disney.Entities;

namespace Disney.Interfaces
{
   public  interface IMailService
    {
        Task SendMail(User user);
    }
}
