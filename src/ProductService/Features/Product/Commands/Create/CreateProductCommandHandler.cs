﻿using MediatR;
using ProductService.DataAccess.Contracts;
using Shared.Exceptions;
using ProductEntity = ProductService.Entities.Product;

namespace ProductService.Features.Product.Commands.Create;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateProductCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        ProductEntity product = new()
        {
            Name = request.Name,
            Description = request.Description,
            CategoryId = request.CategoryId,
            YearOfRelease = request.YearOfRelease,
            ImageUrl = request.ImageUrl,
            Price = request.Price,
            Id = Guid.NewGuid(),
            BrandId = request.BrandId
        };

        await _unitOfWork.ProductRepository.AddAsync(product);

        var result = await _unitOfWork.Complete();

        if (result < 1)
            throw new ApiBadRequestException("An error occurred while saving changes to db");

        return product.Id;
    }
}
