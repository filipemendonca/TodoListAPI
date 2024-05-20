using Xunit;
using Moq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TodoList.Domain.Entities;
using TodoList.Infra.Data.Repository.Interface;
using TodoList.Controllers;
using TodoList.Dto.Notes;

public class NotesControllerTests
{
    private readonly Mock<INotesRepository> _notesRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly NotesController _controller;

    public NotesControllerTests()
    {
        _notesRepositoryMock = new Mock<INotesRepository>();
        _mapperMock = new Mock<IMapper>();
        _controller = new NotesController(_notesRepositoryMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task GetAllNotes_ReturnsOkResult_WhenNotesExist()
    {
        // Arrange
        var notes = new List<Notes> { new Notes { Id = 1, Title = "Test", Content = "Test Content" } };
        _notesRepositoryMock.Setup(repo => repo.GetAsync()).ReturnsAsync(notes);

        // Act
        var result = await _controller.GetAllNotes();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(200, okResult.StatusCode);
    }

    [Fact]
    public async Task GetAllNotes_ReturnsNotFoundResult_WhenNoNotesExist()
    {
        // Arrange
        var notes = new List<Notes>();
        _notesRepositoryMock.Setup(repo => repo.GetAsync()).ReturnsAsync(notes);

        // Act
        var result = await _controller.GetAllNotes();

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        Assert.Equal(404, notFoundResult.StatusCode);
    }

    // [Fact]
    // public async Task GetNoteById_ReturnsOkResult_WhenNoteExists()
    // {
    //     // Arrange
    //     var note = new Notes { Id = 1, Title = "Test", Content = "Test Content" };
    //     _notesRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(note);

    //     // Act
    //     var result = await _controller.GetNoteById(1);

    //     // Assert
    //     var okResult = Assert.IsType<OkObjectResult>(result.Result);
    //     Assert.Equal(200, okResult.StatusCode);
    // }

    [Fact]
    public async Task GetNoteById_ReturnsNotFoundResult_WhenNoteDoesNotExist()
    {
        // Arrange
        _notesRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync((Notes)null);

        // Act
        var result = await _controller.GetNoteById(1);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        Assert.Equal(404, notFoundResult.StatusCode);
    }

    [Fact]
    public async Task CreateNote_ReturnsOkResult_WhenNoteIsCreated()
    {
        // Arrange
        var noteDto = new NotesAddOrUpdateDto { Title = "Test", Content = "Test Content" };
        var note = new Notes { Id = 1, Title = "Test", Content = "Test Content" };
        _mapperMock.Setup(m => m.Map<Notes>(noteDto)).Returns(note);
        _notesRepositoryMock.Setup(repo => repo.CreateOrEdit(note)).ReturnsAsync(note);

        // Act
        var result = await _controller.CreateNote(noteDto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(200, okResult.StatusCode);
    }

    [Fact]
    public async Task UpdateNote_ReturnsOkResult_WhenNoteIsUpdated()
    {
        // Arrange
        var noteDto = new NotesAddOrUpdateDto { Title = "Updated Title", Content = "Updated Content" };
        var note = new Notes { Id = 1, Title = "Test", Content = "Test Content" };
        _notesRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(note);
        _notesRepositoryMock.Setup(repo => repo.CreateOrEdit(It.IsAny<Notes>())).ReturnsAsync(note);

        // Act
        var result = await _controller.UpdateNote(1, noteDto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(200, okResult.StatusCode);
    }

    [Fact]
    public async Task UpdateNote_ReturnsNotFoundResult_WhenNoteDoesNotExist()
    {
        // Arrange
        var noteDto = new NotesAddOrUpdateDto { Title = "Updated Title", Content = "Updated Content" };
        _notesRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync((Notes)null);

        // Act
        var result = await _controller.UpdateNote(1, noteDto);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        Assert.Equal(404, notFoundResult.StatusCode);
    }

    [Fact]
    public async Task DeleteNote_ReturnsOkResult_WhenNoteIsDeleted()
    {
        // Arrange
        var note = new Notes { Id = 1, Title = "Test", Content = "Test Content" };
        _notesRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(note);
        _notesRepositoryMock.Setup(repo => repo.Delete(note)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.DeleteNote(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
    }

    [Fact]
    public async Task DeleteNote_ReturnsNotFoundResult_WhenNoteDoesNotExist()
    {
        // Arrange
        _notesRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync((Notes)null);

        // Act
        var result = await _controller.DeleteNote(1);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(404, notFoundResult.StatusCode);
    }
}