using CleanArchitecture.Core;
using System;

namespace CleanArchitecture.Application.Bill
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

    public static class CategoryExtensions
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

        public static CategoryDto ToEnum(this Category category)
        {
            return category.Name switch
            {
                nameof(Category.Food) => CategoryDto.Food,
                nameof(Category.Flat) => CategoryDto.Flat,
                nameof(Category.Clothes) => CategoryDto.Clothes,
                nameof(Category.Education) => CategoryDto.Education,
                nameof(Category.Pleasure) => CategoryDto.Pleasure,
                nameof(Category.Sport) => CategoryDto.Sport,
                nameof(Category.Travelling) => CategoryDto.Travelling,
                nameof(Category.Car) => CategoryDto.Car,
                nameof(Category.HygieneAndHealth) => CategoryDto.HygieneAndHealth,
                nameof(Category.Gift) => CategoryDto.Gift,
                _ => throw new ArgumentException($"Value {category} is not valid.", nameof(category))
            };
        }
    }
}
