using System;

namespace CleanArchitecture.Tests.Shared.Builder
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

        public Core.User Build()
        {
            return new Core.User(this.id, this.name);
        }
    }
}
