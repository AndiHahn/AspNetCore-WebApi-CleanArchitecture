using System;
using CleanArchitecture.Shopping.Core;

namespace CleanArchitecture.Shopping.UnitTests.Builder
{
    public class UserBuilder
    {
        private Guid id = Guid.NewGuid();
        private string name = "Username";

        public UserBuilder Id(Guid id)
        {
            this.id = id;
            return this;
        }

        public UserBuilder Name(string name)
        {
            this.name = name;
            return this;
        }

        public User Build()
        {
            return new User(this.id, this.name);
        }
    }
}
