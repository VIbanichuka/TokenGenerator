using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TokenGenerator.Application.Dtos
{
    public class TokenDto
    {
        public Guid TokenId { get; set; }

        public Guid UserId { get; set; }

        public string AccessToken { get; set; } = string.Empty;

        public DateTimeOffset AccessTokenExpiryDate { get; set; }
    }
}
