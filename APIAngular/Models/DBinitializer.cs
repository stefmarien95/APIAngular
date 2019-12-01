using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIAngular.Models
{
	public class DBinitializer
	{
		public static void Initialize(PollContext context)
		{
			context.Database.EnsureCreated();
			if (context.Users.Any())
			{
				return;   // DB has been seeded
			}
	
			context.Users.AddRange(new User { UserName = "test", Password = "test", FirstName = "Test", LastName = "Test", Email = "test.test@thomasmore.be" },
				new User { UserName = "stef", Password = "stef", FirstName = "Stef", LastName = "Marien", Email = "stef.test@thomasmore.be" });
			context.SaveChanges();
		}
	}
}

