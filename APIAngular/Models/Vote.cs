using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIAngular.Models
{
	public class Vote
	{
		public int VoteID { get; set; }
		public int AnswerID { get; set; }
		public int UserID { get; set; }
	}
}
