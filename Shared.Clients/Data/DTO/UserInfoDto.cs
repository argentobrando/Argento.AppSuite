using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Clients.Data.DTO
{
	public class UserInfoDto
	{
		public string Username { get; set; }
		public string Email { get; set; }
		public string[] Roles { get; set; }
	}
}
