using CleanArchitecture.Core;
using System;

namespace CleanArchitecture.Web.Api.Models
{
    public enum CategoryDto
    {
        Food = 10,
        Flat = 20,
        Clothes = 30,
        Education = 40,
        Pleasure = 50,
        Sport = 60,
        Travelling = 70,
        Car = 80,
        HygieneAndHealth = 90,
        Gift = 100
    }

    public static class CategoryDtoExtensions
    {
        public static Category ToClass(this CategoryDto category)
        {
            return category switch
            {
                CategoryDto.Food => Category.Food,
                CategoryDto.Flat => Category.Flat,
                CategoryDto.Clothes => Category.Clothes,
                CategoryDto.Education => Category.Education,
                CategoryDto.Pleasure => Category.Pleasure,
                CategoryDto.Sport => Category.Sport,
                CategoryDto.Travelling => Category.Travelling,
                CategoryDto.Car => Category.Car,
                CategoryDto.HygieneAndHealth => Category.HygieneAndHealth,
                CategoryDto.Gift => Category.Gift,
                _ => throw new ArgumentException($"Value {category} is not valid.", nameof(category))
            };
        }
    }
}
