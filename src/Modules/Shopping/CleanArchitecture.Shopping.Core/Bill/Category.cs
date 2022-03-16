using Ardalis.SmartEnum;

namespace CleanArchitecture.Shopping.Core
{
    public sealed class Category : SmartEnum<Category>
    {
        private Category(string name, int value) : base(name, value)
        {
        }

        public static readonly Category Food = new Category(nameof(Food), 10);

        public static readonly Category Flat = new Category(nameof(Flat), 20);

        public static readonly Category Clothes = new Category(nameof(Clothes), 30);

        public static readonly Category Education = new Category(nameof(Education), 40);

        public static readonly Category Pleasure = new Category(nameof(Pleasure), 50);

        public static readonly Category Sport = new Category(nameof(Sport), 60);

        public static readonly Category Travelling = new Category(nameof(Travelling), 70);

        public static readonly Category Car = new Category(nameof(Car), 80);

        public static readonly Category HygieneAndHealth = new Category(nameof(HygieneAndHealth), 90);

        public static readonly Category Gift = new Category(nameof(Gift), 100);
    }
}
