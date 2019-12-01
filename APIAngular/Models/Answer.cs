using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIAngular.Models
{
	public class Answer
	{
		public int AnswerID { get; set; }
		public string Antwoord { get; set; }

		public int PollID { get; set; }

		public int Count { get; set; }

	}
}
