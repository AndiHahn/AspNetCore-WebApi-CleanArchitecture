using System;
using CleanArchitecture.Shared.Application.Cqrs;
using CleanArchitecture.Shared.Core.Result;
using Microsoft.AspNetCore.Http;

namespace CleanArchitecture.Shopping.Application.Bill.Commands.AddImageToBill
{
    public class AddImageToBillCommand : ICommand<Result>
    {
        public AddImageToBillCommand(Guid currentUserId, Guid billId, IFormFile image)
        {
            this.CurrentUserId = currentUserId;
            this.BillId = billId;
            this.Image = image;
        }

        public Guid CurrentUserId { get; }

        public Guid BillId { get; }

        public IFormFile Image { get; }
    }
}
