﻿using Microsoft.EntityFrameworkCore;
using NSE.Catalogo.API.Models;
using NSE.Core.Data;
using System.Linq;
using System.Threading.Tasks;

namespace NSE.Catalogo.API.Data
{
	public class CatalogoContext : DbContext, IUnitOfWork
	{
		public CatalogoContext(DbContextOptions<CatalogoContext> options) 
			: base(options) { }

		public DbSet<Produto> Produtos { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			foreach (var property in modelBuilder.Model.GetEntityTypes().SelectMany(
					e => e.GetProperties().Where(p => p.ClrType == typeof(string))))
			{
				property.SetColumnType("varchar(100)");
			}

			modelBuilder.ApplyConfigurationsFromAssembly(typeof(CatalogoContext).Assembly);
		}

		//Método criado para de fato enviar os dados para o  banco, ou deletar, criado em um único lugar, UNITOFWORK
		public async Task<bool> Commit()
		{
			return await base.SaveChangesAsync() > 0;
		}
	}
}