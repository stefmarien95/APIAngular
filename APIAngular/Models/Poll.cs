using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIAngular.Models
{
	public class Poll
	{
		public int PollID { get; set; }
		public string Naam { get; set; }

		public string Vraag { get; set; }

		public int MakerID { get; set; }

	}
}
