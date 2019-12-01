using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIAngular.Models
{
	public class Friend
	{
		public int FriendID { get; set; }
		public int VerstuurderID { get; set; }

		public int OntvangerID { get; set; }

		public int Status { get; set; }
	}
}
