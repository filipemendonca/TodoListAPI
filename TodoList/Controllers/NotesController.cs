using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TodoList.Domain.Entities;
using TodoList.Domain;
using TodoList.Dto.Notes;
using TodoList.Infra.Data.Repository.Interface;

namespace TodoList.Controllers
{
    [Route("api/notes")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly INotesRepository _notesRepository;
        private readonly IMapper _mapper;

        public NotesController(INotesRepository notesRepository, IMapper mapper)
        {
            _notesRepository = notesRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Get a entire list of notes on the database.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A list of all notes created.</returns>
        /// <response code="200">Return a success status code and the searched list of notes.</response>
        /// <response code="404">If the specific note not found.</response>
        [HttpGet]
        [SwaggerResponse(StatusCodes.Status200OK)]        
        [SwaggerResponse(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<Notes>>> GetAllNotes()
        {
            var notes = await _notesRepository.GetAsync();

            if (notes.Any()) 
                return Ok(new { status = StatusResponse.Success, data = notes });

            return NotFound(new { status = StatusResponse.Failed, message = MessageResponse.NoData });
        }

        /// <summary>
        /// Get a note by Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A simple note.</returns>
        /// <response code="200">Return a success status code and the searched note.</response>
        /// <response code="404">If the specific note not found.</response>
        [HttpGet("{id}")]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Notes>> GetNoteById(int id)
        {
            var note = await _notesRepository.GetByIdAsync(id);

            if (note is null) 
                return NotFound(new { status = StatusResponse.Failed, message = MessageResponse.NoDataById });

            return note;
        }

        /// <summary>
        /// Create a new note.
        /// </summary>
        /// <param name="model"></param>
        /// <returns>A newly created note.</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /notes
        ///     {       
        ///        "title": "Item #1",
        ///        "Content": "Content Test"
        ///     }        
        /// </remarks>
        [HttpPost]
        [SwaggerResponse(StatusCodes.Status200OK)]
        public async Task<ActionResult<Notes>> CreateNote([FromBody] NotesAddOrUpdateDto model)
        {
            return Ok(new { data = await _notesRepository.CreateOrEdit(_mapper.Map<Notes>(model)) });
        }

        /// <summary>
        /// Update a specific note.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns>A previusly note informed by id and body request successful updateded.</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /notes/${id}
        ///     {       
        ///        "title": "Item #1",
        ///        "Content": "Content Test"
        ///     }        
        /// </remarks>
        [HttpPut("{id}")]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Notes>> UpdateNote(int id, [FromBody] NotesAddOrUpdateDto model)
        {
            var singleNote = await _notesRepository.GetByIdAsync(id);

            if (singleNote is null) 
                return NotFound(new { status = StatusResponse.Failed, message = MessageResponse.NoDataById });

            singleNote.Title = model.Title;
            singleNote.Content = model.Content;

            return Ok(new {status = StatusResponse.Success, data = await _notesRepository.CreateOrEdit(singleNote) });
        }

        /// <summary>
        /// Delete a specific note informing the Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A success status code if the delete action was successful, or, failed status code if not.</returns>
        /// <response code="200">Return a success status code.</response>
        /// <response code="404">If the note not found.</response>
        [HttpDelete("{id}")]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status404NotFound)]        
        public async Task<ActionResult> DeleteNote(int id)
        {
            var note = await _notesRepository.GetByIdAsync(id);

            if (note is not null)
            {
                await _notesRepository.Delete(note);
                return Ok(new { status = StatusResponse.Success});
            }

            return NotFound(new { status = StatusResponse.Failed, message = MessageResponse.NoDataById });
        }
    }
}
