﻿using System;
using CleanArchitecture.Domain.Base;
using CleanArchitecture.Domain.Exceptions;

namespace CleanArchitecture.Application.Validations
{
    public static class NotFoundValidationExtensions
    {
        public static TEntity AssertEntityFound<TEntity>(this TEntity entity) where TEntity : BaseEntity
        {
            if (entity == null)
            {
                throw new NotFoundException($"{TryRemoveEntityPostfix(typeof(TEntity).Name)} not found.");
            }
            return entity;
        }

        public static TEntity AssertEntityFound<TEntity>(this TEntity entity, Guid id) where TEntity : BaseEntity
        {
            return AssertEntityFound<TEntity, Guid>(entity, id);
        }

        public static TEntity AssertEntityFound<TEntity>(this TEntity entity, int id) where TEntity : BaseEntity
        {
            return AssertEntityFound<TEntity, int>(entity, id);
        }

        private static TEntity AssertEntityFound<TEntity, TId>(TEntity entity, TId id)
        {
            if (entity == null)
            {
                throw new NotFoundException($"{TryRemoveEntityPostfix(typeof(TEntity).Name)} with id {id} not found.");
            }

            return entity;
        }

        private static string TryRemoveEntityPostfix(string entityName)
        {
            if (entityName.EndsWith(Constants.Common.EntityPostfix))
            {
                return entityName.Remove(entityName.Length - Constants.Common.EntityPostfix.Length);
            }

            return entityName;
        }
    }
}