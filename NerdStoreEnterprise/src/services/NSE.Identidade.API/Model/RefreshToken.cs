using System;

namespace NSE.Identidade.API.Model
{
	public class RefreshToken
	{
		public RefreshToken()
		{
			Id = Guid.NewGuid();
			Token = Guid.NewGuid();
		}

		public Guid Id { get; set; }
		public Guid Token { get; set; }

		public string Username { get; set; }
		public DateTime ExpirationDate { get; set; }
	}
}
