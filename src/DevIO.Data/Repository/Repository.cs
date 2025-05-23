﻿using System.Linq.Expressions;
using DevIO.Business.Intefaces;
using DevIO.Business.Models;
using DevIO.Business.Notificacoes;
using DevIO.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace DevIO.Data.Repository
{
    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity, new()
    {
        protected readonly MeuDbContext Db;
        protected readonly DbSet<TEntity> DbSet;

        protected Repository(MeuDbContext db)
        {
            Db = db;
            DbSet = db.Set<TEntity>();
        }

        public async Task<IEnumerable<TEntity>> Buscar(Expression<Func<TEntity, bool>> predicate)
        {
            return await DbSet.AsNoTracking().Where(predicate).ToListAsync();
        }

        public virtual async Task<TEntity> ObterPorId(Guid id)
        {
            return await DbSet.FindAsync(id);
        }

        public virtual async Task<List<TEntity>> ObterTodos()
        {
            return await DbSet.ToListAsync();
        }

        public virtual async Task Adicionar(TEntity entity)
        {
            DbSet.Add(entity);
            await SaveChanges();
        }


        public virtual async Task Atualizar(TEntity entity)
        {
            // Primeiro verifica se a entidade existe
            var existingEntity = await DbSet.FindAsync(entity.Id);

            if (existingEntity == null)
            {                
                throw new KeyNotFoundException($"Erro ao buscar entidade: " +
                    $"{entity.Id}.{Environment.NewLine} Entre em contato com suporte.");
            }

            // Atualiza as propriedades da entidade existente
            Db.Entry(existingEntity).CurrentValues.SetValues(entity);
            DbSet.Update(existingEntity);
            await SaveChanges();
        }


        public virtual async Task Remover(Guid id)
        {
            DbSet.Remove(new TEntity { Id = id });
            await SaveChanges();
        }

        public async Task<int> SaveChanges()
        {
            return await Db.SaveChangesAsync();
        }

        public void Dispose()
        {
            Db?.Dispose();
        }



    }
}