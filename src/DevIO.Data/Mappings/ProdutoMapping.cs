﻿using DevIO.Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevIO.Data.Mappings
{
	public class ProdutoMapping : IEntityTypeConfiguration<Produto>
	{
		public void Configure(EntityTypeBuilder<Produto> builder)
		{
			builder.HasKey(p => p.Id);

			builder.Property(p => p.Nome)
				.IsRequired()
				.HasColumnType("varchar(200)");

			builder.Property(p => p.Descricao)
				.IsRequired()
				.HasColumnType("varchar(1000)");

			builder.Property(p => p.Imagem)
				.IsRequired()
				.HasColumnType("varchar(100)");

			builder.Property(p => p.Valor)
				.IsRequired()
				.HasColumnType("decimal(18,2)"); // 💡 Agora a precisão do decimal está definida!

			builder.ToTable("Produtos");
		}
	}
}
