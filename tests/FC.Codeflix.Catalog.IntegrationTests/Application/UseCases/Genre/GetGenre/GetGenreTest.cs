﻿using UseCase = FC.Codeflix.Catalog.Application.UseCases.Genre.GetGenre;
using System.Threading.Tasks;
using Xunit;
using FC.Codeflix.Catalog.Infra.Data.EF.Repositories;
using System.Threading;
using FluentAssertions;

namespace FC.Codeflix.Catalog.IntegrationTests.Application.UseCases.Genre.GetGenre;

[Collection(nameof(GetGenreTestFixture))]
public class GetGenreTest
{
    private readonly GetGenreTestFixture _fixture;

    public GetGenreTest(GetGenreTestFixture fixture) 
        => _fixture = fixture;

    [Fact(DisplayName = nameof(GetGenre))]
    [Trait("Integration/Application", "GetGenre - Use Cases")]
    public async Task GetGenre()
    {
        var genresExampleList = _fixture.GetExampleListGenres(10);
        var expectedGenre = genresExampleList[5];
        var dbArrangeContext = _fixture.CreateDbContext();
        await dbArrangeContext.Genres.AddRangeAsync(genresExampleList);
        await dbArrangeContext.SaveChangesAsync();
        var genreRepository = new GenreRepository(_fixture.CreateDbContext(true));
        var useCase = new UseCase.GetGenre(genreRepository);
        var input = new UseCase.GetGenreInput(expectedGenre.Id);

        var output = await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Id.Should().Be(expectedGenre.Id);
        output.Name.Should().Be(expectedGenre.Name);
        output.IsActive.Should().Be(expectedGenre.IsActive);
        output.CreatedAt.Should().Be(expectedGenre.CreatedAt);
    }
}
